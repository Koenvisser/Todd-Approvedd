using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OakHeart;

partial class SpriteGameObject : GameObject
{
    Vector2 projection;
    List<Vector2> axislist, lijnlist, positionlijnlist;

    public bool SAT(SpriteGameObject obj)
    {
        smalloverlap = float.MaxValue;
        smallest = Vector2.Zero;
        axislist = new List<Vector2>();

        initialize(obj); //initialises the lines and positions. 

        foreach (Vector2 lijn in lijnlist) //These two list codes make sure to add the axises needed for the SAT and also delete the axises when the next object collides.
        {
            axislist.Add(Vector2.Normalize(new Vector2(lijn.Y, -lijn.X)));

            if (axislist.Count > lines + obj.lines)
            {
                axislist.RemoveAt(0);
            }
        }


        foreach (Vector2 lijn in obj.lijnlist)
        {
            axislist.Add(Vector2.Normalize(new Vector2(lijn.Y, -lijn.X)));

            if (axislist.Count > obj.lines + lines)
            {
                axislist.RemoveAt(0);
            }
        }

        int f = 0;
        foreach (Vector2 axis in axislist)
        {
            f += 1;
            foreach (Vector2 positionlijn in positionlijnlist) //First we need to calculate the dot product between the positionlines and the axises.
            {
                dotlist.Add(Vector2.Dot(positionlijn, axis));
                if (dotlist.Count > lines + obj.lines)
                {
                    dotlist.RemoveAt(0);
                }
            }

            foreach (Vector2 positionlijn in obj.positionlijnlist)
            {
                obj.dotlist.Add(Vector2.Dot(positionlijn, axis));
                if (obj.dotlist.Count > obj.lines + lines)
                {
                    obj.dotlist.RemoveAt(0);
                }
            }

            foreach (float dot in dotlist) //After calculating the dotproduct we can now calculate the projection!
            {
                projlist.Add(new Vector2(dot / ((float)Math.Pow(axis.X, 2) + (float)Math.Pow(axis.Y, 2)) * axis.X,
                                      dot / ((float)Math.Pow(axis.X, 2) + (float)Math.Pow(axis.Y, 2)) * axis.Y));
                if (projlist.Count > lines)
                {
                    projlist.RemoveAt(0);
                }
            }

            foreach (float dot in obj.dotlist)
            {
                obj.projlist.Add(new Vector2(dot / ((float)Math.Pow(axis.X, 2) + (float)Math.Pow(axis.Y, 2)) * axis.X,
                                      dot / ((float)Math.Pow(axis.X, 2) + (float)Math.Pow(axis.Y, 2)) * axis.Y));

                if (obj.projlist.Count > obj.lines)
                {
                    obj.projlist.RemoveAt(0);
                }
            }

            minmax(obj); //calculates the minimums and maximums

            if (maxx < obj.minx || obj.maxx < minx || maxy < obj.miny || obj.maxy < miny) //if there is no overlap on any of the axises there is no overlap in the game so no more calculation has to be done
                return false;

            MinimumTranslationVector(obj, axislist, f, axis);

        }
        return true; //After all this code, there has to be overlap.
    }

