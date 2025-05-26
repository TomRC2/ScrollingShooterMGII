using UnityEngine;
using UnityEngine.UI;

public class BossHealthHUD : MonoBehaviour
{
    public Slider bossHealthSlider;
    public BossController boss;

    [Header("Fondos del HUD (Cubos)")]
    public Renderer blockLeft;
    public Renderer blockRight;

    void Update()
    {
        if (boss != null)
        {
            bossHealthSlider.maxValue = boss.maxHealth;
            bossHealthSlider.value = boss.currentHealth;
        }
    }

    public void SetRedTheme()
    {
        if (blockLeft != null)
            blockLeft.material.color = Color.red;

        if (blockRight != null)
            blockRight.material.color = Color.red;
    }
}
