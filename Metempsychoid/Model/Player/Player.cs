using Metempsychoid.Model.Card;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Player
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
    }
}
