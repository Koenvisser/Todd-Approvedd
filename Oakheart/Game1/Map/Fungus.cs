using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Fungus : SpriteGameObject
{
    float rotation;
    bool cleansed;
    public Fungus(float rotation, string assetName, int index, Vector2 platformPosition, int layer = 0, string id = "") : base(rotation, assetName, layer, id)
    {
        position = platformPosition + new Vector2(index * 180, 0); //replace with player width
        origin = platformPosition;
        this.rotation = rotation;
    }

    public override void Update(GameTime gameTime)
    {
        if (!cleansed)
        {

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