using MobileAppMAUI.Services;
using MobileAppMAUI.Models;

namespace MobileAppMAUI;

public partial class AchivementFilter : ContentPage
{
    private DataService _dataService;

    public AchivementFilter(DataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CalculatePoints();
    }

    private void CalculatePoints()
    {
        // 1. Flatten all achievements from all goals
        var allGoals = _dataService.Goals;

        // 2. Sum points by Type
        double financePoints = allGoals.Where(g => g.Type == GoalType.Finance)
                                       .SelectMany(g => g.Achievements).Sum(a => a.Points);

        double sportPoints = allGoals.Where(g => g.Type == GoalType.Sport)
                                     .SelectMany(g => g.Achievements).Sum(a => a.Points);

        double learningPoints = allGoals.Where(g => g.Type == GoalType.Learning)
                                        .SelectMany(g => g.Achievements).Sum(a => a.Points);

        double personalPoints = allGoals.Where(g => g.Type == GoalType.Personal)
                                        .SelectMany(g => g.Achievements).Sum(a => a.Points);

        // 3. Update UI
        FinancePointsLabel.Text = $"{financePoints} Точки";
        SportPointsLabel.Text = $"{sportPoints} Точки";
        LearningPointsLabel.Text = $"{learningPoints} Точки";
        PersonalPointsLabel.Text = $"{personalPoints} Точки";
    }
}