using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stotki
{
    public class Player
    {
        public int Life = 19;
        public char[,] PlayerShipsMap = new char[10,10]; // Array that represents map 10*10
        public char[,] PlayerShootingMap = new char[10,10];
        
        /// <summary>
        ///     Inserting ships based on the given coordinates, from one point(x,y) to second point(x2,y2)
        /// </summary>
        /// <param name="firstX"> integer, X value of the first coordinate</param>
        /// <param name="firstY"> integer, Y value of the first coordinate</param>
        /// <param name="secondX"> integer, X value of the second coordinate</param>
        /// <param name="secondY"> integer, Y value of the second coordinate</param>
        public void ShipPlacementFilter(int firstY, int firstX, int secondY, int secondX)
        {
            if (firstX != secondX && firstY==secondY) // Checking if iteration should be Horizontal
            {
                int firstCoordinate = firstX < secondX 
                    ? firstX : secondX;
                int secondCoordinate = firstX < secondX 
                    ? secondX : firstX;
                ShipPlacementIteration(firstCoordinate, secondCoordinate, firstY, 'y');
            }
            else if (firstY != secondY && secondX == firstX) // Checking if iteration should be Vertical
            {
                int firstCoordinate = firstY < secondY 
                    ? firstY : secondY;
                int secondCoordinate = firstY < secondY 
                    ? secondY : firstY;
                ShipPlacementIteration(firstCoordinate, secondCoordinate, firstX, 'x');
            }
        }
        
        /// <summary>
        ///     Iteration over given coordinates in map and inserting ships as #
        /// </summary>
        /// <param name="firstCoord">First coord to begin iteration</param>
        /// <param name="secondCoord">Second coord to begin iteration</param>
        /// <param name="iterationOver">coordinate to iterate over</param>
        /// <param name="iterationSign"></param>
        private void ShipPlacementIteration(int firstCoord, int secondCoord, int iterationOver, char iterationSign)
        {
            for (int i = firstCoord; i <= secondCoord; i++)
            {
                if (iterationSign == 'y')
                    PlayerShipsMap[i, iterationOver] = '#';
                else
                    PlayerShipsMap[iterationOver, i] = '#';
            }
        }

        /// <summary>
        ///     Prints colored Maps in terminal and marks last shot
        /// </summary>
        /// <param name="mapString">String representing Maps</param>
        /// <param name="xCoordinate">First coord of last shot</param>
        /// <param name="yCoordinate">Second coord of last shot</param>
        private void ColorMap(string mapString, int xCoordinate, int yCoordinate)
        {
            char[] maps = mapString.ToCharArray();

            foreach (char character in maps)
            {
                if(character == '#') Console.ForegroundColor = ConsoleColor.Yellow;
                if(character == 'o') // Shot
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if(character == 'O') Console.ForegroundColor = ConsoleColor.Blue;
                if(character == 'x') // Shot
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if(character == 'X') Console.ForegroundColor = ConsoleColor.Red;

                if ("#OoXx".Contains(character))
                {
                    Console.Write($" {character} ");
                    Console.ResetColor();
                }
                else if (character == '^')
                {
                    Console.Write("   ");
                }
                else
                {
                    Console.Write(character);
                }
            }

            if (xCoordinate != -1 && yCoordinate != -1)
            {
                PlayerShootingMap[xCoordinate, yCoordinate] = PlayerShootingMap[xCoordinate, yCoordinate] == 'x' ? 'X' : 'O';
            }
        }
        
        /// <summary>
        ///     Returns string representing map Display
        /// </summary>
        /// <returns>String representing Map UI</returns>
        private string PrepareMapDisplay()
        {
            ArrayList maps = new ArrayList();
            
            string top = "  | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |";
            string horizontalLine = string.Concat(Enumerable.Repeat("-", 43));

            maps.Add("\n"+string.Concat(Enumerable.Repeat(" ", 20))+"Player"+string.Concat(Enumerable.Repeat(" ", 18)));
            maps.Add(string.Concat(Enumerable.Repeat(" ", 19))+"Enemy");
            maps.Add("\n"+top);
            maps.Add(top);
            maps.Add("\n"+horizontalLine);
            maps.Add(horizontalLine);
            
            for (int row = 0; row < 10; row++)
            {
                string playerMap = $"\n{row} |";
                string shootingMap = $"{row} |";
                
                for (int column = 0; column < 10; column++)
                {
                    playerMap += PlayerShipsMap[row, column] == '\0' ? "^|" : $"{PlayerShipsMap[row, column]}|";
                    shootingMap += PlayerShootingMap[row, column] == '\0' ? "^|" : $"{PlayerShootingMap[row, column]}|";
                }
                maps.Add(playerMap);
                maps.Add(shootingMap);
                maps.Add("\n"+horizontalLine);
                maps.Add(horizontalLine);
            }

            maps.Add("\n");
            
            return String.Join(string.Concat(Enumerable.Repeat(" ", 10)), maps.ToArray());
        }
        
        /// <summary>
        ///     Displays maps UI
        /// </summary>
        /// <param name="xCoordinate">X coord of last shot</param>
        /// <param name="yCoordinate">Y coord of last shot</param>
        public void ShowMaps(int xCoordinate=-1, int yCoordinate=-1)
        {
            string maps = PrepareMapDisplay();
            ColorMap(maps, xCoordinate, yCoordinate);
        }
    }

    public class BattleShipsGame
    {
        private static readonly IDictionary<string, int> ShipsValues = new Dictionary<string, int>() {
            {"Carrier", 5},
            {"Battleship", 4},
            {"Submarine1", 3},
            {"Submarine2", 3},
            {"Destroyer1", 2},
            {"Destroyer2", 2}
        };
        
        static readonly Player FirstPlayerClass = new Player();
        static readonly Player SecondPlayerClass = new Player();
        
        static void Main(string[] args)
        {
            Console.WriteLine("PLAYER 1 SETTING SHIPS");
            string cheat1 = Console.ReadLine();
            Console.Clear();
            if (cheat1 == "cheat")
            {
                CheatShipsGenerator("Player1");
            }    
            else
            {
                ShipPlacement("Player1");
            }
            
            FirstPlayerClass.ShowMaps();
            Console.WriteLine("END TURN");
            Console.ReadLine();
            Console.Clear();
            
            Console.WriteLine("PLAYER 2 SETTING SHIPS");
            string cheat2 = Console.ReadLine();
            Console.Clear();
            if (cheat2 == "cheat")
            {
                CheatShipsGenerator("Player2");
            }
            else
            {
                ShipPlacement("Player2");
            }
            
            SecondPlayerClass.ShowMaps();
            Console.WriteLine("END TURN");
            Console.ReadLine();
            Console.Clear();
            
            do
            {
                Console.WriteLine("PLAYER 1 IS SHOOTING");
                if (Console.ReadLine() == "ween")
                    break;
                Console.Clear();
                
                FirstPlayerClass.ShowMaps();
                UserShootingInput(out int xShootInput, out int yShootInput);
                Console.Clear();
                PlayerShoot(xShootInput,yShootInput,"Player1");
                
                Console.WriteLine("END TURN");
                Console.ReadLine();
                Console.Clear();
                
                Console.WriteLine("PLAYER 2 IS SHOOTING");
                Console.ReadLine();
                Console.Clear();
                
                SecondPlayerClass.ShowMaps();
                UserShootingInput(out xShootInput, out yShootInput);
                Console.Clear();
                PlayerShoot(xShootInput,yShootInput,"Player2");
                
                Console.WriteLine("END TURN");
                Console.ReadLine();
                Console.Clear();
                
            } while (FirstPlayerClass.Life != 0 || SecondPlayerClass.Life != 0);
            Console.WriteLine("GG EZ WEENz");
        }
        
        /// <summary>
        ///     Universal method for calling methods responsible for placing ships as and argument takes player
        /// </summary>
        static void ShipPlacement(string player)
        {
            foreach (KeyValuePair<string, int> ship in ShipsValues)
            {
                if (player == "Player1")
                {
                    FirstPlayerClass.ShowMaps();
                }
                else if (player == "Player2")
                {
                    SecondPlayerClass.ShowMaps();
                }
                UserShipPlacementTurn(out int[] firstCoords, out int[] secondCoords, ship.Value, player);Console.Clear();
                if (player == "Player1")
                {
                    FirstPlayerClass.ShipPlacementFilter(firstCoords[0], firstCoords[1], secondCoords[0], secondCoords[1]);
                }
                else if (player == "Player2")
                {
                    SecondPlayerClass.ShipPlacementFilter(firstCoords[0], firstCoords[1], secondCoords[0], secondCoords[1]);
                }
            }
        }
        
        /// <summary>
        ///     Input for ship placement
        /// </summary>
        /// <returns></returns>
        static void UserShipPlacementTurn(out int[] firstCoords, out int[] secondCoords, int shipLen, string player)
        {
            string lenValid;
            int firstCoord = -1;
            int secondCoord = -1;
            do
            {
                string stringinputBeginCoords;
                string singlevalidation;
                do
                {
                    Console.WriteLine($"Please enter where your {shipLen} field ship will BEGIN e.g.: (1,3) ");
                    stringinputBeginCoords = Console.ReadLine();
                    singlevalidation =
                        UserCoordsPairValidation(stringinputBeginCoords, out firstCoord, out secondCoord);

                    if (singlevalidation == "Wrong!")
                    {
                        Console.WriteLine(
                            "Wrong Input, please make sure ship beginning coordinates are correct :v");
                    }
                } while (singlevalidation != "Correct!");

                string stringinputEndCoords;
                do
                {
                    Console.WriteLine($"Please enter where your {shipLen} field ship will END e.g.: (1,3) ");
                    stringinputEndCoords = Console.ReadLine();
                    singlevalidation = UserCoordsPairValidation(stringinputEndCoords, out firstCoord, out secondCoord);

                    if (singlevalidation == "Wrong!")
                    {
                        Console.WriteLine(
                            "Wrong Input, please make sure ship end coordinates are correct :v");
                    }
                } while (singlevalidation != "Correct!");

                lenValid = UserShipPlacementValidation(stringinputBeginCoords + "/" + stringinputEndCoords,
                    out firstCoords, out secondCoords, shipLen, player);
                
                if (lenValid == "Wrong Len!")
                {
                    Console.WriteLine(
                        $"Wrong Ship Length, you need to make ({shipLen}) :v");
                }

                if (lenValid == "Ship Nearby!")
                {
                    Console.WriteLine("You have already placed ship nearby :v");
                }
                
            } while (lenValid != "Correct!");
        }
        
        /// <summary>
        ///     Validation to user input when placing ships
        /// </summary>
        /// <returns></returns>
        static string UserShipPlacementValidation(string userShipInput, out int[] firstCoords, out int[] secondCoords, int shipLen, string player)
        {
            string[] splitUserShipInput = userShipInput.Split("/");
            string[] beginCommaCoords = splitUserShipInput[0].Split(",");
            string[] endCommaCoords = splitUserShipInput[1].Split(",");

            firstCoords = Array.ConvertAll(beginCommaCoords, int.Parse);
            secondCoords = Array.ConvertAll(endCommaCoords, int.Parse);

            if (firstCoords[0] == secondCoords[0])
            {
                string valid = PlacingValidationIterationY(firstCoords, secondCoords, player, shipLen);
                if (valid != "Correct!")
                    return valid;
            }
            else if (firstCoords[1] == secondCoords[1])
            {
                string valid = PlacingValidationIterationX(firstCoords, secondCoords, player, shipLen);
                if (valid != "Correct!")
                    return valid;
            }
            return "Correct!";
        }

        static string PlacingValidationIterationX(int[] firstCoords, int[] secondCoords, string player, int shipLen)
        {
            if (Math.Abs(firstCoords[0] - secondCoords[0]) != shipLen-1)
            {
                return "Wrong Len!";
            }
                
            if (player == "Player1")
            {
                for (int i = firstCoords[0]; i < secondCoords[0]; i++)
                {
                    try
                    {
                        if (FirstPlayerClass.PlayerShipsMap[firstCoords[1] + 1, i] == '#' ||
                            FirstPlayerClass.PlayerShipsMap[firstCoords[1] - 1, i] == '#')
                            return "Ship Nearby!";
                    }
                    catch (IndexOutOfRangeException)
                    {
                        int dupa = 0;
                    }
                }
            }
            else if (player == "Player2")
            {
                for (int i = firstCoords[0]; i < secondCoords[0]; i++)
                {
                    try
                    {
                        if (SecondPlayerClass.PlayerShipsMap[firstCoords[1] + 1, i] == '#' ||
                            SecondPlayerClass.PlayerShipsMap[firstCoords[1] - 1, i] == '#')
                            return "Ship Nearby!";
                    }
                    catch (IndexOutOfRangeException)
                    {
                        int dupa = 0;
                    }
                }
            }
            return "Correct!";
        }

        static string PlacingValidationIterationY(int[] firstCoords, int[] secondCoords, string player, int shipLen)
        {
            if (Math.Abs(firstCoords[1] - secondCoords[1]) != shipLen-1)
            {
                return "Wrong Len!";
            }
                
            if (player == "Player1")
            {
                for (int i = firstCoords[1]; i < secondCoords[1]; i++)
                {
                    try
                    {
                        if (FirstPlayerClass.PlayerShipsMap[i, firstCoords[0] + 1] == '#' ||
                            FirstPlayerClass.PlayerShipsMap[i, firstCoords[0] - 1] == '#')
                            return "Ship Nearby!";
                    }
                    catch (IndexOutOfRangeException)
                    {
                        int dupa = 0;
                    }
                }
            }
            else if (player == "Player2")
            {
                for (int i = firstCoords[1]; i < secondCoords[1]; i++)
                {
                    try
                    {
                        if (SecondPlayerClass.PlayerShipsMap[i, firstCoords[0] + 1] == '#' ||
                            SecondPlayerClass.PlayerShipsMap[i, firstCoords[0] - 1] == '#')
                            return "Ship Nearby!";
                    }
                    catch (IndexOutOfRangeException)
                    {
                        int dupa = 0;
                    }
                }
            }
            return "Correct!";
        }

        /// <summary>
        ///     Method responsible for getting user input for shooting
        /// </summary>
        /// <param name="userInputX"> X coord</param>
        /// <param name="userInputY"> Y coord</param>
        static void UserShootingInput(out int userInputX, out int userInputY)
        {
            string validation;
            int firstShootingCoord;
            int secondShootingCoord;
            do
            {
                Console.WriteLine("Please enter Shooting Coords separated by ',' e.g.: (1,3) ");
                string stringinputCoords = Console.ReadLine();
                validation = UserCoordsPairValidation(stringinputCoords, out firstShootingCoord, out secondShootingCoord);
                if (validation == "Wrong!")
                    Console.WriteLine("Please provide Correct input :/");
            } while (validation == "Wrong!");

            userInputX = firstShootingCoord;
            userInputY = secondShootingCoord;
        }

        /// <summary>
        ///     Saves player shot from given coordinates
        /// </summary>
        /// <param name="xCoordinate"> X coordinate of shot</param>
        /// <param name="yCoordinate"> Y coordinate of shot</param>
        /// <param name="player">String showing who is shooting</param>
        /// <returns></returns>
        static void PlayerShoot(int yCoordinate, int xCoordinate, string player)
        {
            if (player == "Player1")
            {
                FirstPlayerClass.PlayerShootingMap[xCoordinate, yCoordinate] =
                    SecondPlayerClass.PlayerShipsMap[xCoordinate, yCoordinate] == '#'
                        ? 'x'
                        : 'o';
                
                SecondPlayerClass.PlayerShipsMap[xCoordinate, yCoordinate] =
                    SecondPlayerClass.PlayerShipsMap[xCoordinate, yCoordinate] == '#'
                        ? 'X'
                        : '\0';
                SecondPlayerClass.Life -= 1;

                FirstPlayerClass.ShowMaps(xCoordinate, yCoordinate);
            }
            else if (player == "Player2")
            {
                SecondPlayerClass.PlayerShootingMap[xCoordinate, yCoordinate] =
                    FirstPlayerClass.PlayerShipsMap[xCoordinate, yCoordinate] == '#'
                        ? 'x'
                        : 'o';
                
                FirstPlayerClass.PlayerShipsMap[xCoordinate, yCoordinate] =
                    FirstPlayerClass.PlayerShipsMap[xCoordinate, yCoordinate] == '#'
                        ? 'X'
                        : '\0';
                FirstPlayerClass.Life -= 1;
                
                SecondPlayerClass.ShowMaps(xCoordinate, yCoordinate);
            }
        }

        /// <summary>
        ///     Validating of the given user input
        /// </summary>
        /// <param name="stringinputCoords"> User input in string</param>
        /// <param name="firstCoord"></param>
        /// <param name="secondCoord"></param>
        /// <returns></returns>
        static string UserCoordsPairValidation(string stringinputCoords, out int firstCoord, out int secondCoord)
        {
            firstCoord = -1;
            secondCoord = -1;

            string[] splitInputCoords = stringinputCoords.Split(',');
            if (splitInputCoords.Length != 2)
                return "Wrong!";

            bool firstCoordCheck = int.TryParse(splitInputCoords[0], out firstCoord);
            bool secondCoordCheck = int.TryParse(splitInputCoords[1], out secondCoord);

            if (!firstCoordCheck || !secondCoordCheck || firstCoord > 10 || secondCoord > 10)
                return "Wrong!";
            
            return "Correct!";
        }

        static void CheatShipsGenerator(string player)
        {
            if (player == "Player1")
            {
                FirstPlayerClass.PlayerShipsMap = new char[,]
                {
                    {'#', '#', '#', '\0', '#', '#', '#', '#', '#', '#'},
                    {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0'},
                    {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0'},
                    {'#', '#', '#', '#', '\0', '\0', '\0', '\0', '\0', '\0'},
                    {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0'},
                    {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '#', '\0'},
                    {'\0', '\0', '#', '\0', '#', '#', '\0', '\0', '#', '\0'},
                    {'\0', '\0', '#', '\0', '\0', '\0', '\0', '\0', '#', '\0'},
                    {'\0', '\0', '#', '\0', '\0', '\0', '\0', '\0', '\0', '\0'},
                    {'\0', '\0', '#', '\0', '\0', '#', '#', '#', '#', '#'},
                };
            }
            else if (player == "Player2")
            {
                SecondPlayerClass.PlayerShipsMap = new char[,]
                {
                    {'#', '#', '#', '\0', '#', '#', '#', '#', '#', '#'},
                    {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0'},
                    {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0'},
                    {'#', '#', '#', '#', '\0', '\0', '\0', '\0', '\0', '\0'},
                    {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0'},
                    {'\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '#', '\0'},
                    {'\0', '\0', '#', '\0', '#', '#', '\0', '\0', '#', '\0'},
                    {'\0', '\0', '#', '\0', '\0', '\0', '\0', '\0', '#', '\0'},
                    {'\0', '\0', '#', '\0', '\0', '\0', '\0', '\0', '\0', '\0'},
                    {'\0', '\0', '#', '\0', '\0', '#', '#', '#', '#', '#'},
                };
            }
        }
    }
}