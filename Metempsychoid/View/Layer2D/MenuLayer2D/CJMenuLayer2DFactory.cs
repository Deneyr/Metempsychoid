using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.Model.MenuLayer;

namespace Metempsychoid.View.Layer2D.MenuLayer2D
{
    public class CJMenuLayer2DFactory : AObject2DFactory
    {
        public CJMenuLayer2DFactory()
        {
            // Sounds
            this.AddSoundPath("buttonClicked", @"Assets\Sounds\buttonClicked.ogg");
            this.AddSoundPath("buttonFocused", @"Assets\Sounds\buttonFocused.ogg");

            //this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is CJMenuLayer)
            {
                CJMenuLayer backgroundLayer = obj as CJMenuLayer;

                return new CJMenuLayer2D(world2D, this, backgroundLayer);
            }

            return null;
        }
    }
}
