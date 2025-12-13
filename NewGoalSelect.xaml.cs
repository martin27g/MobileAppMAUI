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
        // Shrink to 0.9 scale over 100ms
        await btn.ScaleTo(0.9, 100);
    }

    private async void OnButtonReleased(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        // Return to normal size over 100ms
        await btn.ScaleTo(1.0, 100);
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