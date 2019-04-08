using FinchAPI;
using System;
using System.IO;

namespace CommandArray
{
    // *************************************************************
    // Title: Command Array
    // *************************************************************

    /// <summary>
    /// control commands for the finch robot
    /// </summary>
    public enum FinchCommand
    {
        DONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        DELAY,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF
    }

    class Program
    {
        static void Main(string[] args)
        {
            Finch myFinch = new Finch();

            DisplayOpeningScreen();
            DisplayInitializeFinch(myFinch);
            DisplayMainMenu(myFinch);
            DisplayClosingScreen(myFinch);
        }
        
        /// <summary>
        /// display opening screen
        /// </summary>
        static void DisplayOpeningScreen()
        {
            Console.WriteLine();
            Console.WriteLine("\tProgram Your Finch");
            Console.WriteLine();

            DisplayContinuePrompt();
        }
        
        /// <summary>
        /// display the main menu
        /// </summary>
        /// <param name="myFinch">Finch object</param>
        static void DisplayMainMenu(Finch myFinch)
        {
            string menuChoice;
            bool exiting = false;

            int delayDuration=0;
            int numberOfCommands=0;
            int motorSpeed=0;
            int LEDBrightness=0;
            FinchCommand[] commands=null;
            string[] commandsString = new string[numberOfCommands];

            while (!exiting)
            {
                //
                // display menu
                //
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Main Menu");
                Console.WriteLine();

                Console.WriteLine("\t1) Get Command Parameters");
                Console.WriteLine("\t2) Get Finch Commands");
                Console.WriteLine("\t3) Display Finch Commands");
                Console.WriteLine("\t4) Execute Finch Commands");
                Console.WriteLine("\t5) Save Finch Commands");
                Console.WriteLine("\t6) Retrieve Finch Commands");
                Console.WriteLine("\t7) Terminate Finch");
                Console.WriteLine("\tE) Exit");
                Console.WriteLine();
                Console.Write("Enter Choice:");
                menuChoice = Console.ReadLine();

                //
                // process menu
                //
                switch (menuChoice)
                {
    
                    case "1":
                        numberOfCommands = DisplayGetNumberOfCommands();
                        delayDuration = DisplayGetDelayDuration();
                        motorSpeed = DisplayGetMotorSpeed();
                        LEDBrightness = DisplayGetLEDBrightness();
                        break;
                    case "2":
                        commands = DisplayGetFinchCommands(numberOfCommands);
                        break;
                    case "3":
                        DisplayFinchCommands(numberOfCommands, commands);
                        break;
                    case "4":
                        DisplayExecuteFinchCommands(commands, myFinch, motorSpeed, LEDBrightness, delayDuration);
                        break;
                    case "5":
                        commandsString = DisplaySaveFinchCommands(commands, commandsString);
                        break;
                    case "6":
                        commands = DisplayRetrieveFinchCommands(commands, commandsString);
                        break;
                    case "7":
                        DisplayTerminateFinch(myFinch);
                        break;
                    case "e":
                    case "E":
                        exiting = true;
                        break;
                    default:
                        break;
                }
            }
        }

