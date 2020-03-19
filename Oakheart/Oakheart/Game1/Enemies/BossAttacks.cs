using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

class BossAttacks : SpriteGameObject
{
    public BossAttacks( float rotation, string assetname, int layer = 0, string id = "") : base(rotation, assetname, layer, id)
    {
        
    }
    public override void Update(GameTime gameTime)
    {
        if (visible)
        {
            base.Update(gameTime);
                
        }
    }
}
