using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;

namespace BassNotesMaster.NotePlayer
{
    public static class SliderExtensions
    {
        public static void SetPercent(this Slider progressBar, double percentage,double seconds)
        {
            var animation = new DoubleAnimation(percentage, TimeSpan.FromSeconds(seconds));
            progressBar.BeginAnimation(RangeBase.ValueProperty, animation);
        }
    }
}