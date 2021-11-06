using Astrategia.Animation;
using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.View.Animation;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Maths;
using Astrategia.View.Controls;

namespace Astrategia.View.Layer2D.BoardGameLayer2D
{
    public class CJStarDomain2D : AEntity2D, IHitRect
    {
        private static int NB_MAX_POINTS = 20;
        private static int MARGIN_DOMAIN = 200;

        protected DomainState domainState;

        protected RenderStates render;

        protected List<Vector2f> domainPoints;
        protected List<StarEntity2D> domainStars;

        protected bool isFocused;

        private Color targetedColor;

        private bool isFilled;

        public override bool IsActive
        {
            get
            {
                return this.domainState != DomainState.NOT_ACTIVE;
            }
            set
            {

            }
        }

        public bool IsFocused
        {
            get
            {
                return this.isFocused;
            }
            set
            {
                if (this.isFocused != value)
                {
                    this.isFocused = value;

                    render.Shader.SetUniform("isFocused", this.isFocused);
                }
            }
        }

        public Color TargetedColor
        {
            get
            {
                return this.targetedColor;
            }
            set
            {
                if (this.targetedColor != value)
                {
                    this.targetedColor = value;

                    this.PlayAnimation(new ColorAnimation(base.SpriteColor, this.TargetedColor, Time.FromSeconds(2), Astrategia.Animation.AnimationType.ONETIME, Astrategia.Animation.InterpolationMethod.LINEAR));
                }
            }
        }

        public List<Vector2f> DomainPoints
        {
            get
            {
                return this.domainPoints;
            }
            private set
            {
                this.domainPoints = value;

                //this.PopulateNewDomainPoints2();

                Vec2[] points = new Vec2[NB_MAX_POINTS];
                int i = 0;
                IntRect canevas = this.Canevas;
                foreach(Vector2f point in this.domainPoints)
                {
                    points[i] = new Vec2((point.X - this.Position.X + canevas.Width / 2) / this.ObjectSprite.Texture.Size.X, (point.Y - this.Position.Y + canevas.Height / 2) / this.ObjectSprite.Texture.Size.Y);
                    i++;
                }

                this.render.Shader.SetUniformArray("points", points);
                this.render.Shader.SetUniform("pointsLen", this.domainPoints.Count);
            }
        }

        public IntRect HitZone
        {
            get
            {
                return new IntRect((int)(this.Position.X - this.Canevas.Width / 2),
                    (int)(this.Position.Y - this.Canevas.Height / 2),
                    this.Canevas.Width,
                    this.Canevas.Height);
            }
        }

