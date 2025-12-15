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
        // смалява
        await btn.ScaleTo(0.8, 200);
    }

    private async void OnButtonReleased(object sender, EventArgs e)
    {
        var btn = (ImageButton)sender;
        //връща по подразбиране
        await btn.ScaleTo(1.0, 200);
    }
    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Goal selected)
        {
            await Navigation.PushAsync(new GoalDetails(_dataService, selected.Id));
        }
        ((CollectionView)sender).SelectedItem = null;
    }

    private async void OnFilterButtonClicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        if (button == null) return;

        double activeScale = 1.2; // Уголемява избрания бутон
        double normalScale = 1.0;

        ObservableCollection<Goal> result = new();
        var action = button.CommandParameter?.ToString();

        if (button != lastFilter)
        {
            // връща оригиналното състояние на бутона
            if (lastFilter != null)
            {
                SetUnderscore(lastFilter, false);

                _ = lastFilter.ScaleTo(normalScale, 100, Easing.CubicIn);
            }


            SetUnderscore(button, true); //Подчертава бутона
            await button.ScaleTo(activeScale, 150, Easing.SpringOut); // уголемява избрания бутон

            // Филтрирането
            if (Enum.TryParse<GoalType>(action, true, out var parsedAction))
            {
                result = new ObservableCollection<Goal>(_dataService.Goals.Where(t => t.Type == parsedAction));
            }

            lastFilter = button;
        }
        else
        {
            //При натискане на същия бутон

            // Маха подчертаването
            SetUnderscore(button, false);
            await button.ScaleTo(normalScale, 150, Easing.SpringOut);

            // 2. Reset Data
            result = new ObservableCollection<Goal>(_dataService.Goals.Where(t => true));
            lastFilter = null;
        }

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

        FilteredGoals.Clear();

        IEnumerable<Goal> itemsToShow;

        if (lastFilter != null)
        {
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
            // показва всичко като няма филтър
            itemsToShow = _dataService.Goals;
        }

        // добавя items
        foreach (var item in itemsToShow)
        {
            FilteredGoals.Add(item);
        }
    }
    private async void OnAddButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewGoalSelect(_dataService));
    }
}