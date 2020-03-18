using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Fungus : SpriteGameObject
{
    Rectangle boundingBox;
    float rotation;
    bool cleansed;
    public Fungus(float rotation, string assetName, int index, Rectangle boundingBox, int layer = 0, string id = "") : base(rotation, assetName, layer, id)
    {
        this.boundingBox = new Rectangle(boundingBox.X + (boundingBox.Width / 8) * index, boundingBox.Y, (boundingBox.Width / 8), boundingBox.Height); //replace with player width
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
            spriteBatch.Draw(sprite.Sprite, boundingBox, null, Color.White, rotation, new Vector2(0) + Camera.campos, SpriteEffects.None, 0);
        }
    }
}