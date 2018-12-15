using LiminhaTV.Data;
using LiminhaTV.Models;
using LiminhaTV.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LiminhaTV
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Channel> _list;

        DispatcherTimer _typingTimer;

        DispatcherTimer _toastTimer;

        ResourceLoader resouces;

        private bool itensByCategory = false;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info) {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public ObservableCollection<Channel> List
        {
            get { return _list; }

            set
            {
                _list = value;
                NotifyPropertyChanged("List");
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            resouces = new ResourceLoader();


            LoadChannels();

        }

        private void LoadChannels()
        {
            using (var db = new ChannelContext())
            {
                List = new ObservableCollection<Channel>(db.Channels.OrderBy(x => x.Title).ToList());

                if (List != null && List.Count > 0)
                {
                  grd.ItemsSource = List;
                }

                if (List.Count == 0)
                    txtNoItens.Visibility = Visibility.Visible;
            }
        }

        private async Task LoadChannelList(string url, bool replace)
        {
            var list = await DataService.GetM3U(url);


            if (list == null)
                throw new Exception(resouces.GetString("cant_add_list"));


            List = new ObservableCollection<Channel>(list.OrderBy(x => x.Title).ToList());

           // grd.ItemsSource = List;

            var container = new ListContainer { Type = ListType.Channel, Url = url };


            using (var db = new ChannelContext())
            {
                if (replace)
                {
                    var all = from c in db.Channels select c;
                    db.Channels.RemoveRange(all);
                    db.ListContainer.RemoveRange(from c in db.ListContainer where c.Type == ListType.Channel select c);
                }

                db.ListContainer.Add(container);
                db.Channels.AddRange(List);
                db.SaveChanges();
            }

        }

        private void grd_ItemClick(object sender, ItemClickEventArgs e)
        {
            Channel obj = (Channel)e.ClickedItem;

            if (obj != null)
            {
                Frame.Navigate(typeof(VideoPage), obj);
            }
        }

        private void SaveCurrentItemCategory()
        {
            var item = (Channel)grd.SelectedItem;

            if (item != null)
            {
                if (item.Category == null)
                    item.Category = 1;
                else
                    item.Category = null;

                using (var db = new ChannelContext())
                {
                    db.Channels.Update(item);
                    db.SaveChanges();
                }

                showMessage(string.Format(resouces.GetString("fav_added"), item.Title), 2000);
            }
        }

        private void ToggleItemsByCategory()
        {

            var list = new ObservableCollection<Channel>();

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

            List = list;

            itensByCategory = !itensByCategory;

          //  grd.ItemsSource = list;
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {

            switch (e.OriginalKey)
            {
                case VirtualKey.GamepadY:
                    SaveCurrentItemCategory();
                    e.Handled = true;

                    break;

                case VirtualKey.GamepadRightShoulder:
                    ToggleItemsByCategory();
                    e.Handled = true;

                    break;

                case VirtualKey.GamepadLeftShoulder:
                    AppBarToggleButton_Click(appUpload, null);
                    e.Handled = true;

                    break;

                case VirtualKey.GamepadView:
                    AppBarToggleButton_Click(appUpload, null);
                    e.Handled = true;

                    break;

                default:
                    break;
            }

            base.OnKeyDown(e);
        }

        void timer_Tick(object sender, object e)
        {
            if (List == null)
                return;

            string text = txtPesquisa.Text.ToLower();

            if (String.IsNullOrEmpty(text))
            {
              //  grd.ItemsSource = List;
                return;
            }

            var list = new List<Channel>();

            foreach (var item in List)
            {
                if (item.Title.ToLower().Contains(text))
                {
                    list.Add(item);
                }
            }
            
           // grd.ItemsSource = list;

            _typingTimer.Stop();
        }

        private void RichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            if (_typingTimer == null)
            {

                _typingTimer = new DispatcherTimer();
                _typingTimer.Interval = TimeSpan.FromMilliseconds(500);

                _typingTimer.Tick += timer_Tick;
            }
            _typingTimer.Stop();
            _typingTimer.Start();
        }

        private void AppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private async void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(InfoPage), null);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtUrl.Text))
                {
                    await LoadChannelList(txtUrl.Text, chkReplace.IsChecked.GetValueOrDefault());
                    myFlyout.Hide();
                }
            }
            catch (Exception ex)
            {
                var msg = new MessageDialog(ex.Message);
                msg.ShowAsync();
            }

        }

        private void toast_Tick(object sender, object e)
        {

            notification.Visibility = Visibility.Collapsed;
            _toastTimer.Stop();
        }

        private void showMessage(string message, int time)
        {
            notification.Text = message;

            notification.Visibility = Visibility.Visible;

            _toastTimer = new DispatcherTimer();
            _toastTimer.Interval = TimeSpan.FromMilliseconds(time);

            _toastTimer.Tick += toast_Tick;

            _toastTimer.Start();
        }

        private async void Epg_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(urlEpg.Text))
                {
                    await DataService.GetAndSaveEpg(urlEpg.Text);
                    epgFlyout.Hide();
                }
            }
            catch (Exception ex)
            {
                var msg = new MessageDialog(resouces.GetString("cant_add_epg"));
                msg.ShowAsync();
            }
        }

        private async void appUpdate_Click(object sender, RoutedEventArgs e)
        {
            bool m = false;

            using (var db = new ChannelContext())
            {
                var channelList = db.ListContainer.ToList();

                foreach (var item in channelList)
                {
                    m = true;
                    if (item.Type == ListType.EPG)
                    {
                        await DataService.GetAndSaveEpg(item.Url);
                    }
                    else if (item.Type == ListType.Channel) {
                        await LoadChannelList(item.Url, true);
                    }
                }
            }

            if (m)
            {
                var msg = new MessageDialog("updated");
                msg.ShowAsync();
            }
        }

        private void appPrograms_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ProgramPage));
        }
    }
}
