using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.SoundsManager
{
    public class SoundManager
    {
        private Dictionary<string, SoundBuffer> soundsDictionary;

        public event Action<string, SoundBuffer> SoundLoaded;

        public event Action<string> SoundUnloaded;

        public HashSet<string> bufferPathsToLoad;
        public HashSet<string> bufferPathsToUnload;

        public SoundManager()
        {
            this.soundsDictionary = new Dictionary<string, SoundBuffer>();

            this.bufferPathsToLoad = new HashSet<string>();
            this.bufferPathsToUnload = new HashSet<string>();
        }

        public SoundBuffer GetSound(string path)
        {
            return this.soundsDictionary[path];
        }

        public void AddSoundsToLoad(string texturePath)
        {
            if (this.bufferPathsToUnload.Contains(texturePath))
            {
                this.bufferPathsToUnload.Remove(texturePath);
            }
            else
            {
                if (this.soundsDictionary.ContainsKey(texturePath) == false)
                {
                    this.bufferPathsToLoad.Add(texturePath);
                }
            }
        }

        public void AddSoundsToUnload(string texturePath)
        {
            if (this.soundsDictionary.ContainsKey(texturePath))
            {
                this.bufferPathsToUnload.Add(texturePath);
            }
        }

        public void UpdateSounds()
        {
            this.UnloadSounds();
            this.bufferPathsToUnload.Clear();

            this.LoadSounds();
            this.bufferPathsToLoad.Clear();
        }

        private void LoadSounds()
        {
            foreach(string path in this.bufferPathsToLoad)
            {
                if (this.soundsDictionary.ContainsKey(path) == false)
                {
                    SoundBuffer sound = new SoundBuffer(path);

                    this.soundsDictionary.Add(path, sound);

                    this.NotifySoundLoaded(path, sound);
                }
            }
        }

        private void UnloadSounds()
        {
            foreach (string path in this.bufferPathsToUnload)
            {
                if (this.soundsDictionary.ContainsKey(path))
                {
                    // Dispose or Destroy ???
                    this.soundsDictionary[path].Dispose();

                    this.soundsDictionary.Remove(path);

                    this.NotifySoundUnloaded(path);
                }
            }
        }

        private void NotifySoundLoaded(string path, SoundBuffer sound)
        {
            if(this.SoundLoaded != null)
            {
                this.SoundLoaded(path, sound);
            }
        }

        private void NotifySoundUnloaded(string path)
        {
            if (this.SoundUnloaded != null)
            {
                this.SoundUnloaded(path);
            }
        }
    }
}
