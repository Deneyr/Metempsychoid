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

        public override void InitializeLayer(World world)
        {
            base.InitializeLayer(world);

            this.Player = world.Player;
            this.Opponent = world.Opponent;
        }
    }
}
