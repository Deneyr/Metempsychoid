using Astrategia.Model;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.SoundsManager
{
    public class LayerSoundsLoader
    {
        private Dictionary<ALayer, HashSet<string>> layerToSoundsPaths;

        public LayerSoundsLoader()
        {
            this.layerToSoundsPaths = new Dictionary<ALayer, HashSet<string>>();
        }

        public void LoadLayerSounds(ALayer layer)
        {
            if (this.layerToSoundsPaths.ContainsKey(layer))
            {
                throw new Exception("Sound Manager : Try to load an already loaded chunk");
            }

            HashSet<string> soundsPath = new HashSet<string>();

            HashSet<Type> objectTypes = layer.TypesInChunk;

            IEnumerable<string> sounds = World2D.MappingObjectModelView[layer.GetType()].SoundsPath;
            foreach(string texturePath in sounds)
            {
                soundsPath.Add(texturePath);
            }

            foreach (Type type in objectTypes)
            {
                sounds = World2D.MappingObjectModelView[type].SoundsPath;
                foreach (string texturePath in sounds)
                {
                    soundsPath.Add(texturePath);
                }
            }

            this.layerToSoundsPaths.Add(layer, soundsPath);

            foreach(string soundPath in soundsPath)
            {
                World2D.SoundManager.AddSoundsToLoad(soundPath);
            }
        }

        public void UnloadLayerSounds(ALayer layer)
        {
            if (this.layerToSoundsPaths.ContainsKey(layer) == false)
            {
                throw new Exception("Sound Manager : Try to unload a not loaded chunk");
            }

            HashSet<string> pathsLayerToRemove = this.layerToSoundsPaths[layer];

            foreach (string soundPath in pathsLayerToRemove)
            {
                World2D.SoundManager.AddSoundsToLoad(soundPath);
            }
        }      
    }
}
