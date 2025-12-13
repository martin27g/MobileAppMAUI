using MobileAppMAUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileAppMAUI.Services
{
    public class DataService
    {
        public ObservableCollection<Goal> Goals { get; private set; }

        public DataService()
        {
            // Seed with example data
            Goals = new ObservableCollection<Goal>();
            Goals.Add(new Goal
            {
                Description = "Каш Пара",
                Type = GoalType.Finance,
                Measure = "Euro",
                Quantity = 10000,
                Frequency = GoalFrequency.Daily
            });
            Goals.Add(new Goal
            {
                Description = "Become a person",
                Type = GoalType.Personal,
                Measure = "immeasurable",
                Quantity = 5,
                Frequency = GoalFrequency.Weekly
            });
            Goals.Add(new Goal
            {
                Description = "Fool da humand",
                Type = GoalType.Personal,
                Measure = "deception",
                Quantity = 5,
                Frequency = GoalFrequency.Monthly
            });
            Goals.Add(new Goal
            {
                Description = "Walk da dog",
                Type = GoalType.Sport,
                Measure = "Steps",
                Quantity = 5000,
                Frequency = GoalFrequency.Daily
            });
            Goals.Add(new Goal
            {
                Description = "Study",
                Type = GoalType.Learning,
                Measure = "Procrastination",
                Quantity = 5,
                DueDate = new DateTime(2025, 10, 1)
            });
            Goals[0].Achievements.Add(new Achievement
            {
                Date = DateTime.Now,
                Points = 1,
                Goal = Goals[0]
            });
        }

        public Goal GetGoalById(Guid? id)
        {
            Goal g;
            g = Goals.FirstOrDefault(i => i.Id == id);
            return g;
        }

    }
}

