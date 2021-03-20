﻿using Metempsychoid.Animation;
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

        private static Vector2f HAND_POSITION = new Vector2f(400, -150);
        private static int HAND_CARD_SPACE = 100;

        private int nbCardsToDraw;

        private CardEntity cardFocused;

        public event Action<CardEntity> CardPicked;

        public event Action<CardEntity> CardUnpicked;

        public event Action<CardEntity> CardDrew;

        public event Action<int> NbCardsToDrawChanged;

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

        public int NbCardsToDraw
        {
            get
            {
                return this.nbCardsToDraw;
            }
            set
            {
                if(this.nbCardsToDraw != value)
                {
                    this.nbCardsToDraw = value;

                    this.NotifyNbCardsToDraw();
                }
            }
        }

        public CardEntity CardFocused
        {
            get
            {
                return this.cardFocused;
            }

            set
            {
                if (value != this.cardFocused)
                {
                    this.cardFocused = value;

                    this.UpdateCardsHandPosition();
                }
            }
        }

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

            this.nbCardsToDraw = 0;

            this.cardFocused = null;
        }

        public bool DrawCard()
        {
            if (this.CardsDeck.Any()
                && this.NbCardsToDraw > 0)
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

                this.NbCardsToDraw -= 1;
            }

            return false;
        }

        public bool PickCard(CardEntity cardToPick)
        {
            if (this.CardsHand.Contains(cardToPick))
            {
                this.NotifyCardPicked(cardToPick);

                this.RemoveEntityFromLayer(cardToPick);

                this.CardFocused = null;

                return true;
            }
            return false;
        }

        public void UnpickCard(Card.Card cardToUnpick, Vector2f startPosition)
        {
            CardEntity cardEntity = new CardEntity(this, cardToUnpick, true);

            this.AddEntityToLayer(cardEntity);

            cardEntity.Position = startPosition;

            this.CardsHand.Add(cardEntity);

            this.NotifyCardUnpicked(cardEntity);

            this.UpdateCardsHandPosition();
        }

        public override void RemoveEntityFromLayer(AEntity entity)
        {
            if (entity is CardEntity)
            {
                CardEntity cardEntity = entity as CardEntity;

                this.CardsDeck.Remove(cardEntity);

                this.CardsHand.Remove(cardEntity);

                this.CardsCemetery.Remove(cardEntity);
            }
            base.RemoveEntityFromLayer(entity);
        }

        private void UpdateCardsHandPosition()
        {
            float startWidth = HAND_POSITION.X + HAND_CARD_SPACE * this.CardsHand.Count / 2f;

            int i = 0;
            bool cardFocusedEncountered = false;

            foreach (CardEntity cardEntity in this.CardsHand)
            {
                Vector2f newPosition;
                cardFocusedEncountered |= this.cardFocused == cardEntity;

                if (this.cardFocused != null)
                {
                    if (this.cardFocused == cardEntity)
                    {
                        newPosition = new Vector2f(startWidth - i * HAND_CARD_SPACE, HAND_POSITION.Y);
                    }
                    else if (cardFocusedEncountered)
                    {
                        newPosition = new Vector2f(startWidth - (i + 1) * HAND_CARD_SPACE, HAND_POSITION.Y);
                    }
                    else
                    {
                        newPosition = new Vector2f(startWidth - (i - 1) * HAND_CARD_SPACE, HAND_POSITION.Y);
                    }
                }
                else
                {
                    newPosition = new Vector2f(startWidth - i * HAND_CARD_SPACE, HAND_POSITION.Y);
                }

                IAnimation positionAnimation;
                if (this.cardFocused != null)
                {
                    positionAnimation = new PositionAnimation(cardEntity.Position, newPosition, Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
                }
                else
                {
                    positionAnimation = new PositionAnimation(cardEntity.Position, newPosition, Time.FromSeconds(2f), AnimationType.ONETIME, InterpolationMethod.SIGMOID);
                }

                cardEntity.PlayAnimation(positionAnimation);
                i++;

            }
        }

        protected void NotifyCardDrew(CardEntity obj)
        {
            this.CardDrew?.Invoke(obj);
        }

        protected void NotifyNbCardsToDraw()
        {
            this.NbCardsToDrawChanged?.Invoke(this.NbCardsToDraw);
        }

        protected void NotifyCardPicked(CardEntity cardPicked)
        {
            this.CardPicked?.Invoke(cardPicked);
        }

        protected void NotifyCardUnpicked(CardEntity cardUnpicked)
        {
            this.CardUnpicked?.Invoke(cardUnpicked);
        }
    }
}
