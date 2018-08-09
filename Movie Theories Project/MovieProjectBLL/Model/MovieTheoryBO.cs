using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieProjectBLL.Model
{
    public class MovieTheoryBO
    {
        public long MovieTheoryID { get; set; }

        public string TitleOfTheory { get; set; }

        public string Theory { get; set; }

        public string Status { get; set; }

        public long MovieId { get; set; }
    }
}
