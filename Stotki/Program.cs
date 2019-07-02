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
            if (firstX != secondX && firstY==secondY)
            {
                int firstCoordinate = firstX < secondX 
                    ? firstX : secondX;
                int secondCoordinate = firstX < secondX 
                    ? secondX : firstX;
                ShipPlacementIteration(firstCoordinate, secondCoordinate, firstY);
            }
            else if (firstY != secondY && secondX == firstX)
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
    
    public class ShipsGame
    {
        static void Main(string[] args)
        {
            playerMaps player1PlayingMap = new playerMaps();
            Console.Write(player1PlayingMap.Shoot(2, 4));
            player1PlayingMap.ShipPlacementFilter(0,0,5,0);
        }
    }
}