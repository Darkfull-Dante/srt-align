using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security;

namespace srt_align
{
    class Program
    {

        static TimeStamp param_Linear;
        static float param_Shift = 99999;
        static bool param_Overwrite = false;
        static bool param_Version = false;
        static bool param_Help = false;
        static string VERSION = "0.6.1";

        const float DEFAULT_SHIFT_VALUE = 99999;

        /// <summary>
        /// srt-align entry point
        /// </summary>
        /// <param name="args">list of arguments provided by user in console</param>
        static void Main(string[] args)
        {

            int argsCount = args.Length;
            string input = "";
            string output = "";
            int removeFromArray = 0;

            //check how many filepath argument where provided
            for (int i = argsCount; i --> 0;)
            {

                string currentArg = args[i];

                if (currentArg[0] != '-')
                {
                    if (Path.GetExtension(currentArg) == ".srt")
                    {
                        removeFromArray++;
                    }
                    else
                    {
                        Console.Error.WriteLine("File provided is invalid. The extension must be an .srt file");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    break;
                }
            }

            //check the arguments for input and output
            if (removeFromArray == 1)
            {
                input = FileLocationFormater(args[argsCount - 1], true);
                output = FileLocationFormater(Path.GetFileNameWithoutExtension(input) + "-edited" + Path.GetExtension(input), false);
            }
            else if (removeFromArray == 2)
            {
                input = FileLocationFormater(args[argsCount - 2], true);
                output = FileLocationFormater(args[argsCount - 1], false);
            }
   
            //remove file location from the argument list to prepare for option settings
            Array.Resize(ref args, argsCount - removeFromArray);

            //check parameters provided
            GetOpt(args);

            //make output equal to input if --overwrite is on
            if (param_Overwrite)
            {
                output = input;
            }

            if (param_Version)
            {
                Version();
                Environment.Exit(0);
            }

            if (param_Help)
            {
                Help();
                Environment.Exit(0);
            }

            char mode = ModeSelect(param_Linear, param_Shift);
            List<SubtitleElement> subtitlesList = new List<SubtitleElement>();

            if (mode == 'l' || mode == 's')
            {
                subtitlesList = Utils.SrtRead(input);
            }

            //make the update to the file according to the selection
            switch (mode)
            {
                case 'l':
                    LinearUpdate(ref subtitlesList, param_Linear);
                    break;

                case 's':
                    ShiftUpdate(ref subtitlesList, param_Shift);
                    break;

                case 'n':
                    Console.Error.WriteLine("No alignment method was selected. Please check srt-align --help for more details");
                    Environment.Exit(0);
                    break;

                case 'b':
                    Console.Error.WriteLine("Both alignment methods were selected at the same time. Please check srt-align --help for more details");
                    Environment.Exit(0);
                    break;
                
                default:
                    Console.Error.WriteLine("An error occured. Please verify srt-align --help for proper uses");
                    Environment.Exit(0);
                    break;
            }

            //save the file
            Utils.SrtWrite(subtitlesList, output);

            //message to confirm the edit worked
            Console.WriteLine("{0} was edited succesfully. {1} subtitle elements were edited.\n\nThe file is available under {2}", input, subtitlesList.Count, output);

        }

        /// <summary>
        /// the version page called from the argument -v or --version.
        /// </summary>
        static void Version()
        {
            Console.WriteLine("srt-align " + VERSION);
            Console.WriteLine("Copyright (C) 2021 Felix Cusson");
            Console.WriteLine("Licence GPLv3+: GNU GPL version 3 or later <https://gnu.org/licenses/gpl.html>");
            Console.WriteLine("This is free software: you are free to change and redistribute it.");
            Console.WriteLine("There is NO WARANTY, to the extent permitted by law.");
        }

        /// <summary>
        /// the help paged called from the argument -h or --help. Is also shown when no arguments where provided
        /// </summary>
        static void Help()
        {
            Console.WriteLine("srt-align {--shift|--linear} [OPTIONS...] <input> [output]\n");
            Console.WriteLine("Options:");
            Console.WriteLine("\t-h --help\t\tShow this help");
            Console.WriteLine("\t-v --version\t\tShow package version");
            Console.WriteLine("\t-s --shift=RATIO\tShifts the timestamp of a file based on a mutiple ratio (floating point value) where 1\n\t\t\t\tmeans no change.");
            Console.WriteLine("\t-l --linear=TIMESTAMP\tIncrease or decrease the timestamp value of a file based on a timestamp provided. The\n\t\t\t\ttimestamp must be in a valid format for srt file ([-]##:##:##,###)");
            Console.WriteLine("\t-o --overwrite\t\tTells the program to overwrite the input file that was provided.\n");
            Console.WriteLine("Input/Output values:");
            Console.WriteLine("\t<input>\t\tThe input srt file that must be provided.");
            Console.WriteLine("\t[output]\t\tThe ouptut file for the srt file after modification. This item is optional. If this\n\t\t\t\tvalue is ommited, the file will be named after the input name with \"-edited\" at the\n\t\t\t\tend. The file will be placed in the input file directory.");


        }

        /// <summary>
        /// Method that takes the file location argument provided by the user and ajusts it to an absolute path. If the argument was already an absolute path nothing is changed, but if the path was relative, the method returns an absolute path based on the working directory of the program
        /// </summary>
        /// <param name="locationArg">absolute or relative path of the srt file</param>
        /// <returns>returns an absolute path for the file</returns>
        static string FileLocationFormater(string locationArg, bool mustExist)
        {
            string fullPath = "";


            try
            {
                fullPath = Path.GetFullPath(locationArg);

                
                if (!HasWriteAccessToFolder(fullPath))
                {
                    throw new UnauthorizedAccessException(string.Format("Unauthorized access to {0}.", fullPath));
                }

                
            }
            catch (FileNotFoundException e)
            {

                if (mustExist)
                {
                    Console.Error.WriteLine("{0}", e.Message);
                    Environment.Exit(0);
                }
                
            }
            catch (UnauthorizedAccessException e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(0);
            }

            return fullPath;

        }

        /// <summary>
        /// determines which parameters were inputed by the user and turn them true
        /// </summary>
        /// <param name="paramList">list of arguments from the input of the command prompt</param>
        static void GetOpt(string[] paramList)
        {

            if (paramList == null || paramList.Length == 0)
            {
                Console.Error.WriteLine("No parameter were provided. Check srt-align --help for detail on how to use the software\n");
                Help();
                Environment.Exit(0);
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
                    Console.Error.WriteLine(string.Format("Invalid parameter. {0} was not a recognised option. Check srt-align --help for detail on how to use the software", paramList[i]));
                    Environment.Exit(0);
                }
                
            }
        }

        /// <summary>
        /// manages the long option paramaters
        /// </summary>
        /// <param name="longOpt">a long option provided by the getopt method</param>
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
                        Console.Error.WriteLine("No value were provided for --shift. A floating point value must be provided");
                        Environment.Exit(0);
                    }

                    catch (Exception)
                    {
                        Console.Error.WriteLine("Time shift provided is not a valid number. A floating point value must be provided for shift alignment");
                        Environment.Exit(0);
                    };
                    break;

                case "linear":
                    try
                    {
                        param_Linear = TimeStamp.Parse(optionArray[1]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.Error.WriteLine("No value were provided for --linear. A floating point value must be provided");
                        Environment.Exit(0);
                    }

                    catch (Exception)
                    {
                        Console.Error.WriteLine("Time stamp provided is not a valid number. A valid timeStamp in the format in the form of \"00:00:00,000\" must be provided for linear alignment");
                        Environment.Exit(0);
                    };
                    break;

                case "overwrite":
                    param_Overwrite = true;
                    break;

                case "version":
                    param_Version = true;
                    break;

                case "help":
                    param_Help = true;
                    break;

                default:
                    Console.Error.WriteLine(string.Format("Invalid parameter. {0} was not a recognised option. Check srt-align --help for detail on how to use the software", optionArray[0]));
                    Environment.Exit(0);
                    break;
            }
        }

        /// <summary>
        /// manages the short option parameters
        /// </summary>
        /// <param name="shortOpt">a short option provided by the getopt method</param>
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
                        Console.Error.WriteLine("No value were provided for -s. A floating point value must be provided");
                        Environment.Exit(0);
                    }

                    catch (Exception)
                    {
                        Console.Error.WriteLine("Time shift provided is not a valid number. A floating point value must be provided for shift alignment");
                        Environment.Exit(0);
                    }
                    break;

                case "l":
                    try
                    {
                        param_Linear = TimeStamp.Parse(optionArray[1]);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.Error.WriteLine("No value were provided for -l. A floating point value must be provided");
                        Environment.Exit(0);
                    }

                    catch (Exception)
                    {
                        Console.Error.WriteLine("Time stamp provided is not a valid number. A valid timeStamp in the format in the form of \"00:00:00,000\" must be provided for linear alignment");
                        Environment.Exit(0);
                    };
                    break;

                case "o":
                    param_Overwrite = true;
                    break;

                case "v":
                    param_Version = true;
                    break;

                case "h":
                    param_Help = true;
                    break;

                default:
                    Console.Error.WriteLine(string.Format("Invalid parameter. {0} was not a recognised option. Check srt-align --help for detail on how to use the software", optionArray[0]));
                    Environment.Exit(0);
                    break;
            }
        }

        /// <summary>
        /// method that check which 
        /// </summary>
        /// <param name="linear">the value in param_linear</param>
        /// <param name="shift">the value in param_shift</param>
        /// <returns>returns a character to represent the mode of modification 'l' for linear, 's'. Two error case or present 'n' for none and 'b' for both for shift 'e' for error in any other case</returns>
        static char ModeSelect(TimeStamp linear, float shift)
        {
            char result = 'e';

            if (linear != null && shift == DEFAULT_SHIFT_VALUE)
            {
                result = 'l';
            }
            else if (linear == null && shift != DEFAULT_SHIFT_VALUE)
            {
                result = 's';
            }
            else if (linear == null && shift == DEFAULT_SHIFT_VALUE)
            {
                result = 'n';
            }
            else if (linear != null && shift != DEFAULT_SHIFT_VALUE)
            {
                result = 'b';
            }

            return result;
        }

        /// <summary>
        /// method to update all timestamp of the .srt file in a linear fashion
        /// </summary>
        /// <param name="list">list of subtitle element of the .srt file</param>
        /// <param name="linearValue">the linear timeshift to apply</param>
        static void LinearUpdate(ref List<SubtitleElement> list, TimeStamp linearValue)
        {
            foreach (SubtitleElement subtitle in list)
            {
                subtitle.Start += linearValue;
                subtitle.End += linearValue;
            }
        }

        /// <summary>
        /// method to update all timestamp of the .srt file in a shift fashion
        /// </summary>
        /// <param name="list">list of subtitle element of the .srt file</param>
        /// <param name="shiftValue">the multiple value to apply to the timestamps</param>
        static void ShiftUpdate(ref List<SubtitleElement> list, float shiftValue)
        {
            foreach (SubtitleElement subtitle in list)
            {
                subtitle.Start *= shiftValue;
                subtitle.End *= shiftValue;
            }
        }

        /// <summary>
        /// method to verify if the file provided is accessible to the user
        /// </summary>
        /// <param name="folderPath">the path to verify</param>
        /// <returns></returns>
        static bool HasWriteAccessToFolder(string filePath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                File.ReadAllText(filePath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
    }


}

    /*args list to implement:
        -s && --shift       -> shift correction
        -l && --linear      -> linear correction
        -o && --overwrite   -> override file
        -h && --help        -> help file
        -v && --version     -> version check
    */
