using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace VlcChromeCast.Droid.AppCode_Droid
{
    public static class ServiceHelper
    {
        public static void StartForegroundServiceComapt<T>(this Context context, Bundle args = null) where T : Service
        {
            var intent = new Intent(context, typeof(T));
            if (args != null)
            {
                intent.PutExtras(args);
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                context.StartForegroundService(intent);
            }
            else
            {
                context.StartService(intent);
            }
        }

        public static bool IsServiceRunning(Context context, Type serviceClass)
        {
            ActivityManager manager = (ActivityManager)context.GetSystemService(Context.ActivityService);
            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Process == context.PackageName && service.Service.ClassName.EndsWith(serviceClass.Name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}