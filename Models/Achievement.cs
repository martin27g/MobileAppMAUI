using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileAppMAUI.Models
{
    public class Achievement
    {
        public int Id { get; set; }                  // Unique identifier
        public Guid GoalId { get; set; }              // Foreign key
        public DateTime Date { get; set; }           // When the achievement was reached
        public double Points { get; set; }           // Points

        // Navigation property back to Goal
        public Goal Goal { get; set; }
    }

}

