using Metempsychoid.Maths;
using Metempsychoid.Model.Layer.BoardGameLayer;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class CurvedStarLinkEntity2D : AEntity2D
    {
        private StarEntity2D starEntityFrom;

        private StarEntity2D starEntityTo;

        private RenderStates render;

        private Clock timer = new Clock();

        private int radius;
        private Vector2f center;

        public CurvedStarLinkEntity2D(ALayer2D layer2D, IObject2DFactory factory, CurvedStarLinkEntity entity) :
            base(entity)
        {

            this.starEntityFrom = layer2D.GetEntity2DFromEntity(entity.StarFrom) as StarEntity2D;
            this.starEntityTo = layer2D.GetEntity2DFromEntity(entity.StarTo) as StarEntity2D;

            this.ObjectSprite.Color = Color.Red;

            this.ObjectSprite.Texture = factory.GetTextureByIndex(0);

            this.radius = (int) entity.Radius;
            this.center = new Vector2f(entity.Center.X, entity.Center.Y);

            //Shader shader = new Shader(null, null, @"D:\Projects\Metempsychoid\Assets\Graphics\Shaders\LinkSimpleFrag.frag");

            //Texture distortionMap = factory.GetTextureByIndex(0);
            //this.ObjectSprite.Texture = factory.GetTextureByIndex(0);
            //this.ObjectSprite.Texture.Repeated = true;

            //distortionMap.Repeated = true;
            //distortionMap.Smooth = true;
            //shader.SetUniform("currentTexture", new Shader.CurrentTextureType());
            //shader.SetUniform("distTexture", distortionMap);

            //render = new RenderStates(BlendMode.Alpha);
            //render.Shader = shader;

            this.UpdateScaling();

            this.Priority = 9;
        }

        protected void UpdateScaling()
        {
            if (this.starEntityTo != null && this.starEntityFrom != null)
            {
                Vector2f vectorStar = this.starEntityTo.Position - this.starEntityFrom.Position;
                Vector2f vectorCenter = this.center - this.starEntityFrom.Position;

                int height = (int)(vectorStar.OppProjection(vectorCenter).Len());
                int width = (int)(vectorStar.Projection(vectorCenter).Len());

                this.ObjectSprite.TextureRect = new IntRect(0, 0, width + StarLinkEntity2D.WIDTH_LINK / 2, height + StarLinkEntity2D.WIDTH_LINK / 2);

                this.ObjectSprite.Origin = new Vector2f(StarLinkEntity2D.WIDTH_LINK / 2, this.ObjectSprite.TextureRect.Height);

                this.ObjectSprite.Scale = new Vector2f(1, Math.Sign(this.radius));
            }
        }

        public override Vector2f Position
        {
            set
            {
                base.Position = value;

                this.UpdateScaling();
            }
        }

        public override float Rotation
        {
            set
            {
                base.Rotation = value;

                this.UpdateScaling();
            }
        }

        //public override void DrawIn(RenderWindow window, Time deltaTime)
        //{
        //    render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());

        //    window.Draw(this.ObjectSprite, this.render);
        //}

    }
}
