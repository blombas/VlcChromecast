using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VlcChromeCast.Model;
using LibVLCSharp.Shared;

namespace VlcChromeCast
{
	public partial class App : Application
	{
        public static MediaPlayerData PlayerInitializer = null;

		public App()
		{
			InitializeComponent();

			MainPage = new MainPage();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
            
        }
	}
}
