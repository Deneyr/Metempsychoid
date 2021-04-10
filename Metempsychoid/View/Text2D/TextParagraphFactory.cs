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

        public void CreateTextTokensIn(TextParagraph2D paragraph2D, string id)
        {
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
                            string content = paragraph.Value;

                            List<TextToken2D> token2DsList = this.CreateTextTokens(content);

                            this.idToTokens.Add(key, token2DsList);
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
    }
}
