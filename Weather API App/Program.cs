using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;




namespace APIapp
{
    class Program
    {
        string category = "forecast";
        string format = "json";
        string key = "INSERT KEY HERE";                 // API key
        string city = "gothenburg";
        string days = "3";                                  //Forecast of how many days? Max 3 days when using free version
        string airQuali = "no";                             //yes or no
        string alerts = "no";                               //yes or no
        string response;

        HttpClient client = new HttpClient();

        
        static async Task Main(string[] args)
        {

            Program program = new Program();
            Console.WriteLine("Hello! This is a program to see the average temperature of a place today and 2 days forwards!");
            while (true)
            {
                await program.GetTodoItems();
            }

        }

        private async Task GetTodoItems()
        {

            Console.Write("Write a place: ");
            string place = Console.ReadLine();              //Input
            //Console.WriteLine(
            //  "https://api.weatherapi.com/v1/" + category + "." + format + "?key=" + key + "&q=" + city + "&days=" + days + "&aqi=" + airQuali + "&alerts=" + alerts);
            try
            {
                response = await client.GetStringAsync(
                    "https://api.weatherapi.com/v1/" + category + "." + format + "?key=" + key + "&q=" + place + "&days=" + days + "&aqi=" + airQuali + "&alerts=" + alerts);
                dynamic data = JObject.Parse(response);

                string t = data.current.temp_c.ToString();

                dynamic forecastday = data.forecast.forecastday;

                string[] temps = new string[Int16.Parse(days)];         //Arrays with temperature and dates
                string[] maxTemps = new string[Int16.Parse(days)];
                string[] minTemps = new string[Int16.Parse(days)];
                string[] dates = new string[Int16.Parse(days)];
                string[] condition = new string[Int16.Parse(days)];


                // Add the values in arrays
                for (int i = 0; i < temps.Length; i++)
                {

                    temps[i] = forecastday[i].day.avgtemp_c;            //Average temperature

                    maxTemps[i] = forecastday[i].day.maxtemp_c;         //Maximum temperature, currently not used
                    minTemps[i] = forecastday[i].day.mintemp_c;         //Minimum temperature, currently not used
                    dates[i] = forecastday[i].date;                     //Dates
                    condition[i] = forecastday[i].day.condition.text;   //Condition

                }


                PrintColor("The average temperature for " + data.location.name + ", " + data.location.country, ConsoleColor.Green, true);

                for (int i = 0; i < temps.Length; i++)
                {
                    PrintColor(dates[i] + ": ", ConsoleColor.DarkGreen, false);
                    PrintColor(temps[i] + "°C - ", ConsoleColor.White, false);
                    PrintColor(condition[i], ConsoleColor.White, true);

                }


            }
            catch (Exception ex)
            {
                if (ex.Message == "Response status code does not indicate success: 400 (Bad Request).")
                {
                    Console.WriteLine("Could not find a place called \"{0}\", make sure that you spelt it correctly.", place);
                }

                Console.WriteLine(ex.Message);

            }
        }

        // Method to easily print color with color
        static void PrintColor(String text, ConsoleColor color, bool newLine)
        {

            Console.ForegroundColor = color;

            if (newLine)
            {
                Console.WriteLine(text);
            }
            else
            {
                Console.Write(text);
            }

            Console.ResetColor();

        }







    }
}
