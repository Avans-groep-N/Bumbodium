using Bumbodium.Data.DBModels;
using Bumbodium.WebApp.Models.BusinessLayer;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class ShiftViewModel
    {
        [ScheduleApproval]
        public DateTime ShiftStartDateTime { get; set; }
    }
}
