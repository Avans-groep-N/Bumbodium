using Bumbodium.Data.DBModels;

namespace Bumbodium.WebApp.Models.ManagerSchedule
{
    public class DepartmentSelectionViewModel
    {
        public DepartmentType Type { get; set; }
        public int NeededHours { get; set; }
        public double PlannedHours { get; set; }
    }
}
