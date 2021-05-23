using Metempsychoid.Model.Layer.BoardGameLayer;
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

            CardTemplate cardTemplate = new CardTemplate("wheel", "wheel_of_fortune_title", "wheel_of_fortune_poem", 2, 1);
            cardTemplate.HandlingCardAwakened = this.DefaultAwakenedFunction;
            cardTemplate.HandlingCardUnAwakened = this.DefaultUnAwakenedFunction;

            this.AddCardTemplateToLibrary(cardTemplate);
        }

        private void AddCardTemplateToLibrary(CardTemplate cardTemplate)
        {
            this.cardLibrary.Add(cardTemplate.Name, cardTemplate);
        }

        // Handling methods

        private void DefaultAwakenedFunction(Card card, BoardGameLayer layer)
        {
            this.ApplyBonusValue(card, true);
        }

        private void DefaultUnAwakenedFunction(Card card, BoardGameLayer layer)
        {
            this.ApplyBonusValue(card, false);
        }

        private void ApplyBonusValue(Card card, bool isApplied)
        {
            if (isApplied)
            {
                card.ValueModificator += card.BonusValue;
            }
            else
            {
                card.ValueModificator -= card.BonusValue;
            }
        }
    }
}
