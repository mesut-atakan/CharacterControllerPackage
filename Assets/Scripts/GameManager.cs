using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>

    [Header("Classes")]

    [SerializeField] private CameraController cameraController;

    #endregion <<<< XXX >>>>



    #region <<<< Properties >>>>

    internal CameraController CameraController { get => this.cameraController; set => this.cameraController = value; }

    #endregion <<<< XXX >>>>
}
