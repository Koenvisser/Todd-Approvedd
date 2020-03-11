using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OakHeart;

partial class SpriteGameObject : GameObject
{
    public float tlx, tly, trx, trY, blx, bly, brx, bry, minX, minY, maxX, maxY;

    public override Rectangle BoundingBox //Creates the initial Bounding Box and also rotates points based on some math
    {
        get
        {
            rotrad = rot * (MathHelper.Pi / 180);//the actual calculation to change degrees to radian
            tlx = GlobalPosition.X - origin.X * (float)Math.Cos(rotrad) + 0 * (float)Math.Sin(rotrad);
            tly = GlobalPosition.Y - origin.Y * (float)Math.Sin(rotrad) + 0 * (float)Math.Cos(rotrad);

            trx = GlobalPosition.X - origin.X + (Width * (float)Math.Cos(rotrad)) + (0 * (float)Math.Sin(rotrad));
            trY = GlobalPosition.Y - origin.Y - (Width * (float)Math.Sin(rotrad)) + (0 * (float)Math.Cos(rotrad));

            blx = GlobalPosition.X - origin.X * (float)Math.Cos(rotrad) + Height * (float)Math.Sin(rotrad);
            bly = GlobalPosition.Y - origin.Y * (float)Math.Sin(rotrad) + Height * (float)Math.Cos(rotrad);

            brx = GlobalPosition.X - origin.X + Width * (float)Math.Cos(rotrad) + Height * (float)Math.Sin(rotrad);
            bry = GlobalPosition.Y - origin.Y - Width * (float)Math.Sin(rotrad) + Height * (float)Math.Cos(rotrad);
            minX = Math.Min(tlx, Math.Min(trx, Math.Min(blx, brx)));
            maxX = Math.Max(tlx, Math.Max(trx, Math.Max(blx, brx)));
            minY = Math.Min(tly, Math.Min(trY, Math.Min(bly, bry)));
            maxY = Math.Max(tly, Math.Max(trY, Math.Max(bly, bry)));

            Point min = new Point((int)minX, (int)minY);
            Point max = new Point((int)maxX, (int)maxY);
            return new Rectangle(min, max - min);
        }
    }

    public bool CollidesWith(SpriteGameObject obj) //once the bounding boxes intersect the actual math part will happen thanks to this code
    {
        if (!visible || !obj.visible || !BoundingBox.Intersects(obj.BoundingBox))
            return false;

        Rectangle b = Collision.Intersection(BoundingBox, obj.BoundingBox);
        if (SAT(obj))
            return true;

        return false;
    }
}
