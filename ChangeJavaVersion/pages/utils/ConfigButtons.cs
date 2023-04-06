using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Pages.utils {
    internal class ConfigButtons {
        public string button { get; set; }
        public bool value { get; set; }

        public bool ToBoolean() {
            return value;
        }
    }
}
