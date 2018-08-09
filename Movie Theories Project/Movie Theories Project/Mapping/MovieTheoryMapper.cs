using DAL.Models;
using Movie_Theories_Project.Models;
using MovieProjectBLL.Model;

namespace Movie_Theories_Project.Mapping
{
    public class MovieTheoryMapper
    {
        public MovieTheoryPO MapDoToPo(MovieTheoryDO from)
        {
            MovieTheoryPO to = new MovieTheoryPO();
            to.MovieTheoryID = from.MovieTheoryID;
            to.TitleOfTheory = from.TitleOfTheory;
            to.Theory = from.Theory;
            to.Status = from.Status;
            to.MovieId = from.MovieId;

            return to;
        }

        public MovieTheoryDO MapPoToDo(MovieTheoryPO from)
        {
            MovieTheoryDO to = new MovieTheoryDO();
            to.MovieTheoryID = from.MovieTheoryID;
            to.TitleOfTheory = from.TitleOfTheory;
            to.Theory = from.Theory;
            to.Status = from.Status;
            to.MovieId = from.MovieId;

            return to;
        }

        public MovieTheoryBO MapDoToBo(MovieTheoryDO from)
        {
            MovieTheoryBO to = new MovieTheoryBO();
            to.MovieTheoryID = from.MovieTheoryID;
            to.TitleOfTheory = from.TitleOfTheory;
            to.Theory = from.Theory;
            to.Status = from.Status;
            to.MovieId = from.MovieId;

            return to;
        }
    }
}