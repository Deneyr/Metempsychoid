using Metempsychoid.Model;
using Metempsychoid.View.ResourcesManager;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View
{
    public class World2D
    {
        public static readonly Dictionary<Type, IObject2DFactory> MappingObjectModelView;

        public static readonly TextureManager TextureManager;

        private LayerResourcesLoader layerResourcesLoader;

        private Dictionary<ALayer, ALayer2D> layersDictionary;
        private List<ALayer2D> layersList;

        static World2D()
        {
            TextureManager = new TextureManager();

            MappingObjectModelView = new Dictionary<Type, IObject2DFactory>();

            // Layer mapping
            //MappingObjectModelView.Add(typeof(GroundLandObject), new GroundObject2DFactory());

            // Object mapping
            // MappingObjectModelView.Add(typeof(PlayerEntity), new PlayerEntity2DFactory());

            foreach (IObject2DFactory factory in MappingObjectModelView.Values)
            {
                TextureManager.TextureLoaded += factory.OnTextureLoaded;
                TextureManager.TextureUnloaded += factory.OnTextureUnloaded;
            }
        }

        public World2D(World world)
        {
            this.layersDictionary = new Dictionary<ALayer, ALayer2D>();
            this.layersList = new List<ALayer2D>();

            this.layerResourcesLoader = new LayerResourcesLoader();

            world.LayerAdded += OnLayerAdded;
            world.LayerRemoved += OnLayerRemoved;

            world.LevelStarting += OnLevelStarting;
        }

        public void DrawIn(RenderWindow window)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            foreach (ALayer2D layer2D in this.layersList)
            {
                layer2D.DrawIn(window);
            }

            //sw.Stop();

            //Console.WriteLine("time consume = " + sw.Elapsed);
        }

        private void OnLayerAdded(ALayer layer)
        {
            this.layerResourcesLoader.LoadLayerResources(layer);

            IObject2DFactory layer2DFactory = World2D.MappingObjectModelView[layer.GetType()];

            ALayer2D layer2D = layer2DFactory.CreateObject2D(this, layer) as ALayer2D;

            this.layersDictionary.Add(layer, layer2D);
            this.layersList.Add(layer2D);
        }


        private void OnLayerRemoved(ALayer layer)
        {
            this.layerResourcesLoader.UnloadLayerResources(layer);

            ALayer2D layer2D = this.layersDictionary[layer];

            layer2D.Dispose();

            this.layersDictionary.Remove(layer);
            this.layersList.Remove(layer2D);
        }

        private void OnLevelStarting()
        {
            TextureManager.UpdateTextures();
        }

        public void Dispose(World world)
        {
            world.LayerAdded -= OnLayerAdded;
            world.LayerRemoved -= OnLayerRemoved;
        }
    }
}
