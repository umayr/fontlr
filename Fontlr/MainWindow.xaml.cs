using Fontlr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fontlr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Elysium.Controls.Window
    {
        List<FontHandler> _fontList = new List<FontHandler>();

        public List<FontHandler> FontList
        {
            get
            {
                ICollection<FontFamily> _fontFamilies = Fonts.GetFontFamilies(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Fonts));
                foreach (FontFamily F in _fontFamilies)
                {
                    List<string> TypefaceList = new List<string>();
                    foreach (FamilyTypeface T in F.FamilyTypefaces)
                    {
                        TypefaceList.Add(T.DeviceFontName);
                    }
                    _fontList.Add(new FontHandler()
                    {
                        FontName = F.FamilyNames.First().Value,
                        Family = F,
                        Typefaces = F.FamilyTypefaces,
                        DeviceFontNameList = TypefaceList
                    });
                }
                return _fontList;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearch.GotFocus += txtSearch_GotFocus;
            txtSearch.LostFocus += txtSearch_LostFocus;
        }

        void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            List.Opacity = 1.0;
        }

        void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            List.Opacity = 0.5;
        }

        void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync(txtSearch.Text);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            List<FontHandler> Q = e.Result as List<FontHandler>;
            List.ItemsSource = Q;
            List.UpdateLayout(); 
            prUpdating.Visibility = System.Windows.Visibility.Hidden;
            List.Opacity = 1.0;
        }

        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            
            List<FontHandler> Q = (from F in FontList
                                   where (F.FontName.IndexOf(e.Argument.ToString()) >= 1)
                                   select F).ToList<FontHandler>();
            e.Result = Q;
        }

        class UpdateHandler
        {
            public string Query { set; get; }
            public Elysium.Controls.ProgressRing ProgressRing { set; get; }
        }

    }
}
