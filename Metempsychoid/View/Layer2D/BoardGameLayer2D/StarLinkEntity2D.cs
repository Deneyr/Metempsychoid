using Metempsychoid.Animation;
using Metempsychoid.Maths;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.View.Animation;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class StarLinkEntity2D: AEntity2D
    {
        private StarEntity2D starEntityFrom;

        private StarEntity2D starEntityTo;

        RenderStates render;

        Clock timer = new Clock();

        public StarLinkEntity2D(ALayer2D layer2D, IObject2DFactory factory, StarLinkEntity entity) :
            base(entity)
        {

            this.starEntityFrom = layer2D.GetEntity2DFromEntity(entity.StarFrom) as StarEntity2D;
            this.starEntityTo = layer2D.GetEntity2DFromEntity(entity.StarTo) as StarEntity2D;

            this.ObjectSprite.Color = Color.Blue;

            Shader shader = new Shader(null, null, @"D:\Projects\Metempsychoid\Assets\Graphics\Shaders\LinkSimpleFrag.frag");

            Texture distortionMap = factory.GetTextureByIndex(0);
            this.ObjectSprite.Texture = factory.GetTextureByIndex(0);
            this.ObjectSprite.Texture.Repeated = true;

            distortionMap.Repeated = true;
            distortionMap.Smooth = true;
            shader.SetUniform("currentTexture", new Shader.CurrentTextureType());
            shader.SetUniform("distTexture", distortionMap);

            render = new RenderStates(BlendMode.Alpha);
            render.Shader = shader;

            this.UpdateScaling();

            this.Priority = 9;

            //this.ObjectSprite.Color = Color.Blue;
            //this.ObjectSprite.Texture = factory.GetTextureByIndex(0);

            //this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            //SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(4), AnimationType.LOOP);

            //IAnimation anim = new ZoomAnimation(1, 1.5f, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            //sequence.AddAnimation(0, anim);

            //anim = new ZoomAnimation(1.5f, 1, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            //sequence.AddAnimation(2, anim);

            //this.animationsList.Add(sequence);

            //this.PlayAnimation(0);
        }

        protected void UpdateScaling()
        {
            if (this.starEntityTo != null && this.starEntityFrom != null)
            {
                int width = (int)(this.starEntityTo.Position - this.starEntityFrom.Position).Len();

                this.ObjectSprite.TextureRect = new IntRect(0, 0, width, 50);

                this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);
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

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());

            window.Draw(this.ObjectSprite, this.render);
        }

    }
}
