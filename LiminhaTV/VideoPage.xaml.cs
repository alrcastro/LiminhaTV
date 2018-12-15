using LiminhaTV.Data;
using LiminhaTV.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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
    public sealed partial class VideoPage : Page
    {

        private List<Channel> List;

        private bool itensByCategory = false;

        private DisplayRequest AppDisplayRequest;

        private Channel channel;

        private List<ChProgram> lstPrograms;

        public VideoPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            channel = (Channel)e.Parameter;

            LoadChannels();

            if (channel != null)
            {
                PlayChannel();
            }

            if (AppDisplayRequest == null)
            {
                // This call creates an instance of the displayRequest object
                AppDisplayRequest = new DisplayRequest();
            }

            mediavlc.Width = Window.Current.Bounds.Width;
            mediavlc.Height = Window.Current.Bounds.Height;

            AppDisplayRequest.RequestActive();
        }

        private void PlayChannel()
        {
            mediavlc.Source = new Uri(channel.Path.ToString());
            mediavlc.AutoPlay = true;
            mediavlc.Play();
            lstPrograms = null;
        }


        private void LoadChannels()
        {
            using (var db = new ChannelContext())
            {
                List = db.Channels.ToList();

                if (List != null && List.Count > 0)
                {
                    lstChannel.ItemsSource = List;
                }
            }
        }

        private void ToggleItemsByCategory()
        {
            var list = new List<Channel>();

            if (itensByCategory)
            {
                list = List;
            }
            else
            {
                foreach (var item in List)
                {
                    if (item.Category == 1)
                    {
                        list.Add(item);
                    }
                }
            }

            itensByCategory = !itensByCategory;

            lstChannel.ItemsSource = list;

            if (list.Count > 0)
                lstChannel.SelectedIndex = 0;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            mediavlc.Stop();
            if (AppDisplayRequest != null)
                AppDisplayRequest.RequestRelease();

            base.OnNavigatedFrom(e);
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            switch (e.OriginalKey)
            {
                case VirtualKey.GamepadB:
                    if (mediavlc.IsFullWindow)
                        break;

                    if (splitMenu.IsPaneOpen)
                        splitMenu.IsPaneOpen = false;
                    else
                        Frame.GoBack();

                    e.Handled = true;
                    break;
                case VirtualKey.GamepadLeftShoulder:
                    splitMenu.IsPaneOpen = !splitMenu.IsPaneOpen;
                    e.Handled = true;
                    break;

                case VirtualKey.GamepadRightShoulder:
                    ToggleItemsByCategory();
                    e.Handled = true;
                    break;

                case VirtualKey.GamepadX:


                    var view = ApplicationView.GetForCurrentView();

                    if (view.IsFullScreenMode)
                        view.ExitFullScreenMode();
                    else
                        view.TryEnterFullScreenMode();

                    mediavlc.IsFullWindow = !mediavlc.IsFullWindow;

                    e.Handled = true;
                    break;

                case VirtualKey.GamepadY:
                    epgPopup.IsOpen = !epgPopup.IsOpen;

                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                        showTimeLine();
                    });

                    break;

                default:
                    break;
            }


            base.OnKeyDown(e);
        }

        private void showTimeLine()
        {
            if (lstPrograms == null)
            {
                using (var db = new ChannelContext())
                {
                    lstPrograms = db.Programs.Where(x => x.Channel == channel.Title && x.EndTime > DateTime.Now && x.StartTime < DateTime.Now.AddHours(16)).OrderBy(x => x.StartTime).ToList();

                    epgTime.GenerateTimeline(lstPrograms);
                }
            }
        }

        private void lstChannel_ItemClick(object sender, ItemClickEventArgs e)
        {
            channel = (Channel)e.ClickedItem;

            if (channel != null)
                PlayChannel();
        }
    }
}
