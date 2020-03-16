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

    public Map(int level) // loads level
    {
        textLines = new List<string>();
        streamreader = new StreamReader("Content/Levels/" + level + ".txt");
        string line = streamreader.ReadLine();
        int width = line.Length;

        while (line != null)
        {
            try
            {
                textLines.Add(line);
                line = streamreader.ReadLine();
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("The level is corrupted."); // error handling for when levels are built wrong (not all lines are equal in length)
                // current effect is that tiles out of range are not rendered
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("There is not enough memory to complete this operation."); // error handling for when the computer runs out of memory
                // this is just in case since our old levels are now offloaded when a new level is initialized, freeing up memory
            }
        }

        levelWidth.Add(70 * width);
        levelHeight.Add(70 * (textLines.Count));
            for (int y = 0; y < textLines.Count; ++y)
            {
                Task t1 = Task.Run(() => // streamlines the above instruction into a task
                {
                    string[] lines = line.Split(',');
                    switch (lines[0]) // retrieves the tile based on the given type in the level text file
                    {
                        case "Bark":
                            //platform.Add(new Platform())
                            break;
                    }
                });
                t1.Wait();
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

    }

    public void Clear() // Clears these lists to free up memory, happens when traversing levels
    {
    }
}



