using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

public class Player : AnimatedGameObject
{
    Vector2 startPosition;
    public Vector2 spawnposition;
    public bool isOnFloor, isDead, wallslide, walljump, walljumping, phasing;
    public int maxHealth = 30;
    public int phasingint = 0;
    public int currentHealth;
    public bool reset;
    public float walkingSpeed = 200f, idletime;
    public bool playercol;
    public bool jump, hit;
    public bool hasmoved = false, hasjumped = false;

    public int lives = 20;
    protected float invincibletime;

    private GameTime gameTime;
    public char characterType;

    public Player(Vector2 start) : base(0, 2) // initializes a player character
    {
        startPosition = start;
        position = startPosition;
        velocity = Vector2.Zero;
        previousposition = position;
        currentHealth = maxHealth;
        LoadAnimation("animations/Oakheartidle@10x1", "Idle", true);
        LoadAnimation("animations/Oakheartwalk@5x2", "Walk", true);
        LoadAnimation("animations/Oakheartjump@10x1", "Jump", false);
        LoadAnimation("animations/Oakheartfall@5x1", "Fall", false);
        PlayAnimation("Idle");


        invincibletime = 0.0f;

        this.mass = mass;
        triangle = false;
        //characterType = '#';
    }

    public void Jump(float speed = 300) // method for making the character jump
    {
        PlayAnimation("Jump");
        velocity.Y = -speed;
        isOnFloor = false;
        hasjumped = true;
    }

    public override void HandleInput(InputHelper inputHelper) // player controls
    {
        if (inputHelper.IsKeyDown(Keys.Z))
        {
            phasing = true;
        }
        if ((inputHelper.KeyPressed(Keys.Up) || inputHelper.KeyPressed(Keys.Space) || inputHelper.KeyPressed(Keys.W)) && walljump)
        {
            Jump(300);
            jump = true;
            walljump = false;
            walljumping = true;
            idletime = 0;
        }
        else if ((inputHelper.KeyPressed(Keys.Up) || inputHelper.KeyPressed(Keys.Space) || inputHelper.KeyPressed(Keys.W)) && isOnFloor)
        {
            Jump(400);
            jump = true;
            idletime = 0;
        }
        //if (inputHelper.IsKeyDown(Keys.Down) || inputHelper.IsKeyDown(Keys.A) && !isOnFloor)
        //{
        //    velocity.Y = walkingSpeed;
        //}
        if (inputHelper.IsKeyDown(Keys.Left) || inputHelper.IsKeyDown(Keys.A))
        {
            velocity.X = -walkingSpeed;
            if (isOnFloor)
            {
                PlayAnimation("Walk");

            }
            idletime = 0;
            hasmoved = true;
        }
        else if (inputHelper.IsKeyDown(Keys.Right) || inputHelper.IsKeyDown(Keys.D))
        {
            velocity.X = walkingSpeed;
            if (isOnFloor)
            {
                PlayAnimation("Walk");

            }
            idletime = 0;
            hasmoved = true;
        }
        else
        {
            velocity.X = 0f;
            if (isOnFloor)
            {
                PlayAnimation("Idle");
            }
        }
        if (velocity.X != 0.0f)
        {
            Mirror = velocity.X < 0;
        }
        if(!isOnFloor && velocity.Y > 0)
        {
            PlayAnimation("Fall");
        }
    }

    public override void Update(GameTime gameTime) // updates the player character when in the game
    {
        idletime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        jump = false;
        hit = false;
        base.Update(gameTime);
        position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (invincibletime > 0.0f)
        {
            invincibletime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (velocity.X == 0 && velocity.Y == 0)
        {

        }
        if (reset == true)
        {
            if (spawnposition == new Vector2(0, 0))
                position = startPosition;
            else
                position = spawnposition;
        }
    }

    public void Spawnposition(Vector2 spawnpos)
    {
        spawnposition = spawnpos;
    }

    public void Die() // handles events after the player dies
    {
        reset = true;
    }

    public virtual void CheckPlayerCollision(Enemy testenemy) // enemy collision testing, may be safe to remove
    {
        if (!CollidesWith(testenemy))
        {
            return;
        }

        if (CollidesWith(testenemy))
        {
            position += MTV;
            MTV = Vector2.Zero;
            testenemy.collision = true;
            testenemy.velocity = Vector2.Zero;
            playercol = true;

            if (maxY - 20 < testenemy.minY)
            {
                isOnFloor = true;

                Jump(150);
            }
            if (invincibletime <= 0.0f)
            {
                lives -= 2;
                invincibletime = 3.0f;
            }
        }
    }

    public int SetLives // set the player's lives
    {
        get { return lives; }
        set { lives = value; }
    }
}
