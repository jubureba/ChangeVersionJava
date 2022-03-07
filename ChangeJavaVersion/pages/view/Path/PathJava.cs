using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.pages.view.config {
    public class PathJava {

        public string Text { get; set; }

        public string Value { get; set; }

        public override string ToString() {
            return Value;
        }
    }
}
