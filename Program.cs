class Program
{
    static int gWidth = Console.BufferWidth, gHeight = Console.BufferHeight;

    static int fX, fY, points = 0, tX, tY, n = 0;
    static int[] sX = new int[gWidth * gHeight], sY = new int[gWidth * gHeight];
    static ConsoleKeyInfo kInfo;
    static Random rnd = new Random();
    static bool status = true;

    static void Main()
    {
        Console.CursorVisible = false;
        
        // Set the first position of the snake and the food
        sX[0] = gWidth / 2; sY[0] = gHeight / 2;
        FoodPosition();

        while (status)
        {
            Move();
            Render();
            Thread.Sleep(90);
        }
        Console.Clear();
        Console.SetCursorPosition((gWidth / 2) - 4, gHeight / 2);
        Console.Write($"Score: {points}");
        Console.SetCursorPosition(0, gHeight);
    }
    static void Render()
    {
        for (int i = 0; i <= points; i++)
        {
            Console.SetCursorPosition(sX[i], sY[i]);
            Console.Write('S');
        }
    }
    static void Move()
    {
        if (n < 1)
            n = points;

        // Track the tail's last position
        tX = sX[n]; tY = sY[n];

        // Erase the tail's last position
        Console.SetCursorPosition(tX, tY);
        Console.Write(' ');

        if (points > 0)
        {
            // Bring the tail to the head's last position
            // NOTE: This makes the movement effect
            sX[n] = sX[0]; sY[n] = sY[0];
            n--;
        }

        // Moves the head
        if (Console.KeyAvailable)
            kInfo = Console.ReadKey(true);

        if (kInfo.Key == ConsoleKey.W)
            sY[0]--;
        else if (kInfo.Key == ConsoleKey.A)
            sX[0]--;
        else if (kInfo.Key == ConsoleKey.S)
            sY[0]++;
        else if (kInfo.Key == ConsoleKey.D)
            sX[0]++;

        // Check for the collision with the food
        if (sX[0] == fX && sY[0] == fY)
        {
            points++;
            FoodPosition();
            sX[points] = tX; sY[points] = tY;
            // Reorganize all the parts of the snake in ascending order
            int x, y, t = n + 1;
            int[] sequence = new int[points + 1];
            for (int i = 1; i < points; i++)
            {
                if (t == points)
                    t = 1;
                sequence[i] = t;
                t++;
            }
            sequence[0] = 0;
            sequence[points] = points;
            for (int i = 0; i < sequence.Length - 1; i++)
                for (int j = 0; j < sequence.Length - i - 1; j++)
                    if (sequence[j] > sequence[j + 1])
                    {
                        x = sX[sequence[j + 1]]; y = sY[sequence[j + 1]];
                        sX[sequence[j + 1]] = sX[sequence[j]]; sY[sequence[j + 1]] = sY[sequence[j]];
                        sX[sequence[j]] = x; sY[sequence[j]] = y;
                        int temp = sequence[j];
                        sequence[j] = sequence[j + 1];
                        sequence[j + 1] = temp;
                    }
            n = points;
        }
        // Teleport
        if (sX[0] == -1)
            sX[0] = gWidth - 1;
        else if (sY[0] == -1)
            sY[0] = gHeight - 1;
        else if (sX[0] == gWidth)
            sX[0] = 0;
        else if (sY[0] == gHeight)
            sY[0] = 0;
    }
    static void FoodPosition()
    {
        fX = rnd.Next(1, gWidth - 1);
        fY = rnd.Next(1, gHeight - 1);
        Console.SetCursorPosition(fX, fY);
        Console.Write('F');
    }
}
