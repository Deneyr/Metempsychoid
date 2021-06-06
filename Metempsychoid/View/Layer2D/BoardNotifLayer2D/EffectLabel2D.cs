using Metempsychoid.Animation;
using Metempsychoid.View.Animation;
using Metempsychoid.View.Text2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardNotifLayer2D
{
    public class EffectLabel2D : TextCanevas2D
    {
        public override bool IsActive
        {
            get
            {
                return base.IsActive && this.SpriteColor.A > 0;
            }
        }

        public EffectLabel2D(ALayer2D parentLayer) 
            : base(parentLayer)
        {
            this.CreateTextParagraph2D(new Vector2f(0, 0), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 30);

            this.Canevas = new IntRect(0, 0, 500, 500);
            this.Position = new Vector2f(-250, -50);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(6), AnimationType.ONETIME);

            IAnimation anim = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0.5f, anim);

            anim = new ColorAnimation(new Color(0, 0, 0, 255), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(5, anim);

            this.animationsList.Add(sequence);

            this.IsActive = false;
        }

        public void DisplayEffectLabel(string effectIdLoc)
        {
            this.UpdateTextOfParagraph(0, effectIdLoc);

            this.SpriteColor = new Color(0, 0, 0, 0);

            this.IsActive = true;
            this.PlayAnimation(0);
        }
    }
}
