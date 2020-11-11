using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BackgroundLayer
{
    public class BackgroundLayer: ALayer
    {
        public string BackgroundName
        {
            get;
            private set;
        }

        public BackgroundLayer(string backgroundName)
        {
            this.BackgroundName = backgroundName;

            this.Position = new SFML.System.Vector2f(0, 0);
        }
    }
}
