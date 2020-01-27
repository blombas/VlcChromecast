using System;
using System.Collections.Generic;
using System.Text;

namespace VlcChromeCast.AppCode_Shared
{
    public class Settings
    {
        //Notification channel with ChannelId
        public enum NotificationChannel
        {
            MediaPlayer = 1
        }

        //This is used to transfer value to activity//
        public static readonly string ValueTranferKey = "Value";

        //This is unique notification Id for each service
        public class NotificationId
        {
            public class MediaNotification
            {
                public const int SERVICE_RUNNING_NOTIFICATION_ID = 10000;
                public const int NotificationId = 10000;
            }
            
        }

        public static long currentTime = 0;
    }
}
