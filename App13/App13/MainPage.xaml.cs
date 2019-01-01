using App13.Models;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;

namespace App13
{
    public partial class MainPage : ContentPage
    {
        private string _pass;
        private string _name;
        private string _loadid;
        public static bool bid;
        public static bool refuse;
        public static bool intransit;
        public MainPage(string name, string pass,string loadid)
        {
            InitializeComponent();
            
            offline.IsVisible=false;
            pending.IsVisible=false;
            delivered.IsVisible=false;

            readyForPickup.Clicked+=ReadyForPickup_Clicked;
            offline.Clicked+=Offline_Clicked;
            delivered.Clicked+=Delivered_Clicked;
            currentLocationName.Text="Вы Offline";
            message.Text="";
            username.Text="Welcome back, "+name;
            currentLocation.Text="Для начала работы нажмите 'Готов к загрузке'";
            _name=name;
            _pass=pass;
            _loadid=loadid;
            if (refuse==true)
            {
                bid=false;
                intransit=false;
                currentLocationName.Text="К сожалению , ваше предложение цены отклонено!!";
                message.Text="";
               // username.Text="Welcome back, "+name;
                currentLocation.Text="Для начала работы нажмите 'Готов к загрузке'";
                offline.IsVisible=false;
                readyForPickup.IsVisible=true;
                pending.IsVisible=false;
            }
            if (bid==true)
            {
                intransit=false;
                refuse=false;
                currentLocationName.Text="Запрос принят!";
                message.Text="";
               // username.Text=name+", Ваш запрос обрабатывается ";
                currentLocation.Text="Для отмены звоните диспетчеру";
                offline.IsVisible=false;
                readyForPickup.IsVisible=false;
                pending.IsVisible=true;
            }
            if (intransit==true)
            {
                bid=false;
                refuse=false;
                message.Text="";
                //username.Text="Welcome back, "+name;
                currentLocation.Text="После того как выполните доставку нажмите 'Груз доставлен' ";
                currentLocationName.Text="Запрос подтвержден!!"+Environment.NewLine+" Ожидайте звонка диспетчера в ближайшее время";
                offline.IsVisible=false;
                readyForPickup.IsVisible=false;
                delivered.IsVisible=true;
                pending.IsVisible=false;
            }

        }

        private void Delivered_Clicked(object sender, EventArgs e)
        {
            readyForPickup.IsVisible=true;
             delivered.IsVisible=false;
            offline.IsVisible=false;
            currentLocationName.Text="Доставка оформленна!";
            currentLocation.Text="Для начала работы нажмите 'Готов к загрузке'";
            message.Text="";
            try
            {

                HttpClient client = new HttpClient();
                var makebid = new bid { userName=_name, userKey=_pass, currentbid="Delivered", loadID=_loadid };
                string url = "http://192.168.0.12:45455/api/loads1/";
                var json = JsonConvert.SerializeObject(makebid);
                var resp = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PutAsync(url, resp).Result;


                if (response.StatusCode==HttpStatusCode.OK)
                {

                    //MainPage.bid=true;
                    //App.Current.MainPage=new NavigationPage(new MainPage(_username, _pass));

                }

                else
                {
                    //MainPage.refuse=true;
                    //App.Current.MainPage=new NavigationPage(new MainPage(_username, _pass));

                }




            }
            catch (Exception)
            {
                //MainPage.refuse=true;
                //App.Current.MainPage=new NavigationPage(new MainPage(_username, _pass));

            }

        }

        private void Offline_Clicked(object sender, EventArgs e)
        {
            // await DisplayAlert("Вы Offline", "Спасибо за огромный труд! Поскорее возвращайтесь !!", "Подтвердить");
            readyForPickup.IsVisible=true;
            offline.IsVisible=false;
            currentLocationName.Text="Вы Offline";
            currentLocation.Text="Для начала работы нажмите 'Готов к загрузке'";
            message.Text="";
            HttpClient client = new HttpClient();
            var user = new userLogin { name=_name, pass=_pass, location="USER OFFLINE" };
            string url = "http://192.168.0.12:45455/api/users1/";
            var json = JsonConvert.SerializeObject(user);
            var resp = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, resp).Result;
        }

        private async void ReadyForPickup_Clicked(object sender, EventArgs e)
        {

            
            //await DisplayAlert("Вы Online", "Следите за уведомлениями с грузами вокруг вас, удачной работы !!", "Подтвердить");
            offline.IsVisible=true;
            readyForPickup.IsVisible=false;
            currentLocationName.Text="";
            currentLocation.Text="";
            message.Text="Все готово!! Система ищет грузы для вас в радиусе 150 миль. Ждите автоматические уведомления на ваш телефон.";

            var locator =  CrossGeolocator.Current;
            
            locator.DesiredAccuracy=100;
            try {

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20000));
                //LogitudeLabel.Text=position.Longitude.ToString();
                //LatitudeLabel.Text=position.Latitude.ToString();

                Geocoder geocoder = new Geocoder();
                var pos = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);

                var possibleAddresses = await geocoder.GetAddressesForPositionAsync(pos);

                currentLocation.Text=possibleAddresses.FirstOrDefault();
                currentLocationName.Text="Ваше местоположение:";

                HttpClient client = new HttpClient();
                var user = new userLogin { name=_name, pass=_pass,location =possibleAddresses.FirstOrDefault() };
                string url = "http://192.168.0.12:45455/api/users1/";
                var json = JsonConvert.SerializeObject(user);
                var resp = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, resp).Result;


            }
            catch (Exception) {

                currentLocationName.Text="Нет сигнала GPS";
            }
           
           



        }

     

    }
}
