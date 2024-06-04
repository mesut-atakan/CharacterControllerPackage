using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>
    [SerializeField] GridProperties gridProperties;

    [Space(10f)]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] GridNode[] grids;

    [Header("Objects")]
    [SerializeField] private GameObject planeObject;

    [Header("Classes")]
    [SerializeField] private GameManager gameManager;

    [Header("Settings")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private int surroundingGridRadius = 1; // Çevredeki grid sayýsý


    #endregion <<<< Serialize Fields >>>>




    #region <<<< Properties >>>>

    internal GridNode[] Grids { get => this.grids; }
    internal int SurroundingGridRadius => surroundingGridRadius; // Yeni çevre grid birimi özelliði

    #endregion <<<< XXX >>>>

    private void Awake()
    {
        Grid.Instance.SetGridValues(this.gridProperties.GridAmount, this.gridProperties.GridDistance, this.gridProperties.GizmosColor, this.gridProperties.GizmosSize);
        Grid.Instance.DataDebug();
        CreateGrid();
    }

    private void Start()
    {
    }

    private void OnDrawGizmos()
    {
        if (this.drawGizmos && grids != null)
        {
            foreach (GridNode _grid in this.grids)
            {
                if (_grid.GridIsActive)
                {
                    Gizmos.color = Grid.Instance.GetGizmosColor;
                    Gizmos.DrawWireCube(new Vector3(_grid.GridPos.x, 0, _grid.GridPos.y), new Vector3(Grid.Instance.GetGizmosSize, Grid.Instance.GetGizmosSize, Grid.Instance.GetGizmosSize));
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(new Vector3(_grid.GridPos.x, 0, _grid.GridPos.y), Grid.Instance.GetGizmosSize / 2);
                }
            }
        }
    }



    public List<GridNode> GetSurroundingGrids(GridNode centerNode)
{
    List<GridNode> surroundingNodes = new List<GridNode>();

    int gridAmountX = (int)Grid.Instance.GetGridAmount.x;
    int gridAmountY = (int)Grid.Instance.GetGridAmount.y;
    float gridDistance = Grid.Instance.GetGridDistance;

    for (int x = -surroundingGridRadius; x <= surroundingGridRadius; x++)
    {
        for (int y = -surroundingGridRadius; y <= surroundingGridRadius; y++)
        {
            if (x == 0 && y == 0)
                continue;

            Vector2 checkPos = new Vector2(centerNode.GridPos.x + x * gridDistance, centerNode.GridPos.y + y * gridDistance);
            GridNode node = FindGridNodeForVector(new Vector3(checkPos.x, 0, checkPos.y));
            if (node != null && node.GridIsActive)
            {
                surroundingNodes.Add(node);
            }
        }
    }

    return surroundingNodes;
}

    public GridNode FindGridNodeForVector(Vector3 position)
    {
#if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FindGrid");
#endif
        int gridX = Mathf.FloorToInt(position.z / Grid.Instance.GetGridDistance);
        int gridY = Mathf.FloorToInt(position.x / Grid.Instance.GetGridDistance);

        if (gridX >= 0 && gridX < Grid.Instance.GetGridAmount.x && gridY >= 0 && gridY < Grid.Instance.GetGridAmount.y)
        {
            int index = gridY * (int)Grid.Instance.GetGridAmount.x + gridX;
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

    public GridNode FindNearestGridNode(Vector3 position)
    {
        GridNode nearestNode = null;
        float nearestDistance = float.MaxValue;

        foreach (var node in grids)
        {
            float distance = Vector3.Distance(new Vector3(node.GridPos.x, 0, node.GridPos.y), position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestNode = node;
            }
        }

        return nearestNode;
    }

    public List<GridNode> GetNeighbours(GridNode node)
    {
        List<GridNode> neighbours = new List<GridNode>();

        Vector2[] directions = {
        new Vector2(0, 1), // Up
        new Vector2(1, 0), // Right
        new Vector2(0, -1), // Down
        new Vector2(-1, 0), // Left
        new Vector2(1, 1), // ForwardRight
        new Vector2(-1, 1), // ForwardLeft
        new Vector2(1, -1), // BackwardRight
        new Vector2(-1, -1) // BackwardLeft
    };

        foreach (var direction in directions)
        {
            Vector2 neighbourPos = node.GridPos + direction * Grid.Instance.GetGridDistance;
            GridNode neighbourNode = FindGridNodeForVector(new Vector3(neighbourPos.x, 0, neighbourPos.y));
            if (neighbourNode != null && neighbourNode.GridIsActive)
            {
                neighbours.Add(neighbourNode);
            }
        }

        return neighbours;
    }

    public List<GridNode> FindPathToTarget(GridNode startNode, GridNode targetNode)
    {
        List<GridNode> path = new List<GridNode>();
        HashSet<GridNode> closedSet = new HashSet<GridNode>();
        List<GridNode> openSet = new List<GridNode> { startNode };

        while (openSet.Count > 0)
        {
            GridNode currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                path = RetracePath(startNode, targetNode);
                break;
            }

            foreach (GridNode neighbour in GetNeighbours(currentNode))
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
                if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return path;
    }

    private int GetDistance(GridNode nodeA, GridNode nodeB)
    {
        int dstX = Mathf.Abs(Mathf.FloorToInt(nodeA.GridPos.x - nodeB.GridPos.x));
        int dstY = Mathf.Abs(Mathf.FloorToInt(nodeA.GridPos.y - nodeB.GridPos.y));

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }


    private List<GridNode> RetracePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> path = new List<GridNode>();
        GridNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Reverse();

        return path;
    }

    private Direction GetNextDirection(GridNode currentNode, GridNode targetNode)
    {
        if (Mathf.Abs(targetNode.GridPos.x - currentNode.GridPos.x) > Mathf.Abs(targetNode.GridPos.y - currentNode.GridPos.y))
        {
            if (targetNode.GridPos.x > currentNode.GridPos.x)
            {
                return Direction.Right;
            }
            else
            {
                return Direction.Left;
            }
        }
        else
        {
            if (targetNode.GridPos.y > currentNode.GridPos.y)
            {
                return Direction.Forward;
            }
            else
            {
                return Direction.Backward;
            }
        }
    }

    private GridNode FindValidNeighbour(GridNode currentNode, GridNode targetNode)
    {
        Direction[] directions = { Direction.Forward, Direction.Backward, Direction.Left, Direction.Right };

        foreach (Direction direction in directions)
        {
            GridNode neighbour = GetNeighbour(currentNode, direction);
            if (neighbour != null && neighbour.GridIsActive)
            {
                return neighbour;
            }
        }

        return null;
    }

    public GridNode GetNeighbour(GridNode currentNode, Direction direction)
    {
        Vector2 gridDistance = new Vector2(Grid.Instance.GetGridDistance, Grid.Instance.GetGridDistance);

        switch (direction)
        {
            case Direction.Forward:
                return FindGridNodeForVector(new Vector3(currentNode.GridPos.x, 0, currentNode.GridPos.y + gridDistance.y));
            case Direction.Backward:
                return FindGridNodeForVector(new Vector3(currentNode.GridPos.x, 0, currentNode.GridPos.y - gridDistance.y));
            case Direction.Left:
                return FindGridNodeForVector(new Vector3(currentNode.GridPos.x - gridDistance.x, 0, currentNode.GridPos.y));
            case Direction.Right:
                return FindGridNodeForVector(new Vector3(currentNode.GridPos.x + gridDistance.x, 0, currentNode.GridPos.y));
            case Direction.ForwardRight:
                return FindGridNodeForVector(new Vector3(currentNode.GridPos.x + gridDistance.x, 0, currentNode.GridPos.y + gridDistance.y));
            case Direction.ForwardLeft:
                return FindGridNodeForVector(new Vector3(currentNode.GridPos.x - gridDistance.x, 0, currentNode.GridPos.y + gridDistance.y));
            case Direction.BackwardRight:
                return FindGridNodeForVector(new Vector3(currentNode.GridPos.x + gridDistance.x, 0, currentNode.GridPos.y - gridDistance.y));
            case Direction.BackwardLeft:
                return FindGridNodeForVector(new Vector3(currentNode.GridPos.x - gridDistance.x, 0, currentNode.GridPos.y - gridDistance.y));
            default:
                return null;
        }
    }

    public void CreateGrid()
    {
        Grid.Instance.SetGridValues(this.gridProperties.GridAmount, this.gridProperties.GridDistance, this.gridProperties.GizmosColor, this.gridProperties.GizmosSize);
        int _gridAmountX = (int)Grid.Instance.GetGridAmount.x;
        int _gridAmountY = (int)Grid.Instance.GetGridAmount.y;
        float _distance = Grid.Instance.GetGridDistance;
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
                    GridIsActive = true,
                    Index = _index
                };
                _index++;
            }
        }
        AlignPlaneWithGrid();
