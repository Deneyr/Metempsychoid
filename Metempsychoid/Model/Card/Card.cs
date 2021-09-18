using Metempsychoid.Model.Card.Behaviors;
using Metempsychoid.Model.Constellations;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card
{
    public class Card: CardTemplate
    {
        private CardTemplate cardTemplate;

        private IConstellation constellation;

        private bool isAwakened;

        private int valueModificator;

        private Player.Player currentOwner;

        private bool canBeMoved;
        private bool canBeValueModified;

        private List<ICardBehavior> cardBehaviorsCloned;

        public event Action<string> PropertyChanged;

        public Dictionary<ICardBehavior, int> BehaviorToValueModifier
        {
            get;
            private set;
        }

        public override List<ICardBehavior> CardBehaviors
        {
            get
            {
                return this.cardBehaviorsCloned;
            }
        }

        public override bool CanBeMoved
        {
            get
            {
                return this.canBeMoved;
            }
            set
            {
                if(this.canBeMoved != value)
                {
                    this.canBeMoved = value;

                    this.PropertyChanged.Invoke("CanBeMoved");
                }
            }
        }

        public override bool CanBeValueModified
        {
            get
            {
                return this.canBeValueModified;
            }
            set
            {
                if(this.canBeValueModified != value)
                {
                    this.canBeValueModified = value;

                    this.PropertyChanged.Invoke("CanBeValueModified");
                    this.PropertyChanged.Invoke("Value");
                }
            }
        }

        public bool IsAwakened
        {
            get
            {
                return this.isAwakened;
            }
            private set
            {
                if(this.isAwakened != value)
                {
                    this.isAwakened = value;

                    //if (this.isAwakened)
                    //{
                    //    this.CardAwakened?.Invoke();
                    //}
                    //else
                    //{
                    //    this.CardUnAwakened?.Invoke();
                    //}
                    this.PropertyChanged.Invoke("IsAwakened");
                }
            }
        }

        public Player.Player CurrentOwner
        {
            get
            {
                return this.currentOwner;
            }
            set
            {
                if(this.currentOwner != value)
                {
                    this.currentOwner = value;

                    this.PropertyChanged.Invoke("CurrentOwner");
                }
            }
        }

        public Player.Player FirstOwner
        {
            get;
            private set;
        }

        public int Value
        {
            get
            {
                return Math.Max(0, this.cardTemplate.DefaultValue + this.ValueModifier);
            }
        }

        //public override int BonusValue
        //{
        //    get
        //    {
        //        return this.cardTemplate.BonusValue;
        //    }
        //}

        public override string Name
        {
            get
            {
                return this.cardTemplate.Name;
            }
        }

        public override string NameIdLoc
        {
            get
            {
                return this.cardTemplate.NameIdLoc;
            }
        }

        public override string PoemIdLoc
        {
            get
            {
                return this.cardTemplate.PoemIdLoc;
            }
        }

        public override string EffectIdLoc
        {
            get
            {
                return this.cardTemplate.EffectIdLoc;
            }
        }

        public override IConstellation Constellation
        {
            get
            {
                return this.constellation;
            }
            set
            {
                this.constellation = value;
            }
        }

        public int ValueModifier
        {
            get
            {
                if (this.CanBeValueModified)
                {
                    return this.valueModificator;
                }
                return 0;
            }
            set
            {
                if(this.valueModificator != value)
                {
                    this.valueModificator = value;

                    this.PropertyChanged?.Invoke("Value");
                }
            }
        }

        public Card(CardTemplate cardTemplate, Player.Player player)
        {
            this.cardTemplate = cardTemplate;

            this.FirstOwner = player;
            this.currentOwner = this.FirstOwner;

            this.valueModificator = 0;
            this.BehaviorToValueModifier = new Dictionary<ICardBehavior, int>();

            this.canBeMoved = cardTemplate.CanBeMoved;
            this.canBeValueModified = cardTemplate.CanBeValueModified;

            this.cardBehaviorsCloned = cardTemplate.CardBehaviors.Select(pElem => pElem.Clone()).ToList();

            this.isAwakened = false;

            this.InitConstellations();
        }

        public void AddValueModifier(ICardBehavior behaviorFrom, int valueModifier, bool mustDeleteIfNull)
        {
            if (this.BehaviorToValueModifier.TryGetValue(behaviorFrom, out int value))
            {
                value += valueModifier;
                this.BehaviorToValueModifier[behaviorFrom] = value;

                if(mustDeleteIfNull && value == 0)
                {
                    this.BehaviorToValueModifier.Remove(behaviorFrom);
                }
            }
            else
            {
                this.BehaviorToValueModifier.Add(behaviorFrom, valueModifier);
            }

            this.ValueModifier += valueModifier;
        }

        public void SetValueModifier(ICardBehavior behaviorFrom, int valueModifier)
        {
            int previousValue = 0;
            if (this.BehaviorToValueModifier.ContainsKey(behaviorFrom))
            {
                previousValue = this.BehaviorToValueModifier[behaviorFrom];

                this.BehaviorToValueModifier[behaviorFrom] = valueModifier;
            }
            else
            {
                this.BehaviorToValueModifier.Add(behaviorFrom, valueModifier);
            }

            this.ValueModifier += (valueModifier - previousValue);
        }

        //public void RemoveValueModifier(AEntity entityFrom, int valueModifier)
        //{
        //    if (this.entityToValueModifier.ContainsKey(entityFrom))
        //    {
        //        this.entityToValueModifier[entityFrom] -= valueModifier;

        //        int currentValue = this.entityToValueModifier[entityFrom];

        //        if (currentValue <= 0)
        //        {
        //            this.entityToValueModifier.Remove(entityFrom);
        //            currentValue = 0;
        //        }

        //        this.ValueModifier = currentValue;

        //        this.ValueModifier -= valueModifier;
        //    }
        //}

        public void ClearValueModifier(ICardBehavior behaviorFrom)
        {
            if (this.BehaviorToValueModifier.TryGetValue(behaviorFrom, out int value))
            {
                this.ValueModifier -= value;

                this.BehaviorToValueModifier.Remove(behaviorFrom);
            }
        }

        public bool GetValueModificatorFor(Behaviors.ICardBehavior behaviorFrom, out int valueModificator)
        {
            return this.BehaviorToValueModifier.TryGetValue(behaviorFrom, out valueModificator);
        }

        private void InitConstellations()
        {
            if (this.cardTemplate.Constellation != null)
            {
                this.constellation = this.cardTemplate.Constellation.Clone(this);
            }
            else
            {
                this.constellation = null;
            }
        }

        public void ResetConstellations(BoardGameLayer layer, CardEntity ownerCardEntity)
        {
            if(this.Constellation != null)
            {
                this.Constellation.ResetConstellation();
            }

            this.IsAwakened = false;

            foreach (ICardBehavior cardBehavior in this.CardBehaviors)
            {
                cardBehavior.OnDestroyed(layer, ownerCardEntity);
            }
        }

        internal void OnConstellationAwakened(IConstellation constellationAwakened)
        {           
            this.IsAwakened = true;
        }

        internal void OnConstellationUnawakened(IConstellation constellationUnawakened)
        {
            this.IsAwakened = false;
        }

        //public virtual void CardSocketed(BoardGameLayer layer, StarEntity parentStarEntity)
        //{
        //    //foreach(Constellation constellation in this.constellations)
        //    //{
        //    //    constellation.OnCardSocketed(layer, parentStarEntity);
        //    //}
        //}

        //public void CardUnsocketed(BoardGameLayer layer, StarEntity oldParentStarEntity)
        //{
        //    //foreach (Constellation constellation in this.constellations)
        //    //{
        //    //    constellation.OnCardUnsocketed(layer, oldParentStarEntity);
        //    //}
        //}

        //public virtual void OtherCardSocketed(BoardGameLayer layer, StarEntity starEntity, StarEntity starFromUnsocketedCard)
        //{
        //    //foreach (Constellation constellation in this.constellations)
        //    //{
        //    //    constellation.OnOtherCardSocketed(layer, starEntity, starFromUnsocketedCard);
        //    //}
        //}

        public void NotifyActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionOccured)
        {
            this.cardTemplate.OnActionsOccured(layer, starEntity, actionOccured);

            foreach (ICardBehavior cardBehavior in this.CardBehaviors)
            {
                cardBehavior.OnActionsOccured(layer, starEntity, actionOccured);
            }
        }

        public void ReevaluateAwakening(BoardGameLayer layer, StarEntity starEntity, HashSet<StarEntity> starEntitiesChanged)
        {         
            if(this.Constellation != null)
            {
                this.Constellation.OnOtherStarEntitiesChanged(layer, starEntity, starEntitiesChanged);
            }
        }

        //public virtual void CardEnteredBoard(BoardGameLayer layer)
        //{

        //}

        //public virtual void CardQuittedBoard(BoardGameLayer layer)
        //{
            
        //}

        public void NotifyCardAwakened(BoardGameLayer layer, StarEntity parentStarEntity)
        {
            this.cardTemplate.OnCardAwakened(layer, parentStarEntity);

            foreach (ICardBehavior cardBehavior in this.CardBehaviors)
            {
                cardBehavior.OnAwakened(layer, parentStarEntity);
            }
        }

        public void NotifyCardUnawakened(BoardGameLayer layer, CardEntity ownerCardEntity)
        {
            this.cardTemplate.OnCardUnawakened(layer, ownerCardEntity);

            foreach (ICardBehavior cardBehavior in this.CardBehaviors)
            {
                cardBehavior.OnUnawakened(layer, ownerCardEntity);
            }
        }
    }
}
