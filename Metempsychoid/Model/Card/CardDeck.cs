using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card
{
    public class CardDeck
    {
        public List<Card> Cards
        {
            get;
            protected set;
        }

        public CardDeck()
        {
            this.Cards = new List<Card>();
        }

        public void SortDeck()
        {

        }

    }
}