        public CJStarDomain2D(ALayer2D layer2D, IObject2DFactory factory, CJStarDomain entity) :
            base(layer2D, factory, entity)
        {
            // TO REMOVE
            //this.Test();

            //this.widthRatio = 0;
            //this.heightRatio = 0;

            this.targetedColor = Color.Black;

            Shader shader = new Shader(null, null, @"Assets\Graphics\Shaders\StarDomain.frag");

            Texture distortionMap = factory.GetTextureById("distorsionTexture");
            this.ObjectSprite.Texture = factory.GetTextureById("distorsionTexture");
            this.ObjectSprite.Texture.Repeated = true;

            distortionMap.Repeated = true;
            distortionMap.Smooth = true;
            shader.SetUniform("currentTexture", new Shader.CurrentTextureType());

            render = new RenderStates(BlendMode.Alpha);
            render.Shader = shader;

            this.isFocused = true;
            this.IsFocused = false;

            this.Priority = entity.Priority;

            this.domainStars = entity.Domain.Select(pElem => layer2D.GetEntity2DFromEntity(pElem) as StarEntity2D).ToList();
            this.isFilled = entity.IsFilled;
            shader.SetUniform("isFilled", this.isFilled);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(6), AnimationType.ONETIME);
            IAnimation anim = new ZoomAnimation(1f, 2f, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(0, anim);

            anim = new ZoomAnimation(2f, 1f, Time.FromSeconds(3), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(2, anim);
            this.animationsList.Add(sequence);

            this.UpdateScaling(entity);

            this.StartNotActiveState();
        }

        protected void PopulateNewDomainPoints()
        {
            List<Vector2f> domainPointsCopy = new List<Vector2f>();

            for(int i = 0; i < this.domainPoints.Count; i++)
            {
                Vector2f firstPoint = this.domainPoints[i];
                Vector2f secondPoint = this.domainPoints[(i + 1) % this.domainPoints.Count];
                Vector2f thirdPoint = this.domainPoints[(i + 2) % this.domainPoints.Count];

                Vector2f normVector1 = secondPoint - firstPoint;
                normVector1 = normVector1 / normVector1.Len();

                Vector2f normVector2 = thirdPoint - secondPoint;
                normVector2 = normVector2 / normVector2.Len();

                float distX1 = normVector1.X != 0 ? (secondPoint.X - firstPoint.X) / normVector1.X : (secondPoint.Y - firstPoint.Y) / normVector1.Y;
                float distX2 = normVector2.X != 0 ? distX1 + (thirdPoint.X - secondPoint.X) / normVector2.X : distX1 + (thirdPoint.Y - secondPoint.Y) / normVector2.Y;

                Vector2f firstPointX = new Vector2f(0, firstPoint.X);
                Vector2f secondPointX = new Vector2f(distX1, secondPoint.X);
                Vector2f thirdPointX = new Vector2f(distX2, thirdPoint.X);

                float x = distX1 / 2;
                float newPointX = this.QuadraticInterpolate(x, firstPointX, secondPointX, thirdPointX);

                Vector2f firstPointY = new Vector2f(0, firstPoint.Y);
                Vector2f secondPointY = new Vector2f(distX1, secondPoint.Y);
                Vector2f thirdPointY = new Vector2f(distX2, thirdPoint.Y);

                float newPointY = this.QuadraticInterpolate(x, firstPointY, secondPointY, thirdPointY);

                domainPointsCopy.Add(this.domainPoints[i]);
                domainPointsCopy.Add(new Vector2f(newPointX, newPointY));
            }

            this.domainPoints = domainPointsCopy;
        }

        protected void PopulateNewDomainPoints2()
        {
            List<Vector2f> domainPointsCopy = new List<Vector2f>();

            for (int i = 0; i < this.domainPoints.Count; i++)
            {
                Vector2f firstPoint;
                if (i == 0)
                {
                    firstPoint = this.domainPoints[this.domainPoints.Count - 1];
                }
                else
                {
                    firstPoint = this.domainPoints[i - 1];
                }
                Vector2f secondPoint = this.domainPoints[i];
                Vector2f thirdPoint = this.domainPoints[(i + 1) % this.domainPoints.Count];
                Vector2f fourthPoint = this.domainPoints[(i + 2) % this.domainPoints.Count];

                Vector2f normVector1 = secondPoint - firstPoint;
                normVector1 = normVector1 / normVector1.Len();

                Vector2f normVector2 = thirdPoint - secondPoint;
                normVector2 = normVector2 / normVector2.Len();

                Vector2f normVector3 = fourthPoint - thirdPoint;
                normVector3 = normVector3 / normVector3.Len();

                float distX1 = normVector1.X != 0 ? (secondPoint.X - firstPoint.X) / normVector1.X : (secondPoint.Y - firstPoint.Y) / normVector1.Y;
                float distX2 = normVector2.X != 0 ? distX1 + (thirdPoint.X - secondPoint.X) / normVector2.X : distX1 + (thirdPoint.Y - secondPoint.Y) / normVector2.Y;
                float distX3 = normVector3.X != 0 ? distX2 + (fourthPoint.X - thirdPoint.X) / normVector3.X : distX2 + (fourthPoint.Y - thirdPoint.Y) / normVector3.Y;

                Vector2f firstPointX = new Vector2f(0, firstPoint.X);
                Vector2f secondPointX = new Vector2f(distX1, secondPoint.X);
                Vector2f thirdPointX = new Vector2f(distX2, thirdPoint.X);
                Vector2f fourthPointX = new Vector2f(distX3, fourthPoint.X);

                float x = (distX2 + distX1) / 2;
                float newPointX = this.CubicInterpolate(x, firstPointX, secondPointX, thirdPointX, fourthPointX);

                Vector2f firstPointY = new Vector2f(0, firstPoint.Y);
                Vector2f secondPointY = new Vector2f(distX1, secondPoint.Y);
                Vector2f thirdPointY = new Vector2f(distX2, thirdPoint.Y);
                Vector2f fourthPointY = new Vector2f(distX3, fourthPoint.Y);

                float newPointY = this.CubicInterpolate(x, firstPointY, secondPointY, thirdPointY, fourthPointY);

                domainPointsCopy.Add(this.domainPoints[i]);
                domainPointsCopy.Add(new Vector2f(newPointX, newPointY));
            }

            this.domainPoints = domainPointsCopy;
        }

        private float QuadraticInterpolate(float x, Vector2f point0, Vector2f point1, Vector2f point2)
        {
            return point0.Y * ((x - point1.X) * (x - point2.X)) / ((point0.X - point1.X) * (point0.X - point2.X))
                 + point1.Y * ((x - point0.X) * (x - point2.X)) / ((point1.X - point0.X) * (point1.X - point2.X))
                 + point2.Y * ((x - point0.X) * (x - point1.X)) / ((point2.X - point0.X) * (point2.X - point1.X));
        }

        private float CubicInterpolate(float x, Vector2f point0, Vector2f point1, Vector2f point2, Vector2f point3)
        {
            return point0.Y * ((x - point1.X) * (x - point2.X) * (x - point3.X)) / ((point0.X - point1.X) * (point0.X - point2.X) * (point0.X - point3.X))
                 + point1.Y * ((x - point0.X) * (x - point2.X) * (x - point3.X)) / ((point1.X - point0.X) * (point1.X - point2.X) * (point1.X - point3.X))
                 + point2.Y * ((x - point0.X) * (x - point1.X) * (x - point3.X)) / ((point2.X - point0.X) * (point2.X - point1.X) * (point2.X - point3.X))
                 + point3.Y * ((x - point0.X) * (x - point1.X) * (x - point2.X)) / ((point3.X - point0.X) * (point3.X - point1.X) * (point3.X - point2.X));
        }

        private void Test()
        {
            this.domainPoints = new List<Vector2f>()
            {
                new Vector2f(0, 10),
                new Vector2f(3, 8),
                new Vector2f(2.5f, 5),
                new Vector2f(-2.5f, 5),
                new Vector2f(-3, 8)
            };

            this.PopulateNewDomainPoints2();

            Console.WriteLine();
        }

        protected virtual void UpdateScaling(CJStarDomain entity)
        {
            Vector2f topLeft = new Vector2f(0, 0);
            Vector2f bottomRight = new Vector2f(0, 0);

            bool firstPoint = true;

            foreach(StarEntity starEntity in entity.Domain)
            {
                if(firstPoint || topLeft.X > starEntity.Position.X)
                {
                    topLeft.X = starEntity.Position.X;
                }

                if (firstPoint || topLeft.Y > starEntity.Position.Y)
                {
                    topLeft.Y = starEntity.Position.Y;
                }

                if (firstPoint || bottomRight.X < starEntity.Position.X)
                {
                    bottomRight.X = starEntity.Position.X;
                }

                if (firstPoint || bottomRight.Y < starEntity.Position.Y)
                {
                    bottomRight.Y = starEntity.Position.Y;
                }

                firstPoint = false;
            }

            float width = bottomRight.X - topLeft.X + MARGIN_DOMAIN * 3;
            float height = bottomRight.Y - topLeft.Y + MARGIN_DOMAIN * 3;
            //topLeft -= new Vector2f(MARGIN_DOMAIN, MARGIN_DOMAIN);

            this.ObjectSprite.TextureRect = new IntRect(0, 0, (int) width, (int) height);
            this.ObjectSprite.Origin = new Vector2f(width / 2, height / 2);

            this.ObjectSprite.Position = (bottomRight + topLeft) / 2;

            //this.WidthRatio = ((float)width) / this.ObjectSprite.Texture.Size.X;
            //this.HeightRatio = ((float)height) / this.ObjectSprite.Texture.Size.Y;



            this.DomainPoints = this.PopulateNewDomainPoints3(entity); //entity.Domain.Select(pElem => pElem.Position).ToList();

            this.render.Shader.SetUniform("margin", ((float)MARGIN_DOMAIN) / this.ObjectSprite.Texture.Size.X);
        }

        private List<Vector2f> PopulateNewDomainPoints3(CJStarDomain entity)
        {
            List<Vector2f> pointsToReturn = new List<Vector2f>();

            for(int i = 0; i < entity.Domain.Count; i++)
            {
                StarEntity currentStarEntity = entity.Domain[i];
                StarEntity nextStarEntity = entity.Domain[(i + 1) % entity.Domain.Count];

                pointsToReturn.Add(currentStarEntity.Position);

                if (entity.DomainLinks.TryGetValue(currentStarEntity, out StarLinkEntity currentLink) && currentLink is CurvedStarLinkEntity)
                {
                    int sign = currentLink.StarFrom == currentStarEntity ? 1 : -1;

                    float radiusLink = (currentLink as CurvedStarLinkEntity).Radius * sign;

                    Vector2f currentToNextNorm = nextStarEntity.Position - currentStarEntity.Position;
                    float lenCurrentToNext = currentToNextNorm.Len();
                    currentToNextNorm = currentToNextNorm / lenCurrentToNext;

                    float angleToRotate = (float) Math.Acos(lenCurrentToNext / (2 * radiusLink));

                    Vector2f currentToCenter = currentToNextNorm.Rotate(angleToRotate) * radiusLink;
                    Vector2f centerPoint = currentStarEntity.Position + currentToCenter;

                    Vector2f newPoint = centerPoint + (-currentToCenter).Rotate(Math.Sign(radiusLink) * sign * (Math.PI / 2 - angleToRotate));

                    pointsToReturn.Add(newPoint);
                }
                else
                {
                    Vector2f firstPoint;
                    if (i == 0)
                    {
                        firstPoint = entity.Domain[entity.Domain.Count - 1].Position;
                    }
                    else
                    {
                        firstPoint = entity.Domain[i - 1].Position;
                    }
                    Vector2f secondPoint = entity.Domain[i].Position;
                    Vector2f thirdPoint = entity.Domain[(i + 1) % entity.Domain.Count].Position;
                    Vector2f fourthPoint = entity.Domain[(i + 2) % entity.Domain.Count].Position;

                    Vector2f normVector1 = secondPoint - firstPoint;
                    normVector1 = normVector1 / normVector1.Len();

                    Vector2f normVector2 = thirdPoint - secondPoint;
                    normVector2 = normVector2 / normVector2.Len();

                    Vector2f normVector3 = fourthPoint - thirdPoint;
                    normVector3 = normVector3 / normVector3.Len();

                    float distX1 = normVector1.X != 0 ? (secondPoint.X - firstPoint.X) / normVector1.X : (secondPoint.Y - firstPoint.Y) / normVector1.Y;
                    float distX2 = normVector2.X != 0 ? distX1 + (thirdPoint.X - secondPoint.X) / normVector2.X : distX1 + (thirdPoint.Y - secondPoint.Y) / normVector2.Y;
                    float distX3 = normVector3.X != 0 ? distX2 + (fourthPoint.X - thirdPoint.X) / normVector3.X : distX2 + (fourthPoint.Y - thirdPoint.Y) / normVector3.Y;

                    Vector2f firstPointX = new Vector2f(0, firstPoint.X);
                    Vector2f secondPointX = new Vector2f(distX1, secondPoint.X);
                    Vector2f thirdPointX = new Vector2f(distX2, thirdPoint.X);
                    Vector2f fourthPointX = new Vector2f(distX3, fourthPoint.X);

                    float x = (distX2 + distX1) / 2;
                    float newPointX = this.CubicInterpolate(x, firstPointX, secondPointX, thirdPointX, fourthPointX);

                    Vector2f firstPointY = new Vector2f(0, firstPoint.Y);
                    Vector2f secondPointY = new Vector2f(distX1, secondPoint.Y);
                    Vector2f thirdPointY = new Vector2f(distX2, thirdPoint.Y);
                    Vector2f fourthPointY = new Vector2f(distX3, fourthPoint.Y);

                    float newPointY = this.CubicInterpolate(x, firstPointY, secondPointY, thirdPointY, fourthPointY);

                    pointsToReturn.Add(new Vector2f(newPointX, newPointY));
                }
            }

            return pointsToReturn;
        }

        private void StartNotActiveState()
        {
            this.domainState = DomainState.NOT_ACTIVE;

            Color newColor = this.TargetedColor;
            newColor.A = 0;
            this.TargetedColor = newColor;
        }

        private void StartActiveState()
        {
            this.domainState = DomainState.ACTIVE;

            Color newColor = this.TargetedColor;
            newColor.A = 255;
            this.TargetedColor = newColor;
        }

        private void UpdateNotActive(Time deltaTime)
        {
            StarEntity2D starNotActive = this.domainStars.FirstOrDefault(pElem => pElem.StarEntityState != StarEntity2D.StarState.ACTIVE);

            if(starNotActive == null)
            {
                this.StartActiveState();
            }
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());

            switch (this.domainState)
            {
                case DomainState.ACTIVE:

                    break;
                case DomainState.NOT_ACTIVE:
                    this.UpdateNotActive(deltaTime);
                    break;
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                window.Draw(this.ObjectSprite, this.render);
            }
        }

