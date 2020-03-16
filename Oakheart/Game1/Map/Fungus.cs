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
    Player player;
    public Fungus(float rotation, string assetName, int index, Vector2 platformPosition, Player player, int layer = 0, string id = "") : base(rotation, assetName, layer, id)
    {
        position = platformPosition + new Vector2(index * player.Sprite.Width, 0);
        origin = platformPosition;
        this.rotation = rotation;
        this.player = player;
    }

    public override void Update(GameTime gameTime)
    {
        if (!cleansed)
        {
           // if (player.position.X - position.X < 10 && && player)
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