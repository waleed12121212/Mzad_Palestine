using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mzad_Palestine_Core.DTOs.Listing
{
    public class CreateListingDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public List<string> Images { get; set; }

        public int UserId { get; set; }
    }
} 