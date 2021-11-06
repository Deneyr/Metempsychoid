using Astrategia.Model;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.ResourcesManager
{
    public class LayerResourcesLoader
    {
        private Dictionary<ALayer, HashSet<string>> layerToTexturePaths;

        public LayerResourcesLoader()
        {
            this.layerToTexturePaths = new Dictionary<ALayer, HashSet<string>>();
        }

        public void LoadLayerResources(ALayer layer)
        {
            if (this.layerToTexturePaths.ContainsKey(layer))
            {
                throw new Exception("Texture Manager : Try to load an already loaded chunk");
            }

            HashSet<string> resourcesPath = new HashSet<string>();

            HashSet<Type> objectTypes = layer.TypesInChunk;

            IEnumerable<string> resources = World2D.MappingObjectModelView[layer.GetType()].TexturesPath;
            foreach(string texturePath in resources)
            {
                resourcesPath.Add(texturePath);
            }

            foreach (Type type in objectTypes)
            {
                resources = World2D.MappingObjectModelView[type].TexturesPath;
                foreach (string texturePath in resources)
                {
                    resourcesPath.Add(texturePath);
                }
            }

            this.layerToTexturePaths.Add(layer, resourcesPath);

            foreach(string texturePath in resourcesPath)
            {
                World2D.TextureManager.AddTexturesToLoad(texturePath);
            }
        }

        public void UnloadLayerResources(ALayer layer)
        {
            if (this.layerToTexturePaths.ContainsKey(layer) == false)
            {
                throw new Exception("Texture Manager : Try to unload a not loaded chunk");
            }

            HashSet<string> pathsLayerToRemove = this.layerToTexturePaths[layer];

            foreach (string texturePath in pathsLayerToRemove)
            {
                World2D.TextureManager.AddTexturesToUnload(texturePath);
            }
        }      
    }
}
