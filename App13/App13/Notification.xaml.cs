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
	public partial class Notification : ContentPage
	{
		private string _username;
		private string _pass;
		private string _secKey;
		private string _loadID;
        string froms;
        string tos;
		public Notification (string msg, string username, string userpass, string userkey, string loadid)
		{
			_username=username;
			_pass=userpass;
			_secKey=userkey;
			_loadID=loadid;
			InitializeComponent ();
		   submitbid.Clicked+=_submit;
			cancel.Clicked+=_cancel;
			direction.Clicked+=_direction;

			var result = msg.Split("%%".ToCharArray());
			milestopickup.Text = "Миль до загрузки: "+result[0];
			from.Text ="From: "+result[2];
			to.Text ="To: "+result[4];
			froms = result[2];
			tos = result[4];
			weight.Text ="Вес/кольчество: "+result[6];
		  //totalmiles.Text ="Всего миль: "+result[8];
			pickupdate.Text="Дата загрузки: "+result[10];
			deliverydate.Text="Дата доставки: "+result[12];


		}

		private void _direction(object sender, EventArgs e)
		{
			var uri = new Uri("https://www.google.com/maps/dir/?api=1&origin="+froms+"&destination="+tos);
			Device.OpenUri(uri);
		}

		private void _cancel(object sender, EventArgs e)
		{

			App.Current.MainPage=new NavigationPage(new MainPage(_username, _pass));
		}

		private void _submit(object sender, EventArgs e)
		{
			try
			{
			  
				HttpClient client = new HttpClient();
				var makebid = new bid { userName=_username, userKey=_secKey,currentbid=bid.Text,loadID=_loadID };
				string url = "http://192.168.0.12:45455/api/loads1/";
				var json = JsonConvert.SerializeObject(makebid);
				var resp = new StringContent(json, Encoding.UTF8, "application/json");
				var response = client.PutAsync(url, resp).Result;
				

				if (response.StatusCode==HttpStatusCode.OK)
				{

					MainPage.bid=true;
					App.Current.MainPage=new NavigationPage(new MainPage(_username, _pass));

				}

				else
				{
					MainPage.refuse=true;
					App.Current.MainPage=new NavigationPage(new MainPage(_username, _pass));

				}



			  
			}
			catch (Exception)
			{
				MainPage.refuse =true;
				App.Current.MainPage=new NavigationPage(new MainPage(_username, _pass));

			}



		}
		
	   
		
	}
}