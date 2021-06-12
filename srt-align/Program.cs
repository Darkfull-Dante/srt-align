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
            try
            {
                string fileLocation = FileLocationFormater(args[args.Length - 1]);
            }
            catch (IndexOutOfRangeException)
            {
                Help();
                Environment.Exit(0);
            }

            //remove file location from the argument list to prepare for option settings
            Array.Resize(ref args, args.Length - 1);

            //check parameters provided
            GetOpt(args);

            if (param_Version)
            {
                Version();
                Environment.Exit(0);
            }

            if (param_Help)
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// the version page called from the argument -v or --version.
        /// </summary>
        static void Version()
        {
            Console.WriteLine("srt-align 0.1");
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
            Console.WriteLine("srt-align [--shift|-linear] [OPTIONS...] <file>\n");
            Console.WriteLine("\t-h --help\t\tShow this help");
            Console.WriteLine("\t-v --version\t\tShow package version");
        }

        /// <summary>
        /// Method that takes the file location argument provided by the user and ajusts it to an absolute path. If the argument was already an absolute path nothing is changed, but if the path was relative, the method returns an absolute path based on the working directory of the program
        /// </summary>
        /// <param name="locationArg">absolute or relative path of the srt file</param>
        /// <returns>returns an absolute path for the file</returns>
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

        /// <summary>
        /// determines which parameters were inputed by the user and turn them true
        /// </summary>
        /// <param name="paramList">list of arguments from the input of the command prompt</param>
        static void GetOpt(string[] paramList)
        {

            if (paramList == null || paramList.Length == 0)
            {
                Console.Error.WriteLine("No parameter were provided. Check srt-align --help for detail on how to use the software");
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
                    param_Override = true;
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
    }

    /*args list to implement:
        -s && --shift       -> shift correction
        -l && --linear      -> linear correction
        -o && --override    -> override file
        -h && --help        -> help file
        -v && --version     -> version check
    */
}
