using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public partial class SpriteGameObject : GameObject
{
    // part of the code here has been copied from ticktick, the rest has been tweaked to suit our physics
    protected SpriteSheet sprite;
    protected Vector2 origin;
    public int count = 0;
    public float rot, rotrad;
    public int timer = 0;
    public bool mirrored;
    public bool collision;

    public SpriteGameObject(float rotation, string assetName, int layer = 0, string id = "", int sheetIndex = 0)
        : base(layer, id)
    {
        rot = rotation % 180;
        if (assetName != "")
        {
            sprite = new SpriteSheet(assetName, rot, sheetIndex);
        }
        else
        {
            sprite = null;
        }
    }



    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (!visible || sprite == null)
        {
            return;
        }

        if (layer == 6)
            sprite.Draw(spriteBatch, this.GlobalPosition, origin, MathHelper.PiOver2);
        else if (layer == 100)
            sprite.Draw(spriteBatch, this.GlobalPosition, origin, -rot * MathHelper.Pi / 180);
        else
            sprite.Draw(spriteBatch, this.GlobalPosition - Camera.campos, origin, -rot * MathHelper.Pi / 180);
    }

    public SpriteSheet Sprite
    {
        get { return sprite; }
    }

    public Vector2 Center
    {
        get { return new Vector2(Width, Height) / 2; }
    }

    public int Width
    {
        get
        {
            return sprite.Width;
        }
    }

    public int Height
    {
        get
        {
            return sprite.Height;
        }
    }

    public bool Mirror
    {
        get { return sprite.Mirror; }
        set { sprite.Mirror = value; }
    }

    public Vector2 Origin
    {
        get { return origin; }
        set { origin = value; }
    }



}