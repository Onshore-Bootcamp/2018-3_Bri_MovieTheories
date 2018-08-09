using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Movie_Theories_Project.Models
{
    public class MoviePO
    {
        public long MovieId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Director { get; set; }

        public string OpeningWeekend { get; set; }

        [Required]
        public short YearReleased { get; set; }

        public List<MovieTheoryPO> MovieTheories { get; set; }
    }
}