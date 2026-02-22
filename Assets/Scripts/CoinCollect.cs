using UnityEngine;
using Sample;

public class CoinCollect : MonoBehaviour
{
    [Header("Настройки монетки")]
    public float shrinkSpeed = 2f; // Скорость уменьшения
    public float destroyDelay = 0.5f; // Задержка перед уничтожением после уменьшения
    private bool isCollected = false;
    private Vector3 originalScale;
    private AudioSource audioSource;
    
    void Start()
    {
        originalScale = transform.localScale;
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        // Если монетка собрана - уменьшаем её
        if (isCollected)
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale, 
                Vector3.zero, 
                shrinkSpeed * Time.deltaTime
            );
            // Когда стала достаточно маленькой - уничтожаем
            if (transform.localScale.magnitude < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            Collect(other);
        }
    }
    
    void Collect(Collider other)
    {
        isCollected = true;
        // Отключаем коллайдер, чтобы нельзя было собрать дважды
        GetComponent<Collider>().enabled = false;

        // Увеличиваем счетчик монет в скрипте игрока
        GhostScript player = other.GetComponent<GhostScript>();
        if (player != null)
        {
            player.AddCoin();
        }
        Debug.Log("Монетка собрана!");
    }
}
