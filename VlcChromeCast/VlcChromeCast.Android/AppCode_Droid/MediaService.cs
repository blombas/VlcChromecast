using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using VlcChromeCast.AppCode_Shared;
using VlcChromeCast.Model;
using Xamarin.Forms;
using LibVLCSharp.Shared;

namespace VlcChromeCast.Droid.AppCode_Droid
{
    [Service]
    public class MediaService : Service
    {
        CancellationTokenSource _cts;
        public const int SERVICE_RUNNING_NOTIFICATION_ID = Settings.NotificationId.MediaNotification.SERVICE_RUNNING_NOTIFICATION_ID;
        public const int notificationId = Settings.NotificationId.MediaNotification.NotificationId;


        Notification livenotific = null;

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                try
                {
                    //INVOKE THE SHARED CODE
                    var backgroundwork = new MediaBackgroundWork();
                    //Call any background method needed here
                    backgroundwork.LogMediaState(_cts.Token).Wait();
                    //as of now background work methods are empty, if you need you can use//
                }
                catch (System.OperationCanceledException ex)
                {
                    Toast.MakeText(this, ex.Message.ToString(), ToastLength.Short);
                }
                finally
                {
                    if (_cts.IsCancellationRequested)
                    {
                        var message = new CancelledMessage();
                        Device.BeginInvokeOnMainThread(
                            () => MessagingCenter.Send(message, "CancelledMessage")
                        );
                    }
                }

            }, _cts.Token);


            // Enlist this instance of the service as a foreground service
            var builder = _MakeNotificationBuilder();
            MediaPlayerData data = JsonConvert.DeserializeObject<MediaPlayerData>(intent.GetStringExtra(Settings.ValueTranferKey));
            builder.SetContentText("Media playing");
            builder.SetContentIntent(_MakePendingIntent(intent));
            livenotific = builder.Build();
            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, livenotific);
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
        }

        private Android.Support.V4.App.NotificationCompat.Builder _MakeNotificationBuilder()
        {
            return new Android.Support.V4.App.NotificationCompat.Builder(this, ((int)Settings.NotificationChannel.MediaPlayer).ToString())
                    .SetContentTitle("Media is playing")
                    .SetSmallIcon(Resource.Mipmap.icon_round)
                    .SetOngoing(true)
                    //.AddAction(BuildRestartTimerAction())
                    //.AddAction(BuildStopServiceAction())
                    ;
        }
        private PendingIntent _MakePendingIntent(Intent intent)
        {
            // When the user clicks the notification, SecondActivity will start up.
            var resultIntent = new Intent(this, typeof(MainActivity));

            // Pass some values to SecondActivity:
            Bundle bdl = new Bundle();
            //bdl.PutInt(MainActivity.FormToOpenKey, intent.GetIntExtra(MainActivity.FormToOpenKey, 0));
            bdl.PutString(Settings.ValueTranferKey, intent.GetStringExtra(Settings.ValueTranferKey));
            resultIntent.PutExtras(bdl);

            // Construct a back stack for cross-task navigation:
            var stackBuilder = TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(MainActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:
            return stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

        }
    }
}