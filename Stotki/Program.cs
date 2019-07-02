using System;

namespace Stotki
{
    public class playerMaps
    {
        char[,] playingMap = new char[10,10]; // Array that represents map 10*10
        char[,] shootingMap = new char[10,10];
        
        /// <summary>
        ///     Inserting ships based on the given coordinates, from one point(x,y) to second point(x2,y2)
        /// </summary>
        /// <param name="firstX"> integer, X value of the first coordinate</param>
        /// <param name="firstY"> integer, Y value of the first coordinate</param>
        /// <param name="secondX"> integer, X value of the second coordinate</param>
        /// <param name="secondY"> integer, Y value of the second coordinate</param>
        public void ShipPlacementFilter(int firstX, int firstY, int secondX, int secondY)
        {
            if (firstX != secondX && firstY==secondY) // Checking if iteration should be Horizontal
            {
                int firstCoordinate = firstX < secondX 
                    ? firstX : secondX;
                int secondCoordinate = firstX < secondX 
                    ? secondX : firstX;
                ShipPlacementIteration(firstCoordinate, secondCoordinate, firstY);
            }
            else if (firstY != secondY && secondX == firstX) // Checking if iteration should be Vertical
            {
                int firstCoordinate = firstY < secondY 
                    ? firstY : secondY;
                int secondCoordinate = firstY < secondY 
                    ? secondY : firstY;
                ShipPlacementIteration(firstCoordinate, secondCoordinate, firstX);
            }
        }
        
        /// <summary>
        ///     Iteration over given coordinates in map and inserting ships as #
        /// </summary>
        /// <param name="firstCoord">First coord to begin iteration</param>
        /// <param name="secondCoord">Second coord to begin iteration</param>
        /// <param name="iterationOver">coordinate to iterate over</param>
        private void ShipPlacementIteration(int firstCoord, int secondCoord, int iterationOver)
        {
            for (int i = firstCoord; i <= secondCoord; i++)
            {
                playingMap[i, iterationOver] = '#';
                Console.WriteLine($"Placing Ship at {i},{iterationOver} "); // Checking iteration at ship placement
            }
        }

        /// <summary>
        ///     Saves player shot from given cordinates
        /// </summary>
        /// <param name="xCoordinate"> X coordinate of shot</param>
        /// <param name="yCoordinate"> Y coordinate of shot</param>
        /// <returns></returns>
        public string Shoot(int xCoordinate, int yCoordinate)
        {
            return "Shoot received";
        }
    }
    
    public class BattleShipsGame
    {
        static void Main(string[] args)
        {
            playerMaps player1PlayingMap = new playerMaps();
            Console.Write(player1PlayingMap.Shoot(2, 4));
            UserShootingInput(out int xShootingCoord, out int yShootingCoord);
            Console.Write(player1PlayingMap.Shoot(xShootingCoord, yShootingCoord));
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
                validation = UserShootingInputValidation(stringinputCoords, out firstShootingCoord, out secondShootingCoord);
                if (validation == "Wrong!")
                    Console.WriteLine("Please provide Correct input :/");
            } while (validation == "Wrong!");

            userInputX = firstShootingCoord;
            userInputY = secondShootingCoord;
        }
    
        /// <summary>
        ///     Validatin of the given user input
        /// </summary>
        /// <param name="stringinputCoords"> User input in string</param>
        /// <returns></returns>
        static string UserShootingInputValidation(string stringinputCoords, out int firstCoord, out int secondCoord)
        {
            firstCoord = -1;
            secondCoord = -1;
            
            string[] splitinputCoords = stringinputCoords.Split(',');
            if (splitinputCoords.Length != 2)
                return "Wrong!";
            
            bool firstCoordCheck = int.TryParse(splitinputCoords[0], out int intFirstCoord);
            bool secondCoordCheck = int.TryParse(splitinputCoords[1], out int intSecondCoord);
            
            if (!firstCoordCheck || !secondCoordCheck)
                return "Wrong!";

            firstCoord = intFirstCoord;
            secondCoord = intSecondCoord;
            return "Correct!";
        }
    }
}