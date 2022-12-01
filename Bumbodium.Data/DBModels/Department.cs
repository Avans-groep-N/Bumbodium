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
        Vegetables_Fruit,
        Meat,
        Fish,
        Cheese_Milk,
        Bread,
        Cosmetics,
        Checkout,
        Stockroom,
        InformationDesk
    }
}
