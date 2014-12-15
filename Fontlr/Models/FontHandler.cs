using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Fontlr.Models
{
    public class FontHandler
    {
        public string FontName { set; get; }
        [System.Xml.Serialization.XmlIgnore()]
        public FontFamily Family { set; get; }
        [System.Xml.Serialization.XmlIgnore()]
        public FamilyTypefaceCollection Typefaces { set; get; }
        public List<string> DeviceFontNameList { set; get; }
        

        public static List<FontHandler> GetFontList()
        {

            List<FontHandler> _list = new List<FontHandler>();
            ICollection<FontFamily> _fontFamilies = Fonts.GetFontFamilies(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts));
            foreach (FontFamily F in _fontFamilies)
            {
                List<string> TypefaceList = new List<string>();
                foreach (FamilyTypeface T in F.FamilyTypefaces)
                {
                    TypefaceList.Add(T.DeviceFontName);
                }
                FontHandler oFH = new FontHandler()
                {

                    FontName = F.FamilyNames.First().Value,
                    Family = F,
                    Typefaces = F.FamilyTypefaces,
                    DeviceFontNameList = TypefaceList

                };

                _list.Add(oFH);
            }
            return _list.OrderBy(x => x.FontName).ToList();

        }

    }
}