        static FinchCommand[] DisplayRetrieveFinchCommands(FinchCommand[] commands, string[] commandsString)
        {
            DisplayHeader("Retrieve Finch Commands");

            string datapath = "Data\\FinchCommands.txt";

            Console.WriteLine($"The data will be retrieved from {datapath}.");
            DisplayContinuePrompt();

            try
            {
                commandsString = File.ReadAllLines(datapath);
                commands = new FinchCommand[commandsString.Length];

                //
                // convert
                //
                for (int i = 0; i < commandsString.Length; i++)
                {
                    if(Enum.TryParse(commandsString[i], out commands[i]))
                    {
                        Console.WriteLine($"Command {i+1} Successfully Parsed");
                    }
                    else
                    {
                        Console.WriteLine($"Command {i+1} Parse Was Unsuccessful");
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory Not Found");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File Not Found");
            }

            DisplayContinuePrompt();

            return commands;
        }

        static string[] DisplaySaveFinchCommands(FinchCommand[] commands, string[] commandsString)
        {
            string datapath = "Data\\FinchCommands.txt";
            string data;
            commandsString = new string[commands.Length];
            DisplayHeader("Save Finch Commands");


            Console.WriteLine($"The data will be saved to {datapath}.");
            DisplayContinuePrompt();
            
            for (int i = 0; i < commands.Length; i++)
            {
                commandsString[i] = commands[i].ToString();
            }

            File.WriteAllLines(datapath, commandsString);
            Console.WriteLine("ALL COMMANDS SAVED");

            DisplayContinuePrompt();

            return commandsString;
        }

        #region Get Command Parameters
        /// <summary>
        /// get the number of commands from the user
        /// </summary>
        /// <returns>number of commands</returns>
        static int DisplayGetNumberOfCommands()
        {
            int numberOfCommands;
            string userResponse;
            bool validInput;

            do
            {
                validInput = true;

                DisplayHeader("Number of Commands");

                Console.Write("Enter the number of commands:");
                userResponse = Console.ReadLine();
                    
                if (!int.TryParse(userResponse, out numberOfCommands))
                {
                    Console.WriteLine("Input Invalid. Try Again.");
                    DisplayContinuePrompt();
                    validInput = false;
                }
            } while (!validInput);

            return numberOfCommands;
        }

        static int DisplayGetDelayDuration()
        {
            int delayDuration;
            string userInput;

            bool validInput;

            do
            {
                DisplayHeader("Length of Delay");

                Console.Write("Enter length of delay (milliseconds): ");
                userInput = Console.ReadLine();
                
                validInput = true;
                
                if (!int.TryParse(userInput, out delayDuration))
                {
                    Console.WriteLine("Input Invalid. Try Again.");
                    DisplayContinuePrompt();
                    validInput = false;
                }
            } while (!validInput);
            // delayDuration = Convert.ToInt32(userInput);
            // int.TryParse(Console.ReadLine(), out delayDuration);
            // int.TryParse(userInput, out delayDuration);

            DisplayContinuePrompt();

            return delayDuration;
        }

        /// <summary>
        /// get the motor speed from the user
        /// </summary>
        /// <returns>number of commands</returns>
        static int DisplayGetMotorSpeed()
        {
            int motorSpeed;
            string userResponse;
            bool validInput;

            do
            {

                DisplayHeader("Motor Speed");

                Console.Write("Enter the motor speed [1 - 255]:");
                userResponse = Console.ReadLine();
                validInput = true;
                
                if (!int.TryParse(userResponse, out motorSpeed))
                {
                    Console.WriteLine("Input Invalid. Try Again.");
                    DisplayContinuePrompt();
                    validInput = false;
                }
            } while (!validInput);

            return motorSpeed;
        }

        /// <summary>
        /// get the LED Brightness from the user
        /// </summary>
        /// <returns>number of commands</returns>
        static int DisplayGetLEDBrightness()
        {
            int LEDBrightness;
            string userResponse;

            bool validInput;

            do
            {
                DisplayHeader("LED Brightness");

                Console.Write("Enter brightness for the LED [1 - 255]:");
                userResponse = Console.ReadLine();

                validInput = true;
                
                if (!int.TryParse(userResponse, out LEDBrightness))
                {
                    Console.WriteLine("Input Invalid. Try Again.");
                    DisplayContinuePrompt();
                    validInput = false;
                }
            } while (!validInput);

            return LEDBrightness;
        }
        #endregion

        #region Finch Commands
        static FinchCommand[] DisplayGetFinchCommands(int numberOfCommands)
        {
            FinchCommand[] commands = new FinchCommand[numberOfCommands];

            DisplayHeader("Get Finch Commands");
            
            for (int i = 0; i < numberOfCommands; i++)
            {
                Console.Write($"Command {i+1}: ");
                Enum.TryParse(Console.ReadLine().ToUpper(), out commands[i]);
            }

            Console.WriteLine();
            Console.WriteLine("The Commands:");
            for (int i = 0; i < numberOfCommands; i++)
            {
                Console.WriteLine($"Command {i + 1}: {commands[i]}");

            }

            DisplayContinuePrompt();

            return commands;
        }

        static FinchCommand[] DisplayFinchCommands(int numberOfCommands, FinchCommand[] commands)
        {
            DisplayHeader("Display Finch Commands");
            
            Console.WriteLine("The Commands:");
            for (int i = 0; i < numberOfCommands; i++)
            {
                Console.WriteLine($"Command {i + 1}: {commands[i]}");

            }

            DisplayContinuePrompt();

            return commands;
        }

        static void DisplayExecuteFinchCommands(FinchCommand[] commands, Finch finch, int motorSpeed, int LEDBrightness, int delayDuration)
        {
            DisplayHeader("Execute Finch Commands");

            Console.WriteLine("Press any key to execute commands...");
            Console.ReadKey();

            for (int i = 0; i < commands.Length; i++)
            {
                Console.WriteLine($"Command {i+1}/{commands.Length}: {commands[i]}");

                switch (commands[i])
                {
                    case FinchCommand.DONE:
                        finch.disConnect();
                        break;
                    case FinchCommand.MOVEFORWARD:
                        finch.setMotors(motorSpeed,motorSpeed);
                        break;
                    case FinchCommand.MOVEBACKWARD:
                        finch.setMotors(-motorSpeed, -motorSpeed);
                        break;
                    case FinchCommand.STOPMOTORS:
                        finch.setMotors(0, 0);
                        break;
                    case FinchCommand.DELAY:
                        finch.wait(delayDuration);
                        break;
                    case FinchCommand.TURNRIGHT:
                        finch.setMotors(motorSpeed,motorSpeed/2);
                        break;
                    case FinchCommand.TURNLEFT:
                        finch.setMotors(motorSpeed,motorSpeed/2);
                        break;
                    case FinchCommand.LEDON:
                        finch.setLED(LEDBrightness,LEDBrightness,LEDBrightness);
                        break;
                    case FinchCommand.LEDOFF:
                        finch.setLED(0,0,0);
                        break;
                    default:
                        Console.WriteLine("Function non-existent");
                        break;
                }
            }
            
            DisplayContinuePrompt();
        }
        #endregion

        #region Finch Controls
        /// <summary>
        /// initialize and confirm the finch connects
        /// </summary>
        /// <param name="myFinch"></param>
        static void DisplayInitializeFinch(Finch myFinch)
        {
            int attempts;

            DisplayHeader("Initialize the Finch");

            Console.WriteLine("Please plug your Finch Robot into the computer.");
            Console.WriteLine();
            DisplayContinuePrompt();
            attempts = 1;

            while (!myFinch.connect())
            {
                Console.Write("Attempt "+attempts+") ");
                Console.WriteLine("Please confirm the Finch Robot is connect");
                DisplayContinuePrompt();
                attempts = attempts + 1;
                Console.Clear();
            }

            FinchConnectedAlert(myFinch);
            Console.WriteLine("Your Finch Robot is now connected");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// audio notification that the finch is connected
        /// </summary>
        /// <param name="myFinch">Finch object</param>
        static void FinchConnectedAlert(Finch myFinch)
        {
            myFinch.setLED(0, 255, 0);

            for (int frequency = 17000; frequency > 100; frequency -= 100)
            {
                myFinch.noteOn(frequency);
                myFinch.wait(10);
            }

            myFinch.noteOff();
        }

        static void DisplayTerminateFinch(Finch myFinch)
        {
            Console.WriteLine("Press any key to terminate finch.");
            Console.ReadKey();

            myFinch.noteOn(800);
            myFinch.wait(500);
            myFinch.noteOff();
            myFinch.disConnect();
        }
        #endregion

        /// <summary>
        /// display closing screen and disconnect finch robot
        /// </summary>
        /// <param name="myFinch">Finch object</param>
        static void DisplayClosingScreen(Finch myFinch)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\tThank You!");
            Console.WriteLine();

            myFinch.disConnect();

            DisplayContinuePrompt();
        }
        
        #region HELPER  METHODS

        /// <summary>
        /// display header
        /// </summary>
        /// <param name="header"></param>
        static void DisplayHeader(string header)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + header);
            Console.WriteLine();
        }

        /// <summary>
        /// display the continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        #endregion
    }
}
