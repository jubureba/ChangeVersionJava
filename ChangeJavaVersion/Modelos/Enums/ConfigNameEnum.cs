using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ChangeJavaVersion.Dominio.Modelos.Enums.ConfigNameEnum;

namespace ChangeJavaVersion.Dominio.Modelos.Enums {
    public class ConfigNameEnum {

        public enum names {
            [field: Description("JavaPath.txt")]
            JAVAFILE,
            
            [field: Description("SystemTray.txt")]
            CONFIGFILE
        }

        
    }

    public static class MyEnumExtensions {
        public static DescriptionAttribute GetEnumDescriptionAttribute<T>(this T value) where T : struct {  
            Type type = typeof(T);

            if (!type.IsEnum)
                throw new InvalidOperationException(
                    "The type parameter T must be an enum type.");

            if (!Enum.IsDefined(type, value))
                throw new InvalidEnumArgumentException(
                    "value", Convert.ToInt32(value), type);

            FieldInfo fi = type.GetField(value.ToString(),
                BindingFlags.Static | BindingFlags.Public);

            return fi.GetCustomAttributes(typeof(DescriptionAttribute), true).
                Cast<DescriptionAttribute>().SingleOrDefault();
        }
    }

}
