using MobileAppMAUI.Models;
using MobileAppMAUI.Services;

namespace MobileAppMAUI;

public partial class MainPage : ContentPage
{
    private DataService _dataService;

    public MainPage(DataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateDashboard();
    }

    private void UpdateDashboard()
    {
        var allGoals = _dataService.Goals;

        // Сметка за точките
        // Сумира точките, където achivement-а е маркиран като IsReward.
 
        double totalPoints = allGoals.SelectMany(g => g.Achievements)
                                     .Where(a => a.IsReward)
                                     .Sum(a => a.Points);

        // Постигнати / Всични цели
        int achievedCount = allGoals.Count(g => g.IsAchieved);
        int totalCount = allGoals.Count;

        AchievedPointsLabel.Text = $"{totalPoints} Точки";

        TotalGoalsLabel.Text = $"{achievedCount}/{totalCount} Изпълнени цели";
    }

    private async void OnClickAchivements(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AchivementFilter(_dataService));
    }

    private async void OnClickTodo(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ToDo(_dataService));
    }
    private async void OnClickMyGoals(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyGoals(_dataService));
    }
}