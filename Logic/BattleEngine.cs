using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using TopDownDungeon.Models;


//Utility
internal class BattleEngine
{
    const ConsoleColor defaultTextColor = ConsoleColor.Yellow;
    const int diceCount = 2;
    int[] enemyDice = new int[diceCount];
    int[] playerDice = new int[diceCount];

    int fightLength;
    bool fightComplete;
    char loadingWidgetChar;

    int[] RollPlayerDice(int[] playerDice)
    {
        string result = "";
        foreach (int die in playerDice)
        {
            int index = Array.IndexOf(playerDice, die);
            playerDice[index] = new Random().Next(1, 7);
            result = string.Join($"Die #{index}: ", result, playerDice[index]);
        }

        Console.WriteLine($"You were given {playerDice.Length} dice and rolled the following:\n{result}");
        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();

        return playerDice;
    }
    void RollTargetDice()
    {
        string result = "";
        foreach (int die in enemyDice)
        {
            int index = Array.IndexOf(enemyDice, die);
            enemyDice[index] = new Random().Next(1, 6);
            result = string.Join($"Die #{index}: ", result, enemyDice[index]);
        }

        Console.WriteLine($"The Target rolled the following:\n{result}");
        Console.WriteLine("Press 'Enter' to continue.");
        Console.ReadLine();
    }


    //Game Logic
    (bool Success, int PlayerHP) Fight(int enemyHP, int playerHP)
    {
        bool success = false;
        bool isDraw = false;

        RollPlayerDice(playerDice);
        RollTargetDice();

        do
        {
            foreach (int targetDie in enemyDice)
            {
                bool isTargetHit = false;
                bool isPlayerHit = false;

                foreach (int playerDie in playerDice)
                {
                    if (playerDie > targetDie)
                        isTargetHit = true;
                    if (playerDie < targetDie)
                        isPlayerHit = true;
                }

                if (isTargetHit)
                {
                    enemyHP--;
                    if (enemyHP <= 0)
                        success = true;
                }
                if (isPlayerHit)
                {
                    playerHP--;
                    if (playerHP <= 0)
                        success = false;
                }
            } 
        } while (playerHP > 0 && enemyHP > 0);

        return (success, playerHP);
    }


    //User Interface
    void DisplayFightTimer()
    {
        Console.Clear();

        int cursorLeft = Console.CursorLeft;
        int interval = Convert.ToInt32(fightLength / 20);

        while (!fightComplete)
        {
            PulseLoadingWidget(cursorLeft);
            Task.Delay(500).Wait();
        }
    }
    void PulseLoadingWidget(int cursorLeft)
    {
        Console.CursorLeft = cursorLeft;
        if (loadingWidgetChar != '/' && loadingWidgetChar != '\\')
        {
            Console.Write($"Engaging... \\");
            loadingWidgetChar = '/';
        }
        else if (loadingWidgetChar == '/')
        {
            Console.Write($"Engaging... /");
            loadingWidgetChar = '\\';
        }
        else
        {
            Console.Write($"Engaging... \\");
            loadingWidgetChar = '/';
        }

        fightLength--;
        if (fightLength <= 0)
            fightComplete = true;
    }
    //void DisplayHorizontalLine(ConsoleColor color, int length)
    //{
    //    Console.ForegroundColor = color;
    //    Console.WriteLine("".PadRight(length, '~'));
    //    Console.ForegroundColor = defaultTextColor;
    //}
    //void DisplayWelcomeMessage()
    //{
    //    Console.Clear();

    //    int longestSentenceLength = 0;
    //    foreach (string sentence in welcomeMessage.Split('.'))
    //    {
    //        if (sentence.Length > longestSentenceLength)
    //            longestSentenceLength = sentence.Length;
    //    }

    //    DisplayHorizontalLine(ConsoleColor.Green, longestSentenceLength);

    //    Console.ForegroundColor = ConsoleColor.Cyan;
    //    Console.WriteLine(greeting);
    //    Console.ForegroundColor = defaultTextColor;

    //    DisplayHorizontalLine(ConsoleColor.Green, longestSentenceLength);

    //    Console.ForegroundColor = ConsoleColor.Cyan;
    //    Console.WriteLine(welcomeMessage);
    //    Console.ForegroundColor = defaultTextColor;

    //    DisplayHorizontalLine(ConsoleColor.Green, longestSentenceLength);
    //}
    //void DisplayMessage(string message)
    //{
    //    Console.Clear();
    //    DisplayHorizontalLine(ConsoleColor.DarkGreen, message.Length);

    //    Console.ForegroundColor = ConsoleColor.White;
    //    Console.WriteLine(message);
    //    Console.ForegroundColor = defaultTextColor;

    //    DisplayHorizontalLine(ConsoleColor.DarkGreen, message.Length);
    //}

    internal (bool Success, int PlayerHP) StartEncounter(Encounter encounter, int playerHP)
    {
        Console.Clear();
        do
        {
            DisplayFightTimer();
            return Fight(encounter.Opponent.HealthPoints ,playerHP);

        } while (!fightComplete);
    }
}