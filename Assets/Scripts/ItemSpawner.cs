using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance { get; private set; }

    [Header("Настройки спавна")]
    public GameObject[] itemPrefabs;
    public int maxItemsOnField = 2;
    public float spawnCheckRadius = 0.5f;
    public LayerMask obstacleLayers;

    [Header("Границы спавна")]
    public Vector2 spawnAreaMin = new(-10, -10);
    public Vector2 spawnAreaMax = new(10, 10);

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Canvas mainCanvas;

    [Header("Скорость")]
    public float playerSpeedIncrease = 0.2f;
    public float enemySpeedIncrease = 0.18f;
    public float maxEnemySpeedMultiplier = 3f;
    public float speedIncreasePerPoint = 0.1f;
    public float maxSpeedMultiplier = 2f;

    public int score = 0;
    private List<GameObject> spawnedItems = new();
    private Player player;
    private Enemy[] enemies;
    private float _lastSpawnCheckTime;
    private const float SPAWN_CHECK_INTERVAL = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        InitializeUI();
        FindPlayerAndEnemies();
        UpdateScore();
        SpawnInitialItems();
    }

    void InitializeUI()
    {
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();

        if (mainCanvas != null && !mainCanvas.gameObject.activeInHierarchy)
            mainCanvas.gameObject.SetActive(true);

    }

    private void OnDrawGizmosSelected()
    {
        // Рисуем зону спавна в сцене
        Gizmos.color = Color.green;
        Vector3 center = (spawnAreaMin + spawnAreaMax) / 2;
        Vector3 size = new(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 0);
        Gizmos.DrawWireCube(center, size);
    }

    void FindPlayerAndEnemies()
    {
        player = FindFirstObjectByType<Player>();
        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
    }

    void Update()
    {
        if (Time.time - _lastSpawnCheckTime > SPAWN_CHECK_INTERVAL)
        {
            CleanupItems();
            TrySpawnIfNeeded();
            _lastSpawnCheckTime = Time.time;
        }
    }

    void CleanupItems() => spawnedItems.RemoveAll(item => item == null);
    void TrySpawnIfNeeded()
    {
        if (spawnedItems.Count < maxItemsOnField)
            TrySpawnItem();
    }

    void SpawnInitialItems()
    {
        for (int i = 0; i < maxItemsOnField; i++)
        {
            TrySpawnItem();
        }
    }

    void TrySpawnItem()
    {
        if (itemPrefabs.Length == 0) return;

        if (TryFindValidSpawnPosition(out Vector2 spawnPosition))
        {
            GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)];
            GameObject newItem = Instantiate(prefab, spawnPosition, Quaternion.identity);
            newItem.AddComponent<ItemPickup>();

            spawnedItems.Add(newItem);
        }
        else
        {
            Debug.LogWarning("Не удалось найти допустимую позицию для предмета.");
        }
    }

    bool TryFindValidSpawnPosition(out Vector2 position)
    {
        for (int i = 0; i < 100; i++)
        {
            Vector2 randomPos = new(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            // Проверяем коллизии только с препятствиями, игнорируя врагов и игрока
            Collider2D hit = Physics2D.OverlapCircle(randomPos, spawnCheckRadius, obstacleLayers);

            if (hit == null) // Если нет коллизий с указанными слоями
            {
                position = randomPos;
                return true;
            }
        }

        position = Vector2.zero;
        return false;
    }

    void IncreaseSpeeds()
    {
        // Ускорение игрока
        if (player != null)
        {
            // Рассчитываем новую скорость с ограничением
            float newPlayerSpeed = player.OriginalMovingSpeed + (playerSpeedIncrease * score);
            player.movingSpeed = Mathf.Min(newPlayerSpeed,
                                        player.OriginalMovingSpeed * maxSpeedMultiplier);
        }

        // Ускорение врагов
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                // Базовый расчет скорости врага
                float speedBoost = enemySpeedIncrease * score;

                // Применяем ускорение с ограничением
                enemy.chaseSpeed = Mathf.Min(
                    enemy.originalChaseSpeed + speedBoost,
                    enemy.originalChaseSpeed * maxEnemySpeedMultiplier
                );

                enemy.speed = Mathf.Min(
                    enemy.originalSpeed + speedBoost,
                    enemy.originalSpeed * maxEnemySpeedMultiplier
                );
            }
        }

        // Обновляем список врагов (на случай появления новых)
        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
    }

    public void ItemPicked(string resourceName)
    {
        score++;
        UpdateScore();
        IncreaseSpeeds();

        // Регистрируем сбор ресурса в GameManager
        GameManager.Instance?.RegisterResourceCollection(resourceName);
    }

    void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Очки: {score}";
        }
    }
}
