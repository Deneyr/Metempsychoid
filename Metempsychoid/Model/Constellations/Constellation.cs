using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public class Constellation: IDisposable
    {
        private ConstellationPattern constellationPattern;

        public HashSet<StarEntity> starsInVicinity;
        public HashSet<StarLinkEntity> starLinksInVicinity;

        public HashSet<StarEntity> starsInPattern;
        public HashSet<StarLinkEntity> starLinksInPattern;

        public Action CardAwakened;

        public Action CardUnawakened;

        public Constellation(ConstellationPattern constellationPattern)
        {
            this.constellationPattern = constellationPattern;

            this.starsInVicinity = new HashSet<StarEntity>();
            this.starLinksInVicinity = new HashSet<StarLinkEntity>();

            this.starsInPattern = new HashSet<StarEntity>();
            this.starLinksInPattern = new HashSet<StarLinkEntity>();
        }

        public void OnSocketed(StarEntity starEntity, CardEntity cardEntitySocketed)
        {
            
        }

        public void OnUnsocketed(StarEntity starEntity, CardEntity cardEntityUnsocketed)
        {
            this.starsInVicinity.Clear();
            this.starLinksInVicinity.Clear();

            this.starsInPattern.Clear();
            this.starLinksInPattern.Clear();
        }

        public void OnCardSocketed(BoardGameLayer boardGameLayer, StarEntity starEntity)
        {
            this.constellationPattern.CreateConstellationSystem(boardGameLayer, starEntity);
        }

        public void OnCardUnsocketed(BoardGameLayer boardGameLayer, StarEntity starEntity)
        {

        }

        public void Dispose()
        {
            this.starsInVicinity.Clear();
            this.starLinksInVicinity.Clear();

            this.starsInPattern.Clear();
            this.starLinksInPattern.Clear();
        }
    }
}
