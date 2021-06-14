using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace srt_align
{
    class Utils
    {

        /// <summary>
        /// Method that reads an srt file and creates a List of the différent subtitle elements in the file
        /// </summary>
        /// <param name="fileLocation">the location of the file</param>
        /// <returns>a list of all the subtitle element under the form of a class object SubtitleElement</returns>
        static public List<SubtitleElement> SrtRead(string fileLocation)
        {
            string currentLine;
            List<SubtitleElement> result = new List<SubtitleElement>();
            int elementIndex = 0; //index of the line number of the current subtitle element
            int index = 0;
            int subtitleCount = 0;

            //create the streamReader
            using (StreamReader fileIn = new StreamReader(fileLocation))
            {
                //read the text file
                while ((currentLine = fileIn.ReadLine()) != null)
                {
                    if (currentLine != "")
                    {

                        switch (elementIndex)
                        {
                            case 0:
                                index = int.Parse(currentLine);
                                result.Add(new SubtitleElement(index));
                                break;

                            case 1:
                                result[subtitleCount].Start = TimeStamp.Parse(currentLine.Substring(0, 12));
                                result[subtitleCount].End = TimeStamp.Parse(currentLine.Substring(17, 12));
                                break;

                            default:
                                result[subtitleCount].TextList.Add(currentLine);
                                break;
                        }

                        elementIndex++;

                    }
                    else
                    {
                        elementIndex = 0;
                        subtitleCount++;
                    }
                }
            }

            return result;
        }

        static public void SrtWrite(List<SubtitleElement> list, string outputLocation)
        {
            using (StreamWriter fileOut = new StreamWriter(outputLocation, false))
            {
                foreach (SubtitleElement subtitle in list)
                {
                    fileOut.WriteLine(subtitle);
                }
            }
        }

    }
}
