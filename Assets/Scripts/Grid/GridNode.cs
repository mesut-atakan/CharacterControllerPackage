using UnityEngine;


[System.Serializable]
public class GridNode
{
    #region <<<< Serialize Fields >>>>

    [SerializeField] private Vector2 gridPos;
    [SerializeField] private bool gridIsActive = true;

    #endregion <<<< XXX >>>>






    #region <<<< Properties >>>>

    internal Vector2 GridPos { get => this.gridPos; set => this.gridPos = value; }
    internal bool GridIsActive { get => this.gridIsActive; set => this.gridIsActive = value; }

    #endregion <<<< XXX >>>>
}