#if UNITY_EDITOR
        CameraController.ChangeCameraPos(GridCenterPos(), Grid.Instance.GetGridAmount.x * Grid.Instance.GetGridDistance);
#endif
    }

    public void ClearGrid()
    {
        this.grids = null;
    }

    private void AlignPlaneWithGrid()
    {
        if (planeObject == null) return;

        float totalWidth = Grid.Instance.GetGridAmount.x * Grid.Instance.GetGridDistance;
        float totalHeight = Grid.Instance.GetGridAmount.y * Grid.Instance.GetGridDistance;

        planeObject.transform.position = new Vector3(totalWidth / 2, 0, totalHeight / 2);
        planeObject.transform.localScale = new Vector3(totalWidth / 10, 1, totalHeight / 10);
    }

    private Vector3 GridCenterPos()
    {
        float totalWidth = Grid.Instance.GetGridAmount.x * Grid.Instance.GetGridDistance;
        float totalHeight = Grid.Instance.GetGridAmount.y * Grid.Instance.GetGridDistance;

        return new Vector3(totalWidth / 2.0f, 0, totalHeight / 2);
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }
}

public enum Direction
{
    Forward,
    Backward,
    Left,
    Right,
    ForwardRight,
    ForwardLeft,
    BackwardRight,
    BackwardLeft
}
