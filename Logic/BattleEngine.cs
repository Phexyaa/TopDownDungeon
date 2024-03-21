using TopDownDungeon.Models;
using TopDownDungeon.UI;


//Utility
internal class BattleEngine
{
    private const ConsoleColor defaultTextColor = ConsoleColor.Yellow;
    private const int diceCount = 2;
    private int fightLength;
    private int minDamage = 5;
    private int maxDamage = 35;
    private char loadingWidgetChar;
    private readonly Canvas _screen;

    public BattleEngine(Canvas screen)
    {
        _screen = screen;
    }

    private int RollDice()
    {
        return new Random().Next(1, 7);
    }


    //Game Logic
    private (bool Success, int PlayerHP, int EnemyHP) Fight(int enemyHP, int playerHP)
    {
        bool success = false;
        Random randomDamage = new Random();

        do
        {
            var damage = randomDamage.Next(minDamage, maxDamage);

            var playerRoll = RollDice();
            _screen.ShowMessage($"You rolled: {playerRoll}");
            var enemyRoll = RollDice();
            _screen.ShowMessage($"Your opponent rolled: {playerRoll}");

            bool isTargetHit = false;
            bool isPlayerHit = false;

            if (playerRoll > enemyRoll)
                isTargetHit = true;
            if (enemyRoll < playerRoll)
                isPlayerHit = true;

            if (isTargetHit)
            {
                enemyHP -= damage;
                _screen.ShowMessage($"Enemy hit for {damage} points; Enemy has {enemyHP}HP remaining.");
                if (enemyHP <= 0)
                    success = true;
            }
            if (isPlayerHit)
            {
                playerHP -= damage;
                _screen.ShowMessage($"You are hit for {damage} points; You have {playerHP}HP remaining.");
                if (playerHP <= 0)
                    success = false;
            }
        } while (playerHP > 0 && enemyHP > 0);

        return (success, playerHP, enemyHP);
    }
    internal (bool Success, int PlayerHP) StartEncounter(Encounter encounter, int playerHP)
    {
        _screen.ClearScreen();

        _screen.ShowMessage($"Enemy HP: {encounter.Opponent.HealthPoints} | Your HP:{playerHP}");
        _screen.ShowMessage(Environment.NewLine);
        var result = Fight(encounter.Opponent.HealthPoints, playerHP);
        _screen.ShowModalMessage($"Outcome: Your HP: {result.PlayerHP}; Enemy HP: {result.EnemyHP} ");
        return (result.Success, result.PlayerHP);
    }
}