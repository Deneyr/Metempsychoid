using System;
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
            layer.RegisterNotifBehavior(new ConvertCardNotifBehavior(this, starEntity.CardSocketed));
            //layer.RegisterNotifBehavior(new ResurrectCardNotifBehavior(this, starEntity.CardSocketed));

            //layer.RegisterNotifBehavior(new SocketNewCardNotifBehavior(this, starEntity.CardSocketed, new List<string>() { "wheel", "wheel", "wheel", "wheel" }));
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

            if (behavior is ResurrectCardNotifBehavior)
            {
                Random random = new Random();
                (behavior as ResurrectCardNotifBehavior).FromCardEntities = behavior.NodeLevel.GetLayerFromPlayer(behavior.OwnerCardEntity.Card.Player).CardsCemetery.Where(pElem => random.NextDouble() > 0.5).ToList();
                (behavior as ResurrectCardNotifBehavior).ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed == null).ToList();
            }
            else if (behavior is ConvertCardNotifBehavior)
            {
                (behavior as ConvertCardNotifBehavior).FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.Player != behavior.OwnerCardEntity.Card.Player).ToList();
            }
            else if(behavior is DeleteCardNotifBehavior)
            {
                (behavior as DeleteCardNotifBehavior).FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
            }
            else if(behavior is SocketNewCardNotifBehavior)
            {
                (behavior as SocketNewCardNotifBehavior).ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed == null).ToList();
            }
            //behavior.ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
        }

        public void OnBehaviorEnd(ACardNotifBehavior behavior)
        {
            //(behavior as ResurrectCardNotifBehavior).FromStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
            //behavior.ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null).ToList();
        }

        public void OnBehaviorCardPicked(ACardNotifBehavior behavior, CardEntity cardEntityPicked)
        {
            CardEntity cardEntity = behavior.NodeLevel.BoardGameLayer.CardEntityPicked;

            //if (behavior is ResurrectCardNotifBehavior)
            //{
            //    (behavior as ResurrectCardNotifBehavior).ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed == null).ToList();
            //}
            //HashSet<StarLinkEntity> starLinks = behavior.NodeLevel.BoardGameLayer.StarToLinks[starEntityConcerned];

            //(behavior as DeleteCardNotifBehavior).ToStarEntities = behavior.NodeLevel.BoardGameLayer.StarSystem.Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed != cardEntity).ToList();

            //behavior.ToStarEntities = starLinks.Select(pElem => pElem.StarFrom == starEntityConcerned ? pElem.StarTo : pElem.StarFrom).Where(pElem => pElem.CardSocketed == null).ToList();
        }
    }
}
