using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    
    private void Awake()
    {
        if (this.gridManager == null)
            this.gridManager = FindObjectOfType<GridManager>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Variables
            GridNode _interactionGrid;

            _interactionGrid = InteractionGridNode();
            _interactionGrid.GridIsActive = false;
            Debug.Log($"Interaction Grid {_interactionGrid.GridIsActive} {this.gridManager.FindGridNodeIndex(_interactionGrid)}", this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        FindGrid();
    }


    private void FindGrid()
    {
        // Variables
        GridNode _gridNode;

        _gridNode = gridManager.FindGridNodeForVector(this.transform.position);
        if (_gridNode != null)
            _gridNode.GridIsActive = false;
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte Oyuncunun Gridler ile etkileþime girmesini saðlayabilirsiniz!
    /// </summary>
    /// <returns>Etkileþime girilen Grid geri dönderilir!</returns>
    private GridNode InteractionGridNode()
    {
        // Variables
        Vector2 _mousePos;
        RaycastHit _hit;
        Ray _ray;

        _mousePos = Input.mousePosition;
        _ray = CameraController.camera.ScreenPointToRay(_mousePos);
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
        {
            return this.gridManager.FindGridNodeForVector(_hit.point);
        }
        return null;
    }
}
