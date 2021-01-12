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

        protected override void InternalInitializeLayer(PlayerData playerData)
        {
            StarEntity star = new StarEntity(this);
            this.AddStar(star);

            StarEntity star1 = new StarEntity(this);
            star1.Position = new Vector2f(-400, -400);
            this.AddStar(star1);

            StarEntity star2 = new StarEntity(this);
            star2.Position = new Vector2f(400, 400);
            this.AddStar(star2);

            star = new StarEntity(this);
            star.Position = new Vector2f(0, -400);
            this.AddStar(star);

            this.AddStarLink(star, star1);
            this.AddStarLink(star2, star);
            this.AddStarLink(star1, star2);
        }
    }
}
