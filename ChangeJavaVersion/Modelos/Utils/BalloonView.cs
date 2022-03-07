using ChangeJavaVersion.pages.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace ChangeJavaVersion.pages.abstracts {
    internal class BalloonView {

        public void ShowBalloon(string title, string text, Hardcodet.Wpf.TaskbarNotification.TaskbarIcon taskbarIcon) {
            Balloon balloon = new Balloon();
            balloon.BalloonText = title;
            balloon.BalloonMsg = text;
            taskbarIcon.ShowCustomBalloon(balloon, PopupAnimation.Slide, 4000);
        }
    }
}
