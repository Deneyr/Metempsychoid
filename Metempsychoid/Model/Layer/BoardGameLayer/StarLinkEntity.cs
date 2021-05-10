using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Maths;
using SFML.System;

namespace Metempsychoid.Model.Layer.BoardGameLayer
{
    public class StarLinkEntity: AEntity
    {
        public string Name
        {
            get;
            set;
        }

        public StarEntity StarFrom
        {
            get;
            protected set;
        }

        public StarEntity StarTo
        {
            get;
            protected set;
        }

        public override Vector2f Position
        {
            get
            {
                return (this.StarTo.Position + this.StarFrom.Position) / 2;
            }
        }

        public override float Rotation
        {
            get
            {
                Vector2f offsetVector = this.StarTo.Position - this.StarFrom.Position;

                return offsetVector.Angle();
            }
        }

        public StarLinkEntity(BoardGameLayer boardGameLayer, StarEntity starFrom, StarEntity starTo)
            : base(boardGameLayer)
        {
            this.StarFrom = starFrom;

            this.StarTo = starTo;

            boardGameLayer.EntityPropertyChanged += this.OnEntityPropertyChanged;
        }

        protected virtual void OnEntityPropertyChanged(AEntity entity, string propertyName)
        {
            if (this.parentLayer.TryGetTarget(out Metempsychoid.Model.Layer.EntityLayer.EntityLayer target))
            {
                if (entity == this.StarTo
                    || entity == this.StarFrom)
                {
                    target.NotifyObjectPropertyChanged(this, "Position");
                    target.NotifyObjectPropertyChanged(this, "Rotation");
                }
            }
        }

        public override void Dispose()
        {
            if(this.parentLayer.TryGetTarget(out Metempsychoid.Model.Layer.EntityLayer.EntityLayer target))
            {
                target.EntityPropertyChanged -= this.OnEntityPropertyChanged;
            }
            base.Dispose();
        }

    }
}
