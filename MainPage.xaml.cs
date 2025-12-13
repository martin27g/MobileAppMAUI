using MobileAppMAUI.Models;
using MobileAppMAUI.Services;

namespace MobileAppMAUI;

public partial class MainPage : ContentPage
{
    private DataService _dataService;

    // 1. Inject DataService so we can read the data
    public MainPage(DataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
    }

    // 2. Recalculate every time we return to this page
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateDashboard();
    }

    private void UpdateDashboard()
    {
        var allGoals = _dataService.Goals;

        // --- Logic for Points ---
        // Sum points ONLY from achievements marked as 'IsReward' (the 10pt bonuses)
        // This ensures "Progress" (km/steps) doesn't get mixed into the "Score".
        double totalPoints = allGoals.SelectMany(g => g.Achievements)
                                     .Where(a => a.IsReward)
                                     .Sum(a => a.Points);

        // --- Logic for Goals Ratio ---
        int achievedCount = allGoals.Count(g => g.IsAchieved);
        int totalCount = allGoals.Count;

        // --- Update UI ---
        AchievedPointsLabel.Text = $"{totalPoints} Точки";

        // Shows "2 / 5 Изпълнени цели"
        TotalGoalsLabel.Text = $"{achievedCount} / {totalCount} Изпълнени цели";
    }

    // Navigation Handlers
    private async void OnClickAchivements(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AchivementFilter(_dataService));
    }

    private async void OnClickTodo(object sender, EventArgs e)
    {
        // Assuming your ToDo page also needs the service
        await Navigation.PushAsync(new ToDo(_dataService));
    }

    private async void OnClickMyGoals(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyGoals(_dataService));
    }
}