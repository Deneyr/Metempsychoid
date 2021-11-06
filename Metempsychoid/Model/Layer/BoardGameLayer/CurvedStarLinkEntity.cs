using Astrategia.Maths;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardGameLayer
{
    public class CurvedStarLinkEntity : StarLinkEntity
    {
        public Vector2f Center
        {
            get;
            private set;
        }

        public float Radius
        {
            get;
            private set;
        }

        public CurvedStarLinkEntity(BoardGameLayer boardGameLayer, StarEntity starFrom, StarEntity starTo, float radius) : 
            base(boardGameLayer, starFrom, starTo)
        {
            this.Radius = radius;

            this.UpdateCenter();
        }

        private void UpdateCenter()
        {
            Vector2f offsetVector = this.StarTo.Position - this.StarFrom.Position;
            float offsetLen = offsetVector.Len();
            float adjSide = offsetLen / 2;

            double angle = Math.Acos(adjSide / Math.Abs(this.Radius)) * Math.Sign(this.Radius);

            Vector2f toCenter = offsetVector.Rotate(angle);

            toCenter = toCenter * this.Radius / offsetLen;

            this.Center = this.StarFrom.Position + toCenter;
        }

        protected override void OnEntityPropertyChanged(AEntity entity, string propertyName)
        {
            if (entity == this.StarTo
                    || entity == this.StarFrom)
            {
                this.UpdateCenter();
            }

            base.OnEntityPropertyChanged(entity, propertyName);
        }

        public override float Rotation
        {
            get
            {
                Vector2f offsetVector = this.Center - this.StarFrom.Position;

                offsetVector *= Math.Sign(this.Radius);

                return offsetVector.Angle();
            }
        }

        public override Vector2f Position
        {
            get
            {
                return this.StarFrom.Position;
            }
        }
    }
}
