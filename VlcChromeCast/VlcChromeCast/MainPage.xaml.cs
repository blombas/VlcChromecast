using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
		}

		void OnAppearing(object sender, System.EventArgs e)
		{
			base.OnAppearing();
			((MainPageViewModel)BindingContext).OnAppearing();
		}

		void ButtonStart_Clicked(System.Object sender, System.EventArgs e)
		{
			DependencyService.Get<IForegroundService>().StartForegroundService();
		}

		void ButtonStop_Clicked(System.Object sender, System.EventArgs e)
		{
			DependencyService.Get<IForegroundService>().StopForegroundService();
		}
	}
}
