using System;
using System.Collections.Generic;
using System.Text;

namespace srt_align
{
    class TimeStamp
    {
        //instance properties
        private int hours;      //hours representation of the timestamp
        private int minutes;    //minutes representation of the timestamp
        private int seconds;    //seconds representation of the timestamp
        private int millis;     //milliseconds representation of the timestamp
        private bool negative;  //boolean representing if time goes 

        //constance
        const int HOURS_MIN = 0;
        const int MINUTES_MIN = HOURS_MIN;
        const int SECONDS_MIN = HOURS_MIN;
        const int MILLIS_MIN = HOURS_MIN;

        const int HOURS_MAX = 99;
        const int MINUTES_MAX = 59;
        const int SECONDS_MAX = 59;
        const int MILLIS_MAX = 999;

        const int HOURS_TO_MILLIS = 3600000;
        const int MINUTES_TO_MILLIS = 60000;
        const int SECONDS_TO_MILLIS = 1000;

        /// <summary>
        /// constructor of the Timestamp class
        /// </summary>
        /// <param name="hours">a two(2) digits number reprsenting the hours of the timestamp</param>
        /// <param name="minutes">a number between 0 and 59 representing the minutes of the timestamp</param>
        /// <param name="seconds"> a number between 0 and 59 representing the seconds of the timestamp</param>
        /// <param name="millis">a three(3) digits number representing the milliseconds of the timestamp</param>
        public TimeStamp(int hours, int minutes, int seconds, int millis, bool negative)
        {
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Millis = millis;
            Negative = negative;
        }

        //Access methods
        
        /// <summary>
        /// get and set methods for the hours of the TimeStamp class
        /// </summary>
        public int Hours
        {
            get { return hours; }
            private set
            {
                if (hours >= HOURS_MIN && hours <= HOURS_MAX)
                {
                    hours = value;
                }
                else
                {
                    throw new ArgumentException(string.Format("Hours must be between {0} and {1}", HOURS_MIN, HOURS_MAX), nameof(value));
                }
            }
        }

        /// <summary>
        /// get and set methods for the minutes of the TimeStamp class
        /// </summary>
        public int Minutes
        {
            get { return minutes; }
            private set
            {
                if (minutes >= MINUTES_MIN && minutes <= MINUTES_MAX)
                {
                    minutes = value;
                }
                else
                {
                    throw new ArgumentException(string.Format("Minutes must be between {0} and {1}", MINUTES_MIN, MINUTES_MAX), nameof(value));
                }
                
            }
        }

        /// <summary>
        /// get and set methods for the seconds of the TimeStamp class
        /// </summary>
        public int Seconds
        {
            get { return seconds; }
            private set
            {
                if (seconds >= SECONDS_MIN && seconds <= SECONDS_MAX)
                {
                    seconds = value;
                }
                else
                {
                    throw new ArgumentException(string.Format("Seconds must be between {0} and {1}", SECONDS_MIN, SECONDS_MAX), nameof(value));
                }
                
            }
        }

        /// <summary>
        /// get and set methods for the milliseconds of the TimeStamp class
        /// </summary>
        public int Millis
        {
            get { return millis; }
            private set
            {
                if (millis >= MILLIS_MIN && millis <= MILLIS_MAX)
                {
                    millis = value;
                }
                else
                {
                    throw new ArgumentException(string.Format("Milliseconds must be between {0} and {1}", MILLIS_MIN, MILLIS_MAX), nameof(value));
                }
                
            }
        }

        /// <summary>
        /// get and set methods for the negative factor of the timestamp
        /// </summary>
        public bool Negative
        {
            get { return negative; }
            private set { negative = value; }
        }

        /// <summary>
        /// Method to return a timestamp under its standard format [00:00:00,000] in the form of a string
        /// </summary>
        /// <returns>returns the standardizes .srt timestamp format</returns>
        public override string ToString()
        {
            return string.Format("{0}{1}:{2}:{3},{4}", (negative ? "-" : ""), Hours.ToString("00"), Minutes.ToString("00"), Seconds.ToString("00"), Millis.ToString().PadLeft(3, '0'));
        }

        /// <summary>
        /// private method to update the index table in the creation of a timestamp from a string
        /// </summary>
        /// <param name="indexTable">the array containing the index to update</param>
        /// <param name="negativeFactor">The factor to increase/decrease the indexes</param>
        /// <returns>returns the indexTable updated according to the negativeFactor</returns>
        private static int[] UpdateIndex(int[] indexTable, int negativeFactor)
        {

            for (int i = 0; i < indexTable.Length; i++)
            {
                indexTable[i] += negativeFactor;
            }
            
            return indexTable;
        }

        /// <summary>
        /// Parser function that takes a string representation of a timestamp and converts it in an instance of the class
        /// </summary>
        /// <param name="timeStamp">string representation of a timestamp under the format ##:##:##,###</param>
        /// <returns>returns a timestamp object of the value equal to the string representation</returns>
        public static TimeStamp Parse(string timeStamp)
        {
            //temporary variables to pass the properties
            int[] timeTable = new int[4];
            bool negative;

            //index for the properties of the timestamp in the string
            int[] indexTable = { 0, 3, 6, 9 };


            //validate if timeStamp string should be nagtive and set the factor accordingly
            int negativeFactor = (negative = char.Parse(timeStamp.Substring(0, 1)) == '-') ? 1 : 0;

            //update indexTable
            if (negativeFactor != 0)
            {
                UpdateIndex(indexTable, negativeFactor);
            }

            //extract values
            try
            {
                for (int i = 0; i < timeTable.Length; i++)
                {
                    if (i < timeTable.Length - 1)
                    {
                        timeTable[i] = int.Parse(timeStamp.Substring(indexTable[i], 2));
                    }
                    else
                    {
                        timeTable[i] = int.Parse(timeStamp.Substring(indexTable[i]));
                    }
                }
            }
            catch (Exception)
            {

                throw new ArgumentException("Invalid timeStamp string format");
            }

            //create the instance
            return new TimeStamp(timeTable[0], timeTable[1], timeTable[2], timeTable[3], negative);

        }

        /// <summary>
        /// Parser function that takes an integer value equivalent to the number of milliseconds and converts it in an instance of the class
        /// </summary>
        /// <param name="millis">duration/timestamp in milliseconds</param>
        /// <returns>returns a timestamp object of the value equal to the value in milliseconds provided</returns>
        public static TimeStamp Parse(int millis)
        {

            //determines the sign of the value and make value absolute for the rest of the calculation
            bool negative;
            if (negative = millis < 0)
            {
                millis *= -1;
            }

            int hours = millis / HOURS_TO_MILLIS;
            millis %= HOURS_TO_MILLIS;

            int minutes = millis / MINUTES_TO_MILLIS;
            millis %= MINUTES_TO_MILLIS;

            int seconds = millis / SECONDS_TO_MILLIS;
            millis %= SECONDS_TO_MILLIS;

            return new TimeStamp(hours, minutes, seconds, millis, negative);

        }

        public int ToMillis()
        {
            return ((Hours * HOURS_TO_MILLIS) + (Minutes * MINUTES_TO_MILLIS) + (Seconds * SECONDS_TO_MILLIS) + Millis) * (negative ? -1 : 1);
        }

        public static TimeStamp operator +(TimeStamp A, TimeStamp B)
        {
            int millisA = A.ToMillis();
            int millisB = B.ToMillis();

            return Parse(millisA + millisB);
        }

        public static TimeStamp operator -(TimeStamp A, TimeStamp B)
        {
            int millisA = A.ToMillis();
            int millisB = B.ToMillis();

            return Parse(millisA - millisB);
        }

        public static TimeStamp operator *(TimeStamp A, float B)
        {
            int millisA = A.ToMillis();
            millisA = (int)(millisA * B);

            return Parse(millisA);
        }

    }
}
