using PickAndBook.Data.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PickAndBook.Data.Models
{
    public class Category : IOrderable
    {
        public Category()
        {
            this.Companies = new HashSet<Company>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(50)]
        public string CategoryName { get; set; }

        [Required]
        public string CategoryDescription { get; set; }

        public string CategoryImage { get; set; }

        public virtual ICollection<Company> Companies { get; set; }

        [DefaultValue(0)]
        public int OrderBy { get; set; }
    }
}