using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Modelos.Interfaces {
    public class FindPathImpl : FindPath {

        public string findDocPath() {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public string findJavaPath(string docPath, string nameFile) {
            return System.IO.Path.Combine(docPath, nameFile);
        }
    }
}
