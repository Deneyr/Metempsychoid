using Metempsychoid.View.Controls;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Metempsychoid.View.Text2D.TextParagraph2D;

namespace Metempsychoid.View.Text2D
{
    public class TextCanevas2D : AEntity2D
    {
        protected static TextParagraphFactory textParagraphFactory;

        protected List<TextParagraph2D> textParagraph2Ds;
        //protected List<IHitRect> textParagraphHitRect2Ds;

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

        public override Color SpriteColor
        {
            get
            {
                return base.SpriteColor;
            }

            set
            {
                if (base.SpriteColor != value)
                {
                    base.SpriteColor = value;

                    foreach (TextParagraph2D textParagraph2D in this.textParagraph2Ds)
                    {
                        textParagraph2D.SpriteColor = base.SpriteColor;
                    }
                }
            }
        }

        public void ActiveOnlyParagraph(int index)
        {
            int i = 0;
            foreach (TextParagraph2D paragraph in this.textParagraph2Ds)
            {
                if(i == index)
                {
                    paragraph.IsActive = true;
                }
                else
                {
                    paragraph.IsActive = false;
                }
                i++;
            }
        }

        public void ActiveAllParagraphs()
        {
            foreach (TextParagraph2D paragraph in this.textParagraph2Ds)
            {
                paragraph.IsActive = true;
            }
        }

        public void CreateTextParagraph2D(Vector2f positionOffsetTopLeft, Vector2f positionOffsetBotRight, Alignment alignment, uint characterSize)
        {
            TextParagraph2D textParagraph2D = new TextParagraph2D(this, positionOffsetTopLeft, positionOffsetBotRight, alignment, characterSize);

            this.textParagraph2Ds.Add(textParagraph2D);
        }

        //public void AddTextParagraph2D(TextParagraph2D textParagraphToAdd)
        //{
        //    this.textParagraph2Ds.Add(textParagraphToAdd);
        //    if (textParagraphToAdd is IHitRect)
        //    {
        //        this.textParagraphHitRect2Ds.Add(textParagraphToAdd as IHitRect);
        //    }
        //}

        public void UpdateTextOfParagraph(int index, string id, params string[] parameters)
        {
            textParagraphFactory.CreateTextTokensIn(this.textParagraph2Ds[index], id, parameters);
        }

        public void CreateTextOfParagraph(int index, string text, string tokenType, Color fillColor)
        {
            List<TextToken2D> textTokens = new List<TextToken2D>();

            textParagraphFactory.AppendTextTokens(textTokens, text, tokenType, fillColor);

            this.textParagraph2Ds[index].UpdateTextTokens(textTokens);
        }

        public void LaunchTextOfParagraphScrolling(int index, float speed)
        {
            this.textParagraph2Ds[index].LaunchAnimationScrolling(speed);
        }

        static TextCanevas2D()
        {
            textParagraphFactory = new TextParagraphFactory();

            textParagraphFactory.Culture = "fr";
        }

        public TextCanevas2D(ALayer2D parentLayer):
            base(parentLayer, true)
        {
            this.textParagraph2Ds = new List<TextParagraph2D>();
            //this.textParagraphHitRect2Ds = new List<IHitRect>();
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            foreach (TextParagraph2D textParagraph2D in this.textParagraph2Ds)
            {
                textParagraph2D.UpdateGraphics(deltaTime);
            }
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

        //public void OnMousePressed(ALayer2D parentLayer, ControlEventType eventType)
        //{
        //    Vector2i mousePosition = parentLayer.MousePosition;

        //    foreach (IHitRect textParagraph in this.textParagraphHitRect2Ds)
        //    {
        //        if (textParagraph.IsFocusable(parentLayer)
        //            && textParagraph.HitZone.Contains(mousePosition.X, mousePosition.Y))
        //        {
        //            textParagraph.OnMousePressed(parentLayer, eventType);
        //        }
        //    }
        //}

        //public void OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType)
        //{
        //    Vector2i mousePosition = parentLayer.MousePosition;

        //    foreach (IHitRect textParagraph in this.textParagraphHitRect2Ds)
        //    {
        //        if (textParagraph.IsFocusable(parentLayer)
        //            && textParagraph.HitZone.Contains(mousePosition.X, mousePosition.Y))
        //        {
        //            textParagraph.OnMouseReleased(parentLayer, eventType);
        //        }
        //    }
        //}

        //public void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType)
        //{
        //    Vector2i mousePosition = parentLayer.MousePosition;

        //    foreach (IHitRect textParagraph in this.textParagraphHitRect2Ds)
        //    {
        //        if (textParagraph.IsFocusable(parentLayer)
        //            && textParagraph.HitZone.Contains(mousePosition.X, mousePosition.Y))
        //        {
        //            textParagraph.OnMouseFocused(parentLayer, eventType);
        //        }
        //    }
        //}

        //public void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType)
        //{
        //    Vector2i mousePosition = parentLayer.MousePosition;

        //    foreach (IHitRect textParagraph in this.textParagraphHitRect2Ds)
        //    {
        //        if (textParagraph.IsFocusable(parentLayer)
        //            && textParagraph.HitZone.Contains(mousePosition.X, mousePosition.Y))
        //        {
        //            textParagraph.OnMouseUnFocused(parentLayer, eventType);
        //        }
        //    }
        //}

        //public bool IsFocusable(ALayer2D parentLayer)
        //{
        //    return true;
        //}

        public override void Dispose()
        {
            foreach(TextParagraph2D textParagraph in this.textParagraph2Ds)
            {
                textParagraph.Dispose();
            }

            base.Dispose();
        }
    }
}
