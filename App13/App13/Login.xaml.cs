using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App13
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();

			login.Clicked+=login_Clicked;


		}

		private async void login_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new MainPage());
		}
	}
}