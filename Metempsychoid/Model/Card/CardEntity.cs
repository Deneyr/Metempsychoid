﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model.Constellations;
using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Layer.EntityLayer;
using SFML.System;

namespace Astrategia.Model.Card
{
    public class CardEntity : AEntity
    {
        private StarEntity parentStar;

        private bool isFliped;

        private bool isSelected;

        public event Action<string> PropertyChanged;

        public bool IsValid
        {
            get;
            private set;
        }

        public virtual Card Card
        {
            get;
            private set;
        }

        public bool AreCardBehaviorsActive
        {
            get
            {
                return this.Card.CardBehaviors.Any(pElem => pElem.IsActive);
            }
        }

        //public virtual Vector2f PositionInNotifBoard
        //{
        //    get;
        //    set;
        //}

        public virtual StarEntity ParentStar
        {
            get
            {
                return this.parentStar;
            }
            set
            {
                if (this.parentStar != value)
                {
                    this.parentStar = value;

                    if (this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "IsSocketed");
                    }
                }
            }
        }

        public virtual bool IsFliped
        {
            get
            {
                return this.isFliped;
            }
            set
            {
                if (this.isFliped != value)
                {
                    this.isFliped = value;

                    if (this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "IsFliped");
                    }
                }
            }
        }

        public virtual bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;

                    if (this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "IsSelected");
                    }
                }
            }
        }

        public virtual int CardValue
        {
            get
            {
                return this.Card.Value;
            }
        }

        public virtual int CardValueModifier
        {
            get
            {
                return this.Card.ValueModifier;
            }
        }

        public CardEntity(EntityLayer entityLayer, Card card, bool isFliped) : base(entityLayer)
        {
            this.IsValid = true;

            this.Card = card;

            this.Card.PropertyChanged += OnPropertyChanged;

            this.parentStar = null;

            this.isFliped = isFliped;

            this.isSelected = false;

            //this.PositionInNotifBoard = new Vector2f(0, 0);
        }

        public CardEntity(EntityLayer entityLayer) : base(entityLayer)
        {
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.parentLayer.TryGetTarget(out EntityLayer entityLayer))
            {
                entityLayer.NotifyObjectBeforePropertyChanged(this, propertyName);

                if (propertyName == "IsAwakened")
                {
                    if (this.Card.IsAwakened)
                    {
                        this.Card.NotifyCardAwakened(entityLayer as BoardGameLayer, this.ParentStar);
                    }
                    else
                    {
                        this.Card.NotifyCardUnawakened(entityLayer as BoardGameLayer, this);
                    }
                }

                entityLayer.NotifyObjectPropertyChanged(this, propertyName);

                this.PropertyChanged?.Invoke(propertyName);
            }
        }

        public override void Dispose()
        {
            this.Card.PropertyChanged -= this.OnPropertyChanged;

            this.IsValid = false;

            base.Dispose();
        }
    }
}
