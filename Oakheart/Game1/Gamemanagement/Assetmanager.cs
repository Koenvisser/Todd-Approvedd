using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

public class AssetManager
//This code has been copied from ticktick and therefore has no special comments
{
    protected ContentManager contentManager;

    public AssetManager(ContentManager content)
    {
        contentManager = content;
    }

    public Texture2D GetSprite(string assetName)
    {
        if (assetName == "")
        {
            return null;
        }
        return contentManager.Load<Texture2D>(assetName);
    }

    public void PlaySound(string assetName, bool mute)
    {
        SoundEffect snd = contentManager.Load<SoundEffect>("Music/" + assetName);

        if (!mute)
            snd.Play();
    }

    public void PlayMusic(string assetName, bool repeat = true)
    {
        MediaPlayer.IsRepeating = repeat;
        MediaPlayer.Play(contentManager.Load<Song>("Music/" + assetName));
    }

    public ContentManager Content => contentManager;
}