        public bool IsFocusable(ALayer2D parentLayer)
        {
            return true;
        }

        public bool IsPointHit(ALayer2D parentLayer, Vector2i position)
        {
            return this.HitZone.Contains(position.X, position.Y) && this.IsPointInsideDomain(new Vector2f(position.X, position.Y));
        }

        public bool OnMousePressed(ALayer2D parentLayer, ControlEventType eventType)
        {
            return true;
        }

        public bool OnMouseReleased(ALayer2D parentLayer, ControlEventType eventType)
        {
            return true;
        }

        public bool OnMouseClicked(ALayer2D parentLayer, ControlEventType eventType)
        {
            return true;
        }

        public void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            CJStarDomain domainEntity = parentLayer.GetEntityFromEntity2D(this) as CJStarDomain;

            if (domainEntity != null)
            {
                parentLayer.SendEventToWorld(Model.Event.EventType.FOCUS_DOMAIN_BOARD, domainEntity, null);
            }
        }

        public void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            parentLayer.SendEventToWorld(Model.Event.EventType.FOCUS_DOMAIN_BOARD, null, null);
        }

        private bool IsPointInsideDomain(Vector2f coordinate)
        {
            bool result = false;

            Vector2f origin = new Vector2f(-100000, -100000);

            float minDist = int.MaxValue;

            int nbIntersect = 0;
            for (int i = 0; i < this.domainPoints.Count; i++)
            {
                Vector2f point1 = this.domainPoints[i];
                Vector2f point2;
                if (i == this.domainPoints.Count - 1)
                {
                    point2 = this.domainPoints[0];
                }
                else
                {
                    point2 = this.domainPoints[i + 1];
                }

                float num1 = point1.X * point2.Y - point1.Y * point2.X;
                float num2 = origin.X * coordinate.Y - origin.Y * coordinate.X;
                float denum = (point1.X - point2.X) * (origin.Y - coordinate.Y) - (point1.Y - point2.Y) * (origin.X - coordinate.X);

                if (denum != 0)
                {
                    float intersecX = (num1 * (origin.X - coordinate.X) - num2 * (point1.X - point2.X)) / denum;
                    float intersecY = (num1 * (origin.Y - coordinate.Y) - num2 * (point1.Y - point2.Y)) / denum;

                    Vector2f intersect = new Vector2f(intersecX, intersecY);

                    if (intersect != point2
                        && (intersect - point1).Dot(intersect - point2) < 0 
                        && (intersect - origin).Dot(intersect - coordinate) < 0)
                    {
                        nbIntersect++;
                    }
                }

                Vector2f firstVector = coordinate - point1;

                Vector2f normalizedEdge = (point2 - point1).Normalize();
                //vec2 vector = vector - normalizedEdge * dot(normalizedEdge, vector);

                //vec3 crossVector = cross(vec3(firstVector, 0), vec3(normalizedEdge, 0));
                //vec3 crossVector2 = cross(vec3(vector2, 0), vec3(-normalizedEdge, 0));

                Vector2f secondVector = coordinate - point2;
                if (normalizedEdge.Dot(firstVector) * normalizedEdge.Dot(secondVector) < 0)
                {
                    float crossLen = Math.Abs(firstVector.CrossZ(normalizedEdge));

                    if (crossLen < minDist)
                    {
                        minDist = crossLen;
                    }
                }

                float lenToPoint = firstVector.Len();
                if (lenToPoint < minDist)
                {
                    minDist = lenToPoint;
                }

            }

            if (this.isFilled)
            {
                if (nbIntersect % 2 == 1)
                {
                    result = true;
                }
            }

            if (minDist < MARGIN_DOMAIN / 2)
            {
                result = true;
            }

            return result;
        }

        public enum DomainState
        {
            NOT_ACTIVE,
            ACTIVE
        }
    }
}
