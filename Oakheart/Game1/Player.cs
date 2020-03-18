using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

public class Player : SpriteGameObject
{
    Vector2 startPosition;
    public Vector2 spawnposition;
    public bool isOnFloor, isDead, shieldActive, itemAvailable, crosshairVisible, wallslide, walljump;
    public int maxHealth = 30;
    public int currentHealth;
    public bool reset;
    public float walkingSpeed = 200f;
    public bool playercol;
    public bool jump, hit;

    public int lives = 20;
    protected float invincibletime;

    private GameTime gameTime;
    public char characterType;

    public Player(Vector2 start, string AssetName) : base(0, AssetName, 2) // initializes a player character
    {
        startPosition = start;
        position = startPosition;
        velocity = Vector2.Zero;
        previousposition = position;
        currentHealth = maxHealth;


        invincibletime = 0.0f;

        this.mass = mass;
        triangle = false;
        //characterType = '#';
    }

    public void Jump(float speed = 300) // method for making the character jump
    {
        velocity.Y = -speed;
        isOnFloor = false;
        if (characterType == '#') // higher jump if the character is agile
        {
            velocity.Y = -speed * 1.3f;
        }
    }

    public override void HandleInput(InputHelper inputHelper) // player controls
    {
        if ((inputHelper.KeyPressed(Keys.Up) || inputHelper.KeyPressed(Keys.Space) || inputHelper.KeyPressed(Keys.W)) && isOnFloor)
        {
            Jump(300);
            jump = true;
        }
        //if (inputHelper.IsKeyDown(Keys.Down) || inputHelper.IsKeyDown(Keys.A) && !isOnFloor)
        //{
        //    velocity.Y = walkingSpeed;
        //}
        if (inputHelper.IsKeyDown(Keys.Left) || inputHelper.IsKeyDown(Keys.A))
        {
            velocity.X = -walkingSpeed;
        }
        else if (inputHelper.IsKeyDown(Keys.Right) || inputHelper.IsKeyDown(Keys.D))
        {
            velocity.X = walkingSpeed;
        }
        else
        {
            velocity.X = 0f;
        }
        if (velocity.X != 0.0f)
        {
            Mirror = velocity.X < 0;
        }
    }

    public override void Update(GameTime gameTime) // updates the player character when in the game
    {
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
        lives = 20;
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
