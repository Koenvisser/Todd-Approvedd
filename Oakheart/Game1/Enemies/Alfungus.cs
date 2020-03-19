using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Alfungus : Enemy
{
    public enum Phase { Normal, Snapped}
    public Phase phase;
    Vector2 Center;
    float shotTimer, sporeTimer;
    public List<BossAttacks> Attacks;
    Random random;
    public bool fightStarted = false;
    protected bool enraged;

    public Alfungus(float rotation, Vector2 position, int layer = 0, string id = "") : base(rotation, layer, id)
    {
        LoadAnimation("images/boss/Alfungus", "Alfungus", true);
        this.position = position;
        phase = Phase.Normal;  
        Center = new Vector2(Width / 2, Height / 2);
        Attacks = new List<BossAttacks>();
        shotTimer = 7500;
        sporeTimer = 20000;
    }

    public override void Update(GameTime gameTime)
    {

        if (!fightStarted)
        {
            if (playerpos.X - position.X < 1000)
            {
                fightStarted = true;
            }
        }
        base.Update(gameTime);

        if (fightStarted)
        {
            shotTimer -= (float)gameTime.ElapsedGameTime.Milliseconds;
            sporeTimer -= (float)gameTime.ElapsedGameTime.Milliseconds;

            if (shotTimer < 0)
            {
                FungusShot(playerpos);
                shotTimer = 7500 * ((float)(random.Next(90, 110) / 100));
            }

            if (sporeTimer < 0)
            {
                SporeExplosion(playerpos);
                sporeTimer = 20000 * ((float)(random.Next(90, 110) / 100))
            }

            foreach (BossAttacks attack in Attacks)
            {
                attack.Update(gameTime);
            }
        }
        
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);

        foreach(BossAttacks attack in Attacks)
        {
            attack.Draw(gameTime, spriteBatch);
        }
    }

    public void FungusShot(Vector2 PlayerPosition)
    {
        Vector2 bulletpos;
        if (PlayerPosition.X < Center.X)
        {
            bulletpos = new Vector2(BoundingBox.Left, Center.Y);
        }
        else
        {
            bulletpos = new Vector2(BoundingBox.Right, Center.Y);
        }

        if (phase == Phase.Normal)
        {
            Attacks.Add(new FungusShot(0, bulletpos, PlayerPosition, "images/game/FungusShot", 6));
        }

        if (phase == Phase.Snapped)
        {
            Attacks.Add(new FungusShot(0, bulletpos, PlayerPosition, "images/game/FungusShotRed", 3));
        }
    }

    public void SporeExplosion(Vector2 playerpos)
    {
        Vector2 sporepos = new Vector2(Center.X, BoundingBox.Top);

        if(phase == Phase.Normal)
        {
            Attacks.Add(new SporeCloud(0, sporepos, playerpos, "images/game/SporeCloud", false));
        }
        if(phase == Phase.Snapped)
        {
            Attacks.Add(new SporeCloud(0, sporepos, playerpos, "images/game/SporeCloudRed", true));
        }

    }

    public void RespawnFungus()
    {
        if(phase == Phase.Normal)
        {

        }
        if(phase == Phase.Snapped)
        {

        }
    }
}
