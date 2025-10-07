using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private string _resourceName = " ";
    private ItemSpawner _spawner;

    private void Awake()
    {
        // ���������� ����� ����� (�������� ���������� �������)
        _spawner = FindFirstObjectByType<ItemSpawner>(); // ������� ������ ���������� ������
        // ���
        _spawner = FindAnyObjectByType<ItemSpawner>(); // ����� ������� �������, ���� ������� �� �����

        if (_spawner == null)
            Debug.LogError("ItemSpawner �� ������ �� �����!", this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (string.IsNullOrEmpty(_resourceName))
        {
            Debug.LogError("ResourceName �� �����!", this);
            return;
        }

        _spawner?.ItemPicked(_resourceName);
        Destroy(gameObject);
    }
}