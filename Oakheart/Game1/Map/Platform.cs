using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OakHeart;

public class Platform : SpriteGameObject
{
    Rectangle boundingBox;
    float rotation;
    public Vector2 playerpos;
    bool[] fungusactive = new bool[8];
    List<Fungus> fungus = new List<Fungus>();

    public Platform(bool infected, float rotation, string assetName, Rectangle boundingBox, int layer = 0, string id = "") : base(rotation, assetName, layer, id)
    {
        this.boundingBox = boundingBox;
        this.rotation = rotation;
        position = new Vector2(boundingBox.X, boundingBox.Y);
        if (infected)
        {
            for (int i = 0; boundingBox.Width / (boundingBox.Width/8) > fungus.Count; i++)
            {
                fungus.Add(new Fungus(rotation, "images/game/fungus", i, boundingBox));
                fungusactive[i] = true;
            } 
        }
    }

    public override void Update(GameTime gameTime)
    {
        for (int i = 1; i < fungus.Count; i++)
        {
            if ((playerpos.X < BoundingBox.X + (boundingBox.Width / 8) * i && playerpos.X > BoundingBox.X + (boundingBox.Width / 8) * (i-1)) || playerpos.X + 48 /*playerwidth*/ < BoundingBox.X + (boundingBox.Width / 8) * i && playerpos.X + 48 > BoundingBox.X + (boundingBox.Width / 8) * (i+1))
            {
                fungusactive[i] = false;
                fungusactive[i-1] = false;
            }
            Console.WriteLine(fungusactive[i]);

        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite.Sprite, boundingBox, null, Color.White, rotation, new Vector2(0) + Camera.campos, SpriteEffects.None, 0);

        for (int i = 0; i < fungus.Count; i++)
        {
            if (fungus != null && fungusactive[i])
            {
                    fungus[i].Draw(gameTime, spriteBatch);
                
            }
        }
    }
}
    
    
