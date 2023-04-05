using ChangeJavaVersion.Modelos.Interfaces;
using ChangeJavaVersion.pages;
using ChangeJavaVersion.pages.abstracts;
using ChangeJavaVersion.pages.view.config;
using ChangeJavaVersion.pages.view.Config;
using ChangeJavaVersion.pages.view.Donate;
using ChangeJavaVersion.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Application = System.Windows.Application;

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

            Version version = Assembly.GetEntryAssembly()?.GetName()?.Version;
            if (version != null) {
                string versionString = $"{version.Major}.{version.Minor}.{version.Build}";
                //setTitle
                var appSettings = ConfigurationManager.AppSettings;
                string build = appSettings["build"] ?? "Not Found";
                string dataBuild = appSettings["dataBuild"] ?? "Not Found";
                string rodape = appSettings["rodape"] ?? "Not Found";
                string dev = appSettings["dev"] ?? "Not Found";
                string stringRodape = rodape + " " + dev + " - Versão: " + versionString + " - Build: " + dataBuild;
                tbRodape.Text = stringRodape;

                this.Title = messages.sistema + " - Versão: " + versionString + " - Build: " + dataBuild;
            }

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
            if (!IsRunAsAdmin()) {
                if (RestartAsAdmin()) {
                    Application.Current.Shutdown();
                } else {
                    MessageBox.Show("A aplicação precisa ser executada em modo de administrador.");
                    Application.Current.Shutdown();
                }
            }
        }

        private bool IsRunAsAdmin() {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private bool RestartAsAdmin() {
            if (Environment.GetCommandLineArgs().Length > 0) {
                try {
                    var fileName = Environment.GetCommandLineArgs()[0];

                    if (fileName.EndsWith(".dll")) {
                        fileName = fileName.Replace(".dll", ".exe");
                    }
                    var startInfo = new ProcessStartInfo {
                        UseShellExecute = true,
                        WorkingDirectory = Environment.CurrentDirectory,
                        FileName = fileName,
                        Verb = "runas"
                    };

                    Process.Start(startInfo);

                    return true;
                } catch {
                    return false;
                }
            }
            return false;
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
            JavaVersion pathJava = menuItem.DataContext as JavaVersion;
            Dashboard dashboard = new Dashboard();




            string newVersion = pathJava.Version;
            if (newVersion != null) {
                Boolean result = dashboard.changeVersion(newVersion);
                if (result) {
                    ShowBalloon(messages.sucesso,
                        String.Format(messages.versao_alterada, "\n" + pathJava.Version));
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
            var javaVersion = new List<JavaVersion>();
            var appSettings = ConfigurationManager.AppSettings;
            foreach (var key in appSettings.AllKeys) {
                if (key.StartsWith("Java_")) {
                    javaVersion.Add(new JavaVersion { Version = key, PathJava = appSettings[key] });
                }
            }
            menuTray.ItemsSource = javaVersion;
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            e.Cancel = false;
            Application.Current.Shutdown();
        }

        /*****************
         * AÇOES *********
         *****************/

        public void loadHomePage() {
            if (load_frame.Content == null) {
                load_frame.Content = new Dashboard();
            }

        }

        private void ShowBalloon(string title, string text) {
            BalloonView balloonView = new BalloonView();
            balloonView.ShowBalloon(title, text, MyNotifyIcon);
        }

    }
}

