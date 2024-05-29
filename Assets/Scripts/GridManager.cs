using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region <<<< Serialize Fiedls >>>>
    [SerializeField] GridProperties gridProperties;
    #endregion <<<< XXX >>>>
    private void Awake()
    {
        Grid.SetGridValues(this.gridProperties.GridAmount, this.gridProperties.GridDistance, this.gridProperties.GizmosColor, this.gridProperties.GizmosSize);
    }

    private void Start()
    {
        Grid.DataDebug();
    }
}
