using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Dragonfly : Enemy
{
    Vector2 start, end;
    enum Direction { Horizontal, Vertical, Diagonal}
    Direction direction;
    public Dragonfly(float rotation, Vector2 position, Vector2 endposition, int layer = 0, string id = "") : base (rotation, layer, id)
    {
        LoadAnimation("animations/Dragonfly@5x1", "Dragonfly", true);
        PlayAnimation("Dragonfly");
        this.position = position;
        start = position;
        end = endposition;
        left = true;

        velocity = (start - end)/100;
        if (velocity.X > 0)
        {
            Mirror = true;
            left = false;
        }

        if (velocity.X == 0)
        {
            direction = Direction.Vertical;
            start = new Vector2(position.X, Math.Min(position.Y, endposition.Y));
            end = new Vector2(position.X, Math.Min(position.Y, endposition.Y));

        }
        else if (velocity.Y == 0)
        {
            direction = Direction.Horizontal;
            start = new Vector2(Math.Min(position.X, endposition.X), position.Y);
            end = new Vector2(Math.Max(position.X, endposition.X), position.Y);
        }
        else
        {
            direction = Direction.Diagonal;
            if(position.X < endposition.X)
            {
                start = position;
                end = endposition;
            }
            else
            {
                start = endposition;
                end = position;
            }
           
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (direction == Direction.Horizontal)
        {
            if(position.X < start.X && left || position.X > end.X && !left)
            {
                velocity.X *= -1;
                TurnAround();
            }
        }
        if (direction == Direction.Vertical)
        {
            if(position.Y < start.Y || position.Y > end.Y)
            {
                velocity.Y *= -1;
            }
        }
        if (direction == Direction.Diagonal)
        {
            if(position .X < start.X && left || position.X > end.X && !left)
            {
                velocity *= -1;
                TurnAround();
            }
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);
    }
}
