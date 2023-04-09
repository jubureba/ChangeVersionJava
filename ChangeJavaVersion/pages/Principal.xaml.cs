using ChangeJavaVersion.Modelos.Interfaces;
using ChangeJavaVersion.pages;
using ChangeJavaVersion.pages.abstracts;
using ChangeJavaVersion.pages.view.config;
using ChangeJavaVersion.pages.view.Config;
using ChangeJavaVersion.pages.view.Donate;
using ChangeJavaVersion.Pages.utils;
using ChangeJavaVersion.Properties;
using Hardcodet.Wpf.TaskbarNotification;
using Octokit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace ChangeJavaVersion {

    public partial class Principal : Window {

        private const string JAVA_PREFIX = "Java_";
        private const string JAVA_HOME = "JAVA_HOME";

        public static TaskbarIcon taskbarIcon { get; private set; }

        public Hardcodet.Wpf.TaskbarNotification.TaskbarIcon TaskbarIcon {
            get { return taskbarIcon; }
        }
        private readonly List<MenuItem> _menuItems = new List<MenuItem>();

        private bool runBackground = bool.Parse(ConfigurationManager.AppSettings["config_runBackground"]);


        public Principal() {

            InitializeComponent();

            loadHomePage();
            refreshMenuTray();
            ConfigureTitle();
            ConfigureJavaVersion();
            createTaskBarIcon();
            UpdateContextMenu(taskbarIcon);

            loadGitHubInfo();

            // Obtém a versão do assembly
            Version version = Assembly.GetEntryAssembly().GetName().Version;

            // Obtém a versão semântica do assembly
            string semanticVersion = $"{version.Major}.{version.Minor}.{version.Build}";

            //MessageBox.Show(version + "   " +  semanticVersion);

        }

        private async void loadGitHubInfo() {
            try {
                var github = new GitHubClient(new ProductHeaderValue(messages.github_project));
                var tokenAuth = new Credentials(messages.github_key);
                github.Credentials = tokenAuth;

                var commits = await github.Repository.Commit.GetAll(messages.github_user, messages.github_project);
                var latestCommit = commits[0];
                var lastCommitId = latestCommit.Sha;
                var lastCommitDate = latestCommit.Commit.Author.Date;

                // Atualizar as chaves do app.config com as informações do último commit
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["LastCommitID"].Value = lastCommitId;
                config.AppSettings.Settings["LastCommitDate"].Value = lastCommitDate.ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

            } catch (Exception e) {
                return;
            }  
        }

        private void createTaskBarIcon() {
            if (taskbarIcon != null) {
                return;
            }

            var uri = new Uri(messages.sistema_icone, UriKind.RelativeOrAbsolute);
            var stream = Application.GetResourceStream(uri)?.Stream;
            var icon = new System.Drawing.Icon(stream);
            taskbarIcon = new Hardcodet.Wpf.TaskbarNotification.TaskbarIcon();
            taskbarIcon.Icon = icon;
            taskbarIcon.ToolTipText = "ChangeJavaVersion";
            taskbarIcon.TrayLeftMouseDown += _taskbarIcon_TrayLeftMouseDown;
        }

        public void UpdateContextMenu(TaskbarIcon taskbarIcon) {
            var javaSettings = ConfigurationManager.AppSettings.AllKeys
                .Where(key => key.StartsWith(JAVA_PREFIX))
                .ToDictionary(key => key, key => ConfigurationManager.AppSettings[key]);

            var menuItems = new List<MenuItem>();
            foreach (var javaSetting in javaSettings) {
                var menuItem = new MenuItem();
                menuItem.Header = javaSetting.Key;
                menuItem.Tag = javaSetting.Value;
                menuItem.Click += MenuItem_Click;
                menuItems.Add(menuItem);
            }

            // Adiciona os MenuItems no TaskbarIcon
            if (menuItems.Any()) {
                var contextMenu = new ContextMenu();
                foreach (var menuItem in menuItems) {
                    contextMenu.Items.Add(menuItem);
                    contextMenu.Items.Add(new Separator());
                }

                var restoreMenuItem = new MenuItem();
                restoreMenuItem.Header = "Mostrar";
                restoreMenuItem.Click += RestoreMenuItem_Click;
                contextMenu.Items.Add(restoreMenuItem);

                var minimizeMenuItem = new MenuItem();
                minimizeMenuItem.Header = "Esconder";
                minimizeMenuItem.Click += MinimizeMenuItem_Click;
                contextMenu.Items.Add(new Separator());
                contextMenu.Items.Add(minimizeMenuItem);

                var exitMenuItem = new MenuItem();
                exitMenuItem.Header = "Sair";
                exitMenuItem.Click += ExitMenuItem_Click;
                contextMenu.Items.Add(new Separator());
                contextMenu.Items.Add(exitMenuItem);

                taskbarIcon.ContextMenu = contextMenu;
            }     
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            var menuItem = sender as MenuItem;
            var javaPath = menuItem?.Tag as string;
            Dashboard dashboard = new Dashboard();
            if (!string.IsNullOrEmpty(javaPath)) {
                dashboard.changeVersion(javaPath);
            }
        }

        private void _taskbarIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e) {
            if (_menuItems.Any()) {
                var contextMenu = new ContextMenu();
                foreach (var menuItem in _menuItems) {
                    contextMenu.Items.Add(menuItem);
                }
                contextMenu.IsOpen = true;
            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            taskbarIcon.Dispose();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            var appConfig = ConfigurationManager.AppSettings;
            var runInBackground = bool.Parse(appConfig["config_runBackground"]);
            if (runInBackground) {
                e.Cancel = true; // Cancela o evento de fechamento da janela
                this.Hide(); // Esconde a janela principal
            }
        }

        private void RestoreMenuItem_Click(object sender, RoutedEventArgs e) {
            Show();
        }

        private void MinimizeMenuItem_Click(object sender, RoutedEventArgs e) {
            this.Hide();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }

        private void NotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e) {
            // Restaura a janela e remove o ícone da bandeja
            this.Visibility = Visibility.Visible;
            taskbarIcon.Visibility = Visibility.Collapsed;
        }

        public void loadHomePage() {
            if (load_frame.Content == null) 
                load_frame.Content = new Dashboard();
        }

        private void ConfigureJavaVersion() {
            var javaVersion = new List<JavaVersion>();
            var appSettings = ConfigurationManager.AppSettings;
            foreach (var key in appSettings.AllKeys) {
                if (key.StartsWith(JAVA_PREFIX)) {
                    javaVersion.Add(new JavaVersion { Version = key, PathJava = appSettings[key] });
                }
            }
        }

        private void ConfigureTitle() {
            var version = Assembly.GetEntryAssembly()?.GetName()?.Version;
            var appSettings = ConfigurationManager.AppSettings;

            var build = appSettings["build"] ?? "Not Found";
            var dataBuild = appSettings["dataBuild"] ?? "Not Found";
            var rodape = appSettings["rodape"] ?? "Not Found";
            var dev = appSettings["dev"] ?? "Not Found";

            var versionString = version?.ToString(3) ?? "Unknown";
            var stringRodape = $"{rodape} {dev} - Versão: {versionString} - Build: {dataBuild}";

            Title = $"{messages.sistema} - Versão: {versionString} - Build: {dataBuild}";
            tbRodape.Text = stringRodape;
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
            //verifica o nome da aplicação, e se o final for .dll troca para .exe
            string fileName = Environment.GetCommandLineArgs()[0];
            if (fileName.EndsWith(".dll")) {
                fileName = fileName.Replace(".dll", ".exe");
            }
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName) {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                Verb = "runas"
            };
            try {
                Process.Start(startInfo);
                return true;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

        private void ShowBalloon(string title, string text) {
            BalloonView balloonView = new BalloonView();
            balloonView.ShowBalloon(title, text, taskbarIcon);
        }

        private void TraySystemMenuClick(object sender, RoutedEventArgs e) {
            if (!(sender is MenuItem menuItem)) {
                return;
            }
            if (!(menuItem.DataContext is JavaVersion javaVersion)) {
                return;
            }

            bool result = EnvironmentVariableManager.ChangeVariable(JAVA_HOME, javaVersion.PathJava);

            if (result) {
                ShowBalloon(messages.sucesso, string.Format(messages.versao_alterada, "\n" + javaVersion.Version));
            } else {
                ShowBalloon(messages.erro, string.Format(messages.versao_inalterada));
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
                if (key.StartsWith(JAVA_PREFIX)) {
                    javaVersion.Add(new JavaVersion { Version = key, PathJava = appSettings[key] });
                }
            }
        }

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

        private void Window_Closing(object sender, CancelEventArgs e) {
        
            if (runBackground) {
                e.Cancel = true;
                this.Visibility = Visibility.Hidden;
            } else {
                Application.Current.Shutdown();
            }
        }
    }
}

