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
            AEntity entity = new T_TeleEntity(this);

            this.AddEntityToLayer("Tele", entity);
        }

        public override void UpdateLogic(World world, Time deltaTime)
        {
            base.UpdateLogic(world, deltaTime);

            
        }

        protected override void InternalInitializeLayer(PlayerData playerData)
        {
            AEntity entity = this.NamesToEntity["Tele"];

            entity.Position = new Vector2f(0, -400);

            IAnimation anim = new PositionAnimation(entity.Position, new Vector2f(500, 400), Time.FromSeconds(10), AnimationType.LOOP, InterpolationMethod.SIGMOID);
            entity.AddAnimation(anim);

            entity.PlayAnimation(0);
        }
    }
}
