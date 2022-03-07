using ChangeJavaVersion.pages.view.config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Modelos.Interfaces {
    internal class SystemTrayTextToObjectImpl : SystemTrayTextToObject {

        public List<SystemCloseConfig> ReadFile(string filepath) {
            var lines = File.ReadAllLines(filepath);

            var data = from l in lines.Skip(1)
                       let split = l.Split('=')
                       select new SystemCloseConfig {
                           Name = split[0],
                           Value = Boolean.Parse(split[1])
                       };

            return data.ToList();
        }
    }
}
