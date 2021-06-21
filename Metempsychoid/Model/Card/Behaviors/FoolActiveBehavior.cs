﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using Metempsychoid.Model.Layer.BoardNotifLayer.Behavior;
using Metempsychoid.Model.Node.TestWorld;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class FoolActiveBehavior : ICardBehavior, ICardBehaviorOwner
    {

        public void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionOccured)
        {
            
        }

        public void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            layer.RegisterNotifBehavior(new DeleteCardNotifBehavior(this, starEntity.CardSocketed));
        }

        public void OnUnawakened(BoardGameLayer layer, CardEntity ownerCardEntity)
        {

        }

        public ICardBehavior Clone()
        {
            return new FoolActiveBehavior();
        }

        public void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = 3;

            behavior.FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
            //behavior.ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
        }

        public void OnBehaviorEnd(ACardNotifBehavior behavior)
        {
            behavior.FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
            //behavior.ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
        }

        public void OnBehaviorCardPicked(ACardNotifBehavior behavior)
        {
            CardEntity cardEntity = behavior.NodeLevel.BoardGameLayer.CardEntityPicked;
            //HashSet<StarLinkEntity> starLinks = behavior.NodeLevel.BoardGameLayer.StarToLinks[starEntityConcerned];

            behavior.ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed != cardEntity).ToList();

            //behavior.ToStarEntities = starLinks.Select(pElem => pElem.StarFrom == starEntityConcerned ? pElem.StarTo : pElem.StarFrom).Where(pElem => pElem.CardSocketed == null).ToList();
        }
    }
}