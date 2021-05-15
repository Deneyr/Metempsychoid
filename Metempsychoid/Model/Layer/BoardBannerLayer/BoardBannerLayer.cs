using Metempsychoid.Model.Node;
using Metempsychoid.Model.Node.TestWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardBannerLayer
{
    public class BoardBannerLayer: EntityLayer.EntityLayer
    {
        public Player.Player Player
        {
            get;
            private set;
        }

        public Player.Player Opponent
        {
            get;
            private set;
        }

        protected override void InternalInitializeLayer(World world, ALevelNode levelNode)
        {
            this.Player = world.Player;
            this.Opponent = (levelNode as TestLevel).Opponent;
        }
    }
}
