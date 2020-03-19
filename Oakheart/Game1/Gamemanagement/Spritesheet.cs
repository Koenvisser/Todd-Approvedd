using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OakHeart;

public class SpriteSheet
{
    protected Texture2D sprite;
    protected int sheetIndex;
    protected int sheetColumns;
    protected int sheetRows;
    protected bool mirror;

    public SpriteSheet(string assetname, float rotation, int sheetIndex = 0)
    {
        // retrieve the sprite
        sprite = OakHeart.Game1.AssetManager.GetSprite(assetname);

        // construct the collision mask
        Color[] colorData = new Color[sprite.Width * sprite.Height];
        sprite.GetData(colorData);
        for (int i = 0; i < colorData.Length; ++i)
        {
        }

        this.sheetIndex = sheetIndex;
        sheetColumns = 1;
        sheetRows = 1;

        // see if we can extract the number of sheet elements from the assetname
        string[] assetSplit = assetname.Split('@');
        if (assetSplit.Length <= 1)
        {
            return;
        }

        string sheetNrData = assetSplit[assetSplit.Length - 1];
        string[] colRow = sheetNrData.Split('x');
        sheetColumns = int.Parse(colRow[0]);
        if (colRow.Length == 2)
        {
            sheetRows = int.Parse(colRow[1]);
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float rotation)
    {
        int columnIndex = sheetIndex % sheetColumns;
        int rowIndex = sheetIndex / sheetColumns % sheetRows;
        Rectangle spritePart = new Rectangle(columnIndex * Width, rowIndex * Height, Width, Height);
        SpriteEffects spriteEffects = SpriteEffects.None;
        if (mirror)
        {
            spriteEffects = SpriteEffects.FlipHorizontally;
        }
        spriteBatch.Draw(sprite, position, spritePart, Color.White,
            rotation, origin, 1.0f, spriteEffects, 0.0f);
    }

    public Texture2D Sprite
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
        { return sprite.Width / sheetColumns; }
    }

    public int Height
    {
        get
        { return sprite.Height / sheetRows; }
    }

    public bool Mirror
    {
        get { return mirror; }
        set { mirror = value; }
    }

    public int SheetIndex
    {
        get
        { return sheetIndex; }
        set
        {
            if (value < sheetColumns * sheetRows && value >= 0)
            {
                sheetIndex = value;
            }
        }
    }

    public int NumberSheetElements
    {
        get { return sheetColumns * sheetRows; }
    }

    public Color getPixelColor(int x, int y)
    {
        Rectangle sourceRectangle = new Rectangle(x, y, 1, 1);
        Color[] retrievedColor = new Color[1];
        sprite.GetData<Color>(0, sourceRectangle, retrievedColor, 0, 1);
        return retrievedColor[0];
    }
}