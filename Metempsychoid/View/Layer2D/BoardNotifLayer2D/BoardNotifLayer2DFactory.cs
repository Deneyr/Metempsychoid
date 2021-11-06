using Astrategia.Model;
using Astrategia.Model.Layer.BoardNotifLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardNotifLayer2D
{
    public class BoardNotifLayer2DFactory : AObject2DFactory
    {
        public BoardNotifLayer2DFactory()
        {
            // Sounds
            this.AddSoundPath("buttonClicked", @"Assets\Sounds\buttonClicked.ogg");
            this.AddSoundPath("buttonFocused", @"Assets\Sounds\buttonFocused.ogg");

            //this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is BoardNotifLayer)
            {
                BoardNotifLayer boardNotifLayer = obj as BoardNotifLayer;

                return new BoardNotifLayer2D(world2D, this, boardNotifLayer);
            }

            return null;
        }
    }
}
