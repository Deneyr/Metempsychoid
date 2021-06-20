using Metempsychoid.View.Card2D;
using Metempsychoid.View.Layer2D.BoardBannerLayer2D;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Metempsychoid.View.Text2D
{
    public class TextParagraphFactory
    {
        private Dictionary<string, List<TextToken2D>> idToTokens;

        private DirectoryInfo localizationFolder;

        private string culture;

        public string Culture
        {
            get
            {
                return this.culture;
            }
            set
            {
                if(this.culture != value)
                {
                    this.culture = value;

                    this.UpdateTextTokensFromCulture();
                }
            }
        }

        public TextParagraphFactory()
        {
            this.idToTokens = new Dictionary<string, List<TextToken2D>>();

            this.localizationFolder = new DirectoryInfo(@"Assets\Localization");

            this.culture = null;
        }

        public void CreateTextTokensIn(TextParagraph2D paragraph2D, string id, params string[] parameters)
        {
            List<TextToken2D> textToken2Ds = this.idToTokens[id];
            textToken2Ds = textToken2Ds.Select(pElem => pElem.CloneToken()).ToList();

            if (parameters.Count() > 0)
            {
                IEnumerable<TextToken2D> tokenParameters = textToken2Ds.Where(pElem => pElem.ParameterIndex >= 0);
                foreach (TextToken2D textToken2D in tokenParameters)
                {
                    textToken2D.FullText = parameters[textToken2D.ParameterIndex];
                }
            }

            paragraph2D.UpdateTextTokens(textToken2Ds);
        }

        private void UpdateTextTokensFromCulture()
        {
            this.idToTokens.Clear();

            FileInfo[] locFiles = this.localizationFolder.GetFiles("*.loc");

            foreach(FileInfo locFile in locFiles)
            {
                using (Stream locStream = new FileStream(locFile.FullName, FileMode.Open))
                {
                    XDocument locDocument = XDocument.Load(locStream);

                    XElement rootElement = locDocument.Element("localization");

                    IEnumerable<XElement> paragraphEntries = rootElement.Elements("paragraphEntry");

                    foreach(XElement entry in paragraphEntries)
                    {
                        string key = entry.Element("key").Value;

                        XElement paragraph = entry.Elements("paragraph").FirstOrDefault(pElem => pElem.Attribute("culture").Value == this.Culture);
                        if(paragraph != null)
                        {
                            IEnumerable<XElement> paragraphElements = paragraph.Elements();

                            if (paragraphElements.Count() == 0)
                            {
                                string content = paragraph.Value;

                                List<TextToken2D> token2DsList = this.CreateTextTokens(content, Color.White);

                                this.idToTokens.Add(key, token2DsList);
                            }
                            else
                            {
                                List<TextToken2D> token2DsList = new List<TextToken2D>();

                                foreach (XElement paragraphElement in paragraphElements)
                                {
                                    string content = paragraphElement.Value;

                                    XAttribute colorAttribut = paragraphElement.Attribute("color");
                                    Color tokenFillColor = Color.White;
                                    if (colorAttribut != null)
                                    {
                                        string[] colorTokens = colorAttribut.Value.Split(':');
                                        if(colorTokens.Count() >= 3)
                                        {
                                            tokenFillColor = new Color(byte.Parse(colorTokens[0]), byte.Parse(colorTokens[1]), byte.Parse(colorTokens[2]));
                                        }
                                    }

                                    this.AppendTextTokens(token2DsList, content, paragraphElement.Name.LocalName, tokenFillColor);
                                }

                                this.idToTokens.Add(key, token2DsList);
                            }
                        }
                    }
                }
            }
        }

        private List<TextToken2D> CreateTextTokens(string text, Color fillColor)
        {
            List<TextToken2D> tokens = new List<TextToken2D>();

            string[] textTokens = text.Split(' ');

            foreach (string textToken in textTokens)
            {
                // Replace by ctr choice.
                TextToken2D textToken2D = new TextToken2D(textToken, fillColor);

                tokens.Add(textToken2D);
            }

            return tokens;
        }

        public void AppendTextTokens(List<TextToken2D> tokenListToAppend, string text, string tokenType, Color fillColor)
        {
            string[] textTokens = text.Split(' ');

            foreach (string textToken in textTokens)
            {
                if (string.IsNullOrEmpty(textToken) == false)
                {
                    TextToken2D textToken2D = null;
                    switch (tokenType)
                    {
                        case "Normal":
                            textToken2D = new TextToken2D(textToken, fillColor);
                            break;
                        case "BannerTitle":
                            textToken2D = new TitleBannerTextToken2D(textToken, fillColor);
                            break;
                        case "CardLabel":
                            textToken2D = new CardLabelTextToken2D(textToken, fillColor);
                            break;
                        default:
                            textToken2D = new TextToken2D(textToken, fillColor);
                            break;
                    }

                    tokenListToAppend.Add(textToken2D);
                }
            }
        }
    }
}
