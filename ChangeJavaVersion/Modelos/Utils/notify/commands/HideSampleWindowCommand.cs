using System.Windows;
using System.Windows.Input;

namespace ChangeJavaVersion.pages.notify.commands {
    /// <summary>
    /// Hides the main window.
    /// </summary>
    public class HideSampleWindowCommand : CommandBase<HideSampleWindowCommand>
    {
        public override void Execute(object parameter)
        {
            GetTaskbarWindow(parameter).Visibility = Visibility.Hidden;
            CommandManager.InvalidateRequerySuggested();
        }


        public override bool CanExecute(object parameter)
        {
            Window win = GetTaskbarWindow(parameter);
            return win != null && win.IsVisible;
        }
    }
}