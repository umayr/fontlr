using Fontlr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Fontlr
{
    /// <summary>
    /// Interaction logic for FontWindow.xaml
    /// </summary>
    public partial class FontWindow : Elysium.Controls.Window
    {

        public FontWindow()
        {
            InitializeComponent();

            this.MouseDown += (s, e) => { this.Topmost = true; };

            List<FontHandler> List = FontHandler.GetFontList();
            List<FontHandler> Cached = List;
            
            new Thread(() =>
            {
                DispatcherOperation dp = gListWrapper.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    foreach (FontHandler F in List)
                    {
                        gListWrapper.Children.Add(GetFontPanel(F));
                    }
                }), DispatcherPriority.Background);

                dp.Completed += (s, e) =>
                {
                    lblLoading.Dispatcher.Invoke(
                        new Action(delegate()
                    {
                        lblLoading.Visibility = System.Windows.Visibility.Hidden;
                    }),
                        DispatcherPriority.Background);
                };
            }).Start();


            txtSearch.TextChanged += (s, e) =>
            {
                string T = txtSearch.Text;
                IsProgressBarVisible(true);

                new Thread(() =>
                {

                    DispatcherOperation dp = gListWrapper.Dispatcher.BeginInvoke(new Action(delegate()
                    {

                        if (!string.IsNullOrEmpty(T))
                        {
                            UpdateUIList(UpdateFontList(T, List));
                        }
                        else
                        {
                            UpdateUIList(Cached);
                        }

                    }), DispatcherPriority.Background);

                    dp.Completed += (sender, eventArgs) =>
                    {
                        pbBusy.Dispatcher.Invoke(new Action(delegate()
                        {
                            IsProgressBarVisible(false);
                        }), DispatcherPriority.Background);
                    };
                }).Start();
            };

            btnSearch.Click += (s, e) =>
            {
                string T = txtSearch.Text;
                if (!string.IsNullOrEmpty(T))
                {
                    new Thread(() =>
                    {
                        gListWrapper.Dispatcher.Invoke(new Action(delegate()
                        {
                            UpdateUIList(UpdateFontList(T, List));
                        }), System.Windows.Threading.DispatcherPriority.Background);
                    }).Start();

                }
            };
        }


        private StackPanel GetFontPanel(FontHandler F)
        {
            #region Sequential Code
            StackPanel parent = new StackPanel();

            TextBlock tb = new TextBlock();

            tb.Text = "The quick brown fox jumps over the crazy dog";
            tb.Margin = new Thickness(2);
            tb.FontSize = 32;
            tb.Background = (Brush)(new BrushConverter()).ConvertFrom("#222");
            tb.Padding = new Thickness(10);
            tb.Cursor = Cursors.Hand;

            Label lb = new Label();
            lb.Foreground = (Brush)(new BrushConverter()).ConvertFrom("#CCC");
            lb.FontSize = 16;

            tb.FontFamily = F.Family;
            lb.Content = F.FontName;

            parent.Children.Add(lb);
            parent.Children.Add(tb);

            tb.MouseEnter += (s, e) =>
            {
                tb.Background = (Brush)(new BrushConverter()).ConvertFrom("#CCC");
                tb.Foreground = (Brush)(new BrushConverter()).ConvertFrom("#222");
            };
            tb.MouseLeave += (s, e) =>
            {
                tb.Background = (Brush)(new BrushConverter()).ConvertFrom("#222");
                tb.Foreground = (Brush)(new BrushConverter()).ConvertFrom("#FFF");
            };
            tb.MouseLeftButtonDown += (s, e) =>
            {
                this.Topmost = false;
                string _fn = lb.Content.ToString();
                FontFamily _ff = tb.FontFamily;
                Thread T = new Thread(() =>
                {
                    (new FontPreviewWindow(_fn,_ff)).Show();
                    System.Windows.Threading.Dispatcher.Run();
                }) { IsBackground = true };
                T.SetApartmentState(ApartmentState.STA);
                T.Start();
            };
            return parent;
            #endregion


        }
        private List<FontHandler> UpdateFontList(string Query, List<FontHandler> List)
        {
            List = List.Where(x => x.FontName.ToLower().StartsWith(Query.ToLower())).ToList();
            return List;
        }
        private void UpdateUIList(List<FontHandler> List)
        {
            gListWrapper.Children.Clear();
            new Thread(() =>
            {
                gListWrapper.Dispatcher.Invoke(new Action(delegate()
                {
                    foreach (FontHandler F in List)
                    {
                        gListWrapper.Children.Add(GetFontPanel(F));
                    }
                }), DispatcherPriority.Background);

            }).Start();

        }
        private void IsProgressBarVisible(bool T)
        {
            if (T)
            {
                pbBusy.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                pbBusy.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
