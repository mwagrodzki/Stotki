using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Stotki
{
    public class Player
    {
        public int life = 19;
        public char[,] playerShipsMap = new char[10,10]; // Array that represents map 10*10
        public char[,] playerShootingMap = new char[10,10];
        
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
                    playerShipsMap[i, iterationOver] = '#';
                else
                    playerShipsMap[iterationOver, i] = '#';
            }
        }

        /// <summary>
        ///     Prints character colored depending on parameters
        /// </summary>
        /// <param name="character">Character to be printed</param>
        /// <param name="shot">Information if this was the last shot</param>
        private void PrintColored(char character, bool shot)
        {
            if(character == '#') Console.ForegroundColor = ConsoleColor.Yellow;
            if(character == 'O' && shot)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            if(character == 'O' && !shot) Console.ForegroundColor = ConsoleColor.Blue;
            if(character == 'X' && shot)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Red;
            }
            if(character == 'X' && !shot) Console.ForegroundColor = ConsoleColor.Red;
            
            Console.Write(character == '\0' ? "   " : $" {character} ");
            Console.ResetColor();
        }

        /// <summary>
        ///     Displays provided map
        /// </summary>
        /// <param name="map">Two dimensional representation of a 10x10 map</param>
        /// <param name="firstCoord">First coord of last shot</param>
        /// <param name="secondCoord">Second coord of last shot</param>
        private void MapDisplay(char[,] map, int firstCoord, int secondCoord)
        {
            string horizontalLine = string.Concat(Enumerable.Repeat("-", 43));
            Console.WriteLine();
            Console.WriteLine("  | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 |");
            Console.WriteLine(horizontalLine);
            for (int row = 0; row < 10; row++)
            {
                Console.Write($"{row} |");
                for (int column = 0; column < 10; column++)
                {
                    if (row == firstCoord && column == secondCoord)
                    {
                        PrintColored(map[row,column], true);
                    }
                    else
                    {
                        PrintColored(map[row,column], false);
                    }
                    
                    Console.Write("|");
                    
                }
                Console.WriteLine();
                Console.WriteLine(horizontalLine);
            }
        }
        
        /// <summary>
        ///     Display Playing Maps
        /// </summary>
        public void DisplayShipsMap()
        {
            MapDisplay(playerShipsMap,-1,-1);
        }

        /// <summary>
        ///     Display Shooting Map
        /// </summary>
        /// <param name="firstCoord">First coord of last shot</param>
        /// <param name="secondCoord">Second coord of last shot</param>
        public void DisplayShootingMap(int firstCoord=-1, int secondCoord=-1)
        {
            MapDisplay(playerShootingMap, firstCoord, secondCoord);
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
        
        static readonly Player firstPlayerClass = new Player();
        static readonly Player secondPlayerClass = new Player();
        
        static void Main(string[] args)
        {
            Console.WriteLine("PLAYER 1 SETTING SHIPS");
            string cheat1 = Console.ReadLine();
            if (cheat1 == "Enable Cheat Engine")
            {
                CheatShipsGenerator("Player1");
                firstPlayerClass.DisplayShipsMap();
            }    
            else
            {
                ShipPlacement("Player1");
            }
            
            Console.WriteLine("PLAYER 2 SETTING SHIPS");
            string cheat2 = Console.ReadLine();
            if (cheat2 == "Enable Cheat Engine")
            {
                CheatShipsGenerator("Player2");
                secondPlayerClass.DisplayShipsMap();
            }
            else
            {
                ShipPlacement("Player2");
            }

            do
            {
                Console.WriteLine("PLAYER 1 IS SHOOTING");
                Console.ReadLine();
                
                firstPlayerClass.DisplayShootingMap();
                UserShootingInput(out int xShootInput, out int yShootInput);
                PlayerShoot(xShootInput,yShootInput,"Player1");
                
                Console.WriteLine("PLAYER 2 IS SHOOTING");
                Console.ReadLine();
                
                secondPlayerClass.DisplayShootingMap();
                UserShootingInput(out xShootInput, out yShootInput);
                PlayerShoot(xShootInput,yShootInput,"Player2");
                
            } while (firstPlayerClass.life == 0 || secondPlayerClass.life == 0);
        }
        
        /// <summary>
        ///     Universal method for calling methods responsible for placing ships as and argument takes player
        /// </summary>
        static void ShipPlacement(string player)
        {
            foreach (KeyValuePair<string, int> ship in ShipsValues)
            {
                UserShipPlacementTurn(out int[] firstCoords, out int[] secondCoords, ship.Value, player);
                if (player == "Player1")
                {
                   firstPlayerClass.ShipPlacementFilter(firstCoords[0], firstCoords[1], secondCoords[0], secondCoords[1]);
                   firstPlayerClass.DisplayShipsMap(); 
                }
                else if (player == "Player2")
                {
                    secondPlayerClass.ShipPlacementFilter(firstCoords[0], firstCoords[1], secondCoords[0], secondCoords[1]);
                    secondPlayerClass.DisplayShipsMap(); 
                }
            }
        }
        
        /// <summary>
        ///     Input for ship placement
        /// </summary>
        /// <returns></returns>
        static void UserShipPlacementTurn(out int[] firstCoords, out int[] secondCoords, int shipLen, string player)
        {
            string stringinputBeginCoords = "";
            string stringinputEndCoords = "";
            string singlevalidation = "";
            string lenValid = "";
            int firstCoord = -1;
            int secondCoord = -1;
            stringinputBeginCoords = "";
            stringinputEndCoords = "";
            do
            {
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
                        if (firstPlayerClass.playerShipsMap[firstCoords[1] + 1, i] == '#' ||
                            firstPlayerClass.playerShipsMap[firstCoords[1] - 1, i] == '#')
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
                        if (secondPlayerClass.playerShipsMap[firstCoords[1] + 1, i] == '#' ||
                            secondPlayerClass.playerShipsMap[firstCoords[1] - 1, i] == '#')
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
                        if (firstPlayerClass.playerShipsMap[i, firstCoords[0] + 1] == '#' ||
                            firstPlayerClass.playerShipsMap[i, firstCoords[0] - 1] == '#')
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
                        if (secondPlayerClass.playerShipsMap[i, firstCoords[0] + 1] == '#' ||
                            secondPlayerClass.playerShipsMap[i, firstCoords[0] - 1] == '#')
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
            string validation = "";
            int firstShootingCoord = 0;
            int secondShootingCoord = 0;
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
        /// <returns></returns>
        static void PlayerShoot(int yCoordinate, int xCoordinate, string player)
        {
            if (player == "Player1")
            {
                secondPlayerClass.playerShipsMap[xCoordinate, yCoordinate] =
                    secondPlayerClass.playerShipsMap[xCoordinate, yCoordinate] == '#'
                        ? 'X'
                        : '\0';
                secondPlayerClass.life -= 1;

                firstPlayerClass.playerShootingMap[xCoordinate, yCoordinate] =
                    secondPlayerClass.playerShipsMap[xCoordinate, yCoordinate] == '#'
                        ? 'X'
                        : 'O';
                firstPlayerClass.DisplayShootingMap();
            }
            else if (player == "Player2")
            {
                firstPlayerClass.playerShipsMap[xCoordinate, yCoordinate] =
                    firstPlayerClass.playerShipsMap[xCoordinate, yCoordinate] == '#'
                        ? 'X'
                        : '\0';
                firstPlayerClass.life -= 1;

                secondPlayerClass.playerShootingMap[xCoordinate, yCoordinate] =
                    firstPlayerClass.playerShipsMap[xCoordinate, yCoordinate] == '#'
                        ? 'X'
                        : 'O';
                secondPlayerClass.DisplayShootingMap();
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
                firstPlayerClass.playerShipsMap = new char[,]
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
                secondPlayerClass.playerShipsMap = new char[,]
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