using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [Header("Настройки вращения")]
    [SerializeField] private float rotationSpeed = 180f; // Скорость вращения (градусов в секунду)
    
    void Update()
    {
        // Вращаем объект вокруг оси Y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}