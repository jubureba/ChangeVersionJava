using ChangeJavaVersion.Modelos.Interfaces;
using ChangeJavaVersion.pages.view.config;
using ChangeJavaVersion.Properties;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace ChangeJavaVersion.pages {
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    /// 

  
    public partial class Dashboard : Page {

        private DispatcherTimer dispatcherTimer;

        FindPath fpath = new FindPathImpl();
        TextToObject txtObj = new TextToObjectImpl();
        SystemTrayTextToObject txtObjSTray = new SystemTrayTextToObjectImpl();
        public Dashboard() {
            InitializeComponent();

            if (!File.Exists(System.IO.Path.Combine(fpath.findDocPath(), "JavaPath.txt"))) {
                string[] lines = { "nome;caminho" };
                File.AppendAllLines(System.IO.Path.Combine(fpath.findDocPath(), "JavaPath.txt"), lines);
            } 
            cbVersion.ItemsSource = txtObj.ReadFile(fpath.findJavaPath((fpath.findDocPath()), "JavaPath.txt"));

            if (!File.Exists(System.IO.Path.Combine(fpath.findDocPath(), "SystemTray.txt"))) {
                string[] linesTitle = { "name;value" };
                File.AppendAllLines(System.IO.Path.Combine(fpath.findDocPath(), "SystemTray.txt"), linesTitle);
            }

            startTimer(0,0,1);

        }

       private void startTimer(int hour, int min, int sec) {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(hour, min, sec);
        }

        private void comboBox1_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            foreach (object item in System.Windows.Data.CollectionViewSource.GetDefaultView(cbVersion.ItemsSource)) {
                PathJava emp = item as PathJava;
                if (emp.Text.ToUpper().Contains(e.Text.ToUpper())) {
                    cbVersion.SelectedItem = emp;
                }
            }
        }

        private void btnChange_Click(object sender, RoutedEventArgs e) {
            if (cbVersion.SelectedItem != null) {
                changeVersion(cbVersion.SelectedValue.ToString());
            } else {
                MessageBox.Show("Unable to change version.");
            }   
        }

        public void cleanVersion() {
            cbVersion.SelectedItem = null;
        }

        public Boolean changeVersion(string versionPath) {
            //string javaHomeString = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + versionPath;
            if (System.Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine).Equals(versionPath)) {
                MessageBox.Show("Uma mesma versão da JDK já está no seu JAVA_HOME.\nReabra seu cmd e digite java -version\n" + versionPath, "A mesma versão já está ativa!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else {
                MessageBox.Show(
                    "Versão anterior: " + System.Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine).ToString() +
                    "\n" +
                    "Versão atual: " + versionPath
                    , "Versão alterada com sucesso!", MessageBoxButton.OK, MessageBoxImage.Information) ;
                System.Environment.SetEnvironmentVariable("JAVA_HOME", versionPath, EnvironmentVariableTarget.Machine);
                return true;
            }
            cleanVersion(); 
        }

        private string checkVersion(string variableSystem) {
            return System.Environment.GetEnvironmentVariable(variableSystem);
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e) {
            NavigationService.Content = new PathConfigList();
        }

        private void btnCheckVersion_Click(object sender, RoutedEventArgs e) {
            spinner.Visibility = Visibility.Visible;
            dispatcherTimer.Start();

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {

            spinner.Visibility = Visibility.Hidden;
            dispatcherTimer.Stop();
            MessageBox.Show(System.Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine), messages.sucesso, MessageBoxButton.OK, MessageBoxImage.Information);

        }


    }


}
