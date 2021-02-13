using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.EntityLayer
{
    public class EntityLayer: ALayer
    {
        public EntityLayer()
        {

        }

        public override void UpdateLogic(World world, Time deltaTime)
        {
            base.UpdateLogic(world, deltaTime);           
        }

        protected override void InternalInitializeLayer(World world)
        {
            //AEntity entity = new T_TeleEntity(this);

            //this.AddEntityToLayer(entity);

            //entity.Position = new Vector2f(-400, 0);

            //SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(20), AnimationType.LOOP);

            //IAnimation anim = new PositionAnimation(entity.Position, new Vector2f(400, 0), Time.FromSeconds(20), AnimationType.ONETIME, InterpolationMethod.SIGMOID);
            //sequence.AddAnimation(0, anim);

            //anim = new RotationAnimation(entity.Rotation, 180, Time.FromSeconds(5), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            //sequence.AddAnimation(5, anim);

            //anim = new RotationAnimation(180, entity.Rotation, Time.FromSeconds(5), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            //sequence.AddAnimation(10, anim);

            //entity.AddAnimation(sequence);

            //entity.PlayAnimation(0);
        }
    }
}
