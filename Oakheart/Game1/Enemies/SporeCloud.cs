using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

class SporeCloud : BossAttacks
{
    int visibilityTimer = 3000;
    public bool damaging;
    public SporeCloud(float rotation, Vector2 position, Vector2 playerpos, string assetname, bool damaging, int layer = 0, string id = "") : base(rotation, assetname, layer, id)
    {
        this.position = position;
        this.damaging = damaging;

        velocity = (playerpos - position)/1.2f;  
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        visibilityTimer -= gameTime.ElapsedGameTime.Milliseconds;

        if(visibilityTimer < 0)
        {
            Visible = false;
            position = Vector2.Zero;
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
    }

    public void UpdatePlayerPos(Vector2 playerpos)
    {
        velocity = (playerpos - position)/1.2f;
    }
}
