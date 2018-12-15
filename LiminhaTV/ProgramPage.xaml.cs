using LiminhaTV.Data;
using LiminhaTV.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LiminhaTV
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProgramPage : Page
    {

        private Dictionary<string,List<ChProgram>> lstPrograms;

        public ProgramPage()
        {
            this.InitializeComponent();

            LoadPrograms();
        }

        private void LoadPrograms() {

            using (var db = new ChannelContext())
            {
                lstPrograms = db.Programs.Where(x => x.EndTime > DateTime.Now && x.StartTime < DateTime.Now.AddHours(16)).OrderBy(x => x.StartTime)
                    .GroupBy(x => x.Channel).ToDictionary(x => x.Key, x => x.ToList());

                epgTime.GenerateFullTimeline(lstPrograms);
            }
        }
    }
}
