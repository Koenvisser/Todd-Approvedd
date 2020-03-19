using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

public class Map
{
    StreamReader streamreader;
    public static List<int> levelWidth = new List<int>();
    public static List<int> levelHeight = new List<int>();
    List<string> textLines;
    List<Platform> platform = new List<Platform>();
    List<Enemy> enemy = new List<Enemy>();

    public List<Platform> Platform 
    {
        get { return platform; }
    }

    public List<Enemy> Enemy
    {
        get { return enemy; }
    }

    public Map(int level) // loads level
    {
        levelWidth.Add(0);
        levelHeight.Add(0);
        textLines = new List<string>();
        streamreader = new StreamReader("Content/Levels/" + level + ".txt");
        string line = streamreader.ReadLine();
        int width = line.Length;

        while (line != null)
        {
            string[] lines = line.Split(',');
            if (lines.Count() > 3)
            {
                if (int.Parse(lines[3]) > levelWidth[level - 1])
                {
                    levelWidth[level - 1] = int.Parse(lines[3]);
                }
            }
           
            if (lines.Count() > 4)
            {
                if (int.Parse(lines[4]) > levelHeight[level - 1])
                {
                    levelHeight[level - 1] = int.Parse(lines[4]) + 540;
                }
            }
          
            switch (lines[0]) // retrieves the tile based on the given type in the level text file
            {
                case "Bark":
                    platform.Add(new Platform(bool.Parse(lines[1]), float.Parse(lines[2]), "images/game/Bark", new Rectangle(int.Parse(lines[3]), int.Parse(lines[4]), int.Parse(lines[5]), int.Parse(lines[6]))));
                    break;
                case "Snail":
                    enemy.Add(new Snail(0, new Vector2(float.Parse(lines[1]), float.Parse(lines[2])), float.Parse(lines[3]), int.Parse(lines[4])));
                    break;
                case "Dragonfly":
                    enemy.Add(new Dragonfly(0, new Vector2(float.Parse(lines[1]), float.Parse(lines[2])), new Vector2(float.Parse(lines[3]), float.Parse(lines[4])), float.Parse(lines[5])));
                    break;
                case "Alfungus":
                    enemy.Add(new Alfungus(0, new Vector2(float.Parse(lines[1]), float.Parse(lines[2]))));
                    break;

            }
            line = streamreader.ReadLine();
            /* waits with performing the task again until the earlier instance is finished
                loads slower, but is less likely to cause lag when loading big levels */
        }
        

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) // draws the level
    {
        foreach (Platform platform in platform)
        {
            platform.Draw(gameTime, spriteBatch);
        }
        foreach (Enemy enemy in enemy)
        {
            enemy.Draw(gameTime, spriteBatch);
        }
    }

    public void Clear() // Clears these lists to free up memory, happens when traversing levels
    {
    }
}



