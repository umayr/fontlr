using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Fontlr.Models
{
    public enum FileType
    {
        Favorite,
        Groups
    };
    static class Utils
    {

        public static bool CheckFileExists(FileType T)
        {
            if (T == FileType.Favorite)
            {
                return File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Fontlr\Favorites.ftlr");
            }
            else if (T == FileType.Groups)
            {
                return File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Fontlr\Groups.ftlr");
            }
            else { return false; }
        }
        public static bool CheckDirectoryExists()
        {
            return Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Fontlr");
        }
        public static void CreateFile() { }
        public static void CreateDirectory()
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Fontlr");
        }
        public static void CreateBlankFile(FileType T)
        {
            if (T == FileType.Favorite)
            {
                XmlDocument D = new XmlDocument();
                XmlNode N = D.CreateNode(XmlNodeType.XmlDeclaration, "", "");
                D.AppendChild(N);
                XmlElement R = D.CreateElement("favorites");
                D.AppendChild(R);
                D.Save(GetFilePath(FileType.Favorite));
            }
            else if (T == FileType.Groups)
            {
            }
        }
        public static string GetFilePath(FileType T)
        {
            if (T == FileType.Favorite)
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Fontlr\Favorites.ftlr";
            }
            else if (T == FileType.Groups)
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Fontlr\Groups.ftlr";
            }
            else
            {
                return null;
            }
        }

    }

}
