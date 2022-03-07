using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.pages.view.config {
    public class SystemCloseConfig {


        public string Name { get; set; }

        public Boolean Value { get; set; }

        public override string ToString() {
            return Name;
        }
    }
}
