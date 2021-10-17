using Metempsychoid.Model;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BackgroundLayer;
using Metempsychoid.Model.Layer.BoardBannerLayer;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardNotifLayer;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using Metempsychoid.Model.Layer.EntityLayer;
using Metempsychoid.Model.Layer.MenuTextLayer;
using Metempsychoid.Model.MenuLayer;
using Metempsychoid.View.Card2D;
using Metempsychoid.View.Controls;
using Metempsychoid.View.Layer2D.BackgroundLayer2D;
using Metempsychoid.View.Layer2D.BoardBannerLayer2D;
using Metempsychoid.View.Layer2D.BoardGameLayer2D;
using Metempsychoid.View.Layer2D.BoardNotifLayer2D;
using Metempsychoid.View.Layer2D.BoardPlayerLayer2D;
using Metempsychoid.View.Layer2D.EntityLayer2D;
using Metempsychoid.View.Layer2D.MenuLayer2D;
using Metempsychoid.View.Layer2D.MenuTextLayer2D;
using Metempsychoid.View.ResourcesManager;
using Metempsychoid.View.SoundsManager;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View
{
    public class World2D: IDisposable
    {
        public static readonly Dictionary<Type, IObject2DFactory> MappingObjectModelView;

        public static readonly TextureManager TextureManager;
        public static readonly SoundManager SoundManager;

        private LayerResourcesLoader layerResourcesLoader;
        private LayerSoundsLoader layerSoundsLoader;

        private World world;

        static World2D()
        {
            TextureManager = new TextureManager();
            SoundManager = new SoundManager();

            MappingObjectModelView = new Dictionary<Type, IObject2DFactory>();

            // Layer menu mapping
            MappingObjectModelView.Add(typeof(CJMenuLayer), new CJMenuLayer2DFactory());

            // Layer background mapping
            MappingObjectModelView.Add(typeof(BackgroundLayer), new BackgroundLayer2DFactory("skyBackground"));
            MappingObjectModelView.Add(typeof(ImageBackgroundLayer), new ImageBackgroundLayer2DFactory());

            MappingObjectModelView.Add(typeof(MenuTextLayer), new MenuTextLayer2DFactory());

            // Layer entity Mapping
            MappingObjectModelView.Add(typeof(EntityLayer), new EntityLayer2DFactory());

            MappingObjectModelView.Add(typeof(BoardGameLayer), new BoardGameLayer2DFactory());

            MappingObjectModelView.Add(typeof(BoardPlayerLayer), new BoardPlayerLayer2DFactory());
            MappingObjectModelView.Add(typeof(AvatarBoardPlayerLayer), new BoardPlayerLayer2DFactory());
            MappingObjectModelView.Add(typeof(OppBoardPlayerLayer), new OppBoardPlayerLayer2DFactory());
            MappingObjectModelView.Add(typeof(MenuBoardPlayerLayer), new MenuBoardPlayerLayer2DFactory());

            MappingObjectModelView.Add(typeof(BoardNotifLayer), new BoardNotifLayer2DFactory());
            MappingObjectModelView.Add(typeof(MenuBoardNotifLayer), new MenuBoardNotifLayer2DFactory());

            // Layer foreground mapping
            MappingObjectModelView.Add(typeof(BoardBannerLayer), new BoardBannerLayer2DFactory());

            // Star entities
            MappingObjectModelView.Add(typeof(StarEntity), new StarEntity2DFactory());
            MappingObjectModelView.Add(typeof(StarLinkEntity), new StarLinkEntity2DFactory());
            MappingObjectModelView.Add(typeof(CurvedStarLinkEntity), new CurvedStarLinkEntity2DFactory());

            MappingObjectModelView.Add(typeof(CJStarDomain), new CJStarDomain2DFactory());

            // Card entities
            MappingObjectModelView.Add(typeof(CardEntity), new CardEntity2DFactory());

            MappingObjectModelView.Add(typeof(CardEntityDecorator), new CardEntityDecorator2DFactory());
            MappingObjectModelView.Add(typeof(CardEntityAwakenedDecorator), new CardEntityAwakenedDecorator2DFactory());

            MappingObjectModelView.Add(typeof(ToolTipEntity), new CardToolTip2DFactory());

            // Test Entity mapping
            MappingObjectModelView.Add(typeof(T_TeleEntity), new T_TeleEntity2DFactory());

            //foreach (IObject2DFactory factory in MappingObjectModelView.Values)
            //{
            //    TextureManager.TextureLoaded += factory.OnTextureLoaded;
            //    TextureManager.TextureUnloaded += factory.OnTextureUnloaded;

            //    SoundManager.SoundLoaded += factory.OnSoundLoaded;
            //    SoundManager.SoundUnloaded += factory.OnSoundUnloaded;
            //}
        }

        public  Dictionary<ALayer, ALayer2D> LayersDictionary
        {
            get;
            private set;
        }

        public ControlManager ControlManager
        {
            get;
            private set;
        }

        public List<ALayer2D> LayersList
        {
            get;
            private set;
        }

        public World2D(MainWindow mainWindow)
        {
            this.LayersDictionary = new Dictionary<ALayer, ALayer2D>();
            this.LayersList = new List<ALayer2D>();

            this.layerResourcesLoader = new LayerResourcesLoader();
            this.layerSoundsLoader = new LayerSoundsLoader();

            this.world = mainWindow.World;

            this.world.LayerAdded += OnLayerAdded;
            this.world.LayerRemoved += OnLayerRemoved;

            this.world.LevelStarting += OnLevelStarting;
            this.world.LevelEnding += OnLevelEnding;

            this.world.WorldStarting += OnWorldStarting;
            this.world.WorldEnding += OnWorldEnding;

            this.ControlManager = new ControlManager(mainWindow.Window);
            this.ControlManager.ControlActivated += OnControlActivated;
            this.ControlManager.MouseMoved += OnMouseMoved;
        }

        public void DrawIn(RenderWindow window, Time deltaTime)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            foreach (ALayer2D layer2D in this.LayersList)
            {
                layer2D.DrawIn(window, deltaTime);
            }

            //sw.Stop();

            //Console.WriteLine("time consume = " + sw.Elapsed);
        }

        public void OnControlActivated(ControlEventType eventType, string details)
        {
            int nbLayer2D = this.LayersList.Count;
            bool continueToForwardEvent = true;

            while(nbLayer2D > 0)
            {
                continueToForwardEvent &= this.LayersList[nbLayer2D - 1].OnControlActivated(eventType, details, continueToForwardEvent);

                nbLayer2D--;
            }
            
        }

        private void OnMouseMoved(Vector2i newPosition, Vector2i deltaPosition)
        {
            foreach(ALayer2D layer2D in this.LayersList)
            {
                layer2D.OnMouseMoved(newPosition, deltaPosition);
            }
        }

        private void OnLayerAdded(ALayer layerToAdd)
        {
            this.layerResourcesLoader.LoadLayerResources(layerToAdd);
            this.layerSoundsLoader.LoadLayerSounds(layerToAdd);

            IObject2DFactory layer2DFactory = World2D.MappingObjectModelView[layerToAdd.GetType()];

            ALayer2D layer2D = layer2DFactory.CreateObject2D(this, layerToAdd) as ALayer2D;

            this.LayersDictionary.Add(layerToAdd, layer2D);
        }


        private void OnLayerRemoved(ALayer layerToRemove)
        {
            ALayer2D layer2D = this.LayersDictionary[layerToRemove];

            layer2D.Dispose();

            this.LayersDictionary.Remove(layerToRemove);
            this.LayersList.Remove(layer2D);

            this.layerResourcesLoader.UnloadLayerResources(layerToRemove);
            this.layerSoundsLoader.UnloadLayerSounds(layerToRemove);
        }

        private void OnLevelStarting(World world)
        {
            if (this.LayersList.Any())
            {
                throw new Exception("There is always some layers in the current list at the start of the level");
            }

            foreach (ALayer layer in world.CurrentLayers)
            {
                ALayer2D layer2D = this.LayersDictionary[layer];

                if (layer.ParentLayer != null)
                {
                    this.LayersDictionary[layer.ParentLayer].ChildrenLayer2D.Add(layer2D);
                }

                if (layer2D == null)
                {
                    throw new Exception("The model layer : " + layer + "does not have a associated layer2D at the start of the level");
                }

                this.LayersList.Add(layer2D);

                //layer2D.InitializeLayer(World2D.MappingObjectModelView[layer.GetType()]);
            }

            foreach (ALayer layer in world.CurrentLayers)
            {
                ALayer2D layer2D = this.LayersDictionary[layer];
                layer2D.InitializeLayer(World2D.MappingObjectModelView[layer.GetType()]);
            }
        }

        private void OnLevelEnding(World world)
        {
            foreach(ALayer2D layer in this.LayersList)
            {
                layer.FlushEntities();
            }

            this.LayersList.Clear();
        }

        private void OnWorldStarting(World world)
        {
            TextureManager.UpdateTextures();
            SoundManager.UpdateSounds();
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

            this.ControlManager.ControlActivated -= OnControlActivated;
            this.ControlManager.MouseMoved -= OnMouseMoved;
        }

        public void SendEventToWorld(GameEvent gameEvent)
        {
            this.world.NotifyGameEvent(gameEvent);
        }
    }
}
