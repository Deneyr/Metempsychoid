using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Card;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public class SocketCardNotifBehavior : ResurrectCardNotifBehavior
    {
        public SocketCardNotifBehavior(ICardBehaviorOwner cardBehaviorOwner, CardEntity ownerCardEntity) : base(cardBehaviorOwner, ownerCardEntity)
        {
        }

        public override void EndNotif(World world)
        {
            base.EndNotif(world);

            //this.FromStarEntities.Clear();
            this.FromCardEntities.Clear();
            this.ToStarEntities.Clear();

            BoardPlayerLayer.BoardPlayerLayer currentPlayerLayer = this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.CurrentOwner);

            currentPlayerLayer.SetBehaviorSourceCardEntities(this.FromCardEntities);

            //this.NodeLevel.BoardGameLayer.SetBehaviorSourceStarEntities(this.FromStarEntities);
        }

        public override void StartNotif(World world)
        {
            base.StartNotif(world);

            this.mustNotifyBehaviorEnd = false;

            this.CardBehaviorOwner.OnBehaviorStart(this);

            this.NodeLevel.GetLayerFromPlayer(this.OwnerCardEntity.Card.CurrentOwner).CardPileFocused = BoardPlayerLayer.BoardPlayerLayer.PileFocused.HAND;

            this.State = ResurrectState.PICK_CARD;
        }
    }
}
