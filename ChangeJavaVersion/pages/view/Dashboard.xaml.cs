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

    public partial class Dashboard : Page {

        private DispatcherTimer dispatcherTimer;
        private FindPath fpath { get; set; }
        private TextToObject txtObj { get; set; }
        private SystemTrayTextToObject txtObjSTray { get; set; }
        private String nameVariableJDK { get; set; }

        public Dashboard() {
            InitializeComponent();

            fpath = new FindPathImpl();
            txtObj = new TextToObjectImpl();
            txtObjSTray = new SystemTrayTextToObjectImpl();
            nameVariableJDK = "JAVA_HOME";

            if (!File.Exists(System.IO.Path.Combine(fpath.findDocPath(), "JavaPath.txt"))) {
                string[] lines = { "nome;caminho" };
                File.AppendAllLines(System.IO.Path.Combine(fpath.findDocPath(), "JavaPath.txt"), lines);
            } 
            cbVersion.ItemsSource = txtObj.ReadFile(fpath.findJavaPath((fpath.findDocPath()), "JavaPath.txt"));

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

        public bool changeVersion(string versionPath) {
            if (checkVersion("JAVA_HOME", EnvironmentVariableTarget.Machine).Equals(versionPath)) {
                MessageBox.Show(String.Format(messages.versao_inalterada_completa, versionPath), messages.erro, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else {
                MessageBox.Show(String.Format(messages.versao_alterada_completa, checkVersion(nameVariableJDK, EnvironmentVariableTarget.Machine).ToString(), versionPath), messages.sucesso, MessageBoxButton.OK, MessageBoxImage.Information);
                setVersionInEnvironmentVariable(nameVariableJDK, versionPath, EnvironmentVariableTarget.Machine);
                return true;
            }
            cleanVersion(); 
        }

        private string checkVersion(string variableSystem, EnvironmentVariableTarget target) {
            return System.Environment.GetEnvironmentVariable(variableSystem, target);
        }

        private void setVersionInEnvironmentVariable(string variableName, string path, EnvironmentVariableTarget target) {
            System.Environment.SetEnvironmentVariable(variableName, path, target);
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
            MessageBox.Show(checkVersion(nameVariableJDK, EnvironmentVariableTarget.Machine), messages.sucesso, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
