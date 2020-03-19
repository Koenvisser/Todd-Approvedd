using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class FungusShot : BossAttacks
{
    float rotation;
    public FungusShot(float rotation, Vector2 position, Vector2 destination, string assetname, float speedmod, int layer = 0, string id = "") : base(rotation, assetname, layer, id)
    {
        this.position = position;
        velocity = position - destination;
        this.rotation = (float)Math.Atan2(velocity.Y, velocity.X);
        velocity /= speedmod;
    }

    public override void Update(GameTime gameTime)
    {

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, GlobalPosition - Camera.campos, origin, rotation);
    }
}
