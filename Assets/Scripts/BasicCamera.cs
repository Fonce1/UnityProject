using UnityEngine;
public class PixelPerfectCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 3f;
    [SerializeField] private Vector2 offset = Vector2.zero;

    [Header("Pixel Perfect")]
    [SerializeField] private bool pixelPerfect = true;
    [SerializeField] private float pixelsPerUnit = 32f; // Для 32x32 тайлов

    private Camera cam;
    private float pixelSize;

    private void Start()
    {
        cam = GetComponent<Camera>();
        pixelSize = 1f / pixelsPerUnit;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z
        );

        // Округление для пиксель-перфекта
        if (pixelPerfect)
        {
            desiredPosition.x = Mathf.Round(desiredPosition.x / pixelSize) * pixelSize;
            desiredPosition.y = Mathf.Round(desiredPosition.y / pixelSize) * pixelSize;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}