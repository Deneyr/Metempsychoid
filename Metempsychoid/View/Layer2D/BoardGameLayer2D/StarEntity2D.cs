using Metempsychoid.Animation;
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
    public class StarEntity2D : AEntity2D
    {
        RenderStates render;

        Clock timer = new Clock();

        public StarEntity2D(IObject2DFactory factory, StarEntity entity):
            base(entity)
        {

            Shader shader = new Shader(null, null, @"D:\Projects\Metempsychoid\Assets\Graphics\Shaders\StarFrag.frag");

            Texture distortionMap = factory.GetTextureByIndex(1);
            distortionMap.Repeated = true;
            distortionMap.Smooth = true;
            shader.SetUniform("currentTexture", new Shader.CurrentTextureType());
            shader.SetUniform("distortionMapTexture", distortionMap);

            render = new RenderStates(BlendMode.Alpha);
            render.Shader = shader;

            this.ObjectSprite.Color = Color.Blue;
            this.ObjectSprite.Texture = factory.GetTextureByIndex(0);

            this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(4), AnimationType.LOOP);

            IAnimation anim = new ZoomAnimation(1, 1.5f, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0, anim);

            anim = new ZoomAnimation(1.5f, 1, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(2, anim);

            this.animationsList.Add(sequence);

            this.PlayAnimation(0);
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());

            window.Draw(this.ObjectSprite, this.render);
        }

    }
}
