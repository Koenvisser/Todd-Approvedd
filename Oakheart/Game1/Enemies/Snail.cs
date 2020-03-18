using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Snail : Enemy
{
    protected float startx, endx;
    public Snail(float rotation, Vector2 position, float endpos, int layer = 0, string id = "") : base(rotation, layer, id)
    {
        LoadAnimation("images/game/snail", "Snail", true);
        this.position = position;
        startx = Math.Min(position.X, endpos);
        endx = Math.Max(position.X, endpos);
        movespeed = 10;
        PlayAnimation("Snail");
    }

    public override void Update(GameTime gameTime)
    {
        if (position.X <= startx && left || position.X >= endx && !left)
        {
            TurnAround();
        }

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);

    }
}

