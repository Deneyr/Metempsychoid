using Astrategia.Model.Card;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Astrategia.Model.Player
{
    public static class PlayerSerializer
    {
        private static string ROOT_ELEMENT = "player";

        private static string NAME_ELEMENT = "Name";

        private static string COLOR_ELEMENT = "Color";

        private static string DECK_ELEMENT = "Deck";
        private static string CARD_ELEMENT = "CardName";

        public static void Serialize(Player playerToSerialize, string path)
        {
            XDocument playerDocument = new XDocument();
            XElement rootElement = new XElement(ROOT_ELEMENT);

            XElement nameElement = new XElement(NAME_ELEMENT);
            nameElement.Value = playerToSerialize.PlayerName;
            rootElement.Add(nameElement);

            XElement colorElement = new XElement(COLOR_ELEMENT);
            colorElement.Value = playerToSerialize.PlayerColor.ToString();
            rootElement.Add(colorElement);

            XElement deckElement = new XElement(DECK_ELEMENT);
            foreach(string cardId in playerToSerialize.Deck.CardIds)
            {
                XElement cardElement = new XElement(CARD_ELEMENT);
                cardElement.Value = cardId;
                deckElement.Add(cardElement);
            }
            rootElement.Add(deckElement);

            playerDocument.Add(rootElement);

            playerDocument.Save(path);
        }

        public static Player Deserialize(string path)
        {
            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Exists)
            {
                using (Stream locStream = new FileStream(fileInfo.FullName, FileMode.Open))
                {
                    XDocument playerDocument = XDocument.Load(locStream);
                    XElement rootElement = playerDocument.Element(ROOT_ELEMENT);

                    XElement nameElement = rootElement.Element(NAME_ELEMENT);

                    XElement colorElement = rootElement.Element(COLOR_ELEMENT);
                    Color color = DeserializeColor(colorElement.Value);

                    Player player = new Player(color, nameElement.Value);

                    XElement deckElement = rootElement.Element(DECK_ELEMENT);
                    IEnumerable<XElement> cardElements = deckElement.Elements(CARD_ELEMENT);
                    foreach (XElement cardElement in cardElements)
                    {
                        player.Deck.CardIds.Add(cardElement.Value);
                    }

                    return player;
                }
            }

            return null;
        }

        private static Color DeserializeColor(string color)
        {
            string red = color.Split('R')[1];
            red = red.Split(')')[0];
            red = red.Remove(0, 1);
            byte redValue = byte.Parse(red);

            string green = color.Split('G')[1];
            green = green.Split(')')[0];
            green = green.Remove(0, 1);
            byte greenValue = byte.Parse(green);

            string blue = color.Split('B')[1];
            blue = blue.Split(')')[0];
            blue = blue.Remove(0, 1);
            byte blueValue = byte.Parse(blue);

            string alpha = color.Split('A')[1];
            alpha = alpha.Split(')')[0];
            alpha = alpha.Remove(0, 1);
            byte alphaValue = byte.Parse(alpha);

            return new Color(redValue, greenValue, blueValue, alphaValue);
        }
    }
}
