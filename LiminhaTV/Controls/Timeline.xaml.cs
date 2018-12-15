using LiminhaTV.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace LiminhaTV.Controls
{
    public sealed partial class Timeline : UserControl
    {
        public Timeline()
        {
            this.InitializeComponent();
        }


        public void GenerateFullTimeline(Dictionary<string, List<ChProgram>> programMatrix) {

            foreach (var item in programMatrix)
            {
                GenerateTimeline(item.Value);
            }
        }

        public void GenerateTimeline(List<ChProgram> programs)
        {
            int hour = 300;
            int size = 0;

            noProg.Visibility = Visibility.Collapsed;

            if (programs.Count == 0)
            {
                noProg.Visibility = Visibility.Visible;
                return;
            }

            var parentProgram = new StackPanel();
            parentProgram.Orientation = Orientation.Horizontal;
            parentProgram.BorderThickness = new Thickness(1);

            var channelText = new TextBlock();
            channelText.Text = programs[0].Channel;
            parentProgram.Children.Add(channelText);


            foreach (var item in programs)
            {
                size = Convert.ToInt32((Math.Abs((item.StartTime - item.EndTime).TotalMinutes) / 60) * hour);

                if (item.StartTime < DateTime.Now) {
                    size -= Convert.ToInt32((Math.Abs((DateTime.Now - item.StartTime).TotalMinutes) / 60) * hour);
                }

                var stack = new StackPanel();
                stack.Orientation = Orientation.Vertical;
                stack.Background = new SolidColorBrush(Colors.CadetBlue);
                stack.BorderThickness = new Thickness(1);

                var texttime = new TextBlock();
                texttime.Text = item.StartTime.ToString("HH:mm");

                var textTitle = new TextBlock();
                textTitle.Text = item.Title;
                textTitle.Width = size;

                stack.Children.Add(texttime);
                stack.Children.Add(textTitle);
                parentProgram.Children.Add(stack);
            }

            parent.Children.Add(parentProgram);

            loading.Visibility = Visibility.Collapsed;
           
        }

        
    }
}
