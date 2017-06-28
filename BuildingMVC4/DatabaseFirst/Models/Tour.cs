namespace DatabaseFirst.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tour
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Length { get; set; }

        public decimal Price { get; set; }

        public int RatingId { get; set; }

        public bool IncludesMeals { get; set; }

        public virtual Rating Rating { get; set; }
    }
}
