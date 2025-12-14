using MobileAppMAUI.Models;
using MobileAppMAUI.Services;
using System.Collections.ObjectModel;

namespace MobileAppMAUI
{
    public partial class GoalTypeDetailsPage : ContentPage
    {
        public ObservableCollection<GoalDisplayItem> DisplayItems { get; set; }

        public GoalTypeDetailsPage(DataService dataService, GoalType type)
        {
            InitializeComponent();
            DisplayItems = new ObservableCollection<GoalDisplayItem>();
            GoalsList.ItemsSource = DisplayItems;

            LoadData(dataService, type);
        }

        private void LoadData(DataService dataService, GoalType type)
        {
            // Set Title
            switch (type)
            {
                case GoalType.Finance: PageTitle.Text = "Финанси"; break;
                case GoalType.Sport: PageTitle.Text = "Спорт"; break;
                case GoalType.Learning: PageTitle.Text = "Учене"; break;
                case GoalType.Personal: PageTitle.Text = "Лични"; break;
            }

            // 1. Filter goals: Match Type AND must be Achieved (IsAchieved == true)
            var relevantGoals = dataService.Goals
                .Where(g => g.Type == type && g.IsAchieved)
                .OrderBy(g => g.Description) // 2. Sort Alphabetically
                .ToList();

            foreach (var goal in relevantGoals)
            {
                string infoText = "";

                // 3. Logic based on Type
                if (type == GoalType.Personal || type == GoalType.Learning)
                {
                    // Show DATE (Last time it was marked achieved)
                    var lastAchievement = goal.Achievements
                        .Where(a => a.IsReward)
                        .OrderByDescending(a => a.Date)
                        .FirstOrDefault();

                    infoText = lastAchievement != null
                        ? lastAchievement.Date.ToString("dd.MM.yyyy")
                        : "N/A";
                }
                else // Finance or Sport
                {
                    // Show COUNT (Number of times marked achieved)
                    int count = goal.Achievements.Count(a => a.IsReward);
                    infoText = $"{count} пъти";
                }

                DisplayItems.Add(new GoalDisplayItem
                {
                    Description = goal.Description,
                    DetailInfo = infoText
                });
            }
        }
    }

    // Helper class for the list
    public class GoalDisplayItem
    {
        public string Description { get; set; }
        public string DetailInfo { get; set; }
    }
}