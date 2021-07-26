using Metempsychoid.Animation;
using Metempsychoid.Model.Layer.EntityLayer;
using Metempsychoid.View.Animation;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.EntityLayer2D
{
    public class T_TeleEntity2D : AEntity2D
    {
        public T_TeleEntity2D(IObject2DFactory factory, ALayer2D layer2D, T_TeleEntity entity):
            base(layer2D, factory, entity)
        {
            this.Position = entity.Position;
            this.Rotation = entity.Rotation;

            this.ObjectSprite.Texture = factory.GetTextureByIndex(0);

            this.ObjectSprite.TextureRect = new SFML.Graphics.IntRect(0, 0, 86, 76);

            this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(20), AnimationType.LOOP);

            IAnimation anim = new ZoomAnimation(1, 2, Time.FromSeconds(10), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0, anim);

            anim = new ZoomAnimation(2, 1, Time.FromSeconds(10), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(10, anim);

            anim = new FrameAnimation(new SFML.Graphics.IntRect[] { new SFML.Graphics.IntRect(0, 0, 86, 76), new SFML.Graphics.IntRect(86, 0, 86, 76) }, Time.FromSeconds(2), AnimationType.LOOP, InterpolationMethod.LINEAR);
            sequence.AddAnimation(10.01f, anim);

            this.animationsList.Add(sequence);

            this.PlayAnimation(0);
        }
    }
}
