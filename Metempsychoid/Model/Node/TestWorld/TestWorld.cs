using Metempsychoid.Model.Layer.BackgroundLayer;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node.TestWorld
{
    public class TestWorld: AWorldNode
    {
        public TestWorld(World world) :
            base(world)
        {
            this.nameTolevelNodes.Add("TestLevel", new TestLevel(world));
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            BackgroundLayer background = new BackgroundLayer();
            //EntityLayer entityLayer = new EntityLayer();
            BoardGameLayer boardGameLayer = new BoardGameLayer();

            BoardPlayerLayer boardPlayerLayer = new BoardPlayerLayer();

            boardGameLayer.ParentLayer = background;

            world.InitializeWorld(new List<Tuple<string, ALayer>>() {
                new Tuple<string, ALayer>("VsO7nJK", background),
                new Tuple<string, ALayer>("gameLayer", boardGameLayer),
                new Tuple<string, ALayer>("playerLayer", boardPlayerLayer)
            });

            this.nextLevelNodeName = "TestLevel";
            this.UpdateCurrentLevelNode(world);
        }
    }
}
