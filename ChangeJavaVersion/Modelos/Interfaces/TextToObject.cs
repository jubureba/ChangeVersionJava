using ChangeJavaVersion.pages.view.config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Modelos.Interfaces {
    internal interface TextToObject {

        abstract List<PathJava> ReadFile(string filepath);
    }
}
