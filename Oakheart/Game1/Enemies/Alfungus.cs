using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Alfungus : Enemy
{
    protected enum Phase { Normal, Snapped}
    Phase phase;
    Vector2 Center;
    List<BossAttacks> Attacks;
    public Alfungus(float rotation, Vector2 position, int layer = 0, string id = "") : base(rotation, layer, id)
    {
        LoadAnimation("images/boss/Alfungus", "Alfungus", true);
        this.position = position;
        phase = Phase.Normal;  
        Center = new Vector2(Width / 2, Height / 2);
        Attacks = new List<BossAttacks>();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        foreach (BossAttacks attack in Attacks)
        {
            attack.Update(gameTime);
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

    public void SporeExplosion()
    {
        if(phase == Phase.Normal)
        {

        }
        if(phase == Phase.Snapped)
        {

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
