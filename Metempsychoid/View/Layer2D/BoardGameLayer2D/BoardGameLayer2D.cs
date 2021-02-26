using Metempsychoid.Model;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Player;
using Metempsychoid.View.Card2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class BoardGameLayer2D : ALayer2D
    {
        public BoardGameLayer2D(World2D world2D, IObject2DFactory factory, BoardGameLayer layer) :
            base(world2D, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);
        }


        public override Vector2f Position
        {
            set
            {
                base.Position = value * 0.75f;
            }
        }

        protected override void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            base.OnEntityPropertyChanged(obj, propertyName);

            switch (propertyName)
            {
                case "CardSocketed":
                    StarEntity starEntity = obj as StarEntity;
                    StarEntity2D starEntity2D = this.objectToObject2Ds[obj] as StarEntity2D;

                    starEntity2D.SetCardSocketed(starEntity.CardSocketed);
                    break;
                case "IsSocketed":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsSocketed = (obj as CardEntity).IsSocketed;
                    break;
            }
        }

        public override bool OnControlActivated(Controls.ControlEventType eventType, string details)
        {
            Random rand = new Random();

            Player player;
            if (rand.NextDouble() < 0.5)
            {
                player = new Player(Color.Green);
            }
            else
            {
                player = new Player(Color.Red);
            }
            Card card = new Card(null, player);

            switch (eventType)
            {
                case Controls.ControlEventType.UP:
                    foreach (AEntity2D entity in this.objectToObject2Ds.Values)
                    {
                        if (entity is CardEntity2D && rand.NextDouble() < 0.5)
                        {
                            (entity as CardEntity2D).IsSocketed = !(entity as CardEntity2D).IsSocketed;
                        }
                    }

                    break;
            }

            return true;
        }
    }
}
