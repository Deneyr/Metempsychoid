using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer
{
    public class BoardGameLayer: EntityLayer.EntityLayer
    {
        public HashSet<StarEntity> StarSystem
        {
            get;
            protected set;
        }

        public HashSet<StarLinkEntity> StarLinkSystem
        {
            get;
            protected set;
        }

        public Dictionary<StarEntity, HashSet<StarLinkEntity>> starToLinks;

        public BoardGameLayer()
        {
            this.StarSystem = new HashSet<StarEntity>();
            this.StarLinkSystem = new HashSet<StarLinkEntity>();

            this.starToLinks = new Dictionary<StarEntity, HashSet<StarLinkEntity>>();

            this.TypesInChunk.Add(typeof(StarEntity));
            this.TypesInChunk.Add(typeof(StarLinkEntity));
        }

        protected void AddStar(StarEntity starEntity)
        {
            this.StarSystem.Add(starEntity);
            this.AddEntityToLayer(starEntity);

            this.starToLinks.Add(starEntity, new HashSet<StarLinkEntity>());
        }

        protected void RemoveStar(StarEntity starEntity)
        {
            this.StarSystem.Remove(starEntity);
            this.RemoveEntityFromLayer(starEntity);

            HashSet<StarLinkEntity> linksList = this.starToLinks[starEntity];
            this.starToLinks.Remove(starEntity);

            foreach(StarLinkEntity link in linksList)
            {
                this.RemoveStarLink(link);
            }

            starEntity.Dispose();
        }

        protected StarLinkEntity AddStarLink(StarEntity starEntityFrom, StarEntity starEntityTo)
        {
            StarLinkEntity link = new StarLinkEntity(this, starEntityFrom, starEntityTo);

            this.StarLinkSystem.Add(link);
            this.AddEntityToLayer(link);

            this.starToLinks[starEntityFrom].Add(link);
            this.starToLinks[starEntityTo].Add(link);

            return link;
        }

        protected StarLinkEntity AddCurvedStarLink(StarEntity starEntityFrom, StarEntity starEntityTo, float radius)
        {
            StarLinkEntity link = new CurvedStarLinkEntity(this, starEntityFrom, starEntityTo, radius);

            this.StarLinkSystem.Add(link);
            this.AddEntityToLayer(link);

            this.starToLinks[starEntityFrom].Add(link);
            this.starToLinks[starEntityTo].Add(link);

            return link;
        }

        protected void RemoveStarLink(StarLinkEntity starLinkEntity)
        {
            this.StarLinkSystem.Remove(starLinkEntity);
            this.RemoveEntityFromLayer(starLinkEntity);

            if (this.starToLinks.ContainsKey(starLinkEntity.StarFrom))
            {
                this.starToLinks[starLinkEntity.StarFrom].Remove(starLinkEntity);
            }

            if (this.starToLinks.ContainsKey(starLinkEntity.StarTo))
            {
                this.starToLinks[starLinkEntity.StarTo].Remove(starLinkEntity);
            }
        }

        protected override void InternalInitializeLayer(World world)
        {
            float cosPi4 = (float) Math.Cos(Math.PI / 4);

            // Inner circle
            StarEntity star = new StarEntity(this);
            this.AddStar(star);


            StarEntity star1 = new StarEntity(this);
            star1.Position = new Vector2f(-400 * cosPi4, -400 * cosPi4);
            this.AddStar(star1);

            StarEntity star2 = new StarEntity(this);
            star2.Position = new Vector2f(0, -400);
            this.AddStar(star2);

            StarEntity star3 = new StarEntity(this);
            star3.Position = new Vector2f(400 * cosPi4, -400 * cosPi4);
            this.AddStar(star3);


            StarEntity star4 = new StarEntity(this);
            star4.Position = new Vector2f(400 * cosPi4, 400 * cosPi4);
            this.AddStar(star4);

            StarEntity star5 = new StarEntity(this);
            star5.Position = new Vector2f(0, 400);
            this.AddStar(star5);

            StarEntity star6 = new StarEntity(this);
            star6.Position = new Vector2f(-400 * cosPi4, 400 * cosPi4);
            this.AddStar(star6);

            this.AddStarLink(star, star2);
            this.AddStarLink(star, star5);
            this.AddCurvedStarLink(star1, star2, 400);
            this.AddCurvedStarLink(star2, star3, 400);
            this.AddCurvedStarLink(star4, star5, 400);
            this.AddCurvedStarLink(star5, star6, 400);

            // Circle
            StarEntity star7 = new StarEntity(this);
            star7.Position = new Vector2f(-800, 0);
            this.AddStar(star7);

            StarEntity star8 = new StarEntity(this);
            star8.Position = new Vector2f(0, -800);
            this.AddStar(star8);

            StarEntity star9 = new StarEntity(this);
            star9.Position = new Vector2f(800, 0);
            this.AddStar(star9);

            StarEntity star10 = new StarEntity(this);
            star10.Position = new Vector2f(0, 800);
            this.AddStar(star10);

            this.AddStarLink(star, star7);
            this.AddStarLink(star, star9);
            this.AddStarLink(star1, star7);
            this.AddStarLink(star6, star7);
            this.AddStarLink(star3, star9);
            this.AddStarLink(star4, star9);
            this.AddStarLink(star1, star8);
            this.AddStarLink(star3, star8);
            this.AddStarLink(star4, star10);
            this.AddStarLink(star6, star10);
            this.AddCurvedStarLink(star7, star8, 800);
            this.AddCurvedStarLink(star8, star9, 800);
            this.AddCurvedStarLink(star9, star10, 800);
            this.AddCurvedStarLink(star10, star7, 800);

            // out circle
            StarEntity star11 = new StarEntity(this);
            star11.Position = new Vector2f(-1200, 0);
            this.AddStar(star11);

            StarEntity star12 = new StarEntity(this);
            star12.Position = new Vector2f(0, -1200);
            this.AddStar(star12);

            StarEntity star13 = new StarEntity(this);
            star13.Position = new Vector2f(1200, 0);
            this.AddStar(star13);

            StarEntity star14 = new StarEntity(this);
            star14.Position = new Vector2f(0, 1200);
            this.AddStar(star14);

            this.AddStarLink(star7, star11);
            this.AddStarLink(star9, star13);
            this.AddCurvedStarLink(star11, star12, 1200);
            this.AddCurvedStarLink(star12, star13, 1200);
            this.AddCurvedStarLink(star13, star14, 1200);
            this.AddCurvedStarLink(star14, star11, 1200);
        }
    }
}
