using Astrategia.Model;
using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View
{
    public interface IObject2DFactory
    {
        IObject2D CreateObject2D(World2D world2D, IObject obj);

        IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj);

        HashSet<string> TexturesPath
        {
            get;
        }

        HashSet<string> SoundsPath
        {
            get;
        }

        // Texture GetTextureByIndex(int index);
        Texture GetTextureById(string id);
        SoundBuffer GetSoundById(string id);
        string GetMusicPathById(string id);

        //void OnTextureLoaded(string path, Texture texture);
        //void OnTextureUnloaded(string path);

        //void OnSoundLoaded(string path, SoundBuffer texture);
        //void OnSoundUnloaded(string path);
    }
}
