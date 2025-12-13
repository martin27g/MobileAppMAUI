using MobileAppMAUI.Helpers;
using MobileAppMAUI.Models;
using MobileAppMAUI.Services;
using System.Collections.ObjectModel;

namespace MobileAppMAUI;

public partial class ToDo : ContentPage
{
    private readonly DataService _dataService;
    public ObservableCollection<Goal> FilteredGoals { get; set; } = new();
    private Goal _goal;

    public ToDo(DataService dataService)
    {
        InitializeComponent();
        _dataService = dataService;
        FilteredGoals = new ObservableCollection<Goal>(_dataService.Goals.Where(g =>
                                                      (!g.Achievements.Any()) ||
                                                      (g.Frequency == GoalFrequency.Daily && !g.Achievements.Any(a => a.Date == DateTime.Today)) ||
                                                      (g.Frequency == GoalFrequency.Weekly && !g.Achievements.Any(a => DateHelper.IsDateInCurrentWeek(a.Date))) ||
                                                      (g.Frequency == GoalFrequency.Monthly && !g.Achievements.Any(a => DateHelper.IsDateInCurrentMonth(a.Date)))
                                                      ));
        BindingContext = this;

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