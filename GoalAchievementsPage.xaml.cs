using MobileAppMAUI.Models;
using System.Collections.ObjectModel;

namespace MobileAppMAUI
{
    public partial class GoalAchievementsPage : ContentPage
    {
        private Goal _goal;

        public GoalAchievementsPage(Goal goal)
        {
            InitializeComponent();
            _goal = goal;
            BindingContext = _goal;

            // Ensure the list is initialized if it's null
            if (_goal.Achievements == null)
                _goal.Achievements = new ObservableCollection<Achievement>();

            // Set the correct image based on the goal type
            SetTypeImage();

            UpdateProgress();
        }

        private void SetTypeImage()
        {
            switch (_goal.Type)
            {
                case GoalType.Finance:
                    TypeImage.Source = "fingoal.png";
                    break;
                case GoalType.Learning:
                    TypeImage.Source = "edugoal.png";
                    break;
                case GoalType.Personal:
                    TypeImage.Source = "personalgoal.png";
                    break;
                case GoalType.Sport:
                    TypeImage.Source = "healthgoal.png";
                    break;
            }
        }

        private void UpdateProgress()
        {
            // Calculate total points from all achievements
            double currentTotal = _goal.Achievements.Sum(a => a.Points);

            // Update the label (e.g., "50 / 100 km")
            ProgressLabel.Text = $"{currentTotal} / {_goal.Quantity} {_goal.Measure}";

            // Update the progress bar (value between 0 and 1)
            if (_goal.Quantity > 0)
            {
                double progress = Math.Clamp(currentTotal / _goal.Quantity, 0, 1);
                GoalProgressBar.Progress = progress;

                // Check if goal is achieved
                if (progress >= 1.0)
                {
                    RewardBox.IsVisible = true;
                }
                else
                {
                    RewardBox.IsVisible = false;
                }
            }
        }

        private async void OnAddAchievementClicked(object sender, EventArgs e)
        {
            // Ask user for the amount
            string result = await DisplayPromptAsync("Добави прогрес", $"Колко {_goal.Measure} направихте днес?", keyboard: Keyboard.Numeric);

            if (double.TryParse(result, out double points))
            {
                // Create the new achievement
                var newAchievement = new Achievement
                {
                    Goal = _goal,
                    Date = DateTime.Now,
                    Points = points
                };

                // Add to the list
                _goal.Achievements.Add(newAchievement);

                // Refresh the UI calculations
                UpdateProgress();
            }
        }
    }
}