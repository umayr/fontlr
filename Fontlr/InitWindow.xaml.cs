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

namespace Fontlr
{
    /// <summary>
    /// Interaction logic for InitWindow.xaml
    /// </summary>
    public partial class InitWindow : Elysium.Controls.Window
    {
        InitWindow I = null;
        Thread C = null;
        
        public InitWindow()
        {
            InitializeComponent();
            I = this;
            C = Thread.CurrentThread;

            Thread T = new Thread(() =>
            {
                FontWindow M = new FontWindow();
                M.Show();
                M.ContentRendered += M_ContentRendered;
                System.Windows.Threading.Dispatcher.Run();
                M.Closed += (s, e) => M.Dispatcher.InvokeShutdown();

            }) { IsBackground = true };

            T.SetApartmentState(ApartmentState.STA);
            T.Start();
        }

        void M_ContentRendered(object sender, EventArgs e)
        {
            C.IsBackground = true;
            Thread.CurrentThread.IsBackground = false;
            C.Abort();
        }

    }
}
