using ChangeJavaVersion.pages.abstracts;
using ChangeJavaVersion.pages.view.config;
using ChangeJavaVersion.Pages.utils;
using ChangeJavaVersion.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ChangeJavaVersion.pages {

    public partial class Dashboard : Page {
      
        private DispatcherTimer _dispatcherTimer;
        private const string NameVariableJDK = "JAVA_HOME";

        public Dashboard() {
            InitializeComponent();

            var javaVersion = new List<JavaVersion>();
            var appSettings = ConfigurationManager.AppSettings;

            foreach (var key in appSettings.AllKeys.Where(key => key.StartsWith("Java_"))) {
                javaVersion.Add(new JavaVersion { Version = key, PathJava = appSettings[key] });
            }

            cbVersion.ItemsSource = javaVersion;

            startTimer(0, 0, 1);
        }

        public bool changeVersion(string versionPath) {
            if (checkVersion("JAVA_HOME", EnvironmentVariableTarget.Machine).Equals(versionPath)) {

                ShowBalloon(messages.erro, String.Format(messages.versao_inalterada_completa, versionPath));
                //MessageBox.Show(String.Format(messages.versao_inalterada_completa, versionPath), messages.erro, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else {
                //MessageBox.Show(String.Format(messages.versao_alterada_completa, checkVersion(NameVariableJDK, EnvironmentVariableTarget.Machine).ToString(), versionPath), messages.sucesso, MessageBoxButton.OK, MessageBoxImage.Information);
                setVersionInEnvironmentVariable(NameVariableJDK, versionPath, EnvironmentVariableTarget.Machine);
                ShowBalloon(messages.sucesso, String.Format(String.Format(messages.versao_alterada_completa, checkVersion(NameVariableJDK, EnvironmentVariableTarget.Machine).ToString(), versionPath)));
                return true;
            }
            cleanVersion();
        }

        private void ShowBalloon(string title, string text) {
            foreach (Window window in Application.Current.Windows) {
                if (window.GetType() == typeof(Principal)) {
                    BalloonView balloonView = new BalloonView();
                    balloonView.ShowBalloon(title, text, (window as Principal).TaskbarIcon);
                }
            }
        }

        public void setVersionInEnvironmentVariable(string variableName, string path, EnvironmentVariableTarget target) {
            System.Environment.SetEnvironmentVariable(variableName, path, target);
        }

        public string checkVersion(string variableSystem, EnvironmentVariableTarget target) {
            return System.Environment.GetEnvironmentVariable(variableSystem, target);
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e) {
            if (cbVersion.SelectedItem == null) {
                MessageBox.Show("Unable to change version.");
                return;
            }

            var versionPath = cbVersion.SelectedValue.ToString();
            changeVersion(versionPath);
        }

        private void ComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            var comboBoxItems = System.Windows.Data.CollectionViewSource.GetDefaultView(cbVersion.ItemsSource);

            foreach (var item in comboBoxItems) {
                var javaVersion = item as JavaVersion;

                if (javaVersion?.PathJava?.ToUpper()?.Contains(e.Text.ToUpper()) ?? false) {
                    cbVersion.SelectedItem = javaVersion;
                }
            }
        }

        private void BtnConfig_Click(object sender, RoutedEventArgs e) {
            NavigationService.Content = new PathConfigList();
        }

        private void BtnCheckVersion_Click(object sender, RoutedEventArgs e) {
            spinner.Visibility = Visibility.Visible;
            _dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            spinner.Visibility = Visibility.Hidden;
            _dispatcherTimer.Stop();
            MessageBox.Show(checkVersion(NameVariableJDK, EnvironmentVariableTarget.Machine), messages.sucesso, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void startTimer(int hour, int min, int sec) {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            _dispatcherTimer.Interval = new TimeSpan(hour, min, sec);
        }

        public void cleanVersion() {
            cbVersion.SelectedItem = null;
        }

        private void btnCheckVersion_Click(object sender, RoutedEventArgs e) {
            spinner.Visibility = Visibility.Visible;
            _dispatcherTimer.Start();
        }
    }
}
