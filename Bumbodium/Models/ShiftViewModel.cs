using Bumbodium.Data.DBModels;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.WebApp.Models
{
    public class ShiftViewModel
    {
        [Range(typeof(DateTime), "01/01/0000", "01/01/5000", ErrorMessage = "cannot be added in the past")]
        public DateTime ShiftStartDateTime { get; set; }
    }
}
