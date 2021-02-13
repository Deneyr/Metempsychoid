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

        public Player.Player player
        {
            get;
            set;
        }

        public override int Value
        {
            get
            {
                return this.cardTemplate.Value + this.ValueModificator;
            }
        }

        public int ValueModificator
        {
            get;
            set;
        }

        public Card(CardTemplate cardTemplate, Player.Player player)
        {
            this.cardTemplate = cardTemplate;

            this.player = player;

            this.ValueModificator = 0;
        }


        public virtual void CardEnteredBoard(Layer.BoardGameLayer.BoardGameLayer layer)
        {
            this.cardTemplate.CardEnteredBoard(layer, this.player);
        }

        public void UpdateCard(Layer.BoardGameLayer.BoardGameLayer layer)
        {
            this.cardTemplate.UpdateCard(layer, this.player);
        }

        public virtual void CardQuitBoard(Layer.BoardGameLayer.BoardGameLayer layer)
        {
            this.cardTemplate.CardQuitBoard(layer, this.player);
        }
    }
}
