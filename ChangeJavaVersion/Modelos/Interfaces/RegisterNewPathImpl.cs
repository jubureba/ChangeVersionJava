using ChangeJavaVersion.Dominio.Modelos.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChangeJavaVersion.Modelos.Interfaces {
    internal class RegisterNewPathImpl : RegisterNewPath {
        public bool register(string docPath, string javaVersion, string pathJdk) {

            string javaFileName = ConfigNameEnum.names.JAVAFILE.GetEnumDescriptionAttribute().Description;
            if (!File.Exists(System.IO.Path.Combine(docPath, javaFileName))) {
                string[] lines = { "nome;caminho" };
                File.AppendAllLines(System.IO.Path.Combine(docPath, javaFileName), lines);
                return false;
            } else {
                string[] lines = { javaVersion + ";" + pathJdk };
                File.AppendAllLines(System.IO.Path.Combine(docPath, javaFileName), lines);
                return true;
            }
        }
    }
}
