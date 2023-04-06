using ChangeJavaVersion.Modelos.Utils;
using ChangeJavaVersion.pages.abstracts;
using ChangeJavaVersion.Pages.utils;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ChangeJavaVersion.pages.view.config {

    public partial class PathConfigList : Page {

        private DispatcherTimer dispatcherTimer;
        int page = 1;
        const int qtdRegistrosPage = 6;
        int qtdRegistros = 0;

        private const string prefixJavaVersion = "Java_";
        private const string registerPathJdk = @"SOFTWARE\\JavaSoft\\Java Development Kit\";

        public PathConfigList() {
            InitializeComponent();
            startTimer(0, 0, 5);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            atualizarPaginador();
        }

        private void startTimer(int hour, int min, int sec) {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            //dispatcherTimer.Interval = new TimeSpan(hour, min, sec);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(5);
            dispatcherTimer.Start();
        }

        private void atualizarPaginador() {
            
            enableAllButtons();

            var javaVersion = new List<JavaVersion>();
            var appSettings = ConfigurationManager.AppSettings;
            foreach (var key in appSettings.AllKeys)
            {
                if (key.StartsWith(prefixJavaVersion))
                {
                    javaVersion.Add(new JavaVersion { Version = key, PathJava = appSettings[key] });
                }
            }

            this.qtdRegistros = javaVersion.Count;
            lbRegistros.Text = "Registros: " + this.qtdRegistros;
            tablePath.ItemsSource = PagingExtensions.Page(javaVersion, this.page, qtdRegistrosPage);

            float qtdPaginas = qtdRegistros / qtdRegistrosPage;

            if (page <= 1) {
                page = 1;
                btnPrevious.IsEnabled = false;
            } 
            if (page > qtdPaginas) {
                page = ((int)qtdPaginas);
                btnNext.IsEnabled = false;
            }
        }

        private void atualizarTrayMenu() {
            var taskbarIcon = ((Principal)Application.Current.MainWindow).TaskbarIcon;
            Principal principal = new Principal();
            principal.UpdateContextMenu(taskbarIcon);
        }

        private void enableAllButtons () {
            btnPrevious.IsEnabled = true;
            btnNext.IsEnabled = true;
        }

        private void disableAllButtons() {
            btnPrevious.IsEnabled = false;
            btnNext.IsEnabled = false;
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            while (NavigationService.RemoveBackEntry() != null) ;
            NavigationService.Content = new Dashboard();
        }

        private void btnNovo_Click_1(object sender, RoutedEventArgs e) {
            NavigationService.Content = new PathConfigForm();
        }

        private void btnAdicionarAutomaticamente_Click(object sender, RoutedEventArgs e) {
            spinnerVisibility();

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string keyPath = registerPathJdk;
            string[] versions = getInstalledJavaVersions(keyPath);
            var appSettings = (AppSettingsSection)config.GetSection("appSettings");

            RegistryKey localKey;
            localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            localKey = localKey.OpenSubKey(keyPath);

            var appConfig = ConfigurationManager.AppSettings;

            //REMOVE TODOS OS REGISTROS QUE COMEÇEM COM JAVA_, ANTES DE ADICIONAR NOVAMENTE 
            foreach (string key in appConfig.AllKeys)
            {
                if (key.StartsWith(prefixJavaVersion))
                    appSettings.Settings.Remove(key);
            }

            //VARRE TODAS AS VERSOES LOCALIZADAS NO REGISTRO E PARA CADA ADICIONA NO APP.CONFIG
            foreach (string version in versions)
            {
                localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                localKey = localKey.OpenSubKey(keyPath + @"\" + version);
                string javaHome = localKey.GetValue("JavaHome").ToString();
                appSettings.Settings.Add(prefixJavaVersion + version, javaHome);
            }

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            ShowBalloon("ChangeJavaVersion Sucess!", "Novo Path cadastrado com sucesso.");

            atualizarPaginador();
            atualizarTrayMenu();
            // taskbarIcon.ToolTipText = "Nova dica de ferramenta";
            
        }

        private string[] getInstalledJavaVersions(string keyPath) {
            RegistryKey localKey;
            localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            localKey = localKey.OpenSubKey(keyPath);
            string[] versions = localKey.GetSubKeyNames();
            return Array.FindAll(versions, version => version.Split('.').Length > 2);
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            spinnerVisibility();
            // Get selected item in DataGrid
            var selectedItem = (JavaVersion)tablePath.SelectedItem;

            // Remove item from list
            tablePath.ItemsSource.Cast<JavaVersion>().ToList().Remove(selectedItem);

            // Remove item from app.config
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appSettings = (AppSettingsSection)config.GetSection("appSettings");
            appSettings.Settings.Remove(selectedItem.Version);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            ShowBalloon("Sucesso!", tablePath.SelectedValue.ToString() + " Removido.");

            atualizarPaginador();
            atualizarTrayMenu();
        }

        private void ShowBalloon(string title, string text)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Principal))
                {
                    BalloonView balloonView = new BalloonView();
                    balloonView.ShowBalloon(title, text, (window as Principal).TaskbarIcon);
                }
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            spinner.Visibility = Visibility.Hidden;
            dispatcherTimer.Stop();
            atualizarPaginador();
        }

        private void btnLast_Click(object sender, RoutedEventArgs e) {
            spinnerVisibility();
            page = (qtdRegistros % qtdRegistrosPage);
        }

        private void btnFirst_Click(object sender, RoutedEventArgs e) {
            spinnerVisibility();
            page = 1;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e) {
            spinnerVisibility();
            page += 1;
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e) {
            spinnerVisibility();
            page -= 1;
        }

        private void spinnerVisibility() {
            spinner.Visibility = Visibility.Visible;
            dispatcherTimer.Start();
        }

    }
}


