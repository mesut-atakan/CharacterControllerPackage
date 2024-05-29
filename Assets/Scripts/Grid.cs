using JetBrains.Annotations;
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
    #endregion <<<< XXX >>>>



    private Grid() { }


    public static Grid Instance
    {
        get
        {
            return Instance;
        }
    }







    /// <summary>
    /// Private constructor to prevent direct instantiation.
    /// Initializes the grid properties with specified values.
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
    /// Bu fonksiyon ile datalarýn çýktýlarýný alabilirsiniz!
    /// </summary>
    public static void DataDebug()
    {
        Debug.Log($"GridAmount: {gridAmount}\nGridDistance: {gridDistance}\nGizmosColor: {gizmosColor}\nGizmosSize: {gizmosSize}");
    }
}