using Metempsychoid.Model.Constellations;
using Metempsychoid.Model.Layer.BoardGameLayer;
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

        private List<Constellation> constellations;

        private bool isAwakened;

        public event Action CardAwakened;

        public event Action CardUnAwakened;

        public bool IsAwakened
        {
            get
            {
                return this.isAwakened;
            }
            private set
            {
                if(this.isAwakened != value)
                {
                    this.isAwakened = value;

                    if (this.isAwakened)
                    {
                        this.CardAwakened?.Invoke();
                    }
                    else
                    {
                        this.CardUnAwakened?.Invoke();
                    }
                }
            }
        }

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

        public List<Constellation> Constellations
        {
            get
            {
                return this.constellations;
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

            this.isAwakened = false;

            this.InitConstellations();
        }

        private void InitConstellations()
        {
            this.constellations = new List<Constellation>();
            foreach(ConstellationPattern pattern in this.cardTemplate.Patterns)
            {
                Constellation constellation = new Constellation(this, pattern);
                this.constellations.Add(constellation);
            }
        }

        internal void OnConstellationAwakened(Constellation constellationAwakened)
        {
            bool isAwakened = true;

            foreach (Constellation constellation in this.constellations)
            {
                isAwakened &= constellation.IsAwakened;
            }

            this.IsAwakened = isAwakened;
        }

        internal void OnConstellationUnawakened(Constellation constellationUnawakened)
        {
            this.IsAwakened = false;
        }

        public virtual void CardSocketed(BoardGameLayer layer, StarEntity parentStarEntity)
        {
            foreach(Constellation constellation in this.constellations)
            {
                constellation.OnCardSocketed(layer, parentStarEntity);
            }
        }

        public void CardUnsocketed(BoardGameLayer layer, StarEntity oldParentStarEntity)
        {
            foreach (Constellation constellation in this.constellations)
            {
                constellation.OnCardUnsocketed(layer, oldParentStarEntity);
            }
        }

        public virtual void OtherCardSocketed(BoardGameLayer layer, StarEntity starEntity, StarEntity starFromUnsocketedCard)
        {
            foreach (Constellation constellation in this.constellations)
            {
                constellation.OnOtherCardSocketed(layer, starEntity, starFromUnsocketedCard);
            }
        }

        public void OtherCardUnsocketed(BoardGameLayer layer, StarEntity starEntity, StarEntity starFromUnsocketedCard)
        {
            foreach (Constellation constellation in this.constellations)
            {
                constellation.OnOtherCardUnsocketed(layer, starEntity, starFromUnsocketedCard);
            }
        }

        public virtual void CardEnteredBoard(BoardGameLayer layer)
        {

        }

        public virtual void CardQuittedBoard(BoardGameLayer layer)
        {
            
        }
    }
}
