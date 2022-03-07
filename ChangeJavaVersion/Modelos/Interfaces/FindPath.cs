using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Modelos.Interfaces {
    public interface FindPath {

        abstract string findDocPath();

        abstract string findJavaPath(string docPath, string nameFile);

    }
}
