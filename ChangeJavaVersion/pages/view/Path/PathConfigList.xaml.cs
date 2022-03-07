using ChangeJavaVersion.Dominio.Modelos.Enums;
using ChangeJavaVersion.Modelos.Interfaces;
using ChangeJavaVersion.Modelos.Utils;
using ChangeJavaVersion.pages.abstracts;
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
using System.Windows.Threading;

namespace ChangeJavaVersion.pages.view.config {
    /// <summary>
    /// Interação lógica para ConfigPath.xam
    /// </summary>
    /// 

    public partial class PathConfigList : Page {

        private DispatcherTimer dispatcherTimer;
        int page = 1;
        const int qtdRegistrosPage = 6;
        int qtdRegistros = 0;

        FindPath fpath = new FindPathImpl();
        TextToObject txtObj = new TextToObjectImpl();

        string javaFileName = ConfigNameEnum.names.JAVAFILE.GetEnumDescriptionAttribute().Description;

        public PathConfigList() {
            InitializeComponent();
            startTimer(0, 0, 1);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            atualizarPaginador();
            
        }

        private void startTimer(int hour, int min, int sec) {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(hour, min, sec);
        }

        private void atualizarPaginador() {

            enableAllButtons();

            qtdRegistros = txtObj.ReadFile(fpath.findJavaPath((fpath.findDocPath()), javaFileName)).Count;
            lbRegistros.Text = "Registros: " + qtdRegistros;
            tablePath.ItemsSource = PagingExtensions.Page(txtObj.ReadFile(fpath.findJavaPath((fpath.findDocPath()), javaFileName)), this.page, qtdRegistrosPage);

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

        private void enableAllButtons () {
            btnPrevious.IsEnabled = true;
            //btnFirst.IsEnabled = true;
            //btnLast.IsEnabled = true;
            btnNext.IsEnabled = true;
        }

        private void disableAllButtons() {
            btnPrevious.IsEnabled = false;
           // btnFirst.IsEnabled = false;
           // btnLast.IsEnabled = false;
            btnNext.IsEnabled = false;
        }

        private void btnVoltar_Click(object sender, RoutedEventArgs e) {
            while (NavigationService.RemoveBackEntry() != null) ;
            NavigationService.Content = new Dashboard();
        }

        private void btnNovo_Click_1(object sender, RoutedEventArgs e) {
            NavigationService.Content = new PathConfigForm();
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

        private void btnExcluir_Click(object sender, RoutedEventArgs e) {

            FindPath fpath = new FindPathImpl();
            TextToObject txtObj = new TextToObjectImpl();
            List<PathJava> pathJava = txtObj.ReadFile(fpath.findJavaPath((fpath.findDocPath()), javaFileName));
            string n = "nome;caminho" + Environment.NewLine;

            foreach (PathJava p in pathJava.ToList()) {
                if(p.Value == tablePath.SelectedValue.ToString()) {
                    pathJava.RemoveAt(tablePath.SelectedIndex);                  
                }
            }

            foreach (PathJava p in pathJava.ToList()) {
                n += p.Text + ";" + p.Value + Environment.NewLine;
            }

            File.WriteAllText(fpath.findJavaPath((fpath.findDocPath()), javaFileName), n);

            ShowBalloon("Sucesso!", tablePath.SelectedValue.ToString() + " Removido.");

            atualizarPaginador();

        }

        private void ShowBalloon(string title, string text) {
            foreach (Window window in Application.Current.Windows) {
                if (window.GetType() == typeof(Principal)) {
                    BalloonView balloonView = new BalloonView();
                    balloonView.ShowBalloon(title, text, (window as Principal).MyNotifyIcon);
                }
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            spinner.Visibility = Visibility.Hidden;
            dispatcherTimer.Stop();
            atualizarPaginador();
        }


    }
}
