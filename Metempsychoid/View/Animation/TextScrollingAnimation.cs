using Astrategia.Animation;
using Astrategia.Model;
using Astrategia.View.Text2D;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Animation
{
    public class TextScrollingAnimation : AAnimation
    {
        private int cursorTextFrom;
        private int cursorTextTo;

        public TextScrollingAnimation(int cursorTextFrom, int cursorTextTo, Time animationPeriod, AnimationType type, InterpolationMethod method) :
            base(0, 1, animationPeriod, type, method)
        {
            this.cursorTextFrom = cursorTextFrom;
            this.cursorTextTo = cursorTextTo;
        }

        public override void Visit(IObject parentObject)
        {
            if (parentObject is TextToken2D)
            {
                int currentScrollingValue = (int) (this.currentValue * this.cursorTextTo + (1 - this.currentValue) * this.cursorTextFrom);

                TextToken2D textToken2D = parentObject as TextToken2D;

                textToken2D.TextCursor = currentScrollingValue;
            }
        }
    }
}
