using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Player
{
    public class Player
    {
        public Color PlayerColor
        {
            get;
            private set;
        }

        public Player(Color playerColor)
        {
            this.PlayerColor = playerColor;
        }
    }
}
