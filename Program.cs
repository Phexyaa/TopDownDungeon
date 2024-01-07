
using TopDownDungeon.Enums;
using System.Drawing;
using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using TopDownDungeon.UI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

HostApplicationBuilder builder = new HostApplicationBuilder(args);

builder.Services.AddSingleton<Screen>(new Screen());
using IHost host = builder.Build();

await host.RunAsync();

Screen _screen = host.Services.GetService<Screen>();

int foodCount = 10;
int monsterCount = 20;
int playerFood = 10;
int playerHealth = 100;
int playerStamina = 20;
int playerSpeed = 1;
bool canMove = true;


ConsoleColor borderColor = ConsoleColor.DarkBlue;
ConsoleColor messageColor = ConsoleColor.Cyan;

(int Height, int Width) currentWindoSize = (Console.WindowHeight, Console.WindowWidth);
(int Height, int Width) lastKnownWindowSize = (0, 0);
(int X, int Y) playerLocation = (0, 0);
(int X, int Y) lastPlayerLocation = (0, 0);
(int X, int Y)[] foodLocations = new (int X, int Y)[foodCount];
(int X, int Y)[] monsterLocations = new (int X, int Y)[monsterCount];
Dictionary<ConsumableType, string> foodIcons = new Dictionary<ConsumableType, string>()
{
    {ConsumableType.Pie, "@" },
    {ConsumableType.Bread,"#" },
    {ConsumableType.Meat, "%" }
};
Dictionary<ConsumableType, int> foodValues = new Dictionary<ConsumableType, int>()
{
    {ConsumableType.Pie, 3 },
    {ConsumableType.Bread, 7 },
    {ConsumableType.Meat, 10 }
};







//Utility
ConsoleColor GetPlayerColor()
{
    if (playerHealth > 0 && playerHealth <= 100)
    {
        if (playerHealth > 0 && playerHealth <= 32)
            return ConsoleColor.Red;
        else if (playerHealth > 32 && playerHealth < 65)
            return ConsoleColor.Yellow;
        else
            return ConsoleColor.Green;
    }
    else
        throw new ArgumentOutOfRangeException(nameof(playerHealth));
}

//Board Items


//GUI
void SetWindowSize() => lastKnownWindowSize = currentWindoSize;
void CheckWindowSize()
{
    if (lastKnownWindowSize != currentWindoSize)
        Console.SetWindowSize(lastKnownWindowSize.Width, lastKnownWindowSize.Height);
}


void DrawBoard()
{
    CheckWindowSize();
    DrawBorder();
    DrawFood();
    DrawPlayer();
}
void ShowPosition()
{
    Console.ForegroundColor = messageColor;
    Console.SetCursorPosition(lastKnownWindowSize.Width - 15, 0);
    Console.Write($"({playerLocation.X}, {playerLocation.Y})");
    Console.ResetColor();

}

//Logic
bool CheckWinner()
{
    return playerLocation == (1, 2);
}

void PlacePlayerAtStart()
{
    playerLocation.X = lastKnownWindowSize.Width - 2;
    playerLocation.Y = lastKnownWindowSize.Height - 2;
}
void MovePlayer(MovementDirection direction)
{
    (int x, int y) newLocation = (0, 0);
    lastPlayerLocation = newLocation = playerLocation;

    switch (direction)
    {
        case MovementDirection.North:
            DrainStamina(1);
            newLocation.y--;
            break;
        case MovementDirection.South:
            DrainStamina(1);
            newLocation.y++;
            break;
        case MovementDirection.East:
            DrainStamina(1);
            newLocation.x++;
            break;
        case MovementDirection.West:
            DrainStamina(1);
            newLocation.x--;
            break;
    }

    if (CheckBounds(newLocation))
    {

        DrainStamina(1);

        Console.SetCursorPosition(playerLocation.X, playerLocation.Y);
        Console.Write(" ");

        playerLocation = newLocation;

        DrawPlayer();
        ShowPosition();
        ShowMessage("");
    }
    else
    {
        Console.Beep();
        ShowMessage("At Bounds!");
    }
}
void ResetBoard()
{
    SpawnFood();
    SpawnMonsters();
    PlacePlayerAtStart();
}
void IncreaseStamina(int amount) => playerStamina += amount;
void DrainStamina(int amount) => playerStamina -= amount;
void IncreaseHP(int amount) => playerHealth += amount;
void DecreaseHP(int amount) => playerHealth -= amount;

//Main
Console.CursorVisible = false;
SetWindowSize();
ResetBoard();
DrawBoard();

while (true)
{
    var input = Console.ReadKey(true);
    switch (input.Key)
    {
        case ConsoleKey.Escape:
            break;
        case ConsoleKey.Spacebar:
            break;
        case ConsoleKey.LeftArrow:
            MovePlayer(MovementDirection.West);
            break;
        case ConsoleKey.UpArrow:
            MovePlayer(MovementDirection.North);
            break;
        case ConsoleKey.RightArrow:
            MovePlayer(MovementDirection.East);
            break;
        case ConsoleKey.DownArrow:
            MovePlayer(MovementDirection.South);
            break;
        case ConsoleKey.A:
            MovePlayer(MovementDirection.West);
            break;
        case ConsoleKey.D:
            MovePlayer(MovementDirection.East);
            break;
        case ConsoleKey.S:
            MovePlayer(MovementDirection.South);
            break;
        case ConsoleKey.W:
            MovePlayer(MovementDirection.North);
            break;
    }

    if (CheckWinner())
    {
        Console.Beep();
        ShowMessage("WINNER!");
    }
    if (CheckForFood())
    {
        ConsumeFood(food);
    }
    if (CheckForMonster())
    {
        Console.Beep();
        Console.Beep();
        Console.Beep();
        ShowMessage("MONSTER!");
    }
    //CheckForFood(currentPosition);
    //CheckFormonster(currentPosition);
}


