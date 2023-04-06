using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Pages.utils {
    internal class JavaVersion {
        public string Version { get; set; }
        public string PathJava { get; set; }

        public override string ToString() {
            return PathJava;
        }
    }
}
