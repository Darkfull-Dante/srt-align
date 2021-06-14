using System;
using System.Collections.Generic;
using System.Text;

namespace srt_align
{
    class SubtitleElement : IComparable<SubtitleElement>
    {
        //instance properties
        private int index;
        private TimeStamp start;
        private TimeStamp end;
        private List<string> textList;

        //class constructor
        /// <summary>
        /// class constructor for the subtitle element
        /// </summary>
        /// <param name="start">timestamp of when to start showing the subtitle</param>
        /// <param name="end">timestamp of when to stop showing the subtitle</param>
        /// <param name="textList">List of subtitles lines</param>
        public SubtitleElement(int index, TimeStamp start, TimeStamp end, List<string> textList)
        {
            Index = index;
            Start = start;
            End = end;
            TextList = textList;
        }

        public SubtitleElement(int index) : this(index, TimeStamp.Parse("00:00:00,000"), TimeStamp.Parse("00:00:00,000"), new List<string>()) { }

        //access methods
        public int Index
        {
            get { return index; }
            private set { index = value; }
        }

        public TimeStamp Start
        {
            get { return start; }
            set { start = value; }
        }

        public TimeStamp End
        {
            get { return end; }
            set { end = value; }
        }

        public List<string> TextList
        {
            get { return textList; }
            set { textList = value; }
        }

        /// <summary>
        /// Returns a string representing a standardised .srt file subtitle element
        /// </summary>
        /// <returns>Returns a string representing a standardised .srt file subtitle element</returns>
        public override string ToString()
        {
            string result = string.Format("{0}\n{1} --> {2}\n", Index, Start.ToString(), End.ToString());

            foreach (string lineOfText in textList)
            {
                result = result + lineOfText + "\n";
            }

            return result;
        }
        
        /// <summary>
        /// Comparison method between to subtitle element
        /// </summary>
        /// <param name="element">the subtitle element to compare with the instance of the class</param>
        /// <returns>reffer to ComparTo method</returns>
        public int CompareTo(SubtitleElement element)
        {
            if (element == null) return 1;

            return Index.CompareTo(element.Index);
        }

        /// <summary>
        /// add the string to the textList property
        /// </summary>
        /// <param name="lineOfText">the line of text to add</param>
        public void AddLineOfText(string lineOfText)
        {
            textList.Add(lineOfText);
        }

    }
}
