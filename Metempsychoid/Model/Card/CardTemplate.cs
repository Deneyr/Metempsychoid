using Metempsychoid.Model.Constellations;
using Metempsychoid.Model.Layer.BoardGameLayer;
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

        public HandlingFunction HandlingCardAwakened;
        public HandlingFunction HandlingCardUnAwakened;

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

            this.Patterns = new List<ConstellationPattern>()
            {
                ConstellationPatternFactory.CreateDefaultConstellation()
            };

            this.HandlingCardAwakened = this.DefaultHandlingFunction;
            this.HandlingCardUnAwakened = this.DefaultHandlingFunction;
        }

        public CardTemplate()
        {
            this.NameIdLoc = string.Empty;
        }

        private void DefaultHandlingFunction(Card card, BoardGameLayer layer)
        {
            
        }
    }
}
