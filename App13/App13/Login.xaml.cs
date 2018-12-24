using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            loginfail.IsVisible=false;


        }

		private async void login_Clicked(object sender, EventArgs e)
		{
			

			HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://192.168.0.12:45455/api/users1");
                if (response.StatusCode==HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    var json = await responseContent.ReadAsStringAsync();
                    await Navigation.PushAsync(new MainPage());
                }

                else
                {
                    loginfail.IsVisible=true;
                }
            }
            catch (Exception )
            {
                loginfail.IsVisible=true;
                loginfail.Text="Проблемы с соединением. Перезапустите приложение ";
            }
		


		}
	}
}