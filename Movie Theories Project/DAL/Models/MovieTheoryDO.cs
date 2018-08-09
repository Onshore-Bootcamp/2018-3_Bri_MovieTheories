using System.Collections.Generic;

namespace DAL.Models
{
    public class MovieTheoryDO
    {
        public long MovieTheoryID { get; set; }

        public string TitleOfTheory { get; set; }

        public string Theory { get; set; }

        public string Status { get; set; }

        public long MovieId { get; set; }
    }
}
