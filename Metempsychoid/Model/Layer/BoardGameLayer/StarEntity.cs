﻿using Metempsychoid.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer
{
    public class StarEntity: AEntity
    {
        private CardEntity cardSocketed;

        public string Name
        {
            get;
            set;
        }

        public virtual CardEntity CardSocketed
        {
            get
            {
                return this.cardSocketed;
            }
            set
            {
                if (value != this.cardSocketed)
                {
                    if (this.cardSocketed != null)
                    {
                        this.cardSocketed.ParentStar = null;
                    }

                    this.cardSocketed = value;

                    if (this.cardSocketed != null)
                    {
                        this.cardSocketed.ParentStar = this;

                        this.cardSocketed.Position = this.Position;
                    }

                    if (this.parentLayer.TryGetTarget(out EntityLayer.EntityLayer entityLayer))
                    {
                        entityLayer.NotifyObjectPropertyChanged(this, "CardSocketed");
                    }
                }
            }
        }

        public StarEntity(BoardGameLayer boardGameLayer) : base(boardGameLayer)
        {
            this.cardSocketed = null;
        }

        public bool CanSocketCard(CardEntity cardToSocket)
        {
            bool canSocketOnBoard = true;

            if(this.parentLayer.TryGetTarget(out EntityLayer.EntityLayer entityLayer))
            {
                canSocketOnBoard = (entityLayer as BoardGameLayer).NbCardsAbleToBeSocketed > 0;
            }

            return this.IsActive && this.CardSocketed == null && canSocketOnBoard;
        }

        public bool CanMoveCard(CardEntity cardToMove)
        {
            return this.IsActive 
                && (this.CardSocketed == null || this.CardSocketed.Card.Player != cardToMove.Card.Player);
        }
    }
}
