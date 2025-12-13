using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MobileAppMAUI.Models
{
    public enum GoalFrequency
    {
        None,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public enum GoalType
    {
        Finance,
        Sport,
        Learning,
        Personal
    }

    public class Goal : INotifyPropertyChanged
    {
        private Guid _id = Guid.NewGuid();
        private GoalType _type;
        private string _description;
        private string _measure;
        private double _quantity;
        private GoalFrequency _frequency = GoalFrequency.None;
        private DateTime? _dueDate;
        private ObservableCollection<Achievement> _achievements = new();
        public ObservableCollection<Achievement> Achievements
        {
            get => _achievements;
            set => SetProperty(ref _achievements, value);
        }

        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public GoalType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string Measure
        {
            get => _measure;
            set => SetProperty(ref _measure, value);
        }

        public double Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        public GoalFrequency Frequency
        {
            get => _frequency;
            set => SetProperty(ref _frequency, value);
        }

        public DateTime? DueDate
        {
            get => _dueDate;
            set => SetProperty(ref _dueDate, value);
        }

    

        // 🔔 INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return false;

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}