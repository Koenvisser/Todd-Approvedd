using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OakHeart;

class Platform : SpriteGameObject
{
    List<Fungus> fungusList;
    Rectangle boundingBox;
    Player player;
    public Platform(bool infected, float rotation, string assetName, Rectangle boundingBox, int layer, string id) : base(rotation, assetName, layer, id)
    {
        this.boundingBox = boundingBox;
        position = new Vector2(boundingBox.X, boundingBox.Y);
        if (infected)
        {
            fungusList = new List<Fungus>();
            for (int i = 0; boundingBox.Width / 180 > fungusList.Count; i++)
            {
                fungusList.Add(new Fungus());
            } 
        }

    }
}
    
    
