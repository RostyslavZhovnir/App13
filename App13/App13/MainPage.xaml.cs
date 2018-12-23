using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
           //await DisplayAlert("Запрос принят", "Ожидайте ответ от диспетчера", "Подтвердить");
           await DisplayAlert("Вы Online", "Следите за уведомлениями с грузами вокруг вас, удачной работы !!", "Подтвердить");
        }
    }
}
