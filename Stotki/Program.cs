using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Stotki
{
    public class PlayerMaps
    {
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
                Console.WriteLine($"Placing Ship at {i},{iterationOver} "); // Checking iteration at ship placement
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
        
        static void Main(string[] args)
        {
            PlayerMaps firstPlayerClass = new PlayerMaps();
            PlayerMaps secondPlayerClass = new PlayerMaps();
            
            UserShootingInput(out int xShootingCoord, out int yShootingCoord);
            PlayerShoot(xShootingCoord, yShootingCoord, firstPlayerClass.playerShootingMap, secondPlayerClass.playerShipsMap);
            firstPlayerClass.DisplayShootingMap();

            foreach (KeyValuePair<string, int> ship in ShipsValues)
            {
                UserShipPlacementTurn(out int[] firstCoords, out int[] secondCoords, ship.Value);
                firstPlayerClass.ShipPlacementFilter(firstCoords[0], firstCoords[1], secondCoords[0], secondCoords[1]);
                firstPlayerClass.DisplayShipsMap();
            }
        }
        
        /// <summary>
        ///     Input for ship placement
        /// </summary>
        /// <returns></returns>
        static void UserShipPlacementTurn(out int[] firstCoords, out int[] secondCoords, int shipLen)
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
                    out firstCoords, out secondCoords, shipLen);
                
                if (lenValid == "Wrong Len!")
                {
                    Console.WriteLine(
                        $"Wrong Ship Length, you need to make ({shipLen}) :v");
                }
                
            } while (lenValid != "Correct!");
        }
        
        /// <summary>
        ///     Validation to user input when placing ships
        /// </summary>
        /// <returns></returns>
        static string UserShipPlacementValidation(string userShipInput, out int[] firstCoords, out int[] secondCoords, int shipLen)
        {
            string[] splitUserShipInput = userShipInput.Split("/");
            string[] beginCommaCoords = splitUserShipInput[0].Split(",");
            string[] endCommaCoords = splitUserShipInput[1].Split(",");
            
            // NEED TO MAKE VALIDATION HERE OF PLACED SHIPS
            
            firstCoords = Array.ConvertAll(beginCommaCoords, int.Parse);
            secondCoords = Array.ConvertAll(endCommaCoords, int.Parse);
            
            if (firstCoords[0] == secondCoords[0])
            {
                if (Math.Abs(firstCoords[1] - secondCoords[1]) != shipLen)
                {
                    return "Wrong Len!";
                }
            }
            else if (firstCoords[1] == secondCoords[1])
            {
                if (Math.Abs(firstCoords[0] - secondCoords[0]) != shipLen)
                {
                    return "Wrong Len!";
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
        /// <param name="player1ShootingMap"></param>
        /// <param name="player2ShipsMap"></param>
        /// <returns></returns>
        static void PlayerShoot(int yCoordinate, int xCoordinate, char[,] player1ShootingMap, char[,] player2ShipsMap)
        {
            player2ShipsMap[xCoordinate, yCoordinate] = player2ShipsMap[xCoordinate, yCoordinate] == '#'
                ? 'X' : '\0' ;
            
            player1ShootingMap[xCoordinate, yCoordinate] = player2ShipsMap[xCoordinate, yCoordinate] == '#'
                ? 'X' : 'O' ;
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
    }
}