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
    bool cleansed = false;
    bool touched = false;
    int timer;
    int j;
    float rotation;
    public Vector2 playerpos;
    bool[] fungusactive = new bool[8];
    List<Fungus> fungus = new List<Fungus>();

    public Platform(bool infected, float rotation, string assetName, Rectangle boundingBox, int layer = 0, string id = "") : base(rotation, assetName, layer, id)
    {
        this.boundingBox = boundingBox;
        this.rotation = rotation % 180;
        position = new Vector2(boundingBox.X + Camera.campos.X, boundingBox.Y + Camera.campos.Y);
        if (infected)
        {
            for (int i = 0; boundingBox.Width / (boundingBox.Width/8) > fungus.Count; i++)
            {
                fungus.Add(new Fungus(rotation, "images/game/fungus", i, boundingBox));
                fungusactive[i] = true;
            } 
        }
    }

    public void Update(GameTime gameTime, bool collided)
    {
        if (collided)
        {
            for (int i = 1; i < fungus.Count; i++)
            {
                if ((playerpos.X < BoundingBox.X + (boundingBox.Width / 8) * i && playerpos.X > BoundingBox.X + (boundingBox.Width / 8) * (i - 1)) || playerpos.X + 80 /*playerwidth*/ > BoundingBox.X + (boundingBox.Width / 8) * i && playerpos.X + 80 < BoundingBox.X + (boundingBox.Width / 8) * (i + 1))
                {
                    fungusactive[i] = false;
                    fungusactive[i - 1] = false;
                    touched = true;
                    timer = 0;
                }
            }
        }
        if (!cleansed && touched && !collided)
        {
            for (int i = 0; i < fungus.Count; i++)
            {
                if (fungusactive[i])
                {
                    j++;
                }
            }
            if (j != 0)
            {
                cleansed = false;
                j = 0;
            }
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer > 5000)
            {
                for (int i = 0; i < fungus.Count; i++)
                {
                    if (i == 0)
                    {
                        if (!fungusactive[i] && fungusactive[i + 1])
                        {
                            fungusactive[i] = true;
                            timer = 0;
                            return;
                        }
                    }
                    else if (i == fungus.Count - 1)
                    {
                        if (!fungusactive[i] && fungusactive[i - 1])
                        {
                            fungusactive[i] = true;
                            timer = 0;
                            return;
                        }
                    }
                    else if (!fungusactive[i] && (fungusactive[i - 1] || fungusactive[i + 1]))
                    {
                        fungusactive[i] = true;
                        i = fungus.Count - 1;
                        timer = 0;
                        return;
                    }
                }
            }

        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch, GlobalPosition - Camera.campos, origin, -rot * MathHelper.Pi / 180);

        for (int i = 0; i < fungus.Count; i++)
        {
            if (fungus != null && fungusactive[i])
            {
                    fungus[i].Draw(gameTime, spriteBatch);
                
            }
        }
    }
}
    
    
