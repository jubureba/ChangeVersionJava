using ChangeJavaVersion.Modelos.Interfaces;
using ChangeJavaVersion.pages;
using ChangeJavaVersion.pages.abstracts;
using ChangeJavaVersion.pages.view.config;
using ChangeJavaVersion.pages.view.Config;
using ChangeJavaVersion.pages.view.Donate;
using ChangeJavaVersion.Properties;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChangeJavaVersion {

    public partial class Principal : Window {

        FindPath fpath = new FindPathImpl();
        TextToObject txtObj = new TextToObjectImpl();
        SystemTrayTextToObject txtObjSTray = new SystemTrayTextToObjectImpl();
        IAppConfig appConfig = new IAppConfigImpl();

        public Principal() {

            InitializeComponent();

            loadHomePage();
            refreshMenuTray();
            checkExistsFileConfig();

            //setTitle
            var appSettings = ConfigurationManager.AppSettings;
            string build = appSettings["build"] ?? "Not Found";
            string dataBuild = appSettings["dataBuild"] ?? "Not Found";
            string rodape = appSettings["rodape"] ?? "Not Found";
            string dev = appSettings["dev"] ?? "Not Found";
            string stringRodape = rodape + " " + dev + " - Versão: " + build + " - Build: " + dataBuild;
            tbRodape.Text = stringRodape;

            this.Title = messages.sistema + " - Versão: " + build + " - Build: " + dataBuild;

        }

        /*****************
         * COMPORTAMENTO *
         *****************/

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e) {
            load_frame.Content = new Configuracoes();
        }

        private void btnPathJDK_Click(object sender, RoutedEventArgs e) {
            load_frame.Content = new PathConfigList();
        }

        private void btnDonate_Click(object sender, RoutedEventArgs e) {
            load_frame.Content = new Donate();
        }

        private void TraySystemMenuClick(object sender, RoutedEventArgs e) {
            MenuItem menuItem = sender as MenuItem;
            PathJava pathJava = menuItem.DataContext as PathJava;
            Dashboard dashboard = new Dashboard();

            string newVersion = pathJava.Value;
            if (newVersion != null) {
                Boolean result = dashboard.changeVersion(newVersion);
                if (result) {
                    ShowBalloon(messages.sucesso,
                        String.Format(messages.versao_alterada, "\n" + pathJava.Text));
                } else {
                    ShowBalloon(messages.erro,
                         String.Format(messages.versao_inalterada));
                }
            }
        }

        private void ShowOrHidenWindow(object sender, RoutedEventArgs e) {
            if (this.Visibility == Visibility.Hidden) {
                this.Visibility = Visibility.Visible;
            }
        }

        public void refreshMenuTray() {
            menuTray.ItemsSource = txtObj.ReadFile(fpath.findJavaPath((fpath.findDocPath()), "JavaPath.txt"));
        }

        private bool getSystemTray() {
            return Boolean.Parse(ConfigurationManager.AppSettings.Get("TraySystem"));
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            if (getSystemTray() == true) {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            } else if (getSystemTray() == false) {
                e.Cancel = false;
                Application.Current.Shutdown();
            }
        }

        /*****************
         * AÇOES *********
         *****************/

        public void loadHomePage() {
            if (load_frame.Content == null) {
                load_frame.Content = new Dashboard();
            }
            
        }

        private void checkExistsFileConfig() {
            if (ConfigurationManager.AppSettings.Get("TraySystem") == null || ConfigurationManager.AppSettings.Get("TraySystem").Equals("")) {
                createConfigFileIfDontExists();
            }
        }

        public void createConfigFileIfDontExists() {
            appConfig.AddOrUpdateAppSettings("TraySystem", "False");
            appConfig.AddOrUpdateAppSettings("Startup", "False");
            appConfig.AddOrUpdateAppSettings("Language", "Portuguese");
        }

        private void ShowBalloon(string title, string text) {
            BalloonView balloonView = new BalloonView();
            balloonView.ShowBalloon(title, text, MyNotifyIcon);
        }

    }
}
