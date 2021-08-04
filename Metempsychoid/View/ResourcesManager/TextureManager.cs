using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.ResourcesManager
{
    public class TextureManager
    {
        private Dictionary<string, Texture> texturesDictionary;

        public event Action<string, Texture> TextureLoaded;
        public event Action<string> TextureUnloaded;

        public HashSet<string> bufferPathsToLoad;
        public HashSet<string> bufferPathsToUnload;

        public TextureManager()
        {
            this.texturesDictionary = new Dictionary<string, Texture>();

            this.bufferPathsToLoad = new HashSet<string>();
            this.bufferPathsToUnload = new HashSet<string>();
        }

        public Texture GetTexture(string path)
        {
            return this.texturesDictionary[path];
        }

        public void AddTexturesToLoad(string texturePath)
        {
            if (this.bufferPathsToUnload.Contains(texturePath))
            {
                this.bufferPathsToUnload.Remove(texturePath);
            }
            else
            {
                if (this.texturesDictionary.ContainsKey(texturePath) == false)
                {
                    this.bufferPathsToLoad.Add(texturePath);
                }
            }
        }

        public void AddTexturesToUnload(string texturePath)
        {
            if (this.texturesDictionary.ContainsKey(texturePath))
            {
                this.bufferPathsToUnload.Add(texturePath);
            }
        }

        public void UpdateTextures()
        {
            this.UnloadTextures();
            this.bufferPathsToUnload.Clear();

            this.LoadTextures();
            this.bufferPathsToLoad.Clear();
        }

        private void LoadTextures()
        {
            foreach(string path in this.bufferPathsToLoad)
            {
                if (this.texturesDictionary.ContainsKey(path) == false)
                {
                    Texture texture = new Texture(path);
                    texture.Smooth = true;

                    this.texturesDictionary.Add(path, texture);

                    this.NotifyTextureLoaded(path, texture);
                }
            }
        }

        private void UnloadTextures()
        {
            foreach (string path in this.bufferPathsToUnload)
            {
                if (this.texturesDictionary.ContainsKey(path))
                {
                    this.texturesDictionary[path].Dispose();

                    this.texturesDictionary.Remove(path);

                    this.NotifyTextureUnloaded(path);
                }
            }
        }

        private void NotifyTextureLoaded(string path, Texture texture)
        {
            if(this.TextureLoaded != null)
            {
                this.TextureLoaded(path, texture);
            }
        }

        private void NotifyTextureUnloaded(string path)
        {
            if (this.TextureUnloaded != null)
            {
                this.TextureUnloaded(path);
            }
        }
    }
}
