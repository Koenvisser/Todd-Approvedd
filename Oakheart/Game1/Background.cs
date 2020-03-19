using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Background
{
    Texture2D background;
    
    //oude trinity code
    /*
 Vector2 startPosition;

 Texture2D sky, hill1, hill2, cloud1, cloud2, mountain;
 Random random;

 Rectangle cloud;
 */
 public Background(ContentManager content, int level) // initializes the background
 {
        
     background = content.Load<Texture2D>("images/game/Level_" + level + "_Background");
     //oude trinity code
    /*startPosition = position;

     random = new Random();
     hill1 = content.Load<Texture2D>("Backgrounds/Hills_1");
     hill2 = content.Load<Texture2D>("Backgrounds/Hills_2");
     cloud1 = content.Load<Texture2D>("Backgrounds/Cloud_1");
     cloud2 = content.Load<Texture2D>("Backgrounds/Cloud_2");

     for (int i = 0; i < 6; i++)
     {
         mountain = content.Load<Texture2D>("Backgrounds/Mountain_" + (random.Next(4) + 1));
     }

     cloud = new Rectangle(0, 256, 0, 256);*/
}



public void Draw(GameTime gameTime, SpriteBatch spriteBatch) // draws the background
    {
        spriteBatch.Draw(background, new Rectangle(0 - (int)Camera.campos.X, 0 - (int)Camera.campos.Y, 3000, 3000), Color.White);
        //oude trinity code
       /*
        spriteBatch.Draw(sky, position, Color.White);

        /// drawing random Mountains
        spriteBatch.Draw(cloud1, cloud, Color.White);

        /// drawing random clouds
        for (int i = 0; i < 6; i++)
        {
            spriteBatch.Draw(mountain, position, Color.White);
        }

        ///draw hills
        spriteBatch.Draw(hill2, position, Color.White);
        spriteBatch.Draw(hill1, position, Color.White);
        */
    }
}

