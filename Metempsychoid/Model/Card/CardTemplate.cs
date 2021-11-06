using Astrategia.Model.Card.Behaviors;
using Astrategia.Model.Constellations;
using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Card
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

        public virtual bool CanBeMoved
        {
            get;
            set;
        }

        public virtual bool CanBeValueModified
        {
            get;
            set;
        }

        public virtual int DefaultValue
        {
            get;
            private set;
        }

        //public virtual int BonusValue
        //{
        //    get;
        //    private set;
        //}

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

        public virtual string EffectIdLoc
        {
            get;
            private set;
        }

        public virtual IConstellation Constellation
        {
            get;
            set;
        }

        public CardTemplate(string name, string nameIdLoc, string poemIdLoc, string effectIdLoc, int defaultValue)
        {
            this.DefaultValue = defaultValue;
            //this.BonusValue = bonusValue;

            this.Name = name;

            this.NameIdLoc = nameIdLoc;
            this.PoemIdLoc = poemIdLoc;
            this.EffectIdLoc = effectIdLoc;

            this.CanBeMoved = true;
            this.CanBeValueModified = true;

            this.CardBehaviors = new List<ICardBehavior>();

            this.Constellation = null;
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

        public void OnCardUnawakened(BoardGameLayer layer, CardEntity ownerCardEntity)
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
