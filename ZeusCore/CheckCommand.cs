using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.IO;
using System.Data;

namespace ZeusCore
{
    public class CheckCommand : ICheckCommand
    {
        CommandDefinition consoleCommands = new CommandDefinition();
        WebClient webClient = new WebClient();

        //Singleton-Pattern start
        private static ICheckCommand instance;

        private CheckCommand() { }

        public static ICheckCommand Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CheckCommand();
                }
                return instance;
            }
        }
        //Singleton-Pattern End

        //check string for the command
        public string[] checkCommand(string cmd)
        {
            //input string is split by space into it's words
            string[] cmdInputWordList = cmd.Split(' ');

            //calculate the similarity of the commands foreach word in cmd input string
            double maxSimTmp = 0;
            double maxSim = 0;
            int cmdIndex = 0;
            string[] command = null;

            //foreach command in consoleCommands.commands
            for (int i = 0; i < consoleCommands.commandsDict.Count(); i++)
            {
                //split command in words
                string[] cmdWordList = consoleCommands.commandsDict[i][1].Split(' ');
                //foreach word in command
                for (int j = 0; j < cmdWordList.Count(); j++)
                {
                    double biggestSim = 0;
                    int indexT = 0;
                    //calculate similarity foreach word in cmd-input-string
                    for (int t = 0; t < cmdInputWordList.Count(); t++)
                    {
                        double sim = CalculateSimilarity(cmdWordList[j], cmdInputWordList[t]);
                        if (sim > 0.6 && sim > biggestSim)
                        {
                            biggestSim = sim;
                            indexT = t;
                        }
                    }
                    //if there is no similarity, break out of the function, else add it to maxSimTmp
                    if (biggestSim != 0)
                    {
                        maxSimTmp += biggestSim;
                    }
                    else
                    {
                        maxSimTmp = 0;
                        break;
                    }
                }
                //if maxSim of command is bigger than maxSim of previous command set new cmdIndex and update maxSim with maxSimTmp
                if (maxSimTmp > maxSim)
                {
                    maxSim = maxSimTmp;
                    cmdIndex = i;
                    maxSimTmp = 0;
                }
                else
                {
                    maxSimTmp = 0;
                }
            }
            //if command was recognized in cmd-input-string set commandArray with command-data
            if (maxSim != 0)
            {
                string[] cmdWordList = consoleCommands.commandsDict[cmdIndex][1].Split(' ');
                //foreach word in command
                for (int j = 0; j < cmdWordList.Count(); j++)
                {
                    double biggestSim = 0;
                    int indexT = 0;
                    //calculate similarity foreach word in cmd-input-string
                    for (int t = 0; t < cmdInputWordList.Count(); t++)
                    {
                        double sim = CalculateSimilarity(cmdWordList[j], cmdInputWordList[t]);
                        if (sim > 0.6 && sim > biggestSim)
                        {
                            biggestSim = sim;
                            indexT = t;
                        }
                    }
                    //if there is no similarity, break out of the function, else add it to maxSimTmp
                    if (biggestSim != 0)
                    {
                        cmdInputWordList[indexT] = cmdWordList[j];
                    }
                }
                //replace old cmd words with new cmd words
                command = new string[consoleCommands.commandsDict[cmdIndex].Count() + 1];
                for (int i = 0; i < consoleCommands.commandsDict[cmdIndex].Count(); i++)
                {
                    command[i] = consoleCommands.commandsDict[cmdIndex][i];
                }
                command[consoleCommands.commandsDict[cmdIndex].Count()] = string.Join(" ", cmdInputWordList);
            }

            //checks if the command-function defined in commandDefinition.xml is here defined
            if (command != null && checkIfUsesCore(command))
            {
                string[] usesCoreReturn = new string[command.Length + 2];
                string[] usesCoreTrueData = usesCoreTrue(command);
                for (int i = 0; i < command.Length; i++)
                {
                    usesCoreReturn[i] = command[i];
                }
                for (int i = 0; i < usesCoreTrueData.Length; i++)
                {
                    usesCoreReturn[command.Length + i] = usesCoreTrueData[i];
                }
                return usesCoreReturn;
            }
            else
            {
                return command;
            }
        }        

        //methods according to the commands.xml file
        public string possibleCommands()
        {
            //add each cmd-name and cmd-command to cmdList
            List<string[]> cmdList = new List<string[]>();
            for (int i = 0; i < consoleCommands.commandsDict.Count(); i++)
            {
                cmdList.Add(new string[] { consoleCommands.commandsDict[i][0], consoleCommands.commandsDict[i][1] });
            }
            string[] array = new string[cmdList.Count()];
            for (int i = 0; i < array.Count(); i++)
            {
                array[i] = string.Join(":", cmdList[i]);
            }
            return string.Join(";", array);
        }
        public string getCurrentTime()
        {
            return DateTime.Now.ToString("h:mm tt");
        }
        public string getCurrentDay()
        {
            return DateTime.Now.ToShortDateString(); ;
        }
        public string mathCalculation(string function)
        {
            if (!function.Equals("") && !function.Equals("was ist"))
            {
                try
                {
                    return function.Replace("die wurzel aus", "sqrt") + " equals " + math(function);
                }
                catch (Exception e)
                {
                    return "Error: " + e.Message;
                }
            }
            else
            {
                return "Invalid function";
            }
        }
        public string startProgram(string program)
        {
            return launchProgram(program);
        }
        public string closeProgram(string program)
        {
            return endProgram(program); ;
        }
        public string wiki(string searchString)
        {
            if (searchString.Equals("random"))
            {
                return SearchWiki("", true);
            }
            else
            {
                return SearchWiki(searchString, false);
            }
        }
        public string google(string searchString)
        {
            startGoogle(searchString);
            return "";
        }
        public string callProtocol(string protocoll)
        {
            return protocol(protocoll);
        }
        public string weather()
        {
            try
            {
                //get weatherData
                Dictionary<string, string[]> weatherData = getWeatherData("Hamburg");
                //decode weatherdata
                string city = weatherData["city"][1].Split(new[] { "name=" }, StringSplitOptions.None)[1];
                string temperature = parseFarenheitToCelcius(double.Parse(weatherData["temperature"][0].Split(new[] { "value=" }, StringSplitOptions.None)[1].Replace(".", ","))).ToString();
                var windspeedtmp = weatherData["wind"][0].Split(new[] { "speed " }, StringSplitOptions.None)[1].Split(new[] { ">" }, StringSplitOptions.None)[0].Split(new[] { ' ' }, 2, StringSplitOptions.None);
                var winddirectiontmp = weatherData["wind"][2].Split(new[] { "direction " }, StringSplitOptions.None)[1].Split(new[] { ">" }, StringSplitOptions.None)[0].Split(' ');
                string[] wind = new string[2];
                wind[0] = parseMilesToKmh(double.Parse(windspeedtmp[0].Split(new[] { "value=" }, StringSplitOptions.None)[1].Replace(".", ","))).ToString();
                wind[1] = winddirectiontmp[2].Split(new[] { "name=" }, StringSplitOptions.None)[1];
                string clouds = weatherData["clouds"][1].Split(new[] { "name=" }, StringSplitOptions.None)[1];
                string weather = weatherData["weather"][1].Split(new[] { "value=" }, StringSplitOptions.None)[1];
                string lastupdate = weatherData["lastupdate"][0].Split(new[] { "value=" }, StringSplitOptions.None)[1].Split('T')[1];
                return "The weather in " + city + " is " + weather + ". Currently it is " + temperature + " degrees outside." +
                    "~\rCity: " + city + "\rTemperatur: " + temperature + "℃\rWind:\r  Speed: " + wind[0] + "km/h\r  Direction: " + wind[1] + "\rClouds: " + clouds + "\rLastUpdate: " + lastupdate;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }
        public string RSSNewsFeed(string parameter)
        {
            try
            {
                List<string> tmpl = new List<string>();
                List<string[]> newsData = new List<string[]>();
                //if parameter is empty do general news search
                if (parameter.Equals("") || parameter.Equals("nachrichten"))
                {
                    newsData.Add(GetArticleContent("WORLD", false)[0]);
                    //newsData.Add(GetArticleContent("TECHNOLOGY", false)[0]);
                    newsData.Add(GetArticleContent("ENTERTAINMENT", false)[0]);
                }
                else
                {
                    List<string[]> data = GetArticleContent(parameter, true);
                    //get only the first 5 article
                    for (int i = 0; i < 5; i++)
                    {
                        newsData.Add(data[i]);
                    }
                }
                //decode string into List<string[]>
                foreach (var article in newsData)
                {
                    tmpl.Add(string.Join("join2%%", article));
                }
                return string.Join("join1%%", tmpl);
            }
            catch(Exception e)
            {
                return "Error: " + e.Message;
            }            
        }
        public string SpotifyStartSong()
        {
            return "Command needs to be implemented soon ...";
        }
        public string SpotifyStopSong()
        {
            return "Command needs to be implemented soon ...";
        }
        public string SpotifyNextSong()
        {
            return "Command needs to be implemented soon ...";
        }
        public string SpotifyPreviousSong()
        {
            return "Command needs to be implemented soon ...";
        }
        //end of the methods according to the commands.xml file        

        //parse Farenheit to Celcius
        double parseFarenheitToCelcius(double f)
        {
            return Math.Round((double)5 / (double)9 * (f - (double)32), 1, MidpointRounding.AwayFromZero);
        }

        double parseMilesToKmh(double m)
        {
            return Math.Round(m * 1.609, 1, MidpointRounding.AwayFromZero);
        }

        //get weather data from OpenWeatherMap
        Dictionary<string, string[]> getWeatherData(string city)
        {
            Dictionary<string, string[]> weatherDataDict = new Dictionary<string, string[]>();
            //apiKey for OpenWeatherMap
            string apiKey = "{ key }";
            //apiUrl
            string url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&units=imperial&APPID=" + apiKey;
            //get weather-data
            string weatherData = webClient.DownloadString(url).Replace("\"", "");
            //custom-parse weather-data
            string[] cityTmp = weatherData.Split(new[] { "city " }, StringSplitOptions.None)[1].Split('>')[0].Split(new[] { ' ' }, 2);            
            string[] temperatureTmp = weatherData.Split(new[] { "temperature " }, StringSplitOptions.None)[1].Split('>')[0].Split(new[] { ' ' }, 4);            
            string[] windTmp = weatherData.Split(new[] { "<wind>" }, StringSplitOptions.None)[1].Split(new[] { "</wind>" }, StringSplitOptions.None)[0].Replace("><", ">;<").Replace(">;</", "></").Split(';');            
            string[] cloudTmp = weatherData.Split(new[] { "clouds " }, StringSplitOptions.None)[1].Split('>')[0].Split(new[] { ' ' }, 2);            
            string[] weatherTmp = weatherData.Split(new[] { "weather " }, StringSplitOptions.None)[1].Split('>')[0].Split(new[] { ' ' }, 3);            
            string[] lastupdateTmp = weatherData.Split(new[] { "lastupdate " }, StringSplitOptions.None)[1].Split('>')[0].Split(' ');
            weatherDataDict.Add("city", cityTmp);
            weatherDataDict.Add("temperature", temperatureTmp);
            weatherDataDict.Add("wind", windTmp);
            weatherDataDict.Add("clouds", cloudTmp);
            weatherDataDict.Add("weather", weatherTmp);
            weatherDataDict.Add("lastupdate", lastupdateTmp);
            return weatherDataDict;
        }

        //method that can do all kinds of math-operations, input is only the function, !!! without 'was ist' at the beginning
        static double math(string mathFunction)
        {            
            //satz 'die wurzel aus' wird mit sqrt replaced
            mathFunction = mathFunction.ToLower().Replace("die wurzel aus", "sqrt");
            List<string> functionParts = mathFunction.Split(' ').ToList();
            int times = 0;
            //count how many operations are needed
            foreach (var part in functionParts)
            {
                if (part.Equals("+") || part.Equals("-") || part.Equals("/") || part.Equals("x") || part.Equals("^") || part.Equals("sqrt"))
                {
                    times++;
                }
            }
            double[] tmpResult = new double[times];
            //for-loop performs one operation per loop
            for (int i = 0; i < times; i++)
            {
                //checks if it need to do operation sqrt or ^ first
                if (functionParts.Contains("sqrt"))
                {
                    for (int j = 0; j < functionParts.Count(); j++)
                    {
                        if (functionParts[j].Equals("sqrt"))
                        {
                            tmpResult[i] = Math.Sqrt(double.Parse(functionParts[j + 1]));
                            functionParts[j] = tmpResult[i].ToString();
                            functionParts.RemoveAt(j + 1);
                            break;
                        }
                    }
                }
                else if (functionParts.Contains("^"))
                {
                    for (int j = 0; j < functionParts.Count(); j++)
                    {
                        if (functionParts[j].Equals("^"))
                        {
                            tmpResult[i] = Math.Pow(double.Parse(functionParts[j - 1]), double.Parse(functionParts[j + 1]));
                            functionParts[j] = tmpResult[i].ToString();
                            functionParts.RemoveAt(j - 1);
                            functionParts.RemoveAt(j);
                            break;
                        }
                    }
                } //if not, it checks * and / first, because "punkt-vor-strich-rechnung"                 
                else if (functionParts.Contains("x"))
                {
                    for (int j = 0; j < functionParts.Count(); j++)
                    {
                        if (functionParts[j].Equals("x"))
                        {
                            tmpResult[i] = double.Parse(functionParts[j - 1]) * double.Parse(functionParts[j + 1]);
                            functionParts[j] = tmpResult[i].ToString();
                            functionParts.RemoveAt(j - 1);
                            functionParts.RemoveAt(j);
                            break;
                        }
                    }
                }
                else if (functionParts.Contains("/"))
                {
                    for (int j = 0; j < functionParts.Count(); j++)
                    {
                        if (functionParts[j].Equals("/"))
                        {
                            tmpResult[i] = double.Parse(functionParts[j - 1]) / double.Parse(functionParts[j + 1]);
                            functionParts[j] = tmpResult[i].ToString();
                            functionParts.RemoveAt(j - 1);
                            functionParts.RemoveAt(j);
                            break;
                        }
                    }
                } //if it doesnt need to do anything else than "punktrechnung"
                else
                {
                    if (functionParts[1].Equals("+"))
                    {
                        tmpResult[i] = double.Parse(functionParts[0]) + double.Parse(functionParts[2]);
                        functionParts[1] = tmpResult[i].ToString();
                        functionParts.RemoveAt(0);
                        functionParts.RemoveAt(1);
                    }
                    else if (functionParts[1].Equals("-"))
                    {
                        tmpResult[i] = double.Parse(functionParts[0]) - double.Parse(functionParts[2]);
                        functionParts[1] = tmpResult[i].ToString();
                        functionParts.RemoveAt(0);
                        functionParts.RemoveAt(1);
                    }
                }
            }
            return double.Parse(functionParts[0]);                        
        }

        //calls a specified protocol
        string protocol(string protocol)
        {
            switch (protocol)
            {
                case "ProtocolName":
                    Process.Start("ProtocolPath");
                    return "ProtocolName protocol called";
                default:
                    return "Invalid protocol";
            }
        }

        //opens firefox with google and  the special search term
        public void startGoogle(string searchTerm)
        {
                string newString = searchTerm.Replace(' ', '+');
                Process.Start("firefox.exe", "http://www.google.de/search?hl=de&q=" + newString);
        }

        //method used for the wiki search algorithm
        public string SearchWiki(string article, bool random)
        {
            string searchUrl;
            if (random)
            {
                string url = "https://de.wikipedia.org/w/api.php?action=query&list=random&format=json&rnnamespace=0&rnlimit=1";
                string art = "";
                try
                {
                     art = JObject.Parse(webClient.DownloadString(url))["query"].ToArray()[0].ToString().Split(new[] { "title\": \"" }, StringSplitOptions.None)[1].Split('\"')[0];
                }
                catch(Exception e)
                {
                    return "Error: " + e.Message;
                }                
                searchUrl = "https://de.wikipedia.org/w/api.php?action=opensearch&format=json&search=" + art;
            }
            else
            {
                searchUrl = "https://de.wikipedia.org/w/api.php?action=opensearch&format=json&search=" + article;
            }
            string[] data;
            string[] data2;
            string[] data3;
            string[] link;
            try
            {
                data = downloadJsonData(searchUrl, 1);
                data2 = downloadJsonData(searchUrl, 2);
                data3 = downloadJsonData(searchUrl, 3);
            }            
            catch(Exception e)
            {
                return "Error: " + e.Message;
            }
            if (data[0] == "Error")
            {
                return "Error";
            }
            try
            {
                link = data3[0].Split('\"');
            }
            catch (Exception e)
            {
                var b = e;
                return "Please search just for one word.";
            }            
            try
            {
                return data[0] + "~" + data2[0] + "~" + link[1] + "~";
            }
            catch (Exception)
            {                
                return "Wikipedia Artikel nicht gefunden\n";
            }
        }

        //method used for the wikisearch-algorithm, especially for the wiki-api-jsondata
        string[] downloadJsonData(string url, int b)
        {            
            string data = "";
            try
            {
                data = webClient.DownloadString(new Uri(url));
            }
            catch (Exception e)
            {
                string[] s = new string[] { "Error", e.Message };
                return s;
            }
            var objects = JsonConvert.DeserializeObject<List<object>>(data);
            string[] s1 = objects.Select(obj => JsonConvert.SerializeObject(obj)).ToArray();
            var objects2 = JsonConvert.DeserializeObject<List<object>>(s1[b]);
            string[] s2 = objects2.Select(obj => JsonConvert.SerializeObject(obj)).ToArray();
            return s2;
        }

        //method for starting any program
        public string launchProgram(string programName)
        {
            try
            {
                Process.Start(programName);
                return programName;
            }
            catch (Exception)
            {
                try
                {                    
                    Process.Start(consoleCommands.programNamesDict[programName]);
                    return programName;                   
                }
                catch(Exception e)
                {
                    return "Error: " + e.Message;
                }
            }
        }

        //method for starting any program
        public string endProgram(string programName)
        {
            try
            {
                Process[] processArray = Process.GetProcessesByName(programName);
                foreach (var process in processArray)
                {
                    process.CloseMainWindow();
                }
                return programName;
            }
            catch(Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        //returns list of commands
        public List<string[]> getCommands()
        {
            return consoleCommands.commandsDict;
        }

        //returns list of function for command: possible commands
        public Dictionary<int, string> getCommandDic()
        {
            return consoleCommands.commandDict;
        }

        //checks if command-function uses core
        bool checkIfUsesCore(string[] cmd)
        {
            if (cmd[3].Equals("true"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //if usesCoreTrue is true
        string[] usesCoreTrue(string[] cmd)
        {
            try
            {
                if (cmd[6].Equals("true"))
                {
                    MethodInfo methodInfo = this.GetType().GetMethod(cmd[2], Type.GetTypeArray(new object[] { ""}));
                    return new string[] { "false", (string)methodInfo.Invoke(this, new object[] { cmd[7].Replace(cmd[1] + " ", "") }) };
                }
                else
                {
                    MethodInfo methodInfo = this.GetType().GetMethod(cmd[2]);
                    return new string[] { "false", (string)methodInfo.Invoke(this, null) };
                }                                                
            }
            catch (Exception e)
            {
                return new string[] { "true", e.Message };
            }
        }

        //check the similarity of two strings
        public bool checkSimilarity(string source, string target, double percentage)
        {
            double sim = CalculateSimilarity(source, target);
            if (sim > percentage)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }

        int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }
            return distance[sourceWordCount, targetWordCount];
        }

        //RSS-Feed from GoogleNews, each List-Element contains one article
        //article-element (string[]):
        //[0] searchParam
        //[1] title
        //[2] link
        //[3] item_id
        //[4] pubDate
        //[5] description        
        public static List<string[]> GetArticleContent(string NewsParameters, bool freeSearch)
        {
            List<string[]> Details = new List<string[]>();
            HttpWebRequest request;

            // httpWebRequest with API URL
            if (freeSearch)
            {
                request = (HttpWebRequest)WebRequest.Create
                ("https://news.google.com/news/rss/search/section/q/" + NewsParameters + "?ned=de&gl=DE&hl=de");
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create
                ("https://news.google.com/news/rss/headlines/section/topic/" + NewsParameters + "?ned=de&gl=DE&hl=de");
            }
            
            //Method GET
            request.Method = "GET";

            //HttpWebResponse for result
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();


            //Mapping of status code
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == "")
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                //Get news data in json string

                string data = readStream.ReadToEnd();

                //Declare DataSet for putting data in it.
                DataSet ds = new DataSet();
                StringReader reader = new StringReader(data);
                ds.ReadXml(reader);
                DataTable dtGetNews = new DataTable();

                if (ds.Tables.Count > 3)
                {
                    dtGetNews = ds.Tables["item"];

                    foreach (DataRow article in dtGetNews.Rows)
                    {
                        string[] DataObj = new string[6];
                        DataObj[0] = NewsParameters;
                        DataObj[1] = article["title"].ToString();
                        DataObj[2] = article["link"].ToString();
                        DataObj[3] = article["item_id"].ToString();
                        DataObj[4] = article["pubDate"].ToString();
                        DataObj[5] = article["description"].ToString();
                        Details.Add(DataObj);
                    }
                }
            }
            //Return News array 
            return Details;
        }
    }
}
