using App13.Models;
using Newtonsoft.Json;
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
        public static string seckey ;
       
        public Login ()
		{
			InitializeComponent ();

			login.Clicked+=login_Clicked;
			loginfail.IsVisible=false;


		}

        private void login_Clicked(object sender, EventArgs e)
        {



            try
            {
             

                HttpClient client = new HttpClient();
                var user = new userLogin { name=loginEntry.Text, pass=passwordEntry.Text,seckey=seckey };
                string url = "http://192.168.0.12:45455/api/users1/";
                var json = JsonConvert.SerializeObject(user);
                var resp = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, resp).Result;


                if (response.StatusCode==HttpStatusCode.OK)
                {
                  
                    
                    //MainPage mainPage = new MainPage();
                    //await Navigation.PushAsync(new MainPage(loginEntry.Text, passwordEntry.Text));
                    App.Current.MainPage=new NavigationPage(new MainPage(loginEntry.Text, passwordEntry.Text,null));
                    //mainPage.tempData(loginEntry.Text, passwordEntry.Text);
                    //await Navigation.PushAsync(mainPage);

                }

                else
                {
                    loginfail.IsVisible=true;
                }



                //}

                //else
                //{
                //    loginfail.IsVisible=true;
                //}
            }
            catch (Exception)
            {
                loginfail.IsVisible=true;
                loginfail.Text="Проблемы с соединением. Перезапустите приложение ";
            }



        }
    }
}