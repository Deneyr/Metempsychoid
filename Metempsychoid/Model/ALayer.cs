﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Metempsychoid.Model
{
    public abstract class ALayer : AObject
    {
        private Vector2f position;

        private float rotation;

        private bool raiseEntityEvents;

        protected Dictionary<string, AEntity> namesToEntity;

        public event Action<AEntity> EntityAdded;
        public event Action<AEntity> EntityRemoved;

        public event Action<Vector2f> PositionChanged;
        public event Action<float> RotationChanged;

        public event Action<AEntity, string> EntityPropertyChanged;

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

        public Dictionary<string, AEntity> NamesToEntity
        {
            get
            {
                return this.namesToEntity;
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

        public ALayer()
        {
            this.TypesInChunk = new HashSet<Type>();

            this.namesToEntity = new Dictionary<string, AEntity>();

            this.position = new Vector2f(0, 0);
            this.rotation = 0;

            this.raiseEntityEvents = true;

            this.ParentLayer = null;
        }

        public virtual void InitializeLayer(PlayerData playerData)
        {
            this.raiseEntityEvents = false;

            this.InternalInitializeLayer(playerData);

            this.raiseEntityEvents = true;
        }

        protected abstract void InternalInitializeLayer(PlayerData playerData);

        public override void UpdateLogic(World world, Time deltaTime)
        {
            foreach (AEntity entity in this.namesToEntity.Values)
            {
                entity.UpdateLogic(world, deltaTime);
            }
        }

        public void AddEntityToLayer(string entityName, AEntity entity)
        {
            this.namesToEntity.Add(entityName, entity);

            this.TypesInChunk.Add(entity.GetType());
        }

        public virtual void FlushLayer()
        {
            foreach (AEntity entity in this.namesToEntity.Values)
            {
                this.NotifyObjectRemoved(entity);
            }
        }

        protected void NotifyObjectAdded(AEntity obj)
        {
            this.EntityAdded?.Invoke(obj);
        }

        protected void NotifyObjectRemoved(AEntity obj)
        {
            this.EntityRemoved?.Invoke(obj);
        }

        protected void NotifyPositionChanged(Vector2f position)
        {
            this.PositionChanged?.Invoke(position);
        }

        protected void NotifyRotationChanged(float rotation)
        {
            this.RotationChanged?.Invoke(rotation);
        }

        public void NotifyObjectPropertyChanged(AEntity obj, string propertyName)
        {
            if (this.raiseEntityEvents)
            {
                this.EntityPropertyChanged?.Invoke(obj, propertyName);
            }
        }
    }
}