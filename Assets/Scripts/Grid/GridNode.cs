using UnityEngine;

[System.Serializable]
public class GridNode
{
    #region <<<< Serialize Fields >>>>
    [SerializeField] private Vector2 gridPos;
    [SerializeField] private bool gridIsActive = true;
    #endregion <<<< Serialize Fields >>>>

    #region <<<< Properties >>>>
    internal Vector2 GridPos { get => this.gridPos; set => this.gridPos = value; }
    internal bool GridIsActive { get => this.gridIsActive; set => this.gridIsActive = value; }
    #endregion <<<< Properties >>>>

    // A* algoritmasý için gerekli özellikler
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get => GCost + HCost; }
    public GridNode Parent { get; set; }
    public int Index { get; set; } = -1;
}
