using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WpfExtensions
{
    public static class ControlExtension
    {
        public static void StartStoryboardAnimation(this Control control, Storyboard storyboard)
        {
            Storyboard.SetTarget(storyboard, control);
            ThrowExceptionIfNull(storyboard, control);
            storyboard.Begin();
        }

        private static void ThrowExceptionIfNull(Storyboard storyboard, Control control)
        {
            if (storyboard == null)
                throw new Exception(
                    String.Format("Coulnd't find animation attached to Control {0}}", control));
        }

    }
}
