using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private float minXOffset = -5f;
    [SerializeField] private float maxXOffset = 5f;
    [SerializeField] private GameObject ui;

    private Transform cameraTransform;
    private float targetXOffset;
    private float uiXOffset;
    private float uiYOffset;
    private float uiZOffset;

    private void Start()
    {
        cameraTransform = transform;
        targetXOffset = Mathf.Clamp(ball.position.x, minXOffset, maxXOffset);
        uiXOffset = ui.transform.position.x;
        uiYOffset = ui.transform.position.y;
        uiZOffset = ui.transform.position.z;
    }

    private void LateUpdate()
    {
        targetXOffset = Mathf.Clamp(ball.position.x, minXOffset, maxXOffset);

        Vector3 targetPosition = new Vector3(targetXOffset, cameraTransform.position.y, cameraTransform.position.z);

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime * followSpeed);
        ui.transform.position = new Vector3(cameraTransform.position.x + uiXOffset - 2f, uiYOffset, uiZOffset);
    }
}
