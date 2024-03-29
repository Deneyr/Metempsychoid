﻿using Astrategia.Model.Layer.EntityLayer;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model
{
    public class AEntity: AObject
    {
        private Vector2f position;

        private float rotation;

        private bool isActive;

        protected WeakReference<EntityLayer> parentLayer;

        public EntityLayer ParentLayer
        {
            get
            {
                if(this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
                {
                    return entityLayer;
                }
                return null;
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
                if(value != this.position)
                {
                    this.position = value;

                    if(this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "Position");
                    }
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

                    if (this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "Rotation");
                    }
                }
            }
        }

        public virtual bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                if (value != this.isActive)
                {
                    this.isActive = value;

                    if (this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "IsActive");
                    }
                }
            }
        }

        public AEntity(EntityLayer entityLayer)
        {
            this.parentLayer = new WeakReference<EntityLayer>(entityLayer);

            this.isActive = true;
        }
    }
}
