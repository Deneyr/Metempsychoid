using Astrategia.AI.AICard;
using Astrategia.AI.AIBoardGameLayer;
using Astrategia.Model;
using Astrategia.Model.Card;
using Astrategia.Model.Event;
using Astrategia.Model.Layer.BoardGameLayer;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model.Layer.BoardPlayerLayer;
using Astrategia.AI.AIBoardPlayerLayer;
using Astrategia.Model.Layer.BoardNotifLayer;
using Astrategia.AI.AIBoardNotifLayer;

namespace Astrategia.AI
{
    public class AIWorld : IDisposable
    {
        public static readonly Dictionary<Type, IAIObjectFactory> MappingObjectModelAI;

        private World world;

        static AIWorld()
        {
            MappingObjectModelAI = new Dictionary<Type, IAIObjectFactory>();

            // Layer menu mapping
            //MappingObjectModelView.Add(typeof(CJMenuLayer), new CJMenuLayer2DFactory());

            //// Layer background mapping
            //MappingObjectModelView.Add(typeof(BackgroundLayer), new BackgroundLayer2DFactory("skyBackground"));
            //MappingObjectModelView.Add(typeof(ImageBackgroundLayer), new ImageBackgroundLayer2DFactory());

            //MappingObjectModelView.Add(typeof(MenuTextLayer), new MenuTextLayer2DFactory());

            // Layer entity Mapping
            //MappingObjectModelView.Add(typeof(EntityLayer), new EntityLayer2DFactory());

            MappingObjectModelAI.Add(typeof(BoardGameLayer), new AIBoardGameLayerFactory());

            MappingObjectModelAI.Add(typeof(BoardPlayerLayer), new AIBoardPlayerLayerFactory());
            //MappingObjectModelView.Add(typeof(MenuBoardPlayerLayer), new MenuBoardPlayerLayer2DFactory());

            MappingObjectModelAI.Add(typeof(BoardNotifLayer), new AIBoardNotifLayerFactory());
            //MappingObjectModelView.Add(typeof(MenuBoardNotifLayer), new MenuBoardNotifLayer2DFactory());

            //// Layer foreground mapping
            //MappingObjectModelView.Add(typeof(BoardBannerLayer), new BoardBannerLayer2DFactory());

            // Star entities
            MappingObjectModelAI.Add(typeof(StarEntity), new AIStarEntityFactory());
            MappingObjectModelAI.Add(typeof(StarLinkEntity), new AIStarLinkEntityFactory());
            MappingObjectModelAI.Add(typeof(CurvedStarLinkEntity), new AIStarLinkEntityFactory());

            MappingObjectModelAI.Add(typeof(CJStarDomain), new AICJStarDomainFactory());

            // Card entities
            MappingObjectModelAI.Add(typeof(CardEntity), new AICardEntityFactory());

            MappingObjectModelAI.Add(typeof(CardEntityDecorator), new AICardEntityFactory());
            MappingObjectModelAI.Add(typeof(CardEntityAwakenedDecorator), new AICardEntityFactory());

            //MappingObjectModelView.Add(typeof(ToolTipEntity), new CardToolTip2DFactory());

            //// Test Entity mapping
            //MappingObjectModelView.Add(typeof(T_TeleEntity), new T_TeleEntity2DFactory());

            //foreach (IObject2DFactory factory in MappingObjectModelView.Values)
            //{
            //    TextureManager.TextureLoaded += factory.OnTextureLoaded;
            //    TextureManager.TextureUnloaded += factory.OnTextureUnloaded;

            //    SoundManager.SoundLoaded += factory.OnSoundLoaded;
            //    SoundManager.SoundUnloaded += factory.OnSoundUnloaded;
            //}
        }

        public Dictionary<ALayer, AAILayer> LayersDictionary
        {
            get;
            private set;
        }

        public List<AAILayer> LayersList
        {
            get;
            private set;
        }

        public AIWorld(MainWindow mainWindow)
        {
            this.LayersDictionary = new Dictionary<ALayer, AAILayer>();
            this.LayersList = new List<AAILayer>();

            this.world = mainWindow.World;

            this.world.LayerAdded += OnLayerAdded;
            this.world.LayerRemoved += OnLayerRemoved;

            this.world.LevelStarting += OnLevelStarting;
            this.world.LevelEnding += OnLevelEnding;

            this.world.WorldStarting += OnWorldStarting;
            this.world.WorldEnding += OnWorldEnding;
        }

        public void UpdateAI(Time deltaTime)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            foreach (AAILayer layerAI in this.LayersList)
            {
                layerAI.UpdateAI(deltaTime);
            }

            //sw.Stop();

            //Console.WriteLine("time consume = " + sw.Elapsed);
        }

        private void OnLayerAdded(ALayer layerToAdd)
        {
            if (AIWorld.MappingObjectModelAI.TryGetValue(layerToAdd.GetType(), out IAIObjectFactory layer2DFactory))
            {
                AAILayer layerAI = layer2DFactory.CreateObjectAI(this, layerToAdd) as AAILayer;

                this.LayersDictionary.Add(layerToAdd, layerAI);
            }
        }


        private void OnLayerRemoved(ALayer layerToRemove)
        {
            if (this.LayersDictionary.TryGetValue(layerToRemove, out AAILayer layer))
            {
                layer.Dispose();

                this.LayersDictionary.Remove(layerToRemove);
                this.LayersList.Remove(layer);
            }
        }

        private void OnLevelStarting(World world)
        {
            if (this.LayersList.Any())
            {
                throw new Exception("There is always some layers in the current list at the start of the level");
            }

            foreach (ALayer layer in world.CurrentLayers)
            {
                if (this.LayersDictionary.TryGetValue(layer, out AAILayer layerAI))
                {
                    if (layerAI == null)
                    {
                        throw new Exception("The model layer : " + layer + "does not have a associated layer2D at the start of the level");
                    }

                    this.LayersList.Add(layerAI);
                }
            }

            foreach (ALayer layer in world.CurrentLayers)
            {
                if (this.LayersDictionary.TryGetValue(layer, out AAILayer layerAI))
                {
                    layerAI.InitializeLayer(AIWorld.MappingObjectModelAI[layer.GetType()]);
                }
            }
        }

        private void OnLevelEnding(World world)
        {
            foreach (AAILayer layer in this.LayersList)
            {
                layer.FlushEntities();
            }

            this.LayersList.Clear();
        }

        private void OnWorldStarting(World world)
        {

        }

        private void OnWorldEnding(World world)
        {
            if (this.LayersDictionary.Any())
            {
                throw new Exception("There is always some layer at the end of the world");
            }
        }

        public void Dispose()
        {
            this.world.LayerAdded -= OnLayerAdded;
            this.world.LayerRemoved -= OnLayerRemoved;

            this.world.LevelStarting -= OnLevelStarting;
            this.world.LevelEnding -= OnLevelEnding;

            this.world.WorldStarting -= OnWorldStarting;
            this.world.WorldEnding -= OnWorldEnding;

            this.world = null;
        }

        public void SendEventToWorld(GameEvent gameEvent)
        {
            this.world.NotifyGameEvent(gameEvent);
        }
    }
}
