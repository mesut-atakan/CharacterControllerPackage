using UnityEngine;

public sealed class Grid
{
    private static readonly Grid instance = new Grid();
    private static Vector2 gridAmount;
    private static float gridDistance;
    private static Color gizmosColor;
    private static float gizmosSize;

    #region <<<< Properties >>>>

    internal static Vector2 GetGridAmount => gridAmount;
    internal static float GetGridDistance => gridDistance;
    internal static Color GetGizmosColor => gizmosColor;
    internal static float GetGizmosSize => gizmosSize;

    #endregion <<<< Properties >>>>

    private Grid() { }

    public static Grid Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// Sets the grid values.
    /// </summary>
    /// <param name="GridAmount">The number of grid cells in x and y directions.</param>
    /// <param name="GridDistance">The distance between grid cells.</param>
    /// <param name="GizmosSize">The size of the gizmos drawn in the scene.</param>
    /// <param name="GizmosColor">The color of the gizmos drawn in the scene.</param>
    public static void SetGridValues(Vector2 GridAmount, float GridDistance, Color GizmosColor, float GizmosSize)
    {
        gridAmount = GridAmount;
        gridDistance = GridDistance;
        gizmosColor = GizmosColor;
        gizmosSize = GizmosSize;
    }

    /// <summary>
    /// Logs the grid data to the console.
    /// </summary>
    public static void DataDebug()
    {
        Debug.Log($"GridAmount: {gridAmount}\nGridDistance: {gridDistance}\nGizmosColor: {gizmosColor}\nGizmosSize: {gizmosSize}");
    }
}
