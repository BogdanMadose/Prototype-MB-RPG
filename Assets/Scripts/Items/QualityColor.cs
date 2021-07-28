using System.Collections.Generic;

public static class QualityColor
{
    private static Dictionary<Quality, string> colors = new Dictionary<Quality, string>()
    {
        {Quality.Normal, "#FFFFFF" },
        {Quality.Magic, "#001CF1" },
        {Quality.Rare, "#E2D904" },
        {Quality.Epic, "#8622F3" },
        {Quality.Unique, "#DB4523" }
    };

    public static Dictionary<Quality, string> Colors => colors;
}