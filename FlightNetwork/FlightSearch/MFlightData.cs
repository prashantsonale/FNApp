using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSearch
{
    public class MFlightData
    {
        #region Properties

        public string Origin { get; set; }
        public string Destination { get; set; }
        public string strDepartureTime { get; set; }
        public string strDestinationTime { get; set; }
        public string strPrice { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime DestinationTime { get; set; }
        public decimal Price { get; set; }

        #endregion Properties
    }//end class MFlightData
}//end namespace FlightSearch
