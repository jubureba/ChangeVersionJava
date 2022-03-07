using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Modelos.Interfaces {
    internal interface RegisterNewPath {

        abstract Boolean register(string docPath, string javaVersion, string pathJdk);
    }
}
