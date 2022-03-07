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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChangeJavaVersion.pages.view.Donate {
    /// <summary>
    /// Interação lógica para Donate.xam
    /// </summary>
    public partial class Donate : Page {
        public Donate() {
            InitializeComponent();
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            while (NavigationService.RemoveBackEntry() != null) ;
            NavigationService.Content = new Dashboard();
        }
    }
}
