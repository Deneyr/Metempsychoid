using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card
{
    public class CardDeck
    {
        public List<string> CardIds
        {
            get;
            protected set;
        }

        public CardDeck()
        {
            this.CardIds = new List<string>();
        }

        public void SortDeck()
        {

        }

    }
}
