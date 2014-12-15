using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Fontlr.Models
{
    class Favorites
    {

        static List<favorite> favorites = new List<favorite>();

        public static List<favorite> FavoriteList
        {
            get { return Favorites.favorites; }
            set { Favorites.favorites = value; }
        }



        public static void SerializeFonts(List<FontHandler> L)
        {

            XmlSerializer ser = new XmlSerializer(L.GetType());
            if (!Utils.CheckDirectoryExists()) { Utils.CreateDirectory(); }
            using (FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Fontlr\Favorites.ftlr", FileMode.Create))
            {
                ser.Serialize(fs, L);
            }

        }
        public Favorites()
        {

            if (Utils.CheckFileExists(FileType.Favorite))
            {
                // Fetch All Favorites
                // Generate a list


                ReadFavorites();
            }
            else
            {
                // Create a blank Favorite File
                Utils.CreateBlankFile(FileType.Favorite);
            }
        }
        public static void AddFavorite(favorite F)
        {

            XmlDocument D = new XmlDocument();
            D.Load(Utils.GetFilePath(FileType.Favorite));
            XmlElement _F = D.CreateElement("favorite");
            XmlElement _d = D.CreateElement("date");
            _d.InnerText = F.date;
            XmlElement _t = D.CreateElement("time");
            _t.InnerText = F.time;
            XmlElement _f = D.CreateElement("fontname");
            _f.InnerText = F.fontname;
            _F.AppendChild(_d);
            _F.AppendChild(_t);
            _F.AppendChild(_f);
            D.DocumentElement.AppendChild(_F);
            D.Save(Utils.GetFilePath(FileType.Favorite));


        }
        public static bool Exists(string fontname)
        {
            if (!(FavoriteList.SingleOrDefault(x => x.fontname == fontname) == null)) { return true; }
            else
            {
                return false;
            }
        }

        public static void RemoveFavorite(string Fontname)
        {

            favorite f = FavoriteList.SingleOrDefault(x => x.fontname == Fontname);
            if (!(f == null))
            {
                FavoriteList.Remove(f);
                UpdateFavorites();
            }
        }

        public static void UpdateFavorites()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "favorites";
            xRoot.IsNullable = false;

            XmlSerializer ser = new XmlSerializer(FavoriteList.GetType(),xRoot);
            using (FileStream fs = new FileStream(Utils.GetFilePath(FileType.Favorite), FileMode.OpenOrCreate))
            {
                ser.Serialize(fs, FavoriteList);
            }
            ReadFavorites();
        }

        private static void ReadFavorites()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "favorites";
            xRoot.IsNullable = false;

            XmlSerializer S = new XmlSerializer(FavoriteList.GetType(), xRoot);
            using (FileStream fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Fontlr\Favorites.ftlr", FileMode.Open))
            {
                FavoriteList = (List<favorite>)S.Deserialize(fs);
            }
        }
    }
    public class favorite
    {
        public string date { get; set; }
        public string time { get; set; }
        public string fontname { get; set; }
    }
}
