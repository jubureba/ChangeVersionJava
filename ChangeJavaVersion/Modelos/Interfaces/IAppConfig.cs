using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.Modelos.Interfaces {
    internal interface IAppConfig {
        public abstract void AddOrUpdateAppSettings(string key, string value);
    }
}
