using ChangeJavaVersion.Modelos.Interfaces;
using ChangeJavaVersion.pages.abstracts;
using ChangeJavaVersion.pages.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Application = System.Windows.Application;

namespace ChangeJavaVersion.pages.view.config { 
    /// <summary>
    /// Interaction logic for UsuarioForm.xaml
    /// </summary>
    public partial class PathConfigForm : Page
    {
        public PathConfigForm()
        {
            InitializeComponent();
        }

        private void ShowBalloon(string title, string text) {
            foreach (Window window in Application.Current.Windows) {
                if (window.GetType() == typeof(Principal)) { 
                    BalloonView balloonView = new BalloonView();
                    //balloonView.ShowBalloon(title, text, (window as Principal).MyNotifyIcon);
                }
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e)
        {
            refreshTrayMenu();
            while (NavigationService.RemoveBackEntry() != null) ;
            NavigationService.Content = new PathConfigList();
        }


        private void refreshTrayMenu() {
            foreach (Window window in Application.Current.Windows) {
                if (window.GetType() == typeof(Principal)) {
                    (window as Principal).refreshMenuTray();
                }
            }
        }

        private void btnCadastrar_Click(object sender, RoutedEventArgs e) {
            if (tbJavaVersion.Text == "" || tbJavaVersion.Text == null) {
                System.Windows.MessageBox.Show("O campo 'nome da versão' é obrigatório!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (tbPathJdk.Text == "" || tbPathJdk.Text == null) {
                System.Windows.MessageBox.Show("O campo 'Caminho do JDK' é obrigatório!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (tbPathJdk.Text != ""  && tbJavaVersion.Text != "") {
                RegisterNewPath rnewpath = new RegisterNewPathImpl();
                FindPath fpath = new FindPathImpl();
                rnewpath.register(fpath.findDocPath(), tbJavaVersion.Text, tbPathJdk.Text);
                NavigationService.Content = new PathConfigList();
                ShowBalloon("Sucesso!", "Novo Path cadastrado com sucesso." );

                refreshTrayMenu();
            }

        }

        private void btnSearchPath_Click(object sender, RoutedEventArgs e) {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            tbPathJdk.Text = dialog.SelectedPath;
        }
    }
}
