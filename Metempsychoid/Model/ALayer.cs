﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model.Event;
using Astrategia.Model.Node;
using SFML.System;

namespace Astrategia.Model
{
    public abstract class ALayer : AObject
    {
        private Vector2f position;

        private float rotation;

        private bool raiseEntityEvents;

        protected ALevelNode ownerLevelNode;

        protected HashSet<AEntity> entities;

        public event Action<AEntity> EntityAdded;
        public event Action<AEntity> EntityRemoved;

        public event Action<Vector2f> PositionChanged;
        public event Action<float> RotationChanged;

        public event Action<AEntity, string> EntityPropertyChanged;

        public event Action<string> LevelStateChanged; 

        public ALayer ParentLayer
        {
            get;
            set;
        }

        public HashSet<Type> TypesInChunk
        {
            get;
            protected set;
        }

        public HashSet<AEntity> Entities
        {
            get
            {
                return this.entities;
            }
        }

        public override Vector2f Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (value != this.position)
                {
                    this.position = value;

                    this.NotifyPositionChanged(this.position);
                }
            }
        }

        public override float Rotation
        {
            get
            {
                return this.rotation;
            }
            set
            {
                if (value != this.rotation)
                {
                    this.rotation = value;

                    this.NotifyRotationChanged(this.rotation);
                }
            }
        }

        public virtual Vector2f? StartingArea
        {
            get;
            set;
        }

        public ALayer()
        {
            this.TypesInChunk = new HashSet<Type>();

            this.entities = new HashSet<AEntity>();

            this.position = new Vector2f(0, 0);
            this.rotation = 0;

            this.StartingArea = null;

            this.ownerLevelNode = null;

            this.raiseEntityEvents = true;

            this.ParentLayer = null;
        }

        public virtual void InitializeLayer(World world, ALevelNode levelNode)
        {
            this.ownerLevelNode = levelNode;

            this.raiseEntityEvents = false;

            this.InternalInitializeLayer(world, levelNode);

            this.raiseEntityEvents = true;
        }

        protected abstract void InternalInitializeLayer(World world, ALevelNode levelNode);

        public override void UpdateLogic(World world, Time deltaTime)
        {
            foreach (AEntity entity in this.Entities)
            {
                entity.UpdateLogic(world, deltaTime);
            }
        }

        public void AddEntityToLayer(AEntity entity)
        {
            this.entities.Add(entity);

            this.NotifyObjectAdded(entity);
        }

        public virtual void RemoveEntityFromLayer(AEntity entity)
        {
            //AObject.animationManager.StopAnimation(entity);

            this.entities.Remove(entity);

            this.NotifyObjectRemoved(entity);

            entity.Dispose();
        }

        public virtual void FlushLayer()
        {
            this.ownerLevelNode = null;

            foreach (AEntity entity in this.entities)
            {
                entity.Dispose();

                this.NotifyObjectRemoved(entity);
            }

            this.entities.Clear();
        }

        public override void Dispose()
        {
            this.FlushLayer();

            base.Dispose();
        }

        protected void NotifyObjectAdded(AEntity obj)
        {
            if (this.raiseEntityEvents)
            {
                this.EntityAdded?.Invoke(obj);
            }
        }

        protected void NotifyObjectRemoved(AEntity obj)
        {
            if (this.raiseEntityEvents)
            {
                this.EntityRemoved?.Invoke(obj);
            }
        }

        protected void NotifyPositionChanged(Vector2f position)
        {
            this.PositionChanged?.Invoke(position);
        }

        protected void NotifyRotationChanged(float rotation)
        {
            this.RotationChanged?.Invoke(rotation);
        }

        public virtual void NotifyObjectBeforePropertyChanged(AEntity obj, string propertyName)
        {
            
        }

        public virtual void NotifyObjectPropertyChanged(AEntity obj, string propertyName)
        {
            if (this.raiseEntityEvents)
            {
                this.EntityPropertyChanged?.Invoke(obj, propertyName);
            }
        }

        public void NotifyLevelStateChanged(string state)
        {
            if (this.raiseEntityEvents)
            {
                this.LevelStateChanged?.Invoke(state);
            }
        }
    }
}
