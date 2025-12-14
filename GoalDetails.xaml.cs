using MobileAppMAUI.Models;
using MobileAppMAUI.Services;
using System.Net.Http.Json;

namespace MobileAppMAUI;

public partial class GoalDetails : ContentPage
{
    private DataService _dataService;
    private Goal _goal;
    private bool _editing;
    private HttpClient _httpClient = new HttpClient();

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
        _goal = new Goal { Id = Guid.NewGuid(), Type = _type };
        _editing = false;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing(); // Good practice to call base

        // 1. Setup UI based on Goal Type
        switch (_goal.Type)
        {
            case GoalType.Finance: TypeImage.Source = "fingoal.png"; break;
            case GoalType.Learning: TypeImage.Source = "edugoal.png"; break;
            case GoalType.Personal: TypeImage.Source = "personalgoal.png"; break;
            case GoalType.Sport: TypeImage.Source = "healthgoal.png"; break;
        }

        // 2. Load the specific API data
        await LoadApiData();

        EditButtons.IsVisible = _editing;
        NewButtons.IsVisible = !_editing;
        CheckmarkBtn.IsVisible = _editing;
    }

    private async Task LoadApiData()
    {

        try
        {
            switch (_goal.Type)
            {
                // CASE 1: SPORT -> Weather Forecast
                case GoalType.Sport:
                    var weatherData = await _httpClient.GetFromJsonAsync<Weather>("https://api.open-meteo.com/v1/forecast?latitude=42.6667&longitude=25.25&current=wind_speed_10m,temperature_2m,rain,cloud_cover&timezone=auto");
                    if (weatherData != null)
                    {
                        API.Text = $"Времето навън: {weatherData.current.temperature_2m} °C, Вятър: {weatherData.current.wind_speed_10m} km/h";
                    }
                    break;

                // CASE 2: FINANCE -> Exchange Rate (USD to BGN)
                case GoalType.Finance:
                    var financeData = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>("https://api.frankfurter.app/latest?from=USD&to=BGN");
                    if (financeData != null && financeData.rates.ContainsKey("BGN"))
                    {
                        API.Text = $"Курс на долара: 1 USD = {financeData.rates["BGN"]:F2} BGN";
                    }
                    break;

                // CASE 3: PERSONAL -> Motivational Quote
                case GoalType.Personal:
                    var quoteData = await _httpClient.GetFromJsonAsync<QuoteResponse>("https://dummyjson.com/quotes/random");
                    if (quoteData != null)
                    {
                        API.Text = $"Myсъл за деня:\n\"{quoteData.quote}\"\n- {quoteData.author}";
                    }
                    break;

                // CASE 4: LEARNING (Optional default)
                case GoalType.Learning:
                    API.Text = "Ученето е най-добрата инвестиция!";
                    break;
            }
        }
        catch (Exception ex)
        {
            API.Text = "Няма връзка със сървъра.";
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }

    // --- Button Handlers ---

    private async void OnCompleteGoalClicked(object sender, EventArgs e)
    {
        _goal.IsAchieved = true;

        var achievement = new Achievement
        {
            Goal = _goal,
            GoalId = _goal.Id,
            Date = DateTime.Now,
            Points = 10,
            IsReward = true
        };

        if (_goal.Achievements == null)
            _goal.Achievements = new System.Collections.ObjectModel.ObservableCollection<Achievement>();

        _goal.Achievements.Add(achievement);

        await DisplayAlert("Браво!", "Добавихте 10 точки!", "ОК");
    }

    private async void OnAchievementsClicked(object sender, EventArgs e)
    {
        if (_goal == null || _goal.Id == Guid.Empty)
        {
            await DisplayAlert("Error", "Please save the goal before adding achievements.", "OK");
            return;
        }
        await Navigation.PushAsync(new GoalAchievementsPage(_goal));
    }

    public void OnPickerSelectedIndexChanged(object sender, EventArgs e)
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
            string.IsNullOrEmpty(DescriptionField.Text) ||
            string.IsNullOrEmpty(MeasureField.Text) ||
            string.IsNullOrEmpty(QuantityField.Text))
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
                IncompleteError.IsVisible = true;
                return;
            }
            _goal.Quantity = quantity;

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
        bool answer = await DisplayAlert("Изтриване", "Сигурни ли сте, че искате да изтриете тази цел?", "Да", "Не");
        if (answer)
        {
            if (_dataService.Goals.Contains(_goal))
            {
                _dataService.Goals.Remove(_goal);
            }
            await Navigation.PopAsync();
        }
    }
}

// --- Helper Models for APIs ---

public class ExchangeRateResponse
{
    public double amount { get; set; }
    public string @base { get; set; }
    public string date { get; set; }
    public Dictionary<string, double> rates { get; set; }
}

public class QuoteResponse
{
    public int id { get; set; }
    public string quote { get; set; }
    public string author { get; set; }
}