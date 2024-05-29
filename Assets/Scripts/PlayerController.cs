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
}
