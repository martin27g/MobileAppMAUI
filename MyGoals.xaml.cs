using Microsoft.Maui.Platform;
using MobileAppMAUI.Models;
using MobileAppMAUI.Services;
using System.Collections.ObjectModel;

namespace MobileAppMAUI;

public partial class MyGoals : ContentPage
{
    private readonly DataService _dataService;
    public ObservableCollection<Goal> FilteredGoals { get; set; } = new();

    private List<ImageButton> filterButtons;
    private ImageButton lastFilter; 

    public MyGoals(DataService dataService)
    {
        InitializeComponent();

        _dataService = dataService;

        FilteredGoals = new ObservableCollection<Goal>(_dataService.Goals.Where(x => true));
        BindingContext = this;

        filterButtons = new List<ImageButton> { FinGoalButton, EduGoalButton, HealthGoalButton, PersonalGoalButton };
    }

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
    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Goal selected)
        {
            await Navigation.PushAsync(new GoalDetails(_dataService, selected.Id));
        }
        ((CollectionView)sender).SelectedItem = null; // Clear selection
    }

    private async void OnFilterButtonClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        if (button == null) return;

        // --- CONFIGURATION ---
        double activeScale = 1.2; // The button will be 20% larger when selected
        double normalScale = 1.0;
        // ---------------------

        ObservableCollection<Goal> result = new();
        var action = button.CommandParameter?.ToString();

        if (button != lastFilter)
        {
            // === CASE 1: SWITCHING TO NEW FILTER ===

            // 1. Reset the OLD button (if exists)
            if (lastFilter != null)
            {
                SetUnderscore(lastFilter, false);
                // animate old button back to normal size (Fire-and-forget, don't await)
                _ = lastFilter.ScaleTo(normalScale, 100, Easing.CubicIn);
            }

            // 2. Activate the NEW button
            SetUnderscore(button, true);

            // Bounce Effect: Shrink slightly -> Grow to 'activeScale'
            await button.ScaleTo(0.9, 50, Easing.CubicOut);
            await button.ScaleTo(activeScale, 150, Easing.SpringOut); // SpringOut gives a nice "pop" effect

            // 3. Filter Data
            if (Enum.TryParse<GoalType>(action, true, out var parsedAction))
            {
                result = new ObservableCollection<Goal>(_dataService.Goals.Where(t => t.Type == parsedAction));
            }

            lastFilter = button;
        }
        else
        {
            // === CASE 2: TOGGLING OFF (CLICKING SAME BUTTON) ===

            // 1. Deactivate button
            SetUnderscore(button, false);

            // Bounce Effect: Shrink slightly -> Return to 'normalScale'
            await button.ScaleTo(0.9, 50, Easing.CubicOut);
            await button.ScaleTo(normalScale, 100, Easing.SpringOut);

            // 2. Reset Data
            result = new ObservableCollection<Goal>(_dataService.Goals.Where(t => true));
            lastFilter = null;
        }

        // Refresh List
        FilteredGoals.Clear();
        foreach (var item in result)
        {
            FilteredGoals.Add(item);
        }
    }

    private void SetUnderscore(ImageButton button, bool isVisible)
    {
        if (button == null) return;
        var parent = button.Parent as Layout;
        var underscore = parent?.Children.OfType<BoxView>().FirstOrDefault();
        if (underscore != null) underscore.IsVisible = isVisible;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // 1. Clear the current list so we don't show "ghost" items
        FilteredGoals.Clear();

        // 2. Check if we have an active filter button
        IEnumerable<Goal> itemsToShow;

        if (lastFilter != null)
        {
            // A filter is active, so we only show matching items
            var action = lastFilter.CommandParameter?.ToString();

            if (Enum.TryParse<GoalType>(action, true, out var parsedAction))
            {
                itemsToShow = _dataService.Goals.Where(t => t.Type == parsedAction);
            }
            else
            {
                itemsToShow = _dataService.Goals;
            }
        }
        else
        {
            // No filter is active, show everything
            itemsToShow = _dataService.Goals;
        }

        // 3. Add the valid items back to the screen
        foreach (var item in itemsToShow)
        {
            FilteredGoals.Add(item);
        }
    }
    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewGoalSelect(_dataService));
       // await Navigation.PushAsync(new GoalDetails(_dataService, null));
    }
}