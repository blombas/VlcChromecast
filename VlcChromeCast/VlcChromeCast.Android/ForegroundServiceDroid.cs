using System;
using VlcChromeCast.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ForegroundServiceDroid))]
namespace VlcChromeCast.Droid
{
	public class ForegroundServiceDroid : IForegroundService
	{
		public ForegroundServiceDroid()
		{
		}

		public void StartForegroundService()
		{
			// Implement code that will start a foreground service on android
		}

		public void StopForegroundService()
		{
			// Implement code that will stop the foreground service on android
		}
	}
}
