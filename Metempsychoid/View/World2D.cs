﻿using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BackgroundLayer;
using Metempsychoid.Model.Layer.EntityLayer;
using Metempsychoid.View.Controls;
using Metempsychoid.View.Layer2D.BackgroundLayer2D;
using Metempsychoid.View.Layer2D.EntityLayer2D;
using Metempsychoid.View.ResourcesManager;
using SFML.Graphics;
using SFML.System;
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

        static World2D()
        {
            TextureManager = new TextureManager();

            MappingObjectModelView = new Dictionary<Type, IObject2DFactory>();

            // Background mapping
            MappingObjectModelView.Add(typeof(BackgroundLayer), new BackgroundLayer2DFactory("VsO7nJK"));

            // Entity Mapping
            MappingObjectModelView.Add(typeof(EntityLayer), new EntityLayer2DFactory());

            MappingObjectModelView.Add(typeof(T_TeleEntity), new T_TeleEntity2DFactory());

            // Object mapping
            // MappingObjectModelView.Add(typeof(PlayerEntity), new PlayerEntity2DFactory());

            foreach (IObject2DFactory factory in MappingObjectModelView.Values)
            {
                TextureManager.TextureLoaded += factory.OnTextureLoaded;
                TextureManager.TextureUnloaded += factory.OnTextureUnloaded;
            }
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
            this.layersDictionary = new Dictionary<ALayer, ALayer2D>();
            this.LayersList = new List<ALayer2D>();

            this.layerResourcesLoader = new LayerResourcesLoader();

            mainWindow.World.LayerAdded += OnLayerAdded;
            mainWindow.World.LayerRemoved += OnLayerRemoved;

            mainWindow.World.LevelStarting += OnLevelStarting;
            mainWindow.World.LevelEnding += OnLevelEnding;

            mainWindow.World.WorldStarting += OnWorldStarting;
            mainWindow.World.WorldEnding += OnWorldEnding;

            this.ControlManager = new ControlManager(mainWindow.Window);
            this.ControlManager.ControlActivated += OnControlActivated;
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

            while(nbLayer2D > 0 && continueToForwardEvent)
            {
                continueToForwardEvent = this.LayersList[nbLayer2D - 1].OnControlActivated(eventType, details);

                nbLayer2D--;
            }
            
        }

        private void OnLayerAdded(ALayer layerToAdd)
        {
            this.layerResourcesLoader.LoadLayerResources(layerToAdd);

            IObject2DFactory layer2DFactory = World2D.MappingObjectModelView[layerToAdd.GetType()];

            ALayer2D layer2D = layer2DFactory.CreateObject2D(this, layerToAdd) as ALayer2D;

            this.layersDictionary.Add(layerToAdd, layer2D);
        }


        private void OnLayerRemoved(ALayer layerToRemove)
        {
            ALayer2D layer2D = this.layersDictionary[layerToRemove];

            layer2D.Dispose();

            this.layersDictionary.Remove(layerToRemove);
            this.LayersList.Remove(layer2D);
        }

        private void OnLevelStarting(World world)
        {
            if (this.LayersList.Any())
            {
                throw new Exception("There is always some layers in the current list at the start of the level");
            }

            foreach (ALayer layer in world.CurrentLayers)
            {
                ALayer2D layer2D = this.layersDictionary[layer];

                if(layer.ParentLayer != null)
                {
                    this.layersDictionary[layer.ParentLayer].ChildrenLayer2D.Add(layer2D);
                }

                if (layer2D == null)
                {
                    throw new Exception("The model layer : " + layer + "does not have a associated layer2D at the start of the level");
                }

                this.LayersList.Add(layer2D);

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
        }

        private void OnWorldEnding(World world)
        {
            if (this.layersDictionary.Any())
            {
                throw new Exception("There is always some layer at the end of the world");
            }
        }

        public void Dispose(MainWindow mainWindow)
        {
            mainWindow.World.LayerAdded -= OnLayerAdded;
            mainWindow.World.LayerRemoved -= OnLayerRemoved;

            mainWindow.World.LevelStarting -= OnLevelStarting;
            mainWindow.World.LevelEnding -= OnLevelEnding;

            mainWindow.World.WorldStarting -= OnWorldStarting;
            mainWindow.World.WorldEnding -= OnWorldEnding;

            this.ControlManager.ControlActivated -= OnControlActivated;
        }

        public void SetCanevas(IntRect newCanevas)
        {
            throw new NotImplementedException();
        }

        public void SetZoom(float newZoom)
        {
            throw new NotImplementedException();
        }
    }
}