public interface IState
{
    /// <summary>
    /// Start state machine
    /// </summary>
    /// <param name="parent">Enemy that is using the state</param>
    void Enter(Enemy parent);

    /// <summary>
    /// Apply State machine
    /// </summary>
    void Update();

    /// <summary>
    /// Stop State machine
    /// </summary>
    void Exit();
}
