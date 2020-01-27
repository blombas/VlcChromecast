using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using VlcChromeCast.Model;
using LibVLCSharp.Shared;

namespace VlcChromeCast
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            HandleReceivedMessages();
        }

        void OnAppearing(object sender, System.EventArgs e)
        {
            base.OnAppearing();

            if (App.PlayerInitializer == null)
                ((MainPageViewModel)BindingContext).OnAppearing();
            else
                ReInitialisePlayer(App.PlayerInitializer);
        }

        void ButtonStart_Clicked(object sender, EventArgs e)
        {
            var player = ((MainPageViewModel)BindingContext).MediaPlayer;

            player.TimeChanged -= TimeChanged;
            player.TimeChanged += TimeChanged;

            var playerData = new MediaPlayerData
            {
                //VideoTrackDescription = player.VideoTrackDescription,
                //VideoTrackCount = player.VideoTrackCount,
                //VideoTrack = player.VideoTrack,
                State = player.State,
                //Spu = player.Spu,
                //Position = player.Position,
                //AudioTrack = player.AudioTrack,
                //AudioTrackCount = player.AudioTrackCount,
                //AudioTrackDescription = player.AudioTrackDescription,
                //Channel = player.Channel,
                //Media = player.Media,
                Time = player.Time
            };
            var message = new StartLongRunningTaskMessage<MediaPlayerData> { TaskData = playerData };
            MessagingCenter.Send(message, "StartLongRunningTaskMessage");
        }

        void ButtonStop_Clicked(object sender, EventArgs e)
        {
            var message = new StopLongRunningTaskMessage();
            MessagingCenter.Send(message, nameof(StopLongRunningTaskMessage));

            var player = ((MainPageViewModel)BindingContext).MediaPlayer;
            player.TimeChanged -= TimeChanged;
        }

        private async void TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            await Task.Run(async () =>
            {
                try
                {
                    AppCode_Shared.Settings.currentTime = e.Time;
                }
                catch
                {
                }
                finally
                {
                    
                }
            });
        }

        private async void HandleReceivedMessages()
        {
            try
            {
                MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", callback =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Media Cancelled", "Cancelled", "Ok");
                    });
                });                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private void ReInitialisePlayer(MediaPlayerData data)
        {
            var mediaPlayer = ((MainPageViewModel)BindingContext).MediaPlayer;
            mediaPlayer?.Stop();

            //((MainPageViewModel)BindingContext).Initialize();
            //mediaPlayer = ((MainPageViewModel)BindingContext).MediaPlayer;
            mediaPlayer.Play();
            mediaPlayer.Time = AppCode_Shared.Settings.currentTime;
        }
    }
}
