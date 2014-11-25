using Fontlr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Fontlr
{
    /// <summary>
    /// Interaction logic for FontPreviewWindow.xaml
    /// </summary>
    public partial class FontPreviewWindow : Elysium.Controls.Window
    {
        private static bool isUpper;
        public FontPreviewWindow(string Fontname, FontFamily Fontfamily)
        {
            InitializeComponent();
            Title = Fontname;
            IEnumerable<Label> Labels = null;
            isUpper = false;
            Loaded += (s, e) =>
            {

                Labels = FindVisualChildren<Label>(this);
                foreach (Label L in Labels)
                {
                    L.FontFamily = Fontfamily;
                }
            };
            btnUpper.Click += (s, e) =>
            {

                if (!isUpper)
                {
                    foreach (Label L in Labels)
                    {
                        L.Content = (string)L.Content.ToString().ToUpper();
                    }
                    btnUpper.Content = "Small";
                    isUpper = true;
                }
                else
                {
                    foreach (Label L in Labels)
                    {
                        L.Content = (string)L.Content.ToString().ToLower();
                    }
                    btnUpper.Content = "Capital";
                    isUpper = false;
                }


            };
            btnFav.Click += (s, e) =>
            {

                if (Favorites.Exists(Fontname))
                {
                    Favorites.RemoveFavorite(Fontname);
                    ((Button)s).Content = "Add to Favorites";
                }
                else
                {
                    Favorites.AddFavorite(new favorite
                    {
                        date = DateTime.Now.ToLongDateString(),
                        time = DateTime.Now.ToLongTimeString(),
                        fontname = Fontname
                    });
                    ((Button)s).Content = "Remove Favorite";

                }

                TabbedWindow.IsFavoriteUpdated = true;
            };

            if (Favorites.Exists(Fontname))
            {
                btnFav.Content = "Remove Favorite";
            }

        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

    }
}
