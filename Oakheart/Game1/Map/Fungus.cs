using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Fungus
{
    public Fungus()
    {
    public override void Update(GameTime gameTime)
    {
        if (!cleansed)
        {
            if (player.position.X - position.X < 10)
            { }
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!cleansed)
        {
            spriteBatch.Draw(sprite.Sprite, position, null, Color.White, rotation, origin, 1, SpriteEffects.None, 0);
        }
    }
}
}
