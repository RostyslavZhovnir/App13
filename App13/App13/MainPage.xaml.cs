using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;

namespace App13
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            readyForPickup.Clicked+=ReadyForPickup_Clicked;
            offline.Clicked+=Offline_Clicked;
        }

        private async void Offline_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Вы Offline", "Спасибо за огромный труд! Поскорее возвращайтесь !!", "Подтвердить");
        }

        private async void ReadyForPickup_Clicked(object sender, EventArgs e)
        {

            await DisplayAlert("Вы Online", "Следите за уведомлениями с грузами вокруг вас, удачной работы !!", "Подтвердить");

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy=100;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20000));
            //LogitudeLabel.Text=position.Longitude.ToString();
            //LatitudeLabel.Text=position.Latitude.ToString();
                       
            Geocoder geocoder = new Geocoder();
            var pos = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);

            var possibleAddresses = await geocoder.GetAddressesForPositionAsync(pos);

            currentLocation.Text=possibleAddresses.FirstOrDefault();
            currentLocationName.Text="Ваше местоположение:";




        }




    }
}
