using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Movie_Theories_Project.Models
{
    public class MovieTheoryPO
    {
        public long MovieTheoryID { get; set; }

        [Required]
        public string TitleOfTheory { get; set; }

        [Required]
        public string Theory { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public long MovieId { get; set; }
    }
}