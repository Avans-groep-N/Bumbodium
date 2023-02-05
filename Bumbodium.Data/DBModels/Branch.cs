using System.ComponentModel.DataAnnotations;

namespace Bumbodium.Data.DBModels
{
    public class Branch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string City { get; set; }
        [Required]
        [StringLength(64)]
        public string Street { get; set; }
        [Required]
        [StringLength(64)]
        public string PostalCode { get; set; }
        [Required]
        [StringLength(64)]
        public string HouseNumber { get; set; }

        [Required]
        public Country Country { get; set; }

        public virtual ICollection<BranchEmployee> PartOFEmployee { get; set; }
    }
}
