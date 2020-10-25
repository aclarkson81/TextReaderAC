using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextReaderAC
{
    class EventLine
    {
        DateTime startTime;
        DateTime duration;
        string keynumber;
        string description;

        public EventLine(DateTime startTime, DateTime duration, string keynumber, string description)
        {
            this.startTime = startTime;
            this.duration = duration;
            this.keynumber = keynumber;
            this.description = description;
        }

        //Modifiers
        public void addStartTime(DateTime startTime)
        {
            this.startTime = startTime;
        }

        public void addDuration(DateTime duration)
        {
            this.duration = duration;
        }

        public void addKeynumber(string keynumber)
        {
            this.keynumber = keynumber;
        }

        public void addDescription(string description)
        {
            this.description = description;
        }

        //Getters
        public string getKeynumber()
        {
            return this.keynumber;
        }
    }
        
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            int count = 0;
            string line;
            Queue<EventLine> playlist = new Queue<EventLine>();

            //Read file in line by line and create eventlines in Queue
            System.IO.StreamReader file = 
                //TODO: fix so able to select file from dropdown
                new System.IO.StreamReader(@"c:\Users\Adam\TENStack\10Peach-Melbourne.txt");
            while((line = file.ReadLine()) != null)
            {

                count++;
                DateTime startTime = new DateTime(0001, 1, 1, 0, 0, 0, 0);
                DateTime duration = new DateTime(0001, 1, 1, 0, 0, 0, 0);
                string keynumber = null;
                string description = null;
                //draw out contents of each line
                System.Console.WriteLine(line);
                string[] lineComponents = line.Split(',');
                foreach(string component in lineComponents)
                {
                    string compNoAsterix = component.Replace("\"", "");
                    //System.Console.WriteLine(compNoAsterix);
                    string[] splitComponent = compNoAsterix.Split(':');

                    switch (splitComponent[0])
                    {
                        case "StartDateTime":
                            string[] SDTsplit1 = splitComponent[1].Split('T');
                            string[] SDTsplit2 = SDTsplit1[0].Split('-');
                            string[] SDTsplit3 = splitComponent[3].Split('.');
                            int year = Int16.Parse(SDTsplit2[0]);
                            int month = Int16.Parse(SDTsplit2[1]);
                            int day = Int16.Parse(SDTsplit2[2]);
                            int hour = Int16.Parse(SDTsplit1[1]);
                            int minute = Int16.Parse(splitComponent[2]);
                            int second = Int16.Parse(SDTsplit3[0]);
                            int millisecond = Int16.Parse(SDTsplit3[1]);
                            startTime = new DateTime(year, month, day, hour, minute, second, millisecond);
                            break;
                        case "Title":
                            string[] TitleSplit = splitComponent[1].Split(' ');
                            keynumber = TitleSplit[0];
                            for(int x = 1; x < TitleSplit.Length; x++)
                            {
                                description = description + " " + TitleSplit[x];
                            }
                            break;
                        case "Duration":
                            int dHour = Int32.Parse(splitComponent[2]);
                            int dMinute = Int32.Parse(splitComponent[3]);
                            int dSecond = Int32.Parse(splitComponent[4]);
                            int dmilliseconds = Int32.Parse(splitComponent[5])*40;
                            duration = new DateTime(0001, 1, 1, dHour, dMinute, dSecond, dmilliseconds);
                            break;
                        default:
                            break;
                    }
                }
                EventLine anEvent = new EventLine(startTime, duration, keynumber, description);
                System.Console.WriteLine(startTime.ToString("G") + " " + duration.ToString("G") + " " + keynumber + " " + description);
                System.Console.WriteLine(anEvent.getKeynumber());
                playlist.Enqueue(anEvent);
            }
            /**
            PrintValues(playlist);

            void PrintValues(IEnumerable myCollection)
            {
                foreach (EventLine anEvent in myCollection)
                {
                    Console.Write(anEvent.getKeynumber());
                }
                Console.WriteLine();
            }
            */

            file.Close();
            System.Console.WriteLine("There were {0} lines.", count);
            System.Console.ReadLine();
        }
    }
}
