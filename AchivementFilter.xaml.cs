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
        CalculateStats();
    }

    private void CalculateStats()
    {
        var allGoals = _dataService.Goals;

        // 1. Total Goals Achieved (Count only unique goals that are finished)
        int totalAchieved = allGoals.Count(g => g.IsAchieved);
        TotalAchievedLabel.Text = totalAchieved.ToString();

        // 2. Calculate Stats per Category
        // Points: Sum only "IsReward" points
        // Count: Count all goals of that type (achieved or not)

        // Finance
        double finScore = allGoals.Where(g => g.Type == GoalType.Finance)
                                  .SelectMany(g => g.Achievements)
                                  .Where(a => a.IsReward)
                                  .Sum(a => a.Points);
        int finGoals = allGoals.Count(g => g.Type == GoalType.Finance);

        // Sport
        double sportScore = allGoals.Where(g => g.Type == GoalType.Sport)
                                    .SelectMany(g => g.Achievements)
                                    .Where(a => a.IsReward)
                                    .Sum(a => a.Points);
        int sportGoals = allGoals.Count(g => g.Type == GoalType.Sport);

        // Learning
        double eduScore = allGoals.Where(g => g.Type == GoalType.Learning)
                                  .SelectMany(g => g.Achievements)
                                  .Where(a => a.IsReward)
                                  .Sum(a => a.Points);
        int eduGoals = allGoals.Count(g => g.Type == GoalType.Learning);

        // Personal
        double personalScore = allGoals.Where(g => g.Type == GoalType.Personal)
                                       .SelectMany(g => g.Achievements)
                                       .Where(a => a.IsReward)
                                       .Sum(a => a.Points);
        int personalGoals = allGoals.Count(g => g.Type == GoalType.Personal);

        // 3. Update UI
        finPoints.Text = $"{finScore} т.";
        finCount.Text = $"{finGoals} цели";

        helathPoints.Text = $"{sportScore} т.";
        healthCount.Text = $"{sportGoals} цели";

        eduPoints.Text = $"{eduScore} т.";
        eduCount.Text = $"{eduGoals} цели";

        personalPoints.Text = $"{personalScore} т.";
        personalCount.Text = $"{personalGoals} цели";
    }
}