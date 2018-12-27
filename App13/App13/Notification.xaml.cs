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
	public partial class Notification : ContentPage
	{
		private string _username;
		private string _pass;
		private string _secKey;
		private string _loadID;
	   
		public Notification (string msg, string username, string userpass, string userkey, string loadid)
		{
			_username=username;
			_pass=userpass;
			_secKey=userkey;
			_loadID=loadid;
			InitializeComponent ();
			message.Text=msg;
			submitbid.Clicked+=_submit;
			cancel.Clicked+=_cancel;

			
		}

		private void _cancel(object sender, EventArgs e)
		{

			App.Current.MainPage=new NavigationPage(new MainPage(_username, _pass));
		}

		private void _submit(object sender, EventArgs e)
		{
			MainPage.bid=true;
            App.Current.MainPage=new NavigationPage(new MainPage(_username, _pass));
        }
	}
}