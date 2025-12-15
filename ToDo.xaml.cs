using MobileAppMAUI.Helpers;
using MobileAppMAUI.Models;
using MobileAppMAUI.Services;
using System.Collections.ObjectModel;

namespace MobileAppMAUI;

public partial class ToDo : ContentPage
{
    private readonly DataService _dataService;

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
        var today = DateTime.Today;

        var goalsToShow = _dataService.Goals.Where(g =>
        {
            if (g.Frequency != GoalFrequency.None)
            {
                return g.Frequency switch
                {
                    // Daily: Показва се, ако не е постигната в определения срок, същата идея е и за следващите
                    GoalFrequency.Daily => !g.Achievements.Any(a => a.Date.Date == today),

                    GoalFrequency.Weekly => !g.Achievements.Any(a => DateHelper.IsDateInCurrentWeek(a.Date)),

                    GoalFrequency.Monthly => !g.Achievements.Any(a => DateHelper.IsDateInCurrentMonth(a.Date)),

                    GoalFrequency.Yearly => !g.Achievements.Any(a => DateHelper.IsDateInCurrentYear(a.Date)),

                    _ => false
                };
            }

            //Ако е с дата
            if (g.DueDate.HasValue)
            {
                bool isDueOrOverdue = g.DueDate.Value.Date <= today;
                return isDueOrOverdue && !g.IsAchieved;
            }

            return false;
        }).ToList();

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
       ((CollectionView)sender).SelectedItem = null; 
    }
}