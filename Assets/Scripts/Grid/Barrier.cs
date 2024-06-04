using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class Barrier : MonoBehaviour
{
    private GridManager _gridManager;

    private void Awake()
    {
        if (this._gridManager == null)
            this._gridManager = FindObjectOfType<GridManager>();
    }

    private void Start()
    {
        SetInactiveGrids();
    }

    private void SetInactiveGrids()
    {
        GridNode[] gridNodes = InGrids();
        if (gridNodes != null)
        {
            foreach (var gridNode in gridNodes)
            {
                gridNode.GridIsActive = false;

                // Çevresindeki gridleri de inaktif yap
                List<GridNode> surroundingNodes = _gridManager.GetSurroundingGrids(gridNode);
                foreach (var surroundingNode in surroundingNodes)
                {
                    surroundingNode.GridIsActive = false;
                }
            }
        }
    }

    private GridNode[] InGrids()
    {
        List<GridNode> gridNodes = new List<GridNode>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        if (meshCollider != null)
        {
            Bounds bounds = meshCollider.bounds;
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            foreach (var gridNode in _gridManager.Grids)
            {
                Vector3 gridPosition = new Vector3(gridNode.GridPos.x, 0, gridNode.GridPos.y);
                if (gridPosition.x >= min.x && gridPosition.x <= max.x &&
                    gridPosition.z >= min.z && gridPosition.z <= max.z)
                {
                    gridNodes.Add(gridNode);
                }
            }
        }

        return gridNodes.ToArray();
    }
}