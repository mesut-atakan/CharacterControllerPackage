using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region <<<< Public Fields >>>>
    public static new Camera camera { get => Camera.main; }
    #endregion <<<< XXX >>>>



    private void Awake()
    {
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte kameranýn pozisyonunu deðiþtireblirsiniz!
    /// </summary>
    /// <param name="pos"></param>
    public static void ChangeCameraPos(Vector3 pos, float yAxis = -0.1f)
    {
        Vector3 _cameraPos;
        if (yAxis != -0.1f)
        {
            _cameraPos = pos;
            _cameraPos.y = yAxis;
        }
        else
            _cameraPos = pos;

        camera.transform.position = _cameraPos;
    }
}
