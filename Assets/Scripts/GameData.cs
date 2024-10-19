using System.Collections;
using System.Collections.Generic;

public static class GameData
{
    static int hope = 0;
    static int lives = 3;

    public static void UpdateHope(int amt) {
        hope += amt;
    }

    public static void UpdateLives(int amt)
    {
        lives += amt;
    }
    public static int GetHope() { 
        return hope;
    }
    public static int GetLives()
    {
        return lives;
    }

    public static void ResetHope() { 
        hope = 0;
    }
    public static void ResetLives()
    {
        lives = 3;
    }

}
