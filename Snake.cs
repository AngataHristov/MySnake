namespace MySnake
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Snake
    {
        public static void Main()
        {
            byte right = 0;
            byte left = 1;
            byte down = 2;
            byte up = 3;
            int lastFoodTime = 0;
            int foodDissapearTime = 5000;
            int negativePoints = 0;

            Positions[] directions = new Positions[]
                {
                    // right
                    new Positions(0,1), 

                    // left
                    new Positions(0,-1),

                    // down
                    new Positions(1,0),

                    // top
                    new Positions(-1,0),
                };

            double sleeTime = 100;

            int direction = right; 

            Console.BufferHeight = Console.WindowHeight;

            Random randomNumbersGenerator = new Random();

            List<Positions> obstacles = new List<Positions>()
            {
                new Positions(12, 12),
            };

            foreach (Positions obstacle in obstacles)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(obstacle.Col, obstacle.Row);
                Console.Write("=");
            }

            Queue<Positions> snakeElements = new Queue<Positions>();

            for (int i = 0; i < 5; i++)
            {
                snakeElements.Enqueue(new Positions(0, i));
            }

            Positions food;
            do
            {
                food = new Positions(randomNumbersGenerator.Next(0, Console.WindowHeight), randomNumbersGenerator.Next(0, Console.WindowWidth));
            }
            while (snakeElements.Contains(food) && obstacles.Contains(food));

            lastFoodTime = Environment.TickCount;

            Console.SetCursorPosition(food.Col, food.Row);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("@");

            foreach (Positions position in snakeElements)
            {
                Console.SetCursorPosition(position.Col, position.Row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("*");
            }

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();

                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != right)
                        {
                            direction = left;
                        }
                    }

                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != left)
                        {
                            direction = right;
                        }
                    }

                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != down)
                        {
                            direction = up;
                        }
                    }

                    if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != up)
                        {
                            direction = down;
                        }
                    }
                }

                Positions snakeHead = snakeElements.Last();
                Positions nextDirection = directions[direction];
                Positions snakeNewHead = new Positions(snakeHead.Row + nextDirection.Row, snakeHead.Col + nextDirection.Col);

                if (snakeNewHead.Col < 0)
                {
                    snakeNewHead.Col = Console.WindowWidth - 1;
                }

                if (snakeNewHead.Row < 0)
                {
                    snakeNewHead.Row = Console.WindowHeight - 1;
                }

                if (snakeNewHead.Row >= Console.WindowHeight)
                {
                    snakeNewHead.Row = 0;
                }

                if (snakeNewHead.Col >= Console.WindowWidth)
                {
                    snakeNewHead.Col = 0;
                }

                if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game Over!");

                    int userPoints = (snakeElements.Count - 5) * 100 - negativePoints;

                    if (Math.Max(userPoints, 0) == 0)
                    {
                        userPoints = 0;
                    }

                    Console.WriteLine("Your points are: {0}", userPoints);
                    return;
                }

                Console.SetCursorPosition(snakeHead.Col, snakeHead.Row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("*");

                snakeElements.Enqueue(snakeNewHead);

                Console.SetCursorPosition(snakeNewHead.Col, snakeNewHead.Row);
                Console.ForegroundColor = ConsoleColor.Gray;

                if (direction == right)
                {
                    Console.WriteLine(">");
                }

                if (direction == left)
                {
                    Console.WriteLine("<");
                }

                if (direction == up)
                {
                    Console.WriteLine("^");
                }

                if (direction == down)
                {
                    Console.WriteLine("v");
                }

                if (snakeNewHead.Col == food.Col && snakeNewHead.Row == food.Row)
                {
                    do
                    {
                        food = new Positions(randomNumbersGenerator.Next(0, Console.WindowHeight), randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) && obstacles.Contains(food));

                    lastFoodTime = Environment.TickCount;

                    Console.SetCursorPosition(food.Col, food.Row);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("@");

                    sleeTime--;

                    Positions obstacle = new Positions();
                    do
                    {
                        obstacle = new Positions(randomNumbersGenerator.Next(0, Console.WindowHeight), randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(obstacle) || obstacles.Contains(obstacle) || (food.Row != obstacle.Row && food.Col != obstacle.Col));

                    obstacles.Add(obstacle);

                    Console.SetCursorPosition(obstacle.Col, obstacle.Row);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("=");
                }
                else
                {
                    Positions last = snakeElements.Dequeue();

                    Console.SetCursorPosition(last.Col, last.Row);
                    Console.Write(" ");
                }

                if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
                {
                    negativePoints += +50;

                    Console.SetCursorPosition(food.Col, food.Row);
                    Console.Write(" ");

                    do
                    {
                        food = new Positions(randomNumbersGenerator.Next(0, Console.WindowHeight), randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) && obstacles.Contains(food));

                    lastFoodTime = Environment.TickCount;
                }

                Console.SetCursorPosition(food.Col, food.Row);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("@");

                sleeTime -= 0.01;
                Thread.Sleep((int)sleeTime);
            }
        }
    }
}
