using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

partial class SpriteGameObject : GameObject
{
    bool links, onder, linksboven, linksonder, rechtsboven, rechtsonder, rechtszijde, linkszijde;
    float magoverlap, minpositionx, maxpositionx, minpositionxy, maxpositionxy, minpositiony, maxpositiony, minpositionyx, maxpositionyx;
    public float smalloverlap;
    public Vector2 MTV, smallest;

    void MinimumTranslationVector(SpriteGameObject obj, List<Vector2> axis, int f, Vector2 curraxis)
    {
        Overlap(obj); //Overlap is found, so it need to be calculated

        if (maxX - 20 < obj.minX && minX < obj.minX)// Calculates on what side the collision happens
            links = true;
        else
            links = false;

        if (maxY - 20 > obj.maxY)
            onder = true;
        else
            onder = false;

        xposy(obj); //Caculates the x position for the y points.

        linksonder = false;
        linksboven = false;
        rechtsonder = false;
        rechtsboven = false;
        rechtszijde = false;
        linkszijde = false;

        float objrotation = obj.rot + 3600;

        if (obj.rot % 90 != 0 && (minX + maxX) / 2 < minpositionx && minY - velocity.Y >= minpositiony) //bottomleft collision
            linksonder = true;

        else if (obj.rot % 90 != 0 && (minX + maxX) / 2 < maxpositionx && maxY + velocity.Y >= minpositiony) //topleft collision
            linksboven = true;

        else if (obj.rot % 90 != 0 && maxX - 10 < maxpositionx && maxX - velocity.X <= minpositionx)//collision for the left side
            linkszijde = true;

        else if (obj.rot % 90 != 0 && (minX + maxX) / 2 > minpositionx && minY + velocity.Y <= minpositiony) //bottomright collision
            rechtsonder = true;

        else if (obj.rot % 90 != 0 && (minX + maxX) / 2 > maxpositionx && maxY - velocity.Y <= minpositiony)  //topright collision
            rechtsboven = true;

        else if (obj.rot % 90 != 0 && minX - velocity.X >= maxpositionx && maxX - velocity.X >= minpositionx)
            rechtszijde = true;

        objrotation = objrotation % 90;
        if (magoverlap < smalloverlap) //if the calculated overlap is smaller than the already calculated overlap this part happens. 
        {
            smalloverlap = magoverlap;
            smallest = curraxis;
            if (objrotation == 0) //if the object is not rotated at all the math is simple
            {
                if (!links)
                    smallest.X *= -1;
                if (!onder)
                    smallest.Y *= -1;

                if (Math.Abs(rot) % 360 == 180)
                    smallest *= -1;
                else if (rot == -90)
                    smallest.Y *= -1;
            }
            else //Depending on what side is hit and what the axis is, the direction the object is pushed in needs to change.
            {
                if (rechtsonder)
                {
                    if (curraxis.X < 0 && !rechtsboven)
                        smallest.X *= -1;
                    if (curraxis.Y < 0)
                        smallest.Y *= -1;
                }
                if (rechtszijde)
                {
                    if (curraxis.X < 0)
                        smallest.X *= -1;
                }
                if (linkszijde)
                {
                    if (curraxis.X > 0)
                        smallest.X *= -1;

                }
                if (linksonder)
                {
                    if (curraxis.X > 0)
                        smallest.X *= -1;
                    if (curraxis.Y < 0)
                        smallest.Y *= -1;

                }
                if (linksboven)
                {
                    if (curraxis.X > 0)
                        smallest.X *= -1;
                    if (curraxis.Y > 0)
                        smallest.Y *= -1;

                }
                if (rechtsboven)
                {
                    if (curraxis.X < 0)
                        smallest.X *= -1;
                    if (curraxis.Y > 0)
                        smallest.Y *= -1;
                }
            }
            MTV = smalloverlap * smallest; //after all the math the MTV is easily calculated, this 'pushes' one object out of the other object. It's the minimum translation vector
        }
    }


    void Overlap(SpriteGameObject obj) // detects if there is overlap between objects
    {
        Vector2 overlap = new Vector2(float.MaxValue, float.MaxValue);
        if (maxx < obj.maxx)
        {
            overlap.X = maxx - obj.minx;
        }
        else
        {
            overlap.X = obj.maxx - minx;
        }
        if (maxy > obj.maxy)
        {
            overlap.Y = obj.maxy - miny;
        }
        else
        {
            overlap.Y = maxy - obj.miny;
        }
        magoverlap = (float)Math.Sqrt(Math.Pow(overlap.X, 2) + Math.Pow(overlap.Y, 2));
    }

    void xposy(SpriteGameObject obj)
    {
        float tempminx = float.MinValue;
        float tempmaxx = float.MaxValue;
        float tempminy = float.MinValue;
        float tempmaxy = float.MaxValue;
        minpositionx = 0;
        maxpositionx = 0;
        minpositionxy = 0;
        maxpositionxy = 0;
        minpositiony = 0;
        maxpositiony = 0;
        minpositionyx = 0;
        maxpositionyx = 0;

        foreach (Vector2 positionlijn in obj.positionlijnlist)
        {
            if (positionlijn.Y > tempminx)
            {
                tempminx = Math.Max(tempminx, positionlijn.Y);
                minpositionx = positionlijn.X;
                minpositionxy = positionlijn.Y;
            }

            if (positionlijn.Y < tempmaxx)
            {
                tempmaxx = Math.Min(tempmaxx, positionlijn.Y);
                maxpositionx = positionlijn.X;
                maxpositionxy = positionlijn.Y;
            }

            if (positionlijn.X > tempminy)
            {
                tempminy = Math.Max(tempminy, positionlijn.X);
                minpositiony = positionlijn.Y;
                minpositionyx = positionlijn.X;
            }

            if (positionlijn.X < tempmaxy)
            {
                tempmaxy = Math.Min(tempmaxy, positionlijn.X);
                maxpositiony = positionlijn.Y;
                maxpositionyx = positionlijn.X;
            }
        }
    }
}
