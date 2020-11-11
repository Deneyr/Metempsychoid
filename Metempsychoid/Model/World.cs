using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BackgroundLayer;
using SFML.System;

namespace Metempsychoid.Model
{
    public class World : IUpdatable, IDisposable
    {
        private List<ALayer> layersList;

        // Events
        public event Action<ALayer> LayerAdded;

        public event Action<ALayer> LayerRemoved;

        public event Action LevelStarting;

        public event Action LevelEnding;

        public World()
        {
            this.layersList = new List<ALayer>();
        }

        public void UpdateLogic(World world, Time deltaTime)
        {
            
        }

        public void Dispose()
        {
            this.FlushLayers();
        }

        public void AddLayer(ALayer layer)
        {
            this.layersList.Add(layer);

            this.NotifyLayerAdded(layer);
        }


        public void FlushLayers()
        {
            foreach(ALayer layer in this.layersList)
            {
                this.NotifyLayerRemoved(layer);
            }
            this.layersList.Clear();
        }


        protected void NotifyLayerAdded(ALayer layer)
        {
            this.LayerAdded?.Invoke(layer);
        }

        protected void NotifyLayerRemoved(ALayer layer)
        {
            this.LayerRemoved?.Invoke(layer);
        }

        protected void NotifyLevelStarting()
        {
            this.LevelStarting?.Invoke();
        }

        protected void NotifyLevelEnding()
        {
            this.LevelEnding?.Invoke();
        }

        // Test
        public void TestLevel()
        {
            BackgroundLayer background = new BackgroundLayer("VsO7nJK");

            this.AddLayer(background);

            this.NotifyLevelStarting();
        }
    }
}
