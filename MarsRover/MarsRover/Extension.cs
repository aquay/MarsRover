using System.Linq;
using static MarsRover.Enumeration;

namespace MarsRover
{
    public static class Extension
    {
        public static bool IsFieldCommand(this string input)
        {
            return input.ToCharArray().Select(x => x.ToString()).All(x => int.TryParse(x, out _) || x == " ");
        }

        public static bool IsDeployCommand(this string input)
        {
            return input.ToCharArray().Select(x => x.ToString()).All(x => int.TryParse(x, out _) || x == " " || x == "W" || x == "N" || x == "E" || x == "S");
        }

        public static bool IsMoveCommand(this string input)
        {
            return input.ToCharArray().All(x => x == 'L' || x == 'R' || x == 'M');
        }

        public static bool IsNumeric(this char input)
        {
            return int.TryParse(input.ToString(), out _);
        }

        public static RoverPosition Turn(this RoverPosition input, char subCommand)
        {

            switch (subCommand)
            {
                case 'L':
                    if (input == RoverPosition.West)
                    {
                        return RoverPosition.South;
                    }
                    else
                    {
                        return (RoverPosition)(input - 1);
                    }

                case 'R':
                    if (input == RoverPosition.South)
                    {
                        return RoverPosition.West;
                    }
                    else
                    {
                        return (RoverPosition)(input + 1);
                    }
            }

            return RoverPosition.None;
        }

        public static RoverPosition Deploy(this char input)
        {
            switch (input)
            {
                case 'N':
                    return RoverPosition.North;

                case 'S':
                    return RoverPosition.South;

                case 'W':
                    return RoverPosition.West;

                case 'E':
                    return RoverPosition.East;
            }

            return RoverPosition.None;
        }

        public static char ToChar(this RoverPosition input)
        {
            switch (input)
            {
                case RoverPosition.North:
                    return 'N';

                case RoverPosition.South:
                    return 'S';

                case RoverPosition.West:
                    return 'W';

                case RoverPosition.East:
                    return 'E';
            }

            return '?';
        }
    }
}