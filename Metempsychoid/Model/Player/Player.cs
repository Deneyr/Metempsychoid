using Astrategia.Model.Card;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Player
{
    public class Player
    {
        public CardDeck Deck
        {
            get;
            protected set;
        }

        public Color PlayerColor
        {
            get;
            private set;
        }

        public string PlayerName
        {
            get;
            private set;
        }

        public Player(Color playerColor, string playerName)
        {
            this.PlayerColor = playerColor;

            this.PlayerName = playerName;

            this.Deck = new CardDeck();
        }

        public List<Card.Card> ConstructDeck(CardFactory factory)
        {
            List<Card.Card> deckCardsResult = new List<Card.Card>();

            foreach(string cardId in this.Deck.CardIds)
            {
                deckCardsResult.Add(factory.CreateCard(cardId, this));
            }

            Random rand = new Random();
            deckCardsResult.OrderBy(pElem => rand.Next()).ToList();

            return deckCardsResult;
        }
    }
}
