﻿using App13.Models;
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
        public static bool online;
       
        public Login ()
		{
			InitializeComponent ();

			login.Clicked+=login_Clicked;
			loginfail.IsVisible=false;
            myname.Clicked+=Myname_Clicked;


		}

        private void Myname_Clicked(object sender, EventArgs e)
        {
            var uri = new Uri("https://www.instagram.com/rostyslavzhovnir/");
            Device.OpenUri(uri);
        }

        private void login_Clicked(object sender, EventArgs e)
        {



            try
            {
             

                HttpClient client = new HttpClient();
                var user = new userLogin { name=loginEntry.Text, pass=passwordEntry.Text,seckey=seckey };
                string url = "http://bakunexpress.com/api/users1/";
                var json = JsonConvert.SerializeObject(user);
                var resp = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, resp).Result;


                if (response.StatusCode==HttpStatusCode.OK)
                {
                    online=true;
                    
                    //MainPage mainPage = new MainPage();
                    //await Navigation.PushAsync(new MainPage(loginEntry.Text, passwordEntry.Text));
                    App.Current.MainPage=new NavigationPage(new MainPage(loginEntry.Text, passwordEntry.Text,null));
                    //mainPage.tempData(loginEntry.Text, passwordEntry.Text);
                    //await Navigation.PushAsync(mainPage);

                }

                else
                {
                    loginfail.IsVisible=true;
                    online=false;
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