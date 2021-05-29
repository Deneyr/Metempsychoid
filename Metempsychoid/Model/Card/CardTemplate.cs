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
    public class CardTemplate
    {
        public delegate void HandlingFunction(Card card, BoardGameLayer layer);

        //public HandlingFunction HandlingCardAwakened;
        //public HandlingFunction HandlingCardUnAwakened;

        public virtual List<ICardBehavior> CardBehaviors
        {
            get;
            private set;
        }

        public virtual int DefaultValue
        {
            get;
            private set;
        }

        public virtual int BonusValue
        {
            get;
            private set;
        }

        public virtual string Name
        {
            get;
            private set;
        }

        public virtual string NameIdLoc
        {
            get;
            private set;
        }

        public virtual string PoemIdLoc
        {
            get;
            private set;
        }

        public List<ConstellationPattern> Patterns
        {
            get;
            private set;
        }

        public CardTemplate(string name, string nameIdLoc, string poemIdLoc, int defaultValue, int bonusValue)
        {
            this.DefaultValue = defaultValue;
            this.BonusValue = bonusValue;

            this.Name = name;

            this.NameIdLoc = nameIdLoc;

            this.PoemIdLoc = poemIdLoc;

            this.CardBehaviors = new List<ICardBehavior>();

            this.Patterns = new List<ConstellationPattern>()
            {
                ConstellationPatternFactory.CreateDefaultConstellation()
            };
        }

        public CardTemplate()
        {
            this.NameIdLoc = string.Empty;
        }

        public void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionOccured)
        {
            //foreach(ICardBehavior cardBehavior in this.CardBehaviors)
            //{
            //    cardBehavior.OnActionsOccured(layer, starEntity, actionOccured);
            //}
        }

        public void OnCardAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            //foreach (ICardBehavior cardBehavior in this.CardBehaviors)
            //{
            //    cardBehavior.OnAwakened(layer, starEntity);
            //}
        }

        public void OnCardUnawakened(BoardGameLayer layer, StarEntity starEntity)
        {
            //foreach (ICardBehavior cardBehavior in this.CardBehaviors)
            //{
            //    cardBehavior.OnUnawakened(layer, starEntity);
            //}
        }

        //private void DefaultHandlingFunction(Card card, BoardGameLayer layer)
        //{

        //}
    }
}
