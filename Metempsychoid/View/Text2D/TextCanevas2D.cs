using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Text2D
{
    public class TextCanevas2D : AEntity2D
    {
        protected static TextParagraphFactory textParagraphFactory;

        protected List<TextParagraph2D> textParagraph2Ds;

        public override Vector2f Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                if (base.Position != value)
                {
                    base.Position = value;

                    foreach (TextParagraph2D textParagraph2D in this.textParagraph2Ds)
                    {
                        textParagraph2D.Position = base.Position;
                    }
                }
            }
        }

        public override float Rotation
        {
            get
            {
                return base.Rotation;
            }

            set
            {
                if (base.Rotation != value)
                {
                    base.Rotation = value;

                    foreach (TextParagraph2D textParagraph2D in this.textParagraph2Ds)
                    {
                        textParagraph2D.Rotation = base.Rotation;
                    }
                }
            }
        }

        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }
            set
            {
                if (base.Zoom != value)
                {
                    base.Zoom = value;

                    foreach (TextParagraph2D textParagraph2D in this.textParagraph2Ds)
                    {
                        textParagraph2D.Zoom = base.Zoom;
                    }
                }
            }
        }

        public override IntRect Canevas
        {
            get
            {
                return base.Canevas;
            }
            set
            {
                if (base.Canevas != value)
                {
                    base.Canevas = value;

                    foreach (TextParagraph2D textParagraph2D in this.textParagraph2Ds)
                    {
                        textParagraph2D.Canevas = base.Canevas;
                    }
                }
            }
        }

        public void CreateTextParagraph2D(Vector2f positionOffset, float rotationOffset, uint characterSize)
        {
            TextParagraph2D textParagraph2D = new TextParagraph2D(this, positionOffset, rotationOffset, characterSize);

            this.textParagraph2Ds.Add(textParagraph2D);
        }

        public void UpdateTextOfParagraph(int index, string id)
        {
            textParagraphFactory.CreateTextTokensIn(this.textParagraph2Ds[index], id);
        }

        static TextCanevas2D()
        {
            textParagraphFactory = new TextParagraphFactory();

            textParagraphFactory.Culture = "fr";
        }

        public TextCanevas2D(ALayer2D parentLayer):
            base(parentLayer)
        {

        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                foreach(TextParagraph2D textParagraph2D in this.textParagraph2Ds)
                {
                    textParagraph2D.DrawIn(window, deltaTime);
                }
            }
        }
    }
}
