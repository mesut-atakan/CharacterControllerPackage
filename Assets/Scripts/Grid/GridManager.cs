using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region <<<< Serialize Fiedls >>>>
    [SerializeField] GridProperties gridProperties;

    [Space(10f)]
    [SerializeField] private bool drawGizmos = true;
    [SerializeField] GridNode[] grids;

    [Header("Objects")]
    [SerializeField] private GameObject planeObject;

    [Header("Classes")]
    [SerializeField] private GameManager gameManager;
    #endregion <<<< XXX >>>>

    private void Awake()
    {
        Grid.Instance.SetGridValues(this.gridProperties.GridAmount, this.gridProperties.GridDistance, this.gridProperties.GizmosColor, this.gridProperties.GizmosSize);
    }

    private void Start()
    {
        Grid.Instance.DataDebug();
        CreateGrid();
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



    /// <summary>
    /// Bu fonksiyon ile birlikte arad���n�z konumdaki gridi �ekebilirsiniz!
    /// </summary>
    /// <param name="playerPosition">Hangi konumu arad���n�z� giriniz!</param>
    /// <returns>Geriye grid d�nd�r�lecek!</returns>
    public GridNode FindGridNodeForVector(Vector3 playerPosition)
    {
#if UNITY_EDITOR
        UnityEngine.Profiling.Profiler.BeginSample("FindGrid");
#endif
        int gridX = Mathf.FloorToInt(playerPosition.z / Grid.Instance.GetGridDistance);
        int gridY = Mathf.FloorToInt(playerPosition.x / Grid.Instance.GetGridDistance);

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


    /// <summary>
    /// Bu fonksiyon ile birlikte arad���n�z Gridin dizideki index numaras�n� �ebirebilirsiniz!
    /// </summary>
    /// <param name="pos">Aramak istedi�iniz gridNode nun pozisyonunu girniz!</param>
    /// <returns>Grid nodenin pzoisyonuna ait olan grid node nin index numaras� geri d�nderilecek!</returns>
    public int FindGridNodeIndex(Vector3 pos)
    {
        GridNode _gridNode;
        int _index = 0;

        _gridNode = FindGridNodeForVector(pos);
        foreach(GridNode _grid in this.grids)
        {
            if (_gridNode == _grid) return _index;
            _index++;
            continue;
        }

        return -1;
    }

    /// <summary>
    /// Bu fonksiyon ile Grid Nodenizi parametre olarak girerek bu gridin dizide hangi index de yer ald���n� �evirebilirsiniz!
    /// </summary>
    /// <param name="gridNode">Arad���n�z GridNode s�n�f�n� parametre olarak giriniz!</param>
    /// <returns>Arad���n�z s�n�f�n index numaras� geri d�nderilir. E�er index numaras� bulunamazsa -1 de�ere geri �evrilir!</returns>
    public int FindGridNodeIndex(GridNode gridNode)
    {
        int _index = 0;

        foreach (GridNode _grid in this.grids)
        {
            if (_grid == gridNode) return _index;
            _index++;
            continue;
        }
        return -1;
    }




    /// <summary>
    /// Bu fonksiyon ile grid olu�turabilirsiniz!
    /// </summary>
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
                    GridIsActive = true
                };
                _index++;
            }
        }
        AlignPlaneWithGrid();

#if UNITY_EDITOR
        CameraController.ChangeCameraPos(GridCenterPos(), 20);
#endif
    }


    /// <summary>
    /// Bu fonksiyon ile olu�turdu�unuz gridleri temizleyebilirsiniz!
    /// </summary>
    public void ClearGrid()
    {
        this.grids = null;
    }








    /// <summary>
    /// Bu fonksiyon ile birlikte grid objesinin konumunu tam olarak hizlayabilirsiniz!
    /// </summary>
    private void AlignPlaneWithGrid()
    {
        if (planeObject == null) return;

        // Gridin toplam geni�li�i ve y�ksekli�i
        float totalWidth = Grid.Instance.GetGridAmount.x * Grid.Instance.GetGridDistance;
        float totalHeight = Grid.Instance.GetGridAmount.y * Grid.Instance.GetGridDistance;

        // Plane objesinin pozisyonu ve �l�e�i
        planeObject.transform.position = new Vector3(totalWidth / 2, 0, totalHeight / 2);
        planeObject.transform.localScale = new Vector3(totalWidth / 10, 1, totalHeight / 10);
    }


    /// <summary>
    /// Bu fonksiyon ile birlikte Gridin tam orta noktas�n� �ekebilirsiniz!
    /// </summary>
    /// <returns>Gridin tam olarak orta noktas� geri d�nderilecektir!</returns>
    private Vector3 GridCenterPos()
    {
        float totalWidth = Grid.Instance.GetGridAmount.x * Grid.Instance.GetGridDistance;
        float totalHeight = Grid.Instance.GetGridAmount.y * Grid.Instance.GetGridDistance;

        return new Vector3(totalWidth / 2.0f, 0, totalHeight / 2);
    }
}
