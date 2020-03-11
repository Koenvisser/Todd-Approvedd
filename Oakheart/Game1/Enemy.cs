using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

public class Enemy : SpriteGameObject
{
    public int maxHealth = 30; // maximum enemy health
    protected int currentHealth; // current enemy health
    bool isDead; // boolean to measure if the enemy is alive or not
    public bool fly, hidden, patrol; // enemy type
    protected float waitTime; // the time between two turns
    protected float movespeed; // movement speed
    public float coltimer; // collision timer
    public bool left; // enemy moving left or not

    public bool appear;
    public bool boxcollision;
    public Enemy(float rotation, string assetName, int layer) : base(rotation, assetName, layer)
    {
        triangle = false;
    }

    public void Visible() // sets the enemy as visible
    {
        this.visible = true;
    }

    public void InVisible() // sets the enemy as invisible
    {
        this.visible = false;
    }

    public override void Update(GameTime gameTime) // updates enemies
    {
        base.Update(gameTime);
    }

    public void TurnAround() // turns the enemy around
    {
        left = !left;
        if (left)
        {
            Mirror = !Mirror;
            movespeed *= -1;
        }
    }

}

