using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardPlayerLayer2D
{
    public class BoardPlayerLayer2D: ALayer2D
    {
        public BoardPlayerLayer2D(World2D world2D, IObject2DFactory factory, BoardPlayerLayer layer) :
            base(world2D, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);
        }

        protected override void UpdateViewSize(Vector2f viewSize, Time deltaTime)
        {
            this.DefaultViewSize = viewSize;
            this.view.Size = viewSize;

            foreach(KeyValuePair<AEntity, AEntity2D> pairEntity in this.objectToObject2Ds)
            {
                pairEntity.Value.Position = new Vector2f(pairEntity.Key.Position.X, this.view.Size.Y / 2 + pairEntity.Key.Position.Y);
            }
        }
    }
}
