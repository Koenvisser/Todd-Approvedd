using Microsoft.Xna.Framework;
using OakHeart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Camera : GameObject
{
    //This is a very simple camera class so that the camera follows the player.
    Player player;
    public static Vector2 campos = Vector2.Zero;
    List<int> levelWidth, levelHeight;
    public Camera(Player p)
    {
        this.player = p;
        this.levelWidth = Map.levelWidth;
        this.levelHeight = Map.levelHeight;
    }

    public void camera(GameTime gameTime, int level) // updates the camera such that it follows the player
    {
        int x = level;
        campos = new Vector2(player.Position.X - OakHeart.Game1.Screen.X / 2, player.Position.Y - OakHeart.Game1.Screen.Y / 2);
        campos.X = MathHelper.Clamp(player.Position.X - OakHeart.Game1.Screen.X / 2, 0, levelWidth[x] - OakHeart.Game1.Screen.X + 600);
        campos.Y = MathHelper.Clamp(player.Position.Y - OakHeart.Game1.Screen.Y / 2, -10000, levelHeight[x] - OakHeart.Game1.Screen.Y);
    }
}

