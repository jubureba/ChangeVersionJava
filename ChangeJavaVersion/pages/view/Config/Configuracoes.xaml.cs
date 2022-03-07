using ChangeJavaVersion.Modelos.Interfaces;
using ChangeJavaVersion.pages.abstracts;
using ChangeJavaVersion.pages.view.config;
using System;
using System.Collections.Generic;
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
        
        //Constructor
        public Configuracoes() {
            InitializeComponent();
            PreencherSliderButton();
        }

        FindPath fpath = new FindPathImpl();
        SystemTrayTextToObject txtObjSTray = new SystemTrayTextToObjectImpl();

        private void PreencherSliderButton() {
            List<SystemCloseConfig> sbTraySystemTxtValue = txtObjSTray.ReadFile(fpath.findJavaPath((fpath.findDocPath()), "SystemTray.txt"));

            sbTraySystem.IsChecked = sbTraySystemTxtValue[0].Value;
        }

        private void sbTraySystem_Click(object sender, RoutedEventArgs e) {

            if (sbTraySystem.IsChecked == true) {
                List<SystemCloseConfig> valoresTxt = txtObjSTray.ReadFile(fpath.findJavaPath((fpath.findDocPath()), "SystemTray.txt"));
                string text = valoresTxt[0].Name.ToString() + "=" + valoresTxt[0].Value.ToString();
                string textFinal = text.Replace("False", "True");
                string pathComplete = System.IO.Path.Combine(fpath.findDocPath() + @"\\SystemTray.txt");

                lineChanger(textFinal, pathComplete, 2);
            } else {
                List<SystemCloseConfig> valoresTxt = txtObjSTray.ReadFile(fpath.findJavaPath((fpath.findDocPath()), "SystemTray.txt"));
                string text = valoresTxt[0].Name.ToString() + "=" + valoresTxt[0].Value.ToString();
                string textFinal = text.Replace("True", "False");
                string pathComplete = System.IO.Path.Combine(fpath.findDocPath() + @"\\SystemTray.txt");

                lineChanger(textFinal, pathComplete, 2);
            }
        }

        static void lineChanger(string newText, string fileName, int line_to_edit) {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            while (NavigationService.RemoveBackEntry() != null) ;
            NavigationService.Content = new Dashboard();
        }
    }
}
