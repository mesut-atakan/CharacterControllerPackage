using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    private List<GridNode> path = new List<GridNode>();
    private int pathIndex;
    private LineRenderer lineRenderer;
    private float originalSpeed = 5f;
    private float currentSpeed;

    private void Awake()
    {
        if (this.gridManager == null)
            this.gridManager = FindObjectOfType<GridManager>();

        // LineRenderer bileþenini ekleyin veya bulun
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // LineRenderer ayarlarý
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 0; // Baþlangýçta hiç segment yok

        currentSpeed = originalSpeed; // Baþlangýç hýzý
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridNode targetNode = InteractionGridNode();
            if (targetNode != null)
            {
                GridNode startNode = gridManager.FindGridNodeForVector(this.transform.position);
                path = gridManager.FindPathToTarget(startNode, targetNode);
                pathIndex = 0;
                DrawPath();
            }
        }

        MoveAlongPath();
    }

    private GridNode InteractionGridNode()
    {
        Vector2 _mousePos = Input.mousePosition;
        RaycastHit _hit;
        Ray _ray = CameraController.camera.ScreenPointToRay(_mousePos);

        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
        {
            return gridManager.FindNearestGridNode(_hit.point);
        }
        return null;
    }

    private void MoveAlongPath()
    {
        if (path != null && pathIndex < path.Count)
        {
            Vector3 targetPosition = new Vector3(path[pathIndex].GridPos.x, this.transform.position.y, path[pathIndex].GridPos.y);
            if (Vector3.Distance(this.transform.position, targetPosition) < 0.1f)
            {
                pathIndex++;
            }
            else
            {
                RotateTowards(targetPosition);
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, Time.deltaTime * currentSpeed);
            }
        }
    }

    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        float rotationSpeed = gridManager.GetRotationSpeed();

        // Dönüþ sýrasýnda yavaþlama
        float angle = Quaternion.Angle(transform.rotation, lookRotation);
        currentSpeed = Mathf.Lerp(originalSpeed * 0.5f, originalSpeed, 1 - angle / 180);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void DrawPath()
    {
        if (path == null || path.Count == 0)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(path[i].GridPos.x, 0.1f, path[i].GridPos.y));
        }
    }
}
