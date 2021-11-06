using Astrategia.Model.Card;
using Astrategia.Model.Layer.BoardGameLayer;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardNotifLayer
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

            this.CardEntityDecorated.PropertyChanged += this.OnPropertyChanged;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.parentLayer.TryGetTarget(out EntityLayer.EntityLayer entityLayer))
            {
                entityLayer.NotifyObjectPropertyChanged(this, propertyName);
            }
        }

        public override void Dispose()
        {
            this.CardEntityDecorated.PropertyChanged -= this.OnPropertyChanged;

            base.Dispose();
        }
    }
}
