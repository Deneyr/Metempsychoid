using Metempsychoid.Model.Layer.BackgroundLayer;
using Metempsychoid.Model.Layer.BoardBannerLayer;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardNotifLayer;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using Metempsychoid.Model.Layer.MenuTextLayer;
using Metempsychoid.Model.MenuLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node.TestWorld
{
    public class AstraWorld : AWorldNode
    {
        public AstraWorld(World world) :
            base(world)
        {
            this.nameTolevelNodes.Add("StartPageLevel", new AstraMenuLevel(world));
            this.nameTolevelNodes.Add("RulesLevel", new AstraRulesLevel(world));
            this.nameTolevelNodes.Add("CreditsLevel", new AstraCreditsLevel(world));

            this.nameTolevelNodes.Add("CardBoardLevel", new CardBoardLevel(world));
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            BackgroundLayer background = new BackgroundLayer();
            ImageBackgroundLayer imageBackground = new ImageBackgroundLayer();

            BoardGameLayer boardGameLayer = new BoardGameLayer();
            boardGameLayer.ParentLayer = background;

            BoardPlayerLayer boardPlayerLayer = new BoardPlayerLayer();
            BoardPlayerLayer boardOpponentLayer = new BoardPlayerLayer();
            MenuBoardPlayerLayer menuPlayerLayer = new MenuBoardPlayerLayer();

            BoardNotifLayer boardNotifLayer = new BoardNotifLayer();
            MenuBoardNotifLayer menuBoardNotifLayer = new MenuBoardNotifLayer();

            BoardBannerLayer bannerLayer = new BoardBannerLayer();

            MenuTextLayer menuTextLayer = new MenuTextLayer();
            menuTextLayer.ParentLayer = background;

            world.InitializeWorld(new List<Tuple<string, ALayer>>() {
                new Tuple<string, ALayer>("VsO7nJK", background),
                new Tuple<string, ALayer>("slidesLayer", imageBackground),
                new Tuple<string, ALayer>("menuTextLayer", menuTextLayer),
                new Tuple<string, ALayer>("gameLayer", boardGameLayer),
                new Tuple<string, ALayer>("playerLayer", boardPlayerLayer),
                new Tuple<string, ALayer>("opponentLayer", boardOpponentLayer),
                new Tuple<string, ALayer>("menuPlayerLayer", menuPlayerLayer),
                new Tuple<string, ALayer>("notifLayer", boardNotifLayer),
                new Tuple<string, ALayer>("menuNotifLayer", menuBoardNotifLayer),
                new Tuple<string, ALayer>("bannerLayer", bannerLayer)
            });

            this.nextLevelNodeName = "StartPageLevel";
            this.UpdateCurrentLevelNode(world);
        }
    }
}
