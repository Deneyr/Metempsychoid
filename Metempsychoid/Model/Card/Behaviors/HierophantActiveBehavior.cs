﻿using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using Metempsychoid.Model.Layer.BoardNotifLayer.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class HierophantActiveBehavior : ACardBehavior, ICardBehaviorOwner
    {
        public List<string> NewCardsToAdd
        {
            get;
            private set;
        }

        public HierophantActiveBehavior(List<string> newCardsToAdd)
        {
            this.NewCardsToAdd = newCardsToAdd;
        }

        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            layer.RegisterNotifBehavior(new SocketNewCardNotifBehavior(this, starEntity.CardSocketed, this.NewCardsToAdd));
        }

        public void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = this.NewCardsToAdd.Count;
        }

        public void OnBehaviorEnd(ACardNotifBehavior behavior)
        {

        }

        public void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {
            (behavior as SocketNewCardNotifBehavior).ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed == null).ToList();
        }

        public override ICardBehavior Clone()
        {
            return new HierophantActiveBehavior(this.NewCardsToAdd);
        }
    }
}