    void initialize(SpriteGameObject obj)
    {
        lijnlist = new List<Vector2>();
        obj.lijnlist = new List<Vector2>();
        obj.positionlijnlist = new List<Vector2>();
        positionlijnlist = new List<Vector2>();


        

        if (!triangle)
        {
            lijnlist.Insert(0, new Vector2(tlx - trx, tly - trY));
            lijnlist.Insert(1, new Vector2(tlx - blx, tly - bly));

            positionlijnlist.Insert(0, new Vector2(tlx, tly));
            //-112, 0
            positionlijnlist.Insert(1, new Vector2(trx, trY));
            //0, -94
            positionlijnlist.Insert(2, new Vector2(brx, bry));
            //-112, 0
            positionlijnlist.Insert(3, new Vector2(blx, bly));
            //0, -94
            lines = 4;
        }
        else
        {
            if (mirrored)
            {
                trx = tlx;
                trY = tly;
            }
            lijnlist.Insert(0, new Vector2(trx - brx, trY - bry));
            lijnlist.Insert(1, new Vector2(blx - brx, bly - bry));
            lijnlist.Insert(2, new Vector2(blx - trx, bly - trY));

            positionlijnlist.Insert(0, new Vector2(blx, bly));
            //-112, 0
            positionlijnlist.Insert(1, new Vector2(trx, trY));
            //0, -94
            positionlijnlist.Insert(2, new Vector2(brx, bry));
            //-112, 0
            lines = 3;
        }

        /*if (trx > obj.tlx && tlx < obj.trx)
        {
        obj.lijnlist.Insert(0, new Vector2((obj.tlx + "circlewidth") - (obj.trx - "circlewidth"), obj.tly - obj.trY));
        obj.lijnlist.Insert(1, new Vector2((obj.tlx + "circlewidth") - (obj.blx - "circlewidth"), obj.tly - obj.bly));


        }

        else{

            if (circlemiddel < trx && circlemiddel > tlx)
            {
                obj.lijnlist.Insert(0, new Vector2(0, (circlemiddel + straal - circlemiddel - straal)));
                obj.positionlijnlist.Insert(0, new Vector2(circlemiddel.x, circlemiddel - straal));
                obj.positionlijnlist.Insert(0, new Vector2(circlemiddel.x, circlemiddel + straal));
            }
            else if (circlemiddel < tly && circlemiddel > bly)
            {
                obj.lijnlist.Insert(0, new Vector2((circlemiddel + straal - circlemiddel - straal), 0));
                obj.positionlijnlist.Insert(0, new Vector2(circlemiddel - straal, circlemiddel.y));
                obj.positionlijnlist.Insert(0, new Vector2(circlemiddel + straal, circlemiddel.y));
            }
            else {
                float closest = float.MaxValue;
                Vector2 closestaxis = new Vector2(0,0);
                foreach (Vector2 positionlijn in positionlijnlist)
                {
                    if (closest > Math.Sqrt(Math.Pow(circlemiddel.x - positionlijn.X) + Math.Pow(circlemiddel.Y - positionlijn.Y)))
                    {
                        closest = Math.Sqrt(Math.Pow(circlemiddel.x - positionlijn.X) + Math.Pow(circlemiddel.Y - positionlijn.Y));
                        closestaxis = new Vector2(circlemiddel.x - positionlijn.X, circlemiddel.Y - positionlijn.Y);
                    }
                }
                obj.lijnlist.Insert(0, closestaxis);
                obj.positionlijnlist.Insert(0, new Vector2(circlemiddel.X - (straal * closestaxis.X), circlemiddel.Y - (straal * closestaxis.Y)));
                obj.positionlijnlist.Insert(1, new Vector2(circlemiddel.X + (straal * closestaxis.X), circlemiddel.Y - (straal * closestaxis.Y)));
                obj.positionlijnlist.Insert(2, new Vector2(circlemiddel.X - (straal * closestaxis.X), circlemiddel.Y + (straal * closestaxis.Y)));
                obj.positionlijnlist.Insert(3, new Vector2(circlemiddel.X + (straal * closestaxis.X), circlemiddel.Y + (straal * closestaxis.Y)));
            }
        }
        */
        
        if (!obj.triangle)
        {
            obj.lijnlist.Insert(0, new Vector2(obj.tlx - obj.trx, obj.tly - obj.trY));
            //-108, 0
            obj.lijnlist.Insert(1, new Vector2(obj.tlx - obj.blx, obj.tly - obj.bly));

            obj.positionlijnlist.Insert(0, new Vector2(obj.tlx, obj.tly));
            //-108, 0
            obj.positionlijnlist.Insert(1, new Vector2(obj.trx, obj.trY));
            //-85.74, -49.5
            obj.positionlijnlist.Insert(2, new Vector2(obj.brx, obj.bry));
            //-108, 0
            obj.positionlijnlist.Insert(3, new Vector2(obj.blx, obj.bly));
            //-85.74, -49.5
            obj.lines = 4;
        }
        else
        {
            if (obj.mirrored)
            {
                obj.trx = obj.tlx;
                obj.trY = obj.tly;
            }
            obj.lijnlist.Insert(0, new Vector2(obj.trx - obj.brx, obj.trY - obj.bry));

            obj.lijnlist.Insert(1, new Vector2(obj.blx - obj.brx, obj.bly - obj.bry));
            obj.lijnlist.Insert(2, new Vector2(obj.blx - obj.trx, obj.bly - obj.trY));

            obj.positionlijnlist.Insert(0, new Vector2(obj.blx, obj.bly));
            //-112, 0
            obj.positionlijnlist.Insert(1, new Vector2(obj.trx, obj.trY));
            //0, -94
            obj.positionlijnlist.Insert(2, new Vector2(obj.brx, obj.bry));
            //-112, 0
            obj.lines = 3;
        }
    }

    void minmax(SpriteGameObject obj)
    {

        minx = float.MaxValue;
        miny = float.MaxValue;
        maxx = float.MinValue;
        maxy = float.MinValue;
        obj.minx = float.MaxValue;
        obj.miny = float.MaxValue;
        obj.maxx = float.MinValue;
        obj.maxy = float.MinValue;
        foreach (Vector2 proj in projlist)
        {
            if (proj.X < minx)
                minx = proj.X;
            if (proj.X > maxx)
                maxx = proj.X;

            if (proj.Y < miny)
                miny = proj.Y;
            if (proj.Y > maxy)
                maxy = proj.Y;
        }


        foreach (Vector2 proj in obj.projlist)
        {
            if (proj.X < obj.minx)
                obj.minx = proj.X;
            if (proj.X > obj.maxx)
                obj.maxx = proj.X;

            if (proj.Y < obj.miny)
                obj.miny = proj.Y;
            if (proj.Y > obj.maxy)
                obj.maxy = proj.Y;
        }

    }
}
