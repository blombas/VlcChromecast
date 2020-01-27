using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using LibVLCSharp.Forms.Shared;
using Xamarin.Forms;
using VlcChromeCast.Model;
using VlcChromeCast.AppCode_Shared;
using VlcChromeCast.Droid.AppCode_Droid;
using Newtonsoft.Json;
using Android.Content;
using LibVLCSharp.Shared;

namespace VlcChromeCast.Droid
{
    [Activity(Label = "VlcChromeCast", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            LibVLCSharpFormsRenderer.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            MakeNotificationChannel(Settings.NotificationChannel.MediaPlayer.ToString(), ((int)Settings.NotificationChannel.MediaPlayer).ToString());
            HandleMessageSubscription();

            App.PlayerInitializer = Intent.HasExtra(Settings.ValueTranferKey) ? JsonConvert.DeserializeObject<MediaPlayerData>(Intent.GetStringExtra(Settings.ValueTranferKey)) : null;

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void MakeNotificationChannel(string ChannelName, string channelId)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(channelId, ChannelName, NotificationImportance.High)
            {
                LockscreenVisibility = NotificationVisibility.Public
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        private void HandleMessageSubscription()
        {
            MessagingCenter.Subscribe<StartLongRunningTaskMessage<MediaPlayerData>>(this, "StartLongRunningTaskMessage", callback =>
            {
                if (!ServiceHelper.IsServiceRunning(this, typeof(MediaService)))
                {
                    Bundle bdl = new Bundle();
                    //bdl.PutInt(FormToOpenKey, (int)AppCodeSvk_Shared.Settings.OpenAfterNotificationClick.JourneyPage);
                    bdl.PutString(Settings.ValueTranferKey, JsonConvert.SerializeObject(callback.TaskData));

                    this.StartForegroundServiceComapt<MediaService>(bdl);
                }
            });

            MessagingCenter.Subscribe<StopLongRunningTaskMessage>(this, nameof(StopLongRunningTaskMessage), callback =>
            {
                var intent = new Intent(this, typeof(MediaService));
                StopService(intent);
            });
        }
    }
}