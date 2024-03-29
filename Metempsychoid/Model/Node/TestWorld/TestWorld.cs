﻿using Astrategia.Model.Layer.BackgroundLayer;
using Astrategia.Model.Layer.BoardBannerLayer;
using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Layer.BoardNotifLayer;
using Astrategia.Model.Layer.BoardPlayerLayer;
using Astrategia.Model.MenuLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Node.TestWorld
{
    public class TestWorld: AWorldNode
    {
        public TestWorld(World world) :
            base(world)
        {
            this.nameTolevelNodes.Add("StartPageLevel", new CJMenuLevel(world));
            this.nameTolevelNodes.Add("CardBoardLevel", new CardBoardLevel(world));
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            CJMenuLayer startPage = new CJMenuLayer();

            BackgroundLayer background = new BackgroundLayer();
            //EntityLayer entityLayer = new EntityLayer();
            BoardGameLayer boardGameLayer = new BoardGameLayer();
            boardGameLayer.ParentLayer = background;

            BoardPlayerLayer boardPlayerLayer = new BoardPlayerLayer();
            BoardPlayerLayer boardOpponentLayer = new BoardPlayerLayer();

            BoardNotifLayer boardNotifLayer = new BoardNotifLayer();

            BoardBannerLayer bannerLayer = new BoardBannerLayer();

            world.InitializeWorld(new List<Tuple<string, ALayer>>() {
                new Tuple<string, ALayer>("startPage", startPage),

                new Tuple<string, ALayer>("VsO7nJK", background),
                new Tuple<string, ALayer>("gameLayer", boardGameLayer),
                new Tuple<string, ALayer>("playerLayer", boardPlayerLayer),
                new Tuple<string, ALayer>("opponentLayer", boardOpponentLayer),
                new Tuple<string, ALayer>("notifLayer", boardNotifLayer),
                new Tuple<string, ALayer>("bannerLayer", bannerLayer)
            });

            this.nextLevelNodeName = "StartPageLevel";
            this.UpdateCurrentLevelNode(world);
        }
    }
}
