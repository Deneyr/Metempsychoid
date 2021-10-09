using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BoardNotifLayer;
using Metempsychoid.Model.Layer.BoardNotifLayer.Behavior;
using Metempsychoid.Model.Node.TestWorld;

namespace Metempsychoid.View.Layer2D.BoardNotifLayer2D
{
    public class MenuBoardNotifLayer2D : BoardNotifLayer2D
    {
        public MenuBoardNotifLayer2D(World2D world2D, IObject2DFactory factory, BoardNotifLayer layer) 
            : base(world2D, factory, layer)
        {
        }

        public override TurnPhase LevelTurnPhase
        {
            get
            {
                return this.levelTurnPhase;
            }
            protected set
            {
                if (this.levelTurnPhase != value)
                {
                    this.levelTurnPhase = value;

                    switch (this.levelTurnPhase)
                    {
                        case TurnPhase.CREATE_HAND:
                            break;
                        case TurnPhase.END_TURN:
                            this.FocusedGraphicEntity2D = null;
                            break;
                    }
                }
            }
        }

        protected override void OnNotifBehaviorStarted(IBoardNotifBehavior obj)
        {
            this.IsRunningBehavior = true;

            if (obj.IsThereBehaviorLabel)
            {
                this.effectBehaviorLabel2D.ActiveLabel(obj);

                if (obj is ACardNotifBehavior)
                {
                    this.effectBehaviorLabel2D.Label = (obj as ACardNotifBehavior).NbBehaviorUse;
                }
            }
        }

        protected override void OnNotifBehaviorEnded(IBoardNotifBehavior obj)
        {
            this.IsRunningBehavior = false;

            if (obj.IsThereBehaviorLabel)
            {
                this.effectBehaviorLabel2D.DeactiveLabel();
            }
        }
    }
}
