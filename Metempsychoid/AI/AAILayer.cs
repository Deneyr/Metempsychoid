using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using Astrategia.Model.Event;
using Astrategia.View.Controls;
using Astrategia.View.Layer2D.BackgroundLayer2D;
using Astrategia.View.SoundsManager;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Astrategia.AI
{
    public abstract class AAILayer : AAIObject
    {
        protected WeakReference<AIWorld> worldAI;
        protected ALayer parentLayer;

        protected Dictionary<AEntity, AAIEntity> objectToObjectAIs;
        protected Dictionary<AAIEntity, AEntity> objectAIToObjects;

        public Queue<GameEventContainer> pendingGameEvent;
        public Time gameEventTimer;
        public Time gameEventPeriod;

        public AAILayer(AIWorld world2D, IAIObjectFactory layerFactory, ALayer layer)
            : base(layerFactory)
        {
            this.parentFactory = layerFactory;

            this.objectToObjectAIs = new Dictionary<AEntity, AAIEntity>();
            this.objectAIToObjects = new Dictionary<AAIEntity, AEntity>();

            //this.focusedEntity2Ds = new HashSet<IHitRect>();

            this.worldAI = new WeakReference<AIWorld>(world2D);

            this.pendingGameEvent = new Queue<GameEventContainer>();
            this.gameEventPeriod = Time.FromSeconds(2);

            this.parentLayer = layer;
            this.parentLayer.EntityAdded += OnEntityAdded;
            this.parentLayer.EntityRemoved += OnEntityRemoved;

            this.parentLayer.PositionChanged += OnPositionChanged;
            this.parentLayer.RotationChanged += OnRotationChanged;

            this.parentLayer.EntityPropertyChanged += OnEntityPropertyChanged;

            this.parentLayer.LevelStateChanged += OnLevelStateChanged;
        }

        public AAIEntity GetAIEntityFromEntity(AEntity entity)
        {
            return this.objectToObjectAIs[entity];
        }

        public AEntity GetEntityFromAIEntity(AAIEntity entity2D)
        {
            return this.objectAIToObjects[entity2D];
        }

        public virtual void InitializeLayer(IAIObjectFactory factory)
        {
            foreach (AEntity entity in this.parentLayer.Entities)
            {
                this.AddEntity(entity);
            }

            this.pendingGameEvent.Clear();
            this.gameEventTimer = Time.Zero;
        }

        public override void UpdateAI(Time deltaTime)
        {
            if (this.pendingGameEvent.Any())
            {
                this.gameEventTimer += deltaTime;
                if (this.gameEventTimer >= this.gameEventPeriod)
                {
                    GameEventContainer gameEventToSend = this.pendingGameEvent.Dequeue();

                    this.SendEventToWorld(gameEventToSend.Type, gameEventToSend.Entity, gameEventToSend.Details);

                    this.gameEventTimer = Time.Zero;
                }
            }
        }

        private void OnRotationChanged(float rotation)
        {
            this.Rotation = rotation;
        }

        private void OnPositionChanged(Vector2f position)
        {
            this.Position = position;
        }

        protected virtual void OnEntityRemoved(AEntity obj)
        {
            if (this.objectToObjectAIs.TryGetValue(obj, out AAIEntity entity2DToRemove))
            {
                this.objectAIToObjects.Remove(entity2DToRemove);
                this.objectToObjectAIs.Remove(obj);

                entity2DToRemove.Dispose();
            }
        }

        protected virtual void OnEntityAdded(AEntity obj)
        {
            this.AddEntity(obj);
        }

        protected virtual AAIEntity AddEntity(AEntity obj)
        {
            if (this.worldAI.TryGetTarget(out AIWorld worldAI))
            {
                if (AIWorld.MappingObjectModelView.TryGetValue(obj.GetType(), out IAIObjectFactory objectFactory))
                {
                    AAIEntity objectAI = objectFactory.CreateObjectAI(worldAI, this, obj) as AAIEntity;

                    this.objectAIToObjects.Add(objectAI, obj);
                    this.objectToObjectAIs.Add(obj, objectAI);

                    return objectAI;
                }
            }
            return null;
        }

        protected virtual void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            AAIEntity entityAI;
            switch (propertyName)
            {
                case "Position":
                    if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                    {
                        entityAI.Position = obj.Position;
                    }
                    break;
                case "Rotation":
                    if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                    {
                        entityAI.Rotation = obj.Rotation;
                    }
                    break;
                case "IsActive":
                    if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                    {
                        entityAI.IsActive = obj.IsActive;
                    }
                    break;
            }
        }

        protected virtual void OnLevelStateChanged(string obj)
        {
            // To override
        }     

        internal void SendEventToWorld(Model.Event.EventType eventType, AEntity entityConcerned, string details)
        {
            if (this.worldAI.TryGetTarget(out AIWorld world))
            {
                world.SendEventToWorld(new GameEvent(eventType, this.parentLayer, entityConcerned, details));
            }
        }
        
        public virtual void FlushEntities()
        {
            foreach (IAIObject objectAI in this.objectToObjectAIs.Values)
            {
                objectAI.Dispose();
            }
            this.objectAIToObjects.Clear();
            this.objectToObjectAIs.Clear();
        }

        public override void Dispose()
        {
            this.FlushEntities();

            this.parentLayer.EntityAdded -= OnEntityAdded;
            this.parentLayer.EntityRemoved -= OnEntityRemoved;

            this.parentLayer.PositionChanged -= OnPositionChanged;
            this.parentLayer.RotationChanged -= OnRotationChanged;

            this.parentLayer.EntityPropertyChanged -= OnEntityPropertyChanged;

            this.parentLayer.LevelStateChanged -= OnLevelStateChanged;

            base.Dispose();
        }
    }
}
