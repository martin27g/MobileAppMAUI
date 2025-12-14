using MobileAppMAUI.Helpers;
using MobileAppMAUI.Models;
using MobileAppMAUI.Services;
using System.Collections.ObjectModel;

namespace MobileAppMAUI;

public partial class ToDo : ContentPage
{
    private readonly DataService _dataService;

    // Binding Collection
    public ObservableCollection<Goal> FilteredGoals { get; set; } = new();

    public ToDo(DataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadGoals();
    }

    private void LoadGoals()
    {
        // 1. Get today's date (ignoring time)
        var today = DateTime.Today;

        // 2. Filter logic based on requirements
        var goalsToShow = _dataService.Goals.Where(g =>
        {
            // Case 1: Periodic Goals
            if (g.Frequency != GoalFrequency.None)
            {
                return g.Frequency switch
                {
                    // Daily: Show if NOT achieved today
                    GoalFrequency.Daily => !g.Achievements.Any(a => a.Date.Date == today),

                    // Weekly: Show if NOT achieved this week
                    GoalFrequency.Weekly => !g.Achievements.Any(a => DateHelper.IsDateInCurrentWeek(a.Date)),

                    // Monthly: Show if NOT achieved this month
                    GoalFrequency.Monthly => !g.Achievements.Any(a => DateHelper.IsDateInCurrentMonth(a.Date)),

                    // Annual: Show if NOT achieved this year
                    GoalFrequency.Yearly => !g.Achievements.Any(a => DateHelper.IsDateInCurrentYear(a.Date)),

                    _ => false
                };
            }

            // Case 2: One-time Goals (Specific Date)
            // Show if: Has DueDate AND DueDate is Today or Passed AND Not Achieved
            if (g.DueDate.HasValue)
            {
                bool isDueOrOverdue = g.DueDate.Value.Date <= today;
                return isDueOrOverdue && !g.IsAchieved;
            }

            return false;
        }).ToList();

        // 3. Update the ObservableCollection
        FilteredGoals.Clear();
        foreach (var goal in goalsToShow)
        {
            FilteredGoals.Add(goal);
        }
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Goal selected)
        {
            await Navigation.PushAsync(new GoalDetails(_dataService, selected.Id));
        }
       ((CollectionView)sender).SelectedItem = null; // Clear selection
    }
}