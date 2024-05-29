using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region <<<< Serialize Fiedls >>>>
    [SerializeField] GridProperties gridProperties;

    [Space(10f)]
    [SerializeField] private bool drawGizmos = true;
    
    [SerializeField] GridNode[] grids;
    #endregion <<<< XXX >>>>

    private void Awake()
    {
        Grid.SetGridValues(this.gridProperties.GridAmount, this.gridProperties.GridDistance, this.gridProperties.GizmosColor, this.gridProperties.GizmosSize);
    }

    private void Start()
    {
        Grid.DataDebug();
        CreateGrid();
    }

    private void OnDrawGizmos()
    {
        if (this.drawGizmos && grids != null)
        {
            foreach (GridNode _grid in this.grids)
            {
                if (_grid.GridIsActive)
                    Gizmos.color = Grid.GetGizmosColor;
                else
                    Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(new Vector3(_grid.GridPos.x, 0, _grid.GridPos.y), new Vector3(Grid.GetGizmosSize, Grid.GetGizmosSize, Grid.GetGizmosSize));
            }
        }
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte aradýðýnýz konumdaki gridi çekebilirsiniz!
    /// </summary>
    /// <param name="playerPosition">Hangi konumu aradýðýnýzý giriniz!</param>
    /// <returns>Geriye grid döndürülecek!</returns>
    public GridNode FindGridNodeForVector(Vector3 playerPosition)
    {
#if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FindGrid");
#endif
        int gridZ = Mathf.FloorToInt(playerPosition.z / Grid.GetGridDistance);
        int gridX = Mathf.FloorToInt(playerPosition.x / Grid.GetGridDistance);

        if (gridZ >= 0 && gridZ < Grid.GetGridAmount.x && gridZ >= 0 && gridZ < Grid.GetGridAmount.y)
        {
            int index = gridZ * (int)Grid.GetGridAmount.x + gridZ;
#if UNITY_EDITOR
            UnityEngine.Profiling.Profiler.EndSample();
#endif
            return grids[index];
        }
#if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.EndSample();
#endif
        return null;
    }





    private void CreateGrid()
    {
        int _gridAmountX = (int)Grid.GetGridAmount.x;
        int _gridAmountY = (int)Grid.GetGridAmount.y;
        float _distance = Grid.GetGridDistance;
        grids = new GridNode[_gridAmountX * _gridAmountY];
        int _index = 0;

        for (int x = 0; x < _gridAmountX; x++)
        {
            for (int y = 0; y < _gridAmountY; y++)
            {
                Vector2 position = new Vector2(x * _distance, y * _distance);
                grids[_index] = new GridNode
                {
                    GridPos = position,
                    GridIsActive = true
                };
                _index++;
            }
        }
    }

}
