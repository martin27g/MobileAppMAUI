using MobileAppMAUI.Models;
using MobileAppMAUI.Services;
using System.Collections.ObjectModel;

namespace MobileAppMAUI;

public partial class NewGoalSelect : ContentPage
{
    private readonly DataService dataService;

    private async void OnButtonPressed(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;

        await btn.ScaleTo(0.6, 150);
    }
    // за оразмеряване на бутоните ↑ ↓
    private async void OnButtonReleased(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        await btn.ScaleTo(1.0, 150);
    }

    public NewGoalSelect(DataService _dataService)
	{
		InitializeComponent();
        dataService = _dataService;
	}

    private async void CreateFinClicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new GoalDetails(dataService, GoalType.Finance));
    }

    private async void CreateEduClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GoalDetails(dataService, GoalType.Learning));
    }

    private async void CreateHealthClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GoalDetails(dataService, GoalType.Sport));
    }

    private async void CreatePersonalClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new GoalDetails(dataService, GoalType.Personal));
    }
}