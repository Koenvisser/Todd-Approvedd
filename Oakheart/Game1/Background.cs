using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Background : SpriteGameObject
{
   
    
    //oude trinity code
    /*
 Vector2 startPosition;

 Texture2D sky, hill1, hill2, cloud1, cloud2, mountain;
 Random random;

 Rectangle cloud;
 */
 public Background(ContentManager content, Vector2 position, string assetName) : base(0.0f, assetName, 100) // initializes the background
 {

     //oude trinity code
    /*startPosition = position;

     random = new Random();
     sky = content.Load<Texture2D>("Backgrounds/sky");
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



public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) // draws the background
    {
        
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

