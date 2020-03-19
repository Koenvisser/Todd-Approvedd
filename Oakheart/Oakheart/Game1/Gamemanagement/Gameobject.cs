using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


public abstract class GameObject
//This code has been copied from ticktick and therefore has no special comments
{
    protected GameObject parent;
    public Vector2 position, velocity, previousposition;
    public List<float> dotlist;
    public List<Vector2> projlist;
    public bool triangle;
    public int lines;
    public float mass, minx, maxx, miny, maxy, min, max;
    protected int layer;
    protected string id;
    protected bool visible;


    public GameObject(int layer = 0, string id = "")
    {
        this.layer = layer;
        this.id = id;
        position = Vector2.Zero;
        velocity = Vector2.Zero;
        visible = true;

        dotlist = new List<float>();
        projlist = new List<Vector2>();
    }



    public virtual void HandleInput(InputHelper inputHelper)
    {
    }

    public virtual void Update(GameTime gameTime)
    {
        position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
    }

    public virtual void Reset()
    {
        visible = true;
    }

    public virtual Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    public virtual Vector2 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    public virtual Vector2 PreviousPosition
    {
        get { return previousposition; }
        set { previousposition = value; }
    }

    public virtual Vector2 GlobalPosition
    {
        get
        {
            if (parent != null)
            {
                return parent.GlobalPosition + Position;
            }
            else
            {
                return Position;
            }
        }
    }

    public GameObject Root
    {
        get
        {
            if (parent != null)
            {
                return parent.Root;
            }
            else
            {
                return this;
            }
        }
    }

    public virtual int Layer
    {
        get { return layer; }
        set { layer = value; }
    }

    public virtual GameObject Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public string Id
    {
        get { return id; }
    }

    public bool Visible
    {
        get { return visible; }
        set { visible = value; }
    }

    public virtual Rectangle BoundingBox
    {
        get
        {
            return new Rectangle((int)GlobalPosition.X, (int)GlobalPosition.Y, 0, 0);
        }
    }
}