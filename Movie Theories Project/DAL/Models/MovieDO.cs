namespace DAL.Models
{
    public class MovieDO
    {
        public long MovieId { get; set; }

        public string Title { get; set; }

        public string Director { get; set; }

        public string OpeningWeekend { get; set; }

        public short YearReleased { get; set; }
    }
}
