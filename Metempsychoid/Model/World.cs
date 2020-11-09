using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            
        }
    }
}
