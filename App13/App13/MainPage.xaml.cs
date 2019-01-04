using App13.Models;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections;
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
        private string _adress;
        public static bool bid;
        public static bool refuse;
        public static bool intransit;
        public MainPage(string name, string pass,string loadid)
        {
            InitializeComponent();
            lst.IsVisible=false;
            emptylst.IsVisible=false;
            offline.IsVisible=false;
            pending.IsVisible=false;
            delivered.IsVisible=false;
            orderslist.IsVisible=false;
            username.IsVisible=true;
            readyForPickup.Clicked+=ReadyForPickup_Clicked;
            offline.Clicked+=Offline_Clicked;
            delivered.Clicked+=Delivered_Clicked;
          
            orderslist.Clicked+=Orderslist_ClickedAsync;
           // Login.online=false;
          //  currentLocationName.Text="Вы Offline";
            
            username.Text="Welcome back, "+name;
           // message.Text="Для начала работы нажмите"+Environment.NewLine+" 'Готов к загрузке'";

            _name=name;
            _pass=pass;
            _loadid=loadid;
            if (refuse==true)
            {
                username.IsVisible=false;
                Login.online=true;
                bid=false;
                intransit=false;
                //currentLocationName.Text="К сожалению ,"+Environment.NewLine+" ваше предложение цены отклонено!!";
                //message.Text="";
                // username.Text="Welcome back, "+name;
                currentLocation.Text="";
               // message.Text="Для начала работы нажмите "+Environment.NewLine+"'Готов к загрузке'";
                offline.IsVisible=false;
                readyForPickup.IsVisible=true;
                readyForPickup.Text="Ваше предложение цены отклонено!!"+Environment.NewLine+"Для продолжения нажмите тут";
                pending.IsVisible=false;
            }
            else if (bid==true)
            {
                username.IsVisible=false;
                Login.online=false;
                intransit=false;
                refuse=false;
             
                // username.Text=name+", Ваш запрос обрабатывается ";
              
                offline.IsVisible=false;
                readyForPickup.IsVisible=false;
                pending.IsVisible=true;
                pending.Text="Запрос принят!"+Environment.NewLine+ "Для отмены звоните диспетчеру или ожидайте";
            }
            else if (intransit==true)
            {
                username.IsVisible=false;
                Login.online=false;
                bid=false;
                refuse=false;
               
                //username.Text="Welcome back, "+name;
               // message.Text="Ожидайте звонка диспетчера в ближайшее время"+Environment.NewLine+"После того как выполните доставку нажмите :"+Environment.NewLine+" 'Доставлено' ";
               // currentLocationName.Text="!";
                offline.IsVisible=false;
                readyForPickup.IsVisible=false;
                delivered.IsVisible=true;
                delivered.Text="Запрос подтвержден!"+Environment.NewLine+"После выполнения доставки нажмите сюда";
                pending.IsVisible=false;
            }

            else
            {

                onlineAsync();


            }

        }
       

        private async void Orderslist_ClickedAsync(object sender, EventArgs e)
        {
           
           


                try
                {


                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync("http://192.168.0.12:45455/api/loads1?location="+_adress);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode==HttpStatusCode.OK)
                    {
                        lst.IsVisible=true;
                        //var result = JsonConvert.DeserializeObject<object>(responseBody);
                        var result = JsonConvert.DeserializeObject<IEnumerable<loads>>(responseBody);
                        lst.ItemsSource=result;


                    }
                if (response.StatusCode==HttpStatusCode.NoContent)
                {
                    lst.IsVisible=false;
                    //var result = JsonConvert.DeserializeObject<object>(responseBody);

                    emptylst.IsVisible=true;
                    emptylst.Text="Пока список пуст..";

                       


                }








            }
                catch (Exception)
                {


                }




            
        }

        private void lst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selecteditem = e.SelectedItem as loads;
            string message = selecteditem.finalprice+"%%"+selecteditem.pickupfrom+"%%"+selecteditem.deliveryto+"%%"+selecteditem.weightpcs+"%%"+selecteditem.totalmiles+"%%"+selecteditem.pickupdate+"%%"+selecteditem.deliverydate;
            var refreshedToken = Login.seckey;
            App.Current.MainPage=new NavigationPage(new Notification(message, _name, _pass, refreshedToken, selecteditem.id.ToString()));
        }

        private void Delivered_Clicked(object sender, EventArgs e)
        {
            username.IsVisible=false;
            Login.online=true;
            readyForPickup.IsVisible=true;
             delivered.IsVisible=false;
            offline.IsVisible=false;
            orderslist.IsVisible=false;
            readyForPickup.Text="Доставка оформленна!"+Environment.NewLine+"Для начала работы нажмите сюда";
           
            try
            {

                HttpClient client = new HttpClient();
                var makebid = new bid { userName=_name, userKey=_pass, currentbid="Delivered", loadID=_loadid };
                string url = "http://192.168.0.12:45455/api/loads1/";
                var json = JsonConvert.SerializeObject(makebid);
                var resp = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PutAsync(url, resp).Result;


                //if (response.StatusCode==HttpStatusCode.OK)
                //{

                   

                //}

                //else
                //{
                   
                //}




            }
            catch (Exception)
            {
               

            }

        }

        private void Offline_Clicked(object sender, EventArgs e)
        {
            message.Text="";
            username.IsVisible=false;
            Login.online=false;
            readyForPickup.IsVisible=true;
            offline.IsVisible=false;
            orderslist.IsVisible=false;
            currentLocationName.Text="";
            currentLocation.Text="";

            readyForPickup.Text="Вы Offline"+Environment.NewLine+"Для начала работы нажмите сюда ";
            
         
           
            HttpClient client = new HttpClient();
            var user = new userLogin { name=_name, pass=_pass, location="USER OFFLINE" };
            string url = "http://192.168.0.12:45455/api/users1/";
            var json = JsonConvert.SerializeObject(user);
            var resp = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(url, resp).Result;
        }

        private async void ReadyForPickup_Clicked(object sender, EventArgs e)
        {

            username.IsVisible=true;
            Login.online=true;
            offline.IsVisible=true;
            orderslist.IsVisible=true;
            readyForPickup.IsVisible=false;
            currentLocationName.Text="";
            currentLocation.Text="";
            message.Text="Все готово!! Система ищет грузы для вас в радиусе 100 миль. Ждите автоматические уведомления на ваш телефон.";

            var locator =  CrossGeolocator.Current;
            
            locator.DesiredAccuracy=100;
            try {

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20000));
              

                Geocoder geocoder = new Geocoder();
                var pos = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);

                var possibleAddresses = await geocoder.GetAddressesForPositionAsync(pos);

                currentLocation.Text=possibleAddresses.FirstOrDefault();
                _adress=possibleAddresses.FirstOrDefault();
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

        private async void onlineAsync() {
            Login.online=true;
            offline.IsVisible=true;
            orderslist.IsVisible=true;
            readyForPickup.IsVisible=false;
            currentLocationName.Text="";
            currentLocation.Text="";
            message.Text="Система ищет грузы для вас в радиусе 100 миль. Ждите автоматические уведомления или нажмите 'Oбновить список'";

            var locator =  CrossGeolocator.Current;
            
            locator.DesiredAccuracy=100;
            try {

                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20000));
              

                Geocoder geocoder = new Geocoder();
                var pos = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);

                var possibleAddresses = await geocoder.GetAddressesForPositionAsync(pos);

                currentLocation.Text=possibleAddresses.FirstOrDefault();
                _adress=possibleAddresses.FirstOrDefault();
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
