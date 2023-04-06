using ChangeJavaVersion.Modelos.Interfaces;
using ChangeJavaVersion.pages.abstracts;
using ChangeJavaVersion.pages.view.config;
using ChangeJavaVersion.Pages.utils;
using ChangeJavaVersion.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ChangeJavaVersion.pages.view.Config {
    public partial class Configuracoes : System.Windows.Controls.Page {

        public Configuracoes() {
            InitializeComponent();
            PreencherComboBox();
            Loaded += (sender, args) => {
                PreencherSliderButton();
            };
        }

        private void PreencherComboBox() {
            List<string> idiomas = new List<string> { "Português" };
            cbIdioma.ItemsSource = idiomas;
            cbIdioma.SelectedItem = ConfigurationManager.AppSettings.Get("Language");
        }

        private void PreencherSliderButton() {
            Boolean runBackground, startup;
            Boolean.TryParse(ConfigurationManager.AppSettings["config_runBackground"], out runBackground);
            Boolean.TryParse(ConfigurationManager.AppSettings["config_startup"], out startup);
            btnRunBackground.IsChecked = runBackground;
            btnStartup.IsChecked = startup;
        }

        private void atualizarConfig() {
            ConfigurationManager.AppSettings["config_runBackground"] = btnRunBackground.IsChecked.ToString();
            ConfigurationManager.AppSettings["config_startup"] = btnStartup.IsChecked.ToString();
        }

        private void btnRunBackground_Click(object sender, RoutedEventArgs e) {
            UpdateConfigSetting("config_runBackground", btnRunBackground.IsChecked == true);
            atualizarConfig();
        }

        private void UpdateConfigSetting(string key, bool value) {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value.ToString();
            config.Save(ConfigurationSaveMode.Modified);
        }

        private void btnStartup_Click(object sender, RoutedEventArgs e) {
            ManageStartup(btnStartup.IsChecked == true);
            atualizarConfig();
        }

        private void ManageStartup(bool enable) {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["config_startup"].Value = enable.ToString();
            config.Save(ConfigurationSaveMode.Modified);

            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

            if (enable) {
                string fileName = Process.GetCurrentProcess().MainModule.FileName.Replace(".dll", ".exe");
                registryKey.SetValue(messages.sistema, fileName);
            } else {
                registryKey.DeleteValue(messages.sistema, false);
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            while (NavigationService.RemoveBackEntry() != null) ;
            NavigationService.Content = new Dashboard();
            atualizarConfig();
        }

        private void cbIdioma_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            e.Handled = true;
        }
    }
}
