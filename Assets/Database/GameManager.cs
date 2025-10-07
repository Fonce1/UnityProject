using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int currentSessionId = -1;
    [SerializeField] private int userId = 6; // Теперь с возможностью настройки в инспекторе
    private float startTime;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartNewSession();
    }

    public void StartNewSession()
    {
        if (currentSessionId != -1)
        {
            Debug.LogWarning("Active session found. Ending it...");
            EndGameSession(0);
        }

        startTime = Time.time;
        gameEnded = false;

        try
        {
            currentSessionId = KinNoct.StartGameSession(userId);
            if (currentSessionId == -1)
            {
                Debug.LogError("Failed to start game session");
                return;
            }
            Debug.Log($"New session started: ID {currentSessionId}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Session error: {ex.Message}");
            currentSessionId = -1;
        }
    }

    public void RegisterResourceCollection(string resourceName)
    {
        if (string.IsNullOrEmpty(resourceName))
        {
            Debug.LogError("Invalid resource name!");
            return;
        }

        if (userId <= 0)
        {
            Debug.LogError("Invalid user ID");
            return;
        }

        if (currentSessionId == -1 || gameEnded)
        {
            Debug.LogWarning("No active session to record resource");
            return;
        }

        try
        {
            Debug.Log($"Registering resource '{resourceName}' for UserID: {userId}");
            bool success = KinNoct.CollectResource(userId, resourceName);
            if (!success) Debug.LogWarning($"Resource '{resourceName}' not registered");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Resource error: {ex.Message}");
        }
    }


    public void EndGameSession(int finalScore)
    {
        if (gameEnded || currentSessionId == -1) return;

        gameEnded = true;
        float duration = Time.time - startTime;

        try
        {
            bool success = KinNoct.EndGameSession(
                currentSessionId,
                finalScore,
                (int)duration
            );

            Debug.Log(success ?
                $"Session saved! Score: {finalScore}, Time: {duration:F0}s" :
                "Session save failed");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Session end error: {ex.Message}");
        }
        finally
        {
            currentSessionId = -1;
        }
    }

    // Новый метод для установки пользователя
    public void SetUserId(int newUserId)
    {
        if (newUserId <= 0)
        {
            Debug.LogError("Invalid user ID");
            return;
        }

        if (currentSessionId != -1)
        {
            Debug.LogWarning("Changing user during active session");
            EndGameSession(0);
        }

        userId = newUserId;
    }
}