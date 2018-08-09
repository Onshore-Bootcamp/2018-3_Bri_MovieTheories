using MovieProjectBLL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieProjectBLL
{
    public class StatusLogic
    {
        //KeyValuePair for what the string says and the percent of all the statuses.
        public List<KeyValuePair<string, float>> StatusCount(List<MovieTheoryBO> numberOfStatuses)
        {
            //Collects all the theories into multiple piles for each status to count them and divide them by the total number for the percent.
            List < KeyValuePair<string, float> > status = (from theories in numberOfStatuses
                                                           group theories by theories.Status into newGroup
                                                           select new KeyValuePair<string, float>
                                                           (newGroup.Key, (float)newGroup.Count() / numberOfStatuses.Count() * 100)).ToList();
            return status;
        }
    }
}
