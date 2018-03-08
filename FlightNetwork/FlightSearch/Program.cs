using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FlightSearch
{
    class Program
    {
        #region Properties for file path and names

        public static List<string> FileNames = ConfigurationSettings.AppSettings["ProviderNames"].Split(',').ToList();
        public static string Path = ConfigurationSettings.AppSettings["Path"];

        #endregion Properties for file path and names

        //------------------------------------------------------------------------------------------------------------

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("serchFlights -o ");

                string originCityCode = ReadCityCode();

                Console.Write(" -d ");

                string destinationCityCode = ReadCityCode();

                Console.WriteLine();

                SearchFlight(originCityCode, destinationCityCode);

                Console.WriteLine("Search again ? Y/N");

                ConsoleKeyInfo keyPressed = Console.ReadKey();

                if (keyPressed.Key == ConsoleKey.N || keyPressed.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else
                {
                    Console.WriteLine();
                }//end else

            }//end While

        }//end main

        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Read City Code From Console (Three Characters)
        /// </summary>
        /// <returns>City Code</returns>
        public static string ReadCityCode()
        {
            char[] input = new char[3];

            for (var i = 0; i < input.Length; i++)
            {
                input[i] = Console.ReadKey().KeyChar;
            }//end for

            return (new string(input)).ToUpper();

        }//end ReadCityCode

        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Search Flights for Orgin and Destination And Prints the Record on console
        /// </summary>
        /// <param name="Org">Origin City Code</param>
        /// <param name="Dest">Destination City Code</param>
        public static void SearchFlight(string Org, string Dest)
        {
            //read all Providers data 
            List<MFlightData> lstProviderData = GetFromProvider();
            
            lstProviderData = (from flight in lstProviderData
                               where
                               flight.Origin == Org &&
                               flight.Destination == Dest
                               group flight by 
                               new { flight.Price,flight.DepartureTime,flight.DestinationTime,flight.Origin,flight.Destination }
                               into myGroup
                               select myGroup.FirstOrDefault()
                               )
                               .OrderBy(price => price.Price)
                               .ThenBy(dt => dt.DepartureTime)
                               .ToList();
            

            foreach (MFlightData pd in lstProviderData)
            {
                Console.WriteLine(pd.Origin + " --> " + pd.Destination + " (" + pd.strDepartureTime + " --> " + pd.strDestinationTime + ")" + " - " + pd.strPrice);
            }//end foreach

            if (lstProviderData.Count == 0)
            {
                Console.WriteLine("No Flights Found for " + Org + " --> " + Dest) ;
            }//end if
        }//SearchFlight

        //------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Read Records from All three Provider 
        /// </summary>
        /// <returns>returns all Flights data in Model Flight Data</returns>
        public static List<MFlightData> GetFromProvider()
        {

            List<MFlightData> provider = new List<MFlightData>();

            foreach (String FileName in FileNames)
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(Path + FileName))
                {
                    String line;
                    int Counter = 0; //To skip First Line of File
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (Counter > 0)
                        {
                            MFlightData pData = new MFlightData();
                            string[] arrLine = line.Split(',','|');

                            pData.Origin = arrLine[0];
                            pData.strDepartureTime = arrLine[1];
                            pData.Destination = arrLine[2];
                            pData.strDestinationTime = arrLine[3];
                            pData.strPrice = arrLine[4];

                            pData.DepartureTime = DateTime.Parse(pData.strDepartureTime, System.Globalization.CultureInfo.InvariantCulture);
                            pData.DestinationTime = DateTime.Parse(pData.strDestinationTime, System.Globalization.CultureInfo.InvariantCulture);
                            
                            pData.Price = Convert.ToDecimal((pData.strPrice.Split('$'))[1]);

                            provider.Add(pData);

                        }//end if

                        Counter++;

                    }//end while
                }//end using
            }//end foreach

            return provider;
        }//end GetFromProvider

        //------------------------------------------------------------------------------------------------------------

    }//end class Program

}//end namespace FlightSearch
