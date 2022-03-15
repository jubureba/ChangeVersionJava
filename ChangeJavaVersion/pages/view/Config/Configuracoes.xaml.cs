using ChangeJavaVersion.Modelos.Interfaces;
using ChangeJavaVersion.pages.abstracts;
using ChangeJavaVersion.pages.view.config;
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

namespace ChangeJavaVersion.pages.view.Config {
    public partial class Configuracoes : Page {

        IAppConfig appConfig = new IAppConfigImpl();

        //Constructor
        public Configuracoes() {
            InitializeComponent();
            PreencherSliderButton();
            PreencherComboBox();
        }

        FindPath fpath = new FindPathImpl();
        SystemTrayTextToObject txtObjSTray = new SystemTrayTextToObjectImpl();



        /*
         * ================================================================
         * CONFIGURAÇOES ==================================================
         * ================================================================
         */

        private void PreencherComboBox() {
            List<string> idiomas = new List<string> { "Português", "English" };
            cbIdioma.ItemsSource = idiomas;
            cbIdioma.SelectedItem = ConfigurationManager.AppSettings.Get("Language");         
        }

        private void PreencherSliderButton() {
            bool systemTray = Boolean.Parse(ConfigurationManager.AppSettings.Get("System"));
            bool startup = Boolean.Parse(ConfigurationManager.AppSettings.Get("Startup"));

            sbTraySystem.IsChecked = systemTray;
            sbStartWindows.IsChecked = startup;
        }

        private void RegisterInStartup(bool isChecked) {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked) {
                registryKey.SetValue(messages.sistema, Process.GetCurrentProcess().MainModule.FileName);
            } else {
                registryKey.DeleteValue(messages.sistema);
            }
        }

        /*
         * ================================================================
         * AÇOES ==========================================================
         * ================================================================
         */

        private void sbTraySystem_Click(object sender, RoutedEventArgs e) {
            if (sbTraySystem.IsChecked == true) {
                appConfig.AddOrUpdateAppSettings("System","True");
            } else {             
                appConfig.AddOrUpdateAppSettings("System", "False");
            }
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            while (NavigationService.RemoveBackEntry() != null) ;
            NavigationService.Content = new Dashboard();
        }

                private void sbStartWindows_Click(object sender, RoutedEventArgs e) {
            if (sbStartWindows.IsChecked == true) {
                RegisterInStartup(true);
                appConfig.AddOrUpdateAppSettings("Startup", "True");
            } else {
                RegisterInStartup(false);
                appConfig.AddOrUpdateAppSettings("Startup", "False");
            }
        }

        private void cbIdioma_PreviewTextInput(object sender, TextCompositionEventArgs e) {

        }

        /*
        static void lineChanger(string newText, string fileName, int line_to_edit) {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }*/

    }
}
