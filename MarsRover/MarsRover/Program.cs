using System;
using System.Collections.Generic;
using System.Linq;
using static MarsRover.Enumeration;

namespace MarsRover
{
    class Program
    {
        public static List<string> CommandSet { get; set; } = new List<string>();

        public static void Main(string[] args)
        {
            Console.WriteLine($"" +
                $"Welcome to Rover Simulator.{Environment.NewLine}" +
                $"1 - Please type upper right coordinates of the field. E.g for a 8x8 field please type '7 7' (seperated by space){Environment.NewLine}" +
                $"2 - Type iniitial position of the rover you want to deploy. E.g For deploying to  2 4 facing West please type '2 4 W'{Environment.NewLine}" +
                $"3 - Lastly type movement command set. E.g For moving rover 2 unit W and then 1 unit North, rover just deployed above should get a movemenet command set as 'MMRM'{Environment.NewLine}" +
                $"4 - Second and Third commands can be repeated unlimitedly as possible. Please type 'RUN' to process all commands.{Environment.NewLine}");


            bool again = false;
            bool isCommitted = false;
            string currentCommand = string.Empty;

            do
            {
                again = false;

                while (!isCommitted)
                {
                    currentCommand = Console.ReadLine();

                    if ((currentCommand.IsFieldCommand() && CommandSet.Count == 0) || // IsFieldCommand and First Command...
                        (currentCommand.IsDeployCommand() && CommandSet.Count % 2 == 1) || // Total # of commands is even, means next command's index will be odd. All odd indices are deploy comand. 
                        (currentCommand.IsMoveCommand() && CommandSet.Count % 2 == 0) // Total # of commands is odd, means next command's index will be even. All even indices are move comand. 
                        )
                    {
                        CommandSet.Add(currentCommand);
                    }
                    else if (currentCommand.Equals("RUN", StringComparison.OrdinalIgnoreCase))
                    {
                        ProcessComamndSet();
                    }
                    else
                    {
                        Console.WriteLine("Invalid Command!");
                    }
                }

                // Again ?
                Console.WriteLine("Start Over ? (y)");
                if (Console.ReadKey().KeyChar.ToString().Equals("y", StringComparison.OrdinalIgnoreCase))
                    again = true;

            } while (again);
        }

        public static void ProcessComamndSet()
        {
            var horizontalSize = int.Parse(CommandSet.First()[0].ToString()) + 1;
            var verticalSize = int.Parse(CommandSet.First()[2].ToString()) + 1;

            var field = new RoverPosition[horizontalSize, verticalSize];
            for (int i = 0; i < horizontalSize; i++)
            {
                for (int j = 0; j < verticalSize; j++)
                {
                    field[i, j] = RoverPosition.None;
                }
            }

            var roverCount = (CommandSet.Count - 1) / 2;

            for (int i = 1; i < roverCount * 2; i += 2)
            {
                var xPos = int.Parse(CommandSet[i].Split(" ")[0]);
                var yPos = int.Parse(CommandSet[i].Split(" ")[1]);

                if (field[xPos, yPos] == RoverPosition.None) // Spot is available deploy it!
                {
                    field[xPos, yPos] = CommandSet[i].Last().Deploy();

                    foreach (var subCommand in CommandSet[i + 1].ToCharArray())
                    {
                        if (subCommand == 'M') // Move...
                        {
                            if (field[xPos, yPos] == RoverPosition.East)
                            {
                                field[xPos, yPos] = RoverPosition.None;
                                xPos += 1;
                                field[xPos, yPos] = RoverPosition.East;
                            }
                            else if (field[xPos, yPos] == RoverPosition.West)
                            {
                                field[xPos, yPos] = RoverPosition.None;
                                xPos = xPos - 1;
                                field[xPos, yPos] = RoverPosition.West;
                            }
                            else if (field[xPos, yPos] == RoverPosition.North)
                            {
                                field[xPos, yPos] = RoverPosition.None;
                                yPos = yPos + 1;
                                field[xPos, yPos] = RoverPosition.North;
                            }
                            else if (field[xPos, yPos] == RoverPosition.South)
                            {
                                field[xPos, yPos] = RoverPosition.None;
                                yPos = yPos - 1;
                                field[xPos, yPos] = RoverPosition.South;
                            }
                        }
                        else // Turn...
                        {
                            field[xPos, yPos] = field[xPos, yPos].Turn(subCommand);
                        }
                    }
                }

                Console.WriteLine($"{xPos} {yPos} {field[xPos, yPos].ToChar()}");
            }
        }
    }
}
