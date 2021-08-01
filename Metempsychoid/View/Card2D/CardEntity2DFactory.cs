using Metempsychoid.Model;
using Metempsychoid.Model.Card;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Card2D
{
    public class CardEntity2DFactory : AObject2DFactory
    {
        private Dictionary<string, int> cardNameToIndex;

        public CardEntity2DFactory()
        {
            this.texturesPath.Add(@"Assets\Graphics\Shaders\distortion_map.png");

            this.texturesPath.Add(@"Assets\Graphics\Cards\backCard.png");

            this.texturesPath.Add(@"Assets\Graphics\Entities\StarHalo.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Effects\starEffect.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Effects\beamEffect.png");

            this.texturesPath.Add(@"Assets\Graphics\Cards\wheelCard.jpg");

            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LaForce.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LaJustice.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LaLune.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LaMort.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LAmoureux.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LaPretresse.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LaTemperence.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LeChariot.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LeDiable.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LeFou.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LeHierophante.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LeMagicien.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LeMonde.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LEmpereur.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LePendu.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LErmite.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LeRocher.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\Carte_LImperatrice.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\LaRoue.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\LaTour.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\LeJugement.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\LeSoleil.png");
            this.texturesPath.Add(@"Assets\Graphics\Cards\LEtoile.png");

            this.cardNameToIndex = new Dictionary<string, int>();
            //this.cardNameToIndex.Add("wheel", 5);

            this.cardNameToIndex.Add("strength", 6);
            this.cardNameToIndex.Add("justice", 7);
            this.cardNameToIndex.Add("moon", 8);
            this.cardNameToIndex.Add("death", 9);
            this.cardNameToIndex.Add("lover", 10);
            this.cardNameToIndex.Add("priestess", 11);
            this.cardNameToIndex.Add("temperance", 12);
            this.cardNameToIndex.Add("cart", 13);
            this.cardNameToIndex.Add("devil", 14);
            this.cardNameToIndex.Add("fool", 15);
            this.cardNameToIndex.Add("hierophant", 16);
            this.cardNameToIndex.Add("magician", 17);
            this.cardNameToIndex.Add("world", 18);
            this.cardNameToIndex.Add("emperor", 19);
            this.cardNameToIndex.Add("hangedMan", 20);
            this.cardNameToIndex.Add("hermite", 21);
            this.cardNameToIndex.Add("rock", 22);
            this.cardNameToIndex.Add("empress", 23);
            this.cardNameToIndex.Add("wheel", 24);
            this.cardNameToIndex.Add("tower", 25);
            this.cardNameToIndex.Add("judgement", 26);
            this.cardNameToIndex.Add("sun", 27);
            this.cardNameToIndex.Add("star", 28);

            // Sounds
            this.soundsPath.Add("cardDrawn", @"Assets\Sounds\cardDrawn.ogg");
            this.soundsPath.Add("cardFocused", @"Assets\Sounds\cardFocused.ogg");
            this.soundsPath.Add("cardPicked", @"Assets\Sounds\cardPicked.ogg");

            this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            return this.CreateObject2D(world2D, null, obj);
        }

        public override IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj)
        {
            if (obj is CardEntity)
            {
                CardEntity entity = obj as CardEntity;

                return new CardEntity2D(this, layer2D, entity);
            }

            return null;
        }

        public override void OnTextureLoaded(string path, Texture texture)
        {
            if (this.Resources.ContainsKey(path))
            {
                texture.Smooth = true;

                this.Resources[path] = texture;
            }
        }

        public int GetIndexFromCardName(string cardName)
        {
            return this.cardNameToIndex[cardName];
        }
    }
}
