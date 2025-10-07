using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string _resourceName = " ";
    private ItemSpawner _spawner;

    private void Awake()
    {
        // Используем новый метод (выберите подходящий вариант)
        _spawner = FindFirstObjectByType<ItemSpawner>(); // Находит первый подходящий объект
        // ИЛИ
        _spawner = FindAnyObjectByType<ItemSpawner>(); // Более быстрый вариант, если порядок не важен

        if (_spawner == null)
            Debug.LogError("ItemSpawner не найден на сцене!", this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (string.IsNullOrEmpty(_resourceName))
        {
            Debug.LogError("ResourceName не задан!", this);
            return;
        }

        _spawner?.ItemPicked(_resourceName);
        Destroy(gameObject);
    }
}