using ChangeJavaVersion.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChangeJavaVersion.Pages.utils {
    internal class EnvironmentVariableManager {
        public static bool ChangeVariable(string variableName, string versionPath) {
            try {
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.Verb = "runas";
                processInfo.FileName = Assembly.GetExecutingAssembly().CodeBase;
                Process.Start(processInfo);
                Application.Current.Shutdown();
            } catch (Exception ex) {
                Console.WriteLine("Não foi possível iniciar a aplicação como administrador.");
                Console.WriteLine(ex);
                return false;
            }

            if (CheckVersion(variableName, EnvironmentVariableTarget.Machine).Equals(versionPath)) {
                MessageBox.Show(String.Format(messages.versao_inalterada_completa, versionPath), messages.erro, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else {
                MessageBox.Show(String.Format(messages.versao_alterada_completa, CheckVersion(variableName, EnvironmentVariableTarget.Machine).ToString(), versionPath), messages.sucesso, MessageBoxButton.OK, MessageBoxImage.Information);
                SetVariable(variableName, versionPath, EnvironmentVariableTarget.Machine);
                return true;
            }
        }

        public static void SetVariable(string variableName, string path, EnvironmentVariableTarget target) {
            System.Environment.SetEnvironmentVariable(variableName, path, target);
        }

        public static string CheckVersion(string variableName, EnvironmentVariableTarget target) {
            return System.Environment.GetEnvironmentVariable(variableName, target);
        }
    }
}
