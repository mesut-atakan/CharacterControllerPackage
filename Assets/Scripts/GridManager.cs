using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region <<<< Serialize Fiedls >>>>
    [SerializeField] GridProperties gridProperties;

    [Space(10f)]
    [SerializeField] private bool drawGizmos = true;
    
    [SerializeField] Vector2[] grids;
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
            foreach (Vector2 _grid in this.grids)
            {
                Gizmos.color = Grid.GetGizmosColor;
                Gizmos.DrawWireCube(new Vector3(_grid.x, 0, _grid.y), new Vector3(Grid.GetGizmosSize, Grid.GetGizmosSize, Grid.GetGizmosSize));
            }
        }
    }

    private void CreateGrid()
    {
        int _gridAmountX = (int)Grid.GetGridAmount.x;
        int _gridAmountY = (int)Grid.GetGridAmount.y;
        float _distance = Grid.GetGridDistance;
        grids = new Vector2[_gridAmountX * _gridAmountY];
        int _index = 0;

        for (int x = 0; x < _gridAmountX; x++)
        {
            for (int y = 0; y < _gridAmountY; y++)
            {
                grids[_index] = new Vector2(x * _distance, y * _distance);
                _index++;
            }
        }
    }
}
