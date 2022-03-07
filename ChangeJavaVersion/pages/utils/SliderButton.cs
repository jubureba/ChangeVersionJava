using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangeJavaVersion.pages.utils {
    public class SliderButton : System.Windows.Controls.Primitives.ToggleButton {
        public double ButtonWidth {
            get {
                return (double)GetValue(ButtonWidthProperty);
            }
            set {
                SetValue(ButtonWidthProperty, value);
            }
        }

        public static readonly System.Windows.DependencyProperty ButtonWidthProperty = System.Windows.DependencyProperty.Register("ButtonWidth", typeof(double), typeof(SliderButton), new System.Windows.PropertyMetadata(0.0));

        public string OnLabel {
            get {
                return (string)GetValue(OnLabelProperty);
            }
            set {
                SetValue(OnLabelProperty, value);
            }
        }

        public static readonly System.Windows.DependencyProperty OnLabelProperty = System.Windows.DependencyProperty.Register("OnLabel", typeof(string), typeof(SliderButton), new System.Windows.PropertyMetadata(""));

        public string OffLabel {
            get {
                return (string)GetValue(OffLabelProperty);
            }
            set {
                SetValue(OffLabelProperty, value);
            }
        }

        public static readonly System.Windows.DependencyProperty OffLabelProperty = System.Windows.DependencyProperty.Register("OffLabel", typeof(string), typeof(SliderButton), new System.Windows.PropertyMetadata(""));
    }
}
