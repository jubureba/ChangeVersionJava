using ChangeJavaVersion.pages;
using ChangeJavaVersion.pages.abstracts;
using ChangeJavaVersion.pages.view.config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Resources;
using System.Reflection;
using ChangeJavaVersion.pages.view.Config;
using ChangeJavaVersion.Modelos.Interfaces;
using ChangeJavaVersion.Properties;
using System.IO;
using ChangeJavaVersion.pages.view.Donate;
using System.Configuration;
using System.Collections.Specialized;

namespace ChangeJavaVersion {

    public partial class Principal : Window {
  
        ObservableCollection<PathJava> MyCollection;
        FindPath fpath = new FindPathImpl();
        TextToObject txtObj = new TextToObjectImpl();
        SystemTrayTextToObject txtObjSTray = new SystemTrayTextToObjectImpl();
        IAppConfig appConfig = new IAppConfigImpl();
        
        public Principal() {
            InitializeComponent();

            HomePage();
            refreshMenuTray();
      
            if(ConfigurationManager.AppSettings.Get("System") == null 
                || ConfigurationManager.AppSettings.Get("System").Equals("")) {

                createConfigFileIfDontExists();
            }

            //set title
            this.Title = messages.sistema + messages.build + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            // checkExistsConfigFile();
        }

        public void createConfigFileIfDontExists() {
            appConfig.AddOrUpdateAppSettings("System", "False");
            appConfig.AddOrUpdateAppSettings("Startup", "False");
            appConfig.AddOrUpdateAppSettings("Language", "Portuguese");

        }

        //FIXME: Retirar, alterado o txt para o app.config
        public void checkExistsConfigFile () {
            List<SystemCloseConfig> valoresTxt = txtObjSTray.ReadFile(fpath.findJavaPath((fpath.findDocPath()), "SystemTray.txt"));
            if (valoresTxt.Count == 0) {
                string[] lines = { "System=False" };
                File.AppendAllLines(System.IO.Path.Combine(fpath.findDocPath(), "SystemTray.txt"), lines);
            }
        }
       

        public void teste () {
            this.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Normal;
        }


        public void HomePage() {
            if (load_frame.Content == null) {
                load_frame.Content = new Dashboard();
            }
        }

        private void ShowBalloon(string title, string text) {
            BalloonView balloonView = new BalloonView();
            balloonView.ShowBalloon(title, text, MyNotifyIcon);
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            //List<SystemCloseConfig> sbTraySystemTxtValue = txtObjSTray.ReadFile(fpath.findJavaPath((fpath.findDocPath()), "SystemTray.txt"));
            bool systemTray = Boolean.Parse(ConfigurationManager.AppSettings.Get("System"));
            if (systemTray == true) {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            } else if (systemTray == false) {
                e.Cancel = false;
                Application.Current.Shutdown();
            }
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e) {
            load_frame.Content = new Configuracoes();
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
                        String.Format(messages.versao_alterada, "\n"+ pathJava.Text));
                   
                } else { 
                    ShowBalloon(messages.erro,
                         String.Format(messages.versao_inalterada));
                }
            } 
        }



        private void btnPathJDK_Click(object sender, RoutedEventArgs e) {
            load_frame.Content = new PathConfigList();
        }

   

        public void refreshMenuTray() {
            menuTray.ItemsSource = txtObj.ReadFile(fpath.findJavaPath((fpath.findDocPath()), "JavaPath.txt"));
        }

        private void btnDonate_Click(object sender, RoutedEventArgs e) {
            load_frame.Content = new Donate();
        }
    }
}
