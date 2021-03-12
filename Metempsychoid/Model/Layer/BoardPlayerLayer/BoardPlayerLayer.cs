using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
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

        public List<CardEntity> CardsDeck
        {
            get;
            private set;
        }

        public List<CardEntity> CardsCemetery
        {
            get;
            private set;
        }

        public List<CardEntity> CardsHand
        {
            get;
            private set;
        }


        public event Action<AEntity> CardDrew;

        public BoardPlayerLayer()
        {
            this.CardsDeck = new List<CardEntity>();

            this.CardsCemetery = new List<CardEntity>();

            this.CardsHand = new List<CardEntity>();

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

                this.CardsDeck.Add(cardEntity);
                this.AddEntityToLayer(cardEntity);

                i++;
            }
        }

        public bool DrawCard()
        {
            if (this.CardsDeck.Any())
            {
                CardEntity cardEntity = this.CardsDeck.FirstOrDefault();
                this.CardsDeck.RemoveAt(0);

                cardEntity.IsFliped = true;

                this.CardsHand.Add(cardEntity);

                if (this.CardsDeck.Count >= TOP_DECK)
                {
                    this.CardsDeck[TOP_DECK - 1].IsActive = true;
                }

                this.NotifyCardDrew(cardEntity);

                this.UpdateCardsHandPosition();
            }

            return false;
        }

        private void UpdateCardsHandPosition()
        {
            float startWidth = HAND_POSITION.X + HAND_CARD_SPACE * this.CardsHand.Count / 2f;

            int i = 0;
            foreach(CardEntity cardEntity in this.CardsHand)
            {
                Vector2f newPosition = new Vector2f(startWidth - i * HAND_CARD_SPACE, HAND_POSITION.Y);

                IAnimation positionAnimation = new PositionAnimation(cardEntity.Position, newPosition, Time.FromSeconds(2f), AnimationType.ONETIME, InterpolationMethod.SIGMOID);

                cardEntity.PlayAnimation(positionAnimation);
                i++;
            }
        }

        protected void NotifyCardDrew(AEntity obj)
        {
            this.CardDrew?.Invoke(obj);
        }
    }
}
