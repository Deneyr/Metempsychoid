using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card
{
    public class CardFactory
    {
        Dictionary<string, CardTemplate> cardLibrary;

        public CardFactory()
        {
            this.InitializeCardLibrary();
        }

        public Card CreateCard(string name, Player.Player player)
        {
            Card card = new Card(this.cardLibrary[name], player);

            return card;
        }

        private void InitializeCardLibrary()
        {
            this.cardLibrary = new Dictionary<string, CardTemplate>();

            CardTemplate cardTemplate = new CardTemplate("wheel", 2);
            this.AddCardTemplateToLibrary(cardTemplate);
        }

        private void AddCardTemplateToLibrary(CardTemplate cardTemplate)
        {
            this.cardLibrary.Add(cardTemplate.Name, cardTemplate);
        }

    }
}
