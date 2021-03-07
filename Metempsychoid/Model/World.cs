using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BackgroundLayer;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.EntityLayer;
using Metempsychoid.Model.Node;
using Metempsychoid.Model.Node.TestWorld;
using SFML.System;

namespace Metempsychoid.Model
{
    public class World : IUpdatable, IDisposable
    {
        private Dictionary<string, ALayer> loadedLayers;

        private List<ALayer> currentLayers;

        private RootGameNode gameNode;

        // Events
        public event Action<ALayer> LayerAdded;
        public event Action<ALayer> LayerRemoved;

        public event Action<World> WorldStarting;
        public event Action<World> WorldEnding;

        public event Action<World> LevelStarting;
        public event Action<World> LevelEnding;

        public CardFactory CardLibrary
        {
            get;
            private set;
        }

        public Player.Player Player
        {
            get;
            private set;
        }

        public List<ALayer> CurrentLayers
        {
            get
            {
                return this.currentLayers;
            }
        }

        public Dictionary<string, ALayer> LoadedLayers
        {
            get
            {
                return this.loadedLayers;
            }
        }

        public World()
        {
            this.currentLayers = new List<ALayer>();

            this.loadedLayers = new Dictionary<string, ALayer>();

            this.Player = new Player.Player(SFML.Graphics.Color.Red);

            this.CardLibrary = new CardFactory();

            this.gameNode = new RootGameNode(this);
        }

        public void UpdateLogic(World world, Time deltaTime)
        {
            this.gameNode.UpdateLogic(this, deltaTime);

            foreach(ALayer layer in this.currentLayers)
            {
                layer.UpdateLogic(this, deltaTime);
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
                layer.InitializeLayer(this);
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

        public void NotifyGameEvent(GameEvent gameEvent)
        {
            this.gameNode.OnGameEvent(this, gameEvent);
        }

        public void NotifyInternalGameEvent(InternalGameEvent internalGameEvent)
        {
            this.gameNode.OnInternalGameEvent(this, internalGameEvent);
        }

        public void InitializeGameNode()
        {
            for (int i = 0; i < 30; i++)
            {
                this.Player.Deck.Cards.Add(this.CardLibrary.CreateCard("wheel", this.Player));
            }

            this.gameNode.VisitStart(this);
        }
    }
}
