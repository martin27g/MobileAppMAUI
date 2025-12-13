using MobileAppMAUI.Models;
using MobileAppMAUI.Services;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace MobileAppMAUI;

public partial class GoalDetails : ContentPage
{
    private DataService _dataService;
    private Goal _goal;
    private bool _editing;

    private HttpClient _httpClient = new HttpClient();
    private Weather weatherApi;

    private async void OnAchievementsClicked(object sender, EventArgs e)
    {
        // Check if the goal has been saved first (it needs an ID)
        if (_goal == null || _goal.Id == Guid.Empty)
        {
            await DisplayAlert("Error", "Please save the goal before adding achievements.", "OK");
            return;
        }

        // Navigate to the new Achievements page (we will create this next)
        await Navigation.PushAsync(new GoalAchievementsPage(_goal));
    }

    public GoalDetails(DataService dataService, Guid GoalId)
    {
        InitializeComponent();
        _dataService = dataService;

        _goal = _dataService.GetGoalById(GoalId);
        BindingContext = _goal;


        _editing = true;
    }

    public GoalDetails(DataService dataService, GoalType _type)
    {
        InitializeComponent();
        _dataService = dataService;

        _goal = new Goal
        {
            Id = Guid.NewGuid(),
            Type = _type
        };

        _editing = false;
    }

    protected override void OnAppearing()
    {
        LoadApi();

        switch (_goal.Type)
        {
            case GoalType.Finance: TypeImage.Source = "fingoal.png"; break;
            case GoalType.Learning: TypeImage.Source = "edugoal.png"; break;
            case GoalType.Personal: TypeImage.Source = "personalgoal.png"; break;
            case GoalType.Sport: TypeImage.Source = "healthgoal.png"; break;
        }

        EditButtons.IsVisible = _editing;
        NewButtons.IsVisible = !_editing;
    }

    public async void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (TypePicker.SelectedItem != null)
        {
            string selected = TypePicker.SelectedItem.ToString();
            DateLabel.IsVisible = selected == "Дата";
            DatePicker.IsVisible = selected == "Дата";
        }
    }

    public async void OnSaveClicked(object sender, EventArgs e)
    {
        if (TypePicker.SelectedItem == null ||
            DescriptionField.Text.Equals(String.Empty) ||
            MeasureField.Text.Equals(String.Empty) ||
            QuantityField.Text.Equals(String.Empty))
        {
            IncompleteError.IsVisible = true;
        }
        else
        {
            switch (TypePicker.SelectedItem.ToString())
            {
                case "Ежедневно": _goal.Frequency = GoalFrequency.Daily; _goal.DueDate = null; break;
                case "Седмично": _goal.Frequency = GoalFrequency.Weekly; _goal.DueDate = null; break;
                case "Месечно": _goal.Frequency = GoalFrequency.Monthly; _goal.DueDate = null; break;
                case "Годишно": _goal.Frequency = GoalFrequency.Yearly; _goal.DueDate = null; break;
                case "Дата": _goal.Frequency = GoalFrequency.None; _goal.DueDate = DatePicker.Date; break;
            }
            _goal.Description = DescriptionField.Text;
            _goal.Measure = MeasureField.Text;

            if (!double.TryParse(QuantityField.Text, out double quantity))
            {
                // Show an error message to the user  
                IncompleteError.IsVisible = true;
                return;
            }
            _goal.Quantity = quantity;
            _goal.Quantity = double.Parse(QuantityField.Text);

            if (!_editing)
            {
                _dataService.Goals.Add(_goal);
            }

            await Navigation.PushAsync(new MyGoals(_dataService));
        }
    }

    public async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MyGoals(_dataService));
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        // 1. Ask for confirmation
        bool answer = await DisplayAlert("Изтриване", "Сигурни ли сте, че искате да изтриете тази цел?", "Да", "Не");

        if (answer)
        {
            // 2. Remove the goal from the global list
            // Note: This assumes your DataService instance variable is named '_dataService'
            if (_dataService.Goals.Contains(_goal))
            {
                _dataService.Goals.Remove(_goal);
            }

            // 3. Close the page and go back to the list
            await Navigation.PopAsync();
        }
    }

    private async void LoadApi()
    {
        weatherApi = await _httpClient.GetFromJsonAsync<Weather>("https://api.open-meteo.com/v1/forecast?latitude=42.6667&longitude=25.25&current=wind_speed_10m,temperature_2m,rain,cloud_cover&timezone=auto");
        API.Text = $"Температура: {weatherApi.current.temperature_2m.ToString()} °C, вятър: {weatherApi.current.wind_speed_10m.ToString()} Облачност: {weatherApi.current.cloud_cover.ToString()} %";
    }
}