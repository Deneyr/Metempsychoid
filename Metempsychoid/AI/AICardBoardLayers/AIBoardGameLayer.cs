using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.AI.AICard;
using Astrategia.Model;
using Astrategia.Model.Card;
using Astrategia.Model.Layer.BoardGameLayer;
using SFML.System;

namespace Astrategia.AI.AICardBoardLayers
{
    public class AIBoardGameLayer : AAILayer
    {
        public Dictionary<AIStarEntity, HashSet<AIStarLinkEntity>> StarToLinks
        {
            get;
            private set;
        }

        public HashSet<AIStarEntity> StarSystem
        {
            get;
            protected set;
        }

        public HashSet<AIStarLinkEntity> StarLinkSystem
        {
            get;
            protected set;
        }

        public AIBoardGameLayer(AIWorld world2D, IAIObjectFactory layerFactory, ALayer layer) 
            : base(world2D, layerFactory, layer)
        {
            this.StarToLinks = new Dictionary<AIStarEntity, HashSet<AIStarLinkEntity>>();

            this.StarSystem = new HashSet<AIStarEntity>();

            this.StarLinkSystem = new HashSet<AIStarLinkEntity>();
        }

        public override void InitializeLayer(IAIObjectFactory factory)
        {
            this.StarToLinks.Clear();
            this.StarSystem.Clear();
            this.StarLinkSystem.Clear();

            base.InitializeLayer(factory);

            foreach(AIStarLinkEntity starLinkEntity in this.StarLinkSystem)
            {
                starLinkEntity.UpdateReference();

                HashSet<AIStarLinkEntity> linksConnectedTo = null;
                if (this.StarToLinks.TryGetValue(starLinkEntity.StarFrom, out linksConnectedTo) == false)
                {
                    linksConnectedTo = new HashSet<AIStarLinkEntity>();

                    this.StarToLinks.Add(starLinkEntity.StarFrom, linksConnectedTo);
                }
                linksConnectedTo.Add(starLinkEntity);

                linksConnectedTo = null;
                if (this.StarToLinks.TryGetValue(starLinkEntity.StarTo, out linksConnectedTo) == false)
                {
                    linksConnectedTo = new HashSet<AIStarLinkEntity>();

                    this.StarToLinks.Add(starLinkEntity.StarTo, linksConnectedTo);
                }
                linksConnectedTo.Add(starLinkEntity);
            }
        }

        protected override void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            base.OnEntityPropertyChanged(obj, propertyName);

            switch (propertyName)
            {
                case "CardSocketed":
                    StarEntity starEntity = obj as StarEntity;
                    AIStarEntity starEntityAI = this.objectToObjectAIs[obj] as AIStarEntity;

                    if (starEntity.CardSocketed != null)
                    {
                        starEntityAI.CardEntitySocketed = this.objectToObjectAIs[starEntity.CardSocketed] as AICardEntity;
                    }
                    else
                    {
                        starEntityAI.CardEntitySocketed = null;
                    }
                    break;
                case "IsSocketed":
                    (this.objectToObjectAIs[obj] as AICardEntity).IsSocketed = (obj as CardEntity).ParentStar != null;
                    break;
                case "IsFliped":
                    (this.objectToObjectAIs[obj] as AICardEntity).IsFliped = (obj as CardEntity).IsFliped;
                    break;
                case "IsSelected":
                    (this.objectToObjectAIs[obj] as AICardEntity).IsSelected = (obj as CardEntity).IsSelected;
                    break;
                case "IsAwakened":
                    (this.objectToObjectAIs[obj] as AICardEntity).IsAwakened = (obj as CardEntity).Card.IsAwakened;
                    break;
                case "CurrentOwner":
                    (this.objectToObjectAIs[obj] as AICardEntity).CardOwnerName = (obj as CardEntity).Card.CurrentOwner.PlayerName;
                    break;
                case "Value":
                    (this.objectToObjectAIs[obj] as AICardEntity).Value = (obj as CardEntity).Card.Value;
                    break;
                case "DomainOwner":
                    CJStarDomain starDomain = obj as CJStarDomain;
                    (this.objectToObjectAIs[obj] as AICJStarDomain).DomainOwnerName = starDomain.DomainOwner != null ? starDomain.DomainOwner.PlayerName : null;
                    break;

            }
        }

        protected override AAIEntity AddEntity(AEntity obj)
        {
            AAIEntity entityAdded = base.AddEntity(obj);

            if(entityAdded != null)
            {
                if(entityAdded is AIStarEntity)
                {
                    this.StarSystem.Add(entityAdded as AIStarEntity);
                }
                else if(entityAdded is AIStarLinkEntity)
                {
                    this.StarLinkSystem.Add(entityAdded as AIStarLinkEntity);
                }
            }

            return entityAdded;
        }

        public override void UpdateAI(Time deltaTime)
        {
            
        }
    }
}
