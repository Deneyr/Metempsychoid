using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node.TestWorld
{
    public class TestLevel: ALevelNode
    {
        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            world.InitializeLevel(new List<string>() { "VsO7nJK", "TestLayer" });
        }
    }
}
