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

            if (_goal.Achievements == null)
                _goal.Achievements = new ObservableCollection<Achievement>();

            SetTypeImage();
            UpdateProgress();
        }

        private void SetTypeImage()
        {
            switch (_goal.Type)
            {
                case GoalType.Finance: TypeImage.Source = "fingoal.png"; break;
                case GoalType.Learning: TypeImage.Source = "edugoal.png"; break;
                case GoalType.Personal: TypeImage.Source = "personalgoal.png"; break;
                case GoalType.Sport: TypeImage.Source = "healthgoal.png"; break;
            }
        }

        private void UpdateProgress()
        {
            double currentTotal = _goal.Achievements
                                       .Where(a => !a.IsReward)
                                       .Sum(a => a.Points);

            ProgressLabel.Text = $"{currentTotal} / {_goal.Quantity} {_goal.Measure}";

            if (_goal.Quantity > 0)
            {
                double progress = Math.Clamp(currentTotal / _goal.Quantity, 0, 1);
                GoalProgressBar.Progress = progress;


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
            string result = await DisplayPromptAsync("Добави прогрес", $"Колко {_goal.Measure} направи днес?", keyboard: Keyboard.Numeric);

            if (double.TryParse(result, out double points))
            {
                var newAchievement = new Achievement
                {
                    Goal = _goal,
                    Date = DateTime.Now,
                    Points = points,
                    IsReward = false //Да не се брои към точките
                };

                _goal.Achievements.Add(newAchievement);
                UpdateProgress();
            }
        }
    }
}