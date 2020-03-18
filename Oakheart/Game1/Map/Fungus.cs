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
        this.boundingBox = new Rectangle(boundingBox.X + ((int)(tlx - this.trx) / 8) * index, boundingBox.Y - ((boundingBox.Height - 120) / 8) * index, (boundingBox.Width / 8), boundingBox.Height); //replace with player width
        this.rotation = rotation % 180;
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
            spriteBatch.Draw(sprite.Sprite, new Rectangle(boundingBox.X - (int)Camera.campos.X, boundingBox.Y - (int)Camera.campos.Y, boundingBox.Width , boundingBox.Height), null, Color.White, -rotation * MathHelper.Pi / 180, new Vector2(0), SpriteEffects.None, 0);
        }
    }
}