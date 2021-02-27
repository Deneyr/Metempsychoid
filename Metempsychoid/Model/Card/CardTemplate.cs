using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card
{
    public class CardTemplate
    {
        private int defaultValue;

        public virtual int Value
        {
            get
            {
                return this.defaultValue;
            }
        }

        public virtual string Name
        {
            get;
            private set;
        }

        public CardTemplate(string name, int defaultValue)
        {
            this.defaultValue = defaultValue;

            this.Name = name;
        }

        public CardTemplate()
        {
            this.Name = string.Empty;
        }

        public virtual void CardEnteredBoard(Layer.BoardGameLayer.BoardGameLayer layer, Player.Player player)
        {

        }

        public virtual void UpdateCard(Layer.BoardGameLayer.BoardGameLayer layer, Player.Player player)
        {

        }

        public virtual void CardQuitBoard(Layer.BoardGameLayer.BoardGameLayer layer, Player.Player player)
        {

        }
    }
}
