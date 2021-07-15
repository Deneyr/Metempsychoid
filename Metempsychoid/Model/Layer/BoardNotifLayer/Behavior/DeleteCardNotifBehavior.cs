using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public class DeleteCardNotifBehavior : ACardNotifBehavior
    {
        protected DeleteState state;

        protected bool mustNotifyBehaviorEnd;

        private int nbCurrentBehaviorUse;

        public List<StarEntity> FromStarEntities
        {
            get;
            set;
        }

        public virtual int NbCurrentBehaviorUse
        {
            get
            {
                return this.nbCurrentBehaviorUse;
            }
            set
            {
                if(this.nbCurrentBehaviorUse != value)
                {
                    this.nbCurrentBehaviorUse = value;

                    if(this.nbCurrentBehaviorUse == 0)
                    {
                        this.State = DeleteState.PICK_CARD;
                    }
                    else
                    {
                        this.State = DeleteState.CAN_DELETE_CARD;
                    }
                }

                this.NodeLevel.BoardNotifLayer.NotifyNotifBehaviorUseChanged(this.NbBehaviorUse - this.NbCurrentBehaviorUse);
            }
        }

        public virtual DeleteState State
        {
            get
            {
                return this.state;
            }

            protected set
            {
                if (this.state != value)
                {
                    this.state = value;

                    //switch (this.state)
                    //{
                    //    case DeleteState.PICK_CARD:
                    //        this.NodeLevel.BoardGameLayer.SetBehaviorSourceStarEntities(this.FromStarEntities);
                    //        break;
                    //    case DeleteState.CAN_DELETE_CARD:
                    //        //this.NodeLevel.BoardGameLayer.SetBehaviorSourceStarEntities(null);
                    //        //this.NodeLevel.BoardGameLayer.SetBehaviorTargetStarEntities(this.ToStarEntities);
                    //        break;
                    //}

                    this.NodeLevel.BoardNotifLayer.NotifyNotifBehaviorPhaseChanged("DeleteCardNotifBehavior." + this.State.ToString());
                }
            }
        }

        public DeleteCardNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity)//List<StarEntity> fromStarEntities, List<StarEntity> toStarEntities) :
            : base(cardBehaviorOwner, ownerCardEntity)
        {
            this.FromStarEntities = new List<StarEntity>();

            this.state = DeleteState.VOID;
        }

        public override void EndNotif(World world)
        {
            base.EndNotif(world);

            this.FromStarEntities.Clear();

            this.NodeLevel.BoardGameLayer.SetBehaviorSourceStarEntities(this.FromStarEntities);
        }

        public override void StartNotif(World world)
        {
            base.StartNotif(world);

            this.nbCurrentBehaviorUse = 0;

            this.mustNotifyBehaviorEnd = false;

            //this.CardBehaviorOwner.OnBehaviorStart(this);

            this.State = DeleteState.PICK_CARD;

            this.NodeLevel.BoardGameLayer.SetBehaviorSourceStarEntities(this.FromStarEntities);
        }

        public override bool UpdateNotif(World world)
        {
            if (this.mustNotifyBehaviorEnd)
            {
                //this.CardBehaviorOwner.OnBehaviorEnd(this);
                this.mustNotifyBehaviorEnd = false;

                if (this.IsActive)
                {
                    this.State = DeleteState.PICK_CARD;
                }
            }

            return this.IsActive;
        }

        public override void HandleGameEvents(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            base.HandleGameEvents(gameEvents);

            //switch (this.state)
            //{
            //    case MoveState.PICK_CARD:
            //        this.HandlePickState(gameEvents);
            //        break;
            //    case MoveState.DELETE_CARD:
            //        this.HandleSocketState(gameEvents);
            //        break;
            //}

            this.HandlePickState(gameEvents);
        }

        private void HandlePickState(Dictionary<EventType, List<GameEvent>> gameEvents)
        {
            if (gameEvents.TryGetValue(EventType.NEXT_BEHAVIOR, out List<GameEvent> gameEventsNextBehavior))
            {
                if (gameEventsNextBehavior.Any())
                {
                    IEnumerable<StarEntity> starEntitiesConcerned = this.FromStarEntities.Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.IsSelected);
                    foreach (StarEntity starEntity in starEntitiesConcerned)
                    {
                        starEntity.CardSocketed.IsSelected = false;

                        this.ExecuteBehavior(starEntity);
                    }
                    this.IsActive = false;
                    this.mustNotifyBehaviorEnd = true;
                    return;
                }
            }


            if (gameEvents.TryGetValue(EventType.PICK_CARD, out List<GameEvent> gameEventsPicks))
            {
                GameEvent boardGameEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == NodeLevel.BoardGameLayer);

                if (boardGameEvent != null)
                {
                    GameEvent boardNotifEvent = gameEventsPicks.FirstOrDefault(pElem => pElem.Layer == NodeLevel.BoardNotifLayer);

                    CardEntity cardEntityPicked = boardGameEvent.Entity as CardEntity;
                    if (cardEntityPicked != null)
                    {
                        //this.NodeLevel.BoardNotifLayer.AddCardToBoard(cardEntityPicked, Node.TestWorld.TestLevel.GetPositionFrom(boardNotifEvent.Details));

                        if (cardEntityPicked.IsSelected)
                        {
                            this.NbCurrentBehaviorUse--;

                            cardEntityPicked.IsSelected = false;

                            this.CardBehaviorOwner.OnBehaviorCardPicked(this, cardEntityPicked);
                        }
                        else if(this.NbCurrentBehaviorUse < this.NbBehaviorUse)
                        {
                            this.NbCurrentBehaviorUse++;

                            cardEntityPicked.IsSelected = true;

                            this.CardBehaviorOwner.OnBehaviorCardPicked(this, cardEntityPicked);
                        }
                    }
                }
            }
        }

        protected override void ExecuteBehavior(StarEntity starEntity)
        {
            base.ExecuteBehavior(starEntity);

            this.NodeLevel.BoardGameLayer.DeleteCard(starEntity);
        }

        public enum DeleteState
        {
            VOID,
            PICK_CARD,
            CAN_DELETE_CARD
        }
    }
}
