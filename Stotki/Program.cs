using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
                Console.WriteLine($"Placing Ship at {i},{iterationOver} "); // Checking iteration at ship placement
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
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if(character == 'X') Console.ForegroundColor = ConsoleColor.Red;
            
                Console.Write(character);
                Console.ResetColor();
            }

            if (xCoordinate != -1 && yCoordinate != -1)
            {
                playerShootingMap[xCoordinate, yCoordinate] = playerShootingMap[xCoordinate, yCoordinate] == 'o' ? 'O' : 'X';
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
            maps.Add(string.Concat(Enumerable.Repeat(" ", 20))+"Enemy");
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
                    playerMap += playerShipsMap[row, column] == '\0' ? "   |" : $" {playerShipsMap[row, column]} |";
                    shootingMap += playerShootingMap[row, column] == '\0' ? "   |" : $" {playerShootingMap[row, column]} |";
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
        
        static readonly Player firstPlayerClass = new Player();
        static readonly Player secondPlayerClass = new Player();
        
        static void Main(string[] args)
        {
            Console.WriteLine("PLAYER 1 SETTING SHIPS");
            string cheat1 = Console.ReadLine();
            if (cheat1 == "Enable Cheat Engine")
            {
                CheatShipsGenerator("Player1");
                firstPlayerClass.ShowMaps();
            }    
            else
            {
                ShipPlacement("Player1");
            }
            
            Console.WriteLine("END TURN");
            Console.ReadLine();
            Console.Clear();
            
            Console.WriteLine("PLAYER 2 SETTING SHIPS");
            string cheat2 = Console.ReadLine();
            if (cheat2 == "Enable Cheat Engine")
            {
                CheatShipsGenerator("Player2");
                secondPlayerClass.ShowMaps();
            }
            else
            {
                ShipPlacement("Player2");
            }
            
            Console.WriteLine("END TURN");
            Console.ReadLine();
            Console.Clear();
            
            do
            {
                Console.WriteLine("PLAYER 1 IS SHOOTING");
                Console.ReadLine();
                Console.Clear();
                
                UserShootingInput(out int xShootInput, out int yShootInput);
                PlayerShoot(xShootInput,yShootInput,"Player1");
                
                Console.WriteLine("END TURN");
                Console.ReadLine();
                Console.Clear();
                
                Console.WriteLine("PLAYER 2 IS SHOOTING");
                Console.ReadLine();
                Console.Clear();
                
                UserShootingInput(out xShootInput, out yShootInput);
                PlayerShoot(xShootInput,yShootInput,"Player2");
                
                Console.WriteLine("END TURN");
                Console.ReadLine();
                Console.Clear();
                
            } while (firstPlayerClass.life == 0 || secondPlayerClass.life == 0);
        }
        
        /// <summary>
        ///     Universal method for calling methods responsible for placing ships as and argument takes player
        /// </summary>
        static void ShipPlacement(string player)
        {
            foreach (KeyValuePair<string, int> ship in ShipsValues)
            {
                Console.Clear();
                UserShipPlacementTurn(out int[] firstCoords, out int[] secondCoords, ship.Value);
                if (player == "Player1")
                {
                   firstPlayerClass.ShipPlacementFilter(firstCoords[0], firstCoords[1], secondCoords[0], secondCoords[1]);
                   firstPlayerClass.ShowMaps(); 
                }
                else if (player == "Player2")
                {
                    secondPlayerClass.ShipPlacementFilter(firstCoords[0], firstCoords[1], secondCoords[0], secondCoords[1]);
                    secondPlayerClass.ShowMaps(); 
                }
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
                        ? 'x'
                        : 'o';
                firstPlayerClass.ShowMaps(xCoordinate, yCoordinate);
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
                        ? 'x'
                        : 'o';
                secondPlayerClass.ShowMaps(xCoordinate, yCoordinate);
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