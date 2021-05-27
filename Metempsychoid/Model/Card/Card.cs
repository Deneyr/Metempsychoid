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

        private List<Constellation> constellations;

        private bool isAwakened;

        public event Action<string> PropertyChanged;

        private int valueModificator;
        public Dictionary<ICardBehavior, int> BehaviorToValueModifier
        {
            get;
            private set;
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

        public Player.Player Player
        {
            get;
            set;
        }

        public int Value
        {
            get
            {
                return this.cardTemplate.DefaultValue + this.ValueModifier;
            }
        }

        public override int BonusValue
        {
            get
            {
                return this.cardTemplate.BonusValue;
            }
        }

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

        public List<Constellation> Constellations
        {
            get
            {
                return this.constellations;
            }
        }

        public int ValueModifier
        {
            get
            {
                return this.valueModificator;
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

            this.Player = player;

            this.valueModificator = 0;
            this.BehaviorToValueModifier = new Dictionary<ICardBehavior, int>();

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
            this.constellations = new List<Constellation>();
            foreach(ConstellationPattern pattern in this.cardTemplate.Patterns)
            {
                Constellation constellation = new Constellation(this, pattern);
                this.constellations.Add(constellation);
            }
        }

        public void ResetConstellations()
        {
            foreach (Constellation constellation in this.constellations)
            {
                constellation.ResetConstellation();
            }

            this.IsAwakened = false;
        }

        internal void OnConstellationAwakened(Constellation constellationAwakened)
        {
            bool isAwakened = true;

            foreach (Constellation constellation in this.constellations)
            {
                isAwakened &= constellation.IsAwakened;
            }

            this.IsAwakened = isAwakened;
        }

        internal void OnConstellationUnawakened(Constellation constellationUnawakened)
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
        }

        public void ReevaluateAwakening(BoardGameLayer layer, StarEntity starEntity, HashSet<StarEntity> starEntitiesChanged)
        {         
            foreach (Constellation constellation in this.constellations)
            {
                constellation.OnOtherStarEntitiesChanged(layer, starEntity, starEntitiesChanged);
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
        }

        public void NotifyCardUnawakened(BoardGameLayer layer, StarEntity parentStarEntity)
        {
            this.cardTemplate.OnCardUnawakened(layer, parentStarEntity);
        }
    }
}
