using Metempsychoid.View.Card2D;
using Metempsychoid.View.Layer2D.BoardBannerLayer2D;
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

            if (parameters.Count() > 0)
            {
                IEnumerable<TextToken2D> tokenParameters = textToken2Ds.Where(pElem => pElem.ParameterIndex >= 0);
                foreach (TextToken2D textToken2D in tokenParameters)
                {
                    textToken2D.FullText = parameters[textToken2D.ParameterIndex];
                }
            }

            paragraph2D.UpdateTextTokens(this.idToTokens[id]);
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

                                List<TextToken2D> token2DsList = this.CreateTextTokens(content);

                                this.idToTokens.Add(key, token2DsList);
                            }
                            else
                            {
                                List<TextToken2D> token2DsList = new List<TextToken2D>();

                                foreach (XElement paragraphElement in paragraphElements)
                                {
                                    string content = paragraphElement.Value;

                                    this.AppendTextTokens(token2DsList, content, paragraphElement.Name.LocalName);
                                }

                                this.idToTokens.Add(key, token2DsList);
                            }
                        }
                    }
                }
            }
        }

        private List<TextToken2D> CreateTextTokens(string text)
        {
            List<TextToken2D> tokens = new List<TextToken2D>();

            string[] textTokens = text.Split(' ');

            foreach (string textToken in textTokens)
            {
                // Replace by ctr choice.
                TextToken2D textToken2D = new TextToken2D(textToken);

                tokens.Add(textToken2D);
            }

            return tokens;
        }

        public void AppendTextTokens(List<TextToken2D> tokenListToAppend, string text, string tokenType)
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
                            textToken2D = new TextToken2D(textToken);
                            break;
                        case "BannerTitle":
                            textToken2D = new TitleBannerTextToken2D(textToken);
                            break;
                        case "CardLabel":
                            textToken2D = new CardLabelTextToken2D(textToken);
                            break;
                        default:
                            textToken2D = new TextToken2D(textToken);
                            break;
                    }

                    tokenListToAppend.Add(textToken2D);
                }
            }
        }
    }
}
