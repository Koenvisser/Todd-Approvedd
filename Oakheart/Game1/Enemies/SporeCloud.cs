using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

class SporeCloud : BossAttacks
{
    public bool damaging;
    public SporeCloud(float rotation, Vector2 position, Vector2 playerpos, string assetname, bool damaging, int layer = 0, string id = "") : base(rotation, assetname, layer, id)
    {
        this.position = position;
        this.damaging = damaging;

        velocity = (position - playerpos) / 10;  
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
    }

    public void UpdatePlayerPos(Vector2 playerpos)
    {
        velocity = (position - playerpos);
    }
}
