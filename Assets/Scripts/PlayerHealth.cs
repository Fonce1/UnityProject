using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health = 0f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthSlider;

    private void Start()
    {
        health = maxHealth;
        UpdateHealthBar();
    }

    public void UpdateHealth(float mod)
    {
        health += mod;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0f)
        {
            health = 0f;
            // Завершаем игру при смерти игрока
            GameManager.Instance?.EndGameSession(ItemSpawner.Instance?.score ?? 0);
           
            Debug.Log("Player Death");
        }

        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = health / maxHealth;
        }
    }
}