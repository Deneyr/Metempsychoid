using Astrategia.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardPlayerLayer
{
    public class AvatarBoardPlayerLayer : BoardPlayerLayer
    {
        public override void OnStartTurn()
        {
            this.IsActiveTurn = true;

            foreach (CardEntity cardInHand in this.CardsHand)
            {
                cardInHand.IsFliped = true;
            }
        }

        public override void OnEndTurn()
        {
            this.IsActiveTurn = false;
        }
    }
}
