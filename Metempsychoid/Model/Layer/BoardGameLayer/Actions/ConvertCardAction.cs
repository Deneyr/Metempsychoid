using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
using Metempsychoid.Model.Card;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public class ConvertCardAction : IModifyStarEntityAction
    {
        public CardEntity CardToConvert
        {
            get;
            private set;
        }

        public StarEntity OwnerStar
        {
            get;
            private set;
        }

        public Player.Player NewCardOwner
        {
            get;
            private set;
        }

        public ConvertCardAction(CardEntity cardEntity, Player.Player newCardOwner)
        {
            this.OwnerStar = cardEntity.ParentStar;

            this.CardToConvert = cardEntity;

            this.CardToConvert = cardEntity;

            this.NewCardOwner = newCardOwner;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            this.CardToConvert.Card.CurrentOwner = this.NewCardOwner;
        }
    }
}

