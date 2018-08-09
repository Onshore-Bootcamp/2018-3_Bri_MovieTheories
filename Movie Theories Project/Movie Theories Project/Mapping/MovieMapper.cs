using DAL.Models;
using Movie_Theories_Project.Models;

namespace Movie_Theories_Project.Mapping
{
    public class MovieMapper
    {
        public MoviePO MapDoToPo(MovieDO from)
        {
            MoviePO to = new MoviePO();
            to.MovieId = from.MovieId;
            to.Title = from.Title;
            to.Director = from.Director;
            to.OpeningWeekend = from.OpeningWeekend;
            to.YearReleased = from.YearReleased;

            return to;
        }

        public MovieDO MapPoToDo(MoviePO from)
        {
            MovieDO to = new MovieDO();
            to.MovieId = from.MovieId;
            to.Title = from.Title;
            to.Director = from.Director;
            to.OpeningWeekend = from.OpeningWeekend;
            to.YearReleased = from.YearReleased;

            return to;
        }
    }
}