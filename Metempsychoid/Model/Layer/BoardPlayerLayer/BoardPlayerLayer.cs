using Metempsychoid.Model.Card;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardPlayerLayer
{
    public class BoardPlayerLayer : EntityLayer.EntityLayer
    {
        private static int TOP_DECK = 3;

        private static Vector2f DECK_POSITION = new Vector2f(-600, -150);

        private static Vector2f CEMETERY_POSITION = new Vector2f(-500, -150);

        private static Vector2f HAND_POSITION = new Vector2f(600, -150);
        private static int HAND_CARD_SPACE = 100;

        private List<CardEntity> cardsDeck;

        private List<CardEntity> cardsCemetery;

        private List<CardEntity> cardsHand;

        public BoardPlayerLayer()
        {
            this.cardsDeck = new List<CardEntity>();

            this.cardsCemetery = new List<CardEntity>();

            this.cardsHand = new List<CardEntity>();

            this.TypesInChunk.Add(typeof(CardEntity));
        }

        protected override void InternalInitializeLayer(World world)
        {
            int i = 0;
            foreach(Card.Card card in world.Player.Deck.Cards)
            {
                CardEntity cardEntity = new CardEntity(this, card, false);

                cardEntity.Position = DECK_POSITION;

                cardEntity.IsActive = i < TOP_DECK;

                this.cardsDeck.Add(cardEntity);
                this.AddEntityToLayer(cardEntity);

                i++;
            }
        }

        public bool DrawCard()
        {
            if (this.cardsDeck.Any())
            {
                CardEntity cardEntity = this.cardsDeck.FirstOrDefault();
                this.cardsDeck.RemoveAt(0);

                cardEntity.IsFliped = true;

                this.cardsHand.Add(cardEntity);

                if (this.cardsDeck.Count >= TOP_DECK)
                {
                    this.cardsHand[TOP_DECK - 1].IsActive = true;
                }

                this.UpdateCardsHandPosition();
            }

            return false;
        }

        private void UpdateCardsHandPosition()
        {
            float startWidth = HAND_POSITION.X - HAND_CARD_SPACE * this.cardsHand.Count / 2f;

            int i = 0;
            foreach(CardEntity cardEntity in this.cardsDeck)
            {
                cardEntity.Position = new Vector2f(startWidth + i * HAND_CARD_SPACE, HAND_POSITION.Y);
                i++;
            }
        }
    }
}
