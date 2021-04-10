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

        public Player.Player Player
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

        public int ValueModificator
        {
            get;
            set;
        }

        public Card(CardTemplate cardTemplate, Player.Player player)
        {
            this.cardTemplate = cardTemplate;

            this.Player = player;

            this.ValueModificator = 0;
        }


        public virtual void CardEnteredBoard(Layer.BoardGameLayer.BoardGameLayer layer)
        {
            this.cardTemplate.CardEnteredBoard(layer, this.Player);
        }

        public void UpdateCard(Layer.BoardGameLayer.BoardGameLayer layer)
        {
            this.cardTemplate.UpdateCard(layer, this.Player);
        }

        public virtual void CardQuitBoard(Layer.BoardGameLayer.BoardGameLayer layer)
        {
            this.cardTemplate.CardQuitBoard(layer, this.Player);
        }
    }
}
