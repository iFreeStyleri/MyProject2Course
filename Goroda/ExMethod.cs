using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Controls;
using Goroda.DataSource;

namespace Goroda
{
    public static class ExMethod
    {
        public static void RightAnimCard(this Card card)
        {
            var anim = new ThicknessAnimation(new Thickness(card.Margin.Left - 745, card.Margin.Top, card.Margin.Right + 745, card.Margin.Bottom), new Duration(TimeSpan.FromSeconds(1)));
            anim.AccelerationRatio = 0.8;
            card.BeginAnimation(Card.MarginProperty, anim);
        }
        public static void LeftAnimCard(this Card card)
        {
            var anim = new ThicknessAnimation(new Thickness(card.Margin.Left + 745, card.Margin.Top, card.Margin.Right - 745, card.Margin.Bottom), new Duration(TimeSpan.FromSeconds(1)));
            anim.AccelerationRatio = 0.8;
            card.BeginAnimation(Card.MarginProperty, anim);
        }
        public static void UpAnimCard(this Card card)
        {
            var anim = new ThicknessAnimation(new Thickness(card.Margin.Left, card.Margin.Top - 500, card.Margin.Right, card.Margin.Bottom + 500), new Duration(TimeSpan.FromMilliseconds(250)));
            anim.Completed += (s, a) =>
            {
                card.IsEnabled = true;
            };
            anim.AccelerationRatio = 0.8;
            card.BeginAnimation(Card.MarginProperty, anim);
        }
        public static void DownAnimCard(this Card card, TextBox[] textBoxes)
        {
            var anim = new ThicknessAnimation(new Thickness(card.Margin.Left, card.Margin.Top + 500, card.Margin.Right, card.Margin.Bottom - 500), new Duration(TimeSpan.FromMilliseconds(250)));
            anim.Completed += (s,a) => 
            {
                card.IsEnabled = false;
                foreach(var box in textBoxes)
                {
                    box.Text = null;
                }
            };
            anim.AccelerationRatio = 0.8;
            card.BeginAnimation(Card.MarginProperty, anim);
        }
        public static void AddRange(this ItemCollection view, object[] objs)
        {
            foreach(var item in objs)
            {
                view.Add(item);
            }
        }
    }
}
