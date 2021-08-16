public class XPManager
{
    /// <summary>
    /// Calculate experience points
    /// </summary>
    /// <param name="e">Enemy killed</param>
    public static int CalculateXP(Enemy e)
    {
        int baseXP = (Player.Instance.Level * 5) + 60;
        int grayLevel = CalulateGrayLevel();
        int totalXP = 0;
        if (e.Level >= Player.Instance.Level)
        {
            totalXP = (int)(baseXP * (1 + 0.05f * (e.Level - Player.Instance.Level)));
        }
        else if (e.Level > grayLevel)
        {
            totalXP = baseXP * (1 - ((Player.Instance.Level - e.Level) / ZeroDifference()));
        }
        return totalXP;
    }

    /// <summary>
    /// Calculate when XP reaches 0 if level differences reach a certain point
    /// </summary>
    /// <returns></returns>
    private static int ZeroDifference()
    {
        if (Player.Instance.Level <= 7)
        {
            return 5;
        }
        if (Player.Instance.Level >= 8 && Player.Instance.Level <= 9)
        {
            return 6;
        }
        if (Player.Instance.Level >= 10 && Player.Instance.Level <= 11)
        {
            return 7;
        }
        if (Player.Instance.Level >= 12 && Player.Instance.Level <= 15)
        {
            return 8;
        }
        if (Player.Instance.Level >= 16 && Player.Instance.Level <= 19)
        {
            return 9;
        }
        if (Player.Instance.Level >= 20 && Player.Instance.Level <= 29)
        {
            return 11;
        }
        if (Player.Instance.Level >= 30 && Player.Instance.Level <= 39)
        {
            return 12;
        }
        if (Player.Instance.Level >= 40 && Player.Instance.Level <= 44)
        {
            return 13;
        }
        if (Player.Instance.Level >= 45 && Player.Instance.Level <= 49)
        {
            return 14;
        }
        if (Player.Instance.Level >= 50 && Player.Instance.Level <= 54)
        {
            return 15;
        }
        if (Player.Instance.Level >= 55 && Player.Instance.Level <= 59)
        {
            return 16;
        }
        if (Player.Instance.Level >= 60 && Player.Instance.Level <= 64)
        {
            return 17;
        }
        if (Player.Instance.Level >= 65 && Player.Instance.Level <= 70)
        {
            return 18;
        }
        else
        {
            return 19;
        }
    }

    /// <summary>
    /// Calculate gray level area
    /// </summary>
    public static int CalulateGrayLevel()
    {
        if (Player.Instance.Level <= 5)
        {
            return 0;
        }
        else if (Player.Instance.Level >= 6 && Player.Instance.Level <= 49)
        {
            return Player.Instance.Level - (Player.Instance.Level / 10) - 5;
        }
        else if (Player.Instance.Level == 50)
        {
            return Player.Instance.Level - 10;
        }
        else if (Player.Instance.Level >= 51 && Player.Instance.Level <= 69)
        {
            return Player.Instance.Level - (Player.Instance.Level / 5) - 1;
        }
        return Player.Instance.Level - 9;
    }
}
