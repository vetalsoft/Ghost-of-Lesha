using UnityEngine;

public class CameraGhost : MonoBehaviour
{
    [Header("Камера")]
    public Transform cameraFollow;

    [Header("Смещение относительно персонажа")]
    public Vector3 offset = new Vector3(0f, 5f, 2.5f);

    [Header("Smooth Settings")]
    // Скорость сглаживания
    public float smoothSpeed = 5f;
    // Скорость сглаживания поворота, устраняет мелкие тряски камеры
    public float rotationSmoothSpeed = 3f;
    private Vector3 velocity = Vector3.zero;

    private void Start() {
        if (cameraFollow == null) {
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            cameraFollow = cam.transform;
        }
    }
    void LateUpdate()
    {
        Transform target = this.transform;

        if (target == null) return;

        // Целевая позиция = позиция персонажа + смещение
        Vector3 targetPosition = target.position + offset;

        // Плавно перемещаем камеру
        cameraFollow.position =
            Vector3.SmoothDamp(
                cameraFollow.position,
                targetPosition, ref velocity,
                smoothSpeed * Time.deltaTime
            );

        // Камера всегда смотрит на персонажа
        Quaternion targetRotation =
            Quaternion.LookRotation(
                target.position - cameraFollow.position
            );
        cameraFollow.rotation =
            Quaternion.Slerp(
                cameraFollow.rotation,
                targetRotation,
                rotationSmoothSpeed * Time.deltaTime
            );
    }
}
