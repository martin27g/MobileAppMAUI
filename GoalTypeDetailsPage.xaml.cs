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
            switch (type)
            {
                case GoalType.Finance: PageTitle.Text = "Финанси"; break;
                case GoalType.Sport: PageTitle.Text = "Спорт"; break;
                case GoalType.Learning: PageTitle.Text = "Учене"; break;
                case GoalType.Personal: PageTitle.Text = "Лични"; break;
            }

            // Филтрира типа и дали е isAchieved
            var relevantGoals = dataService.Goals
                .Where(g => g.Type == type && g.IsAchieved)
                .OrderBy(g => g.Description) // сортиране по азбучен
                .ToList();

            foreach (var goal in relevantGoals)
            {
                string infoText = "";

                if (type == GoalType.Personal || type == GoalType.Learning)// Presonal или Learning
                {
                    var lastAchievement = goal.Achievements
                        .Where(a => a.IsReward)
                        .OrderByDescending(a => a.Date)
                        .FirstOrDefault();

                    infoText = lastAchievement != null
                        ? lastAchievement.Date.ToString("dd.MM.yyyy")
                        : "N/A";
                }
                else // Finance или Sport
                {
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

    public class GoalDisplayItem
    {
        public string Description { get; set; }
        public string DetailInfo { get; set; }
    }
}