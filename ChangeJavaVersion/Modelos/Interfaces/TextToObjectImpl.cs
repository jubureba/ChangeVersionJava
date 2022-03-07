using ChangeJavaVersion.pages.view.config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Modelos.Interfaces {
    internal class TextToObjectImpl : TextToObject {

        public List<PathJava> ReadFile(string filepath) {
            var lines = File.ReadAllLines(filepath);

            var data = from l in lines.Skip(1)
                       let split = l.Split(';')
                       select new PathJava {
                           Text = split[0],
                           Value = split[1]
                       };

            return data.ToList();
        }
    }
}
