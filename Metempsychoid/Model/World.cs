using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BackgroundLayer;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.EntityLayer;
using SFML.System;

namespace Metempsychoid.Model
{
    public class World : IUpdatable, IDisposable
    {
        private Dictionary<string, ALayer> loadedLayers;

        private List<ALayer> currentLayers;

        private PlayerData playerData;

        // Events
        public event Action<ALayer> LayerAdded;
        public event Action<ALayer> LayerRemoved;

        public event Action<World> WorldStarting;
        public event Action<World> WorldEnding;

        public event Action<World> LevelStarting;
        public event Action<World> LevelEnding;

        public List<ALayer> CurrentLayers
        {
            get
            {
                return this.currentLayers;
            }
        }

        public World()
        {
            this.currentLayers = new List<ALayer>();

            this.loadedLayers = new Dictionary<string, ALayer>();

            this.playerData = new PlayerData();
        }

        public void UpdateLogic(World world, Time deltaTime)
        {
            foreach(ALayer layer in this.currentLayers)
            {
                layer.UpdateLogic(world, deltaTime);
            }
        }

        public void Dispose()
        {
            this.EndWorld();
        }

        protected void NotifyLayerAdded(ALayer layer)
        {
            this.LayerAdded?.Invoke(layer);
        }

        protected void NotifyLayerRemoved(ALayer layer)
        {
            this.LayerRemoved?.Invoke(layer);
        }

        protected void NotifyWorldStarting()
        {
            this.WorldStarting?.Invoke(this);
        }

        protected void NotifyWorldEnding()
        {
            this.WorldEnding?.Invoke(this);
        }

        protected void NotifyLevelStarting()
        {
            this.LevelStarting?.Invoke(this);
        }

        protected void NotifyLevelEnding()
        {
            this.LevelEnding?.Invoke(this);
        }

        public void InitializeLevel(List<string> layersToAdd)
        {
            this.currentLayers.Clear();
            foreach(string layerName in layersToAdd)
            {
                if (this.loadedLayers.ContainsKey(layerName))
                {
                    this.currentLayers.Add(this.loadedLayers[layerName]);
                }
            }

            foreach(ALayer layer in this.currentLayers)
            {
                layer.InitializeLayer(this.playerData);
            }

            this.NotifyLevelStarting();
        }

        public void InitializeWorld(List<Tuple<string, ALayer>> layersToLoad)
        {
            foreach(Tuple<string, ALayer> tupleLayer in layersToLoad)
            {
                this.loadedLayers.Add(tupleLayer.Item1, tupleLayer.Item2);

                this.NotifyLayerAdded(tupleLayer.Item2);
            }

            this.NotifyWorldStarting();
        }

        public void EndLevel()
        {
            foreach (ALayer layer in this.currentLayers)
            {
                layer.FlushLayer();
            }

            this.currentLayers.Clear();

            this.NotifyLevelEnding();
        }

        public void EndWorld()
        {
            foreach (ALayer layer in this.currentLayers)
            { 
                this.NotifyLayerRemoved(layer);

                layer.Dispose();
            }
            this.currentLayers.Clear();
            this.loadedLayers.Clear();

            this.NotifyWorldEnding();
        }

        // Test
        public void TestLevel()
        {
            BackgroundLayer background = new BackgroundLayer();
            //EntityLayer entityLayer = new EntityLayer();
            BoardGameLayer boardGameLayer = new BoardGameLayer();

            boardGameLayer.ParentLayer = background;

            this.InitializeWorld(new List<Tuple<string, ALayer>>() {
                new Tuple<string, ALayer>("VsO7nJK", background),
                new Tuple<string, ALayer>("TestLayer", boardGameLayer)
            });

            this.InitializeLevel(new List<string>() { "VsO7nJK", "TestLayer" });
        }
    }
}
