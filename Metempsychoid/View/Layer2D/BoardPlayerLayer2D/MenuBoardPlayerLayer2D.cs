using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using SFML.Graphics;
using SFML.System;

namespace Metempsychoid.View.Layer2D.BoardPlayerLayer2D
{
    public class MenuBoardPlayerLayer2D : BoardPlayerLayer2D
    {
        public MenuBoardPlayerLayer2D(World2D world2D, IObject2DFactory factory, BoardPlayerLayer layer) 
            : base(world2D, factory, layer)
        {
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            this.UpdateGraphics(deltaTime);

            List<AEntity2D> listObjects = this.objectToObject2Ds.Values.ToList();
            listObjects.Sort(new EntityComparer());

            foreach (AEntity2D entity2D in listObjects)
            {
                entity2D.UpdateGraphics(deltaTime);
            }

            SFML.Graphics.View defaultView = window.DefaultView;

            this.UpdateViewSize(defaultView.Size, deltaTime);

            window.SetView(this.view);

            FloatRect bounds = this.Bounds;
            foreach (AEntity2D entity2D in listObjects)
            {
                if (entity2D.IsActive
                    && entity2D.Bounds.Intersects(bounds))
                {
                    entity2D.DrawIn(window, deltaTime);
                }
            }

            window.SetView(defaultView);
        }
    }
}
