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
                    Gizmos.color = Grid.Instance.GetGizmosColor;
                else
                    Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(new Vector3(_grid.GridPos.x, 0, _grid.GridPos.y), new Vector3(Grid.Instance.GetGizmosSize, Grid.Instance.GetGizmosSize, Grid.Instance.GetGizmosSize));
            }
        }
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte aradığınız konumdaki gridi çekebilirsiniz!
    /// </summary>
    /// <param name="playerPosition">Hangi konumu aradığınızı giriniz!</param>
    /// <returns>Geriye grid döndürülecek!</returns>
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
    /// Bu fonksiyon ile grid oluşturabilirsiniz!
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
        CameraController.ChangeCameraPos(GridCenterPos(), 12);
#endif
    }


    /// <summary>
    /// Bu fonksiyon ile oluşturduğunuz gridleri temizleyebilirsiniz!
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

        // Gridin toplam genişliği ve yüksekliği
        float totalWidth = Grid.Instance.GetGridAmount.x * Grid.Instance.GetGridDistance;
        float totalHeight = Grid.Instance.GetGridAmount.y * Grid.Instance.GetGridDistance;

        // Plane objesinin pozisyonu ve ölçeği
        planeObject.transform.position = new Vector3(totalWidth / 2, 0, totalHeight / 2);
        planeObject.transform.localScale = new Vector3(totalWidth / 10, 1, totalHeight / 10);
    }


    /// <summary>
    /// Bu fonksiyon ile birlikte Gridin tam orta noktasını çekebilirsiniz!
    /// </summary>
    /// <returns>Gridin tam olarak orta noktası geri dönderilecektir!</returns>
    private Vector3 GridCenterPos()
    {
        float totalWidth = Grid.Instance.GetGridAmount.x * Grid.Instance.GetGridDistance;
        float totalHeight = Grid.Instance.GetGridAmount.y * Grid.Instance.GetGridDistance;

        return new Vector3(totalWidth / 2.0f, 0, totalHeight / 2);
    }
}
