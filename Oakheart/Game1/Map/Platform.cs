using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OakHeart;

class Platform : SpriteGameObject
{
    List<Fungus> fungusList;
    Rectangle boundingBox;
    Player player;
    float rotation;
    public Platform(bool infected, float rotation, string assetName, Rectangle boundingBox, Player player, int layer, string id) : base(rotation, assetName, layer, id)
    {
        this.boundingBox = boundingBox;
        this.rotation = rotation;
        this.player = player;
        position = new Vector2(boundingBox.X, boundingBox.Y);
        if (infected)
        {
            fungusList = new List<Fungus>();
            for (int i = 0; boundingBox.Width / player.Sprite.Width > fungusList.Count; i++)
            {
                fungusList.Add(new Fungus(rotation, "fungus", i, position, player));
            }
        }

    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite.Sprite, boundingBox, null, Color.White, rotation, new Vector2(0), SpriteEffects.None, 0);

        if (fungusList != null)
        {
            for (int i = 0; i < fungusList.Count; i++)
            {
                fungusList[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}


