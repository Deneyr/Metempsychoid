using Metempsychoid.Model;
using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View
{
    public interface IObject2DFactory
    {
        IObject2D CreateObject2D(World2D world2D, IObject obj);

        IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj);

        Dictionary<string, Texture> Resources
        {
            get;
        }

        Dictionary<string, SoundBuffer> Sounds
        {
            get;
        }

        Texture GetTextureByIndex(int index);
        SoundBuffer GetSoundById(string id);

        void OnTextureLoaded(string path, Texture texture);
        void OnTextureUnloaded(string path);

        void OnSoundLoaded(string path, SoundBuffer texture);
        void OnSoundUnloaded(string path);
    }
}
