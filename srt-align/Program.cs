using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace srt_align
{
    class Program
    {

        static TimeStamp param_Linear;
        static float param_Shift;
        static bool param_Override = false;
        static bool param_Version = false;
        static bool param_Help = false;

        static void Main(string[] args)
        {
            //take the file location from the last argument present
            string fileLocation = FileLocationFormater(args[args.Length - 1]);

            //check parameters provided
            Array.Resize(ref args, args.Length - 1);
            GetOpt(args);
        }

        static string FileLocationFormater(string locationArg)
        {
            Regex driveLetterTest = new Regex("^[a-zA-Z]:\\$");

            //check if location argument is already an absolute path
            if (driveLetterTest.IsMatch(locationArg.Substring(0, 3)))
            {
                return locationArg;
            }

            //for a relative path check if a slash is present at the start of the string for correct formatting
            if (locationArg[0] != '\\')
            {
                locationArg = "\\" + locationArg;
            }

            //Append at the start the current working directory for a relative path
            return Directory.GetCurrentDirectory() + locationArg;
        }

        static void GetOpt(string[] paramList)
        {

            if (paramList == null)
            {
                throw new ArgumentException("No parameter were provided. Check srt-align --help for detail on how to use the software");
            }
            
            for (int i = 0; i < paramList.Length; i++)
            {
                if (paramList[i].StartsWith("--"))
                {
                    LongOpt(paramList[i].Substring(2));
                }
                else if (paramList[i].StartsWith("-"))
                {
                    ShortOpt(paramList[i].Substring(1));
                }
                else
                {
                    throw new ArgumentException(string.Format("Invalid parameter. {0} was not a recognised option. Check srt-align --help for detail on how to use the software", paramList[i]));
                }
                
            }
        }

        static void LongOpt(string longOpt)
        {
            string[] optionArray = longOpt.ToLower().Split('=');


            switch (optionArray[0])
            {
                case "shift":
                    try
                    {
                        param_Shift = float.Parse(optionArray[1]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new ArgumentOutOfRangeException("No value were provided for --shift. A floating point value must be provided");
                    }

                    catch (Exception)
                    {
                        throw new Exception("Time shift provided is not a valid number. A floating point value must be provided for shift alignment");
                    };
                    break;

                case "linear":
                    try
                    {
                        param_Linear = TimeStamp.Parse(optionArray[1]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new ArgumentOutOfRangeException("No value were provided for --linear. A floating point value must be provided");
                    }

                    catch (Exception)
                    {
                        throw new Exception("Time stamp provided is not a valid number. A valid timeStamp in the format in the form of \"00:00:00,000\" must be provided for linear alignment");
                    };
                    break;

                case "override":
                    param_Override = true;
                    break;

                case "version":
                    param_Version = true;
                    break;

                case "help":
                    param_Help = true;
                    break;

                default:
                    throw new ArgumentException(string.Format("Invalid parameter. {0} was not a recognised option. Check srt-align --help for detail on how to use the software", optionArray[0]));
            }
        }

        static void ShortOpt(string shortOpt)
        {
            string[] optionArray = shortOpt.ToLower().Split('=');


            switch (optionArray[0])
            {
                case "s":
                    try
                    {
                        param_Shift = float.Parse(optionArray[1]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new ArgumentOutOfRangeException("No value were provided for -s. A floating point value must be provided");
                    }

                    catch (Exception)
                    {
                        throw new Exception("Time shift provided is not a valid number. A floating point value must be provided for shift alignment");
                    };
                    break;

                case "l":
                    try
                    {
                        param_Linear = TimeStamp.Parse(optionArray[1]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        throw new ArgumentOutOfRangeException("No value were provided for -l. A floating point value must be provided");
                    }

                    catch (Exception)
                    {
                        throw new Exception("Time stamp provided is not a valid number. A valid timeStamp in the format in the form of \"00:00:00,000\" must be provided for linear alignment");
                    };
                    break;

                case "o":
                    param_Override = true;
                    break;

                case "v":
                    param_Version = true;
                    break;

                case "h":
                    param_Help = true;
                    break;

                default:
                    throw new ArgumentException(string.Format("Invalid parameter. {0} was not a recognised option. Check srt-align --help for detail on how to use the software", optionArray[0]));
            }
        }
    }

    /*args list to implement:
        -s && --shift       -> shift correction
        -l && --linear      -> linear correction
        -o && --override    -> override file
        -h && --help        -> help file
        -v && --version     -> version check
    */
}
