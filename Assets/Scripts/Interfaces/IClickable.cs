using UnityEngine.UI;

/// <summary>
/// Interface for Objects that can be clicked
/// </summary>
public interface IClickable
{
    Image Icon { get; set; }
    int Count { get; }
    Text StackText { get; }
}

