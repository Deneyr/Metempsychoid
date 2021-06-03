using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.EntityLayer;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer
{
    public class CardEntityDecorator : CardEntity
    {
        public CardEntity CardEntityDecorated
        {
            get;
            private set;
        }

        public override Card.Card Card
        {
            get
            {
                return this.CardEntityDecorated.Card;
            }
        }

        public override StarEntity ParentStar
        {
            get
            {
                return this.CardEntityDecorated.ParentStar;
            }
        }

        public override bool IsFliped
        {
            get
            {
                return this.CardEntityDecorated.IsFliped;
            }
        }

        //public override Vector2f PositionInNotifBoard
        //{
        //    get
        //    {
        //        return this.CardEntityDecorated.PositionInNotifBoard;
        //    }
        //}

        public ALayer CardDecoratedParentLayer
        {
            get
            {
                return this.CardEntityDecorated.ParentLayer;
            }
        }

        public Vector2f CardDecoratedPosition
        {
            get
            {
                return this.CardEntityDecorated.Position;
            }
            set
            {
                this.CardEntityDecorated.Position = value;
            }
        }

        public CardEntityDecorator(EntityLayer.EntityLayer entityLayer, CardEntity cardEntity) : base(entityLayer)
        {
            this.CardEntityDecorated = cardEntity;

            this.CardEntityDecorated.PropertyChanged += this.OnAwakenPropertyChanged;
        }

        private void OnAwakenPropertyChanged(string propertyName)
        {
            if (this.parentLayer.TryGetTarget(out EntityLayer.EntityLayer entityLayer))
            {
                entityLayer.NotifyObjectPropertyChanged(this, propertyName);
            }
        }

        public override void Dispose()
        {
            this.CardEntityDecorated.PropertyChanged -= this.OnAwakenPropertyChanged;

            base.Dispose();
        }
    }
}
