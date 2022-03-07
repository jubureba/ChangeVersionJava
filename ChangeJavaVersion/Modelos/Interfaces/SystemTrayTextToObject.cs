using ChangeJavaVersion.pages.view.config;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Modelos.Interfaces {

    internal interface SystemTrayTextToObject {

        abstract List<SystemCloseConfig> ReadFile(string filepath);
    }
}
