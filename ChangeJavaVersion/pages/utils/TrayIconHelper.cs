using ChangeJavaVersion.pages;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChangeJavaVersion.Pages.utils
{
    class TrayIconHelper
    {

        private readonly TaskbarIcon _taskbarIcon;
        private readonly List<MenuItem> _menuItems = new List<MenuItem>();

        public TrayIconHelper(TaskbarIcon taskbarIcon) {
            _taskbarIcon = taskbarIcon;
            UpdateContextMenu();
        }

        public void UpdateContextMenu() {
            var contextMenu = new ContextMenu();

            var javaKeys = ConfigurationManager.AppSettings.AllKeys
                .Where(k => k.StartsWith("java_"));

            foreach (var key in javaKeys) {
                var value = ConfigurationManager.AppSettings[key];
                var menuItem = new MenuItem { Header = key };

                var dashboard = new Dashboard();

                menuItem.Click += (sender, args) => dashboard.changeVersion(value);

                contextMenu.Items.Add(menuItem);
                _menuItems.Add(menuItem);
            }

            _taskbarIcon.ContextMenu = contextMenu;
        }

    }
}
