using System.Collections.Generic;
using System.Linq;

public class GameEndMessageHelper
{
    public static string CreateGameEndMessage(Dictionary<int,int> scoreMapping, int playerId)
    {
        int playerScore = scoreMapping[playerId];

        int enemiesScore = scoreMapping.
            Where(kvp => kvp.Key != playerId).
            Select(kvp => kvp.Value).
            Sum();

        string msg;

        if (playerScore > enemiesScore)
        {
            msg = "You are best!\n";
        }
        else if (playerScore < enemiesScore)
        {
            msg = "Oh noes, you lost :(\n";
        }
        else
        {
            msg = "It's a draw!\n";
        }

        return msg;
    }
}