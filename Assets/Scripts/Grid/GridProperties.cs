using UnityEngine;
using System;

[Serializable]
public struct GridProperties
{
    #region <<<< Serialize Fields >>>>
    [Header("Grid Properties")]
    [SerializeField] private Vector2 gridAmount;
    [SerializeField] private float gridDistance;
    [SerializeField] private float gizmosSize;
    [SerializeField] private Color gizmosColor;
    #endregion <<<< XXX >>>>


    #region <<<< Properties >>>>

    internal Vector2 GridAmount { get => this.gridAmount; set => this.gridAmount = value; }
    internal float GridDistance { get => this.gridDistance; set => this.gridDistance = value; }
    internal float GizmosSize { get => this.gizmosSize; set => gizmosSize = value; }
    internal Color GizmosColor { get => this.gizmosColor; set => this.gizmosColor = value; }

    #endregion <<<< XXX >>>>
}