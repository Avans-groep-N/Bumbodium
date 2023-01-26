using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bumbodium.Data.DBModels
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DepartmentType Name { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public int SurfaceAreaInM2 { get; set; }

        [Required]
        public virtual Branch Branch { get; set; }
        public virtual int BranchId { get; set; }
        public virtual ICollection<Forecast>? Forecast { get; set; }
        public virtual ICollection<Shift>? Shifts { get; set; }
        public virtual ICollection<DepartmentEmployee>? PartOFEmployee { get; set; }
    }

    public enum DepartmentType
    {
        [Display(Name ="Groente en Fruit")]
        Vegetables_Fruit,
        [Display(Name ="Vlees")]
        Meat,
        [Display(Name ="Vis")]
        Fish,
        [Display(Name = "Zuivel")]
        Cheese_Milk,
        [Display(Name = "Brood")]
        Bread,
        [Display(Name = "Cosmetica")]
        Cosmetics,
        [Display(Name = "Kassa")]
        Checkout,
        [Display(Name = "Stockroom? wat de fuck is da ook al weer")]
        Stockroom,
        [Display(Name = "Informatie balie")]
        InformationDesk
    }
}
