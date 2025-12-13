using MobileAppMAUI.Models;
using MobileAppMAUI.Services;
using System.Collections.ObjectModel;

namespace MobileAppMAUI
{
    public partial class MainPage : ContentPage
    {
        private readonly DataService _dataService;
        public MainPage()
        {
            InitializeComponent();
            _dataService = new DataService();
        }

        public async void OnClickAchivements(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AchivementFilter());
            //await Navigation.PopAsync();
        }

        public async void OnClickMyGoals(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MyGoals(_dataService));
            //await Navigation.PopAsync();
        }

        public async void OnClickTodo(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ToDo(_dataService));
            //await Navigation.PopAsync();
        }

    }
}