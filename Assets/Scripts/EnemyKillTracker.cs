using UnityEngine;

public class EnemyKillTracker : MonoBehaviour
{
    public int killsToSpawnBoss = 20;
    private int killCount = 0;

    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public GameObject enemySpawner;
    public GameObject bossHealthBarCanvas;

    public GameObject[] redHudBlocks;
    public Material redMaterial;

    private bool bossSpawned = false;

    public void RegisterKill()
    {
        if (bossSpawned) return;

        killCount++;

        if (killCount >= killsToSpawnBoss)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        bossSpawned = true;

        if (bossPrefab != null && bossSpawnPoint != null)
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

        if (enemySpawner != null)
            enemySpawner.SetActive(false);

        if (bossHealthBarCanvas != null)
            bossHealthBarCanvas.SetActive(true);

        SetRedTheme();
    }

    void SetRedTheme()
    {
        foreach (GameObject block in redHudBlocks)
        {
            Renderer renderer = block.GetComponent<Renderer>();
            if (renderer != null && redMaterial != null)
                renderer.material = redMaterial;
        }
    }
}
