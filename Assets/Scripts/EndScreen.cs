using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public TMP_Text flavourTextElement;
    public Transform layout;

    [Space]
    public TextAsset flavourTextSource;

    [Space]
    public float shakeStrength;
    public float shakeFrequency;

    [Space]
    public Team playerTeam;

    bool runSetup = false;

    private void Start()
    {
        if (runSetup) return;

        foreach (TeamPlayer player in playerTeam.players)
        {
            if (player.TryGetComponent(out Health health))
            {
                health.DeathEvent += Show;
            }
        }

        runSetup = true;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        foreach (TeamPlayer player in playerTeam.players)
        {
            if (player.TryGetComponent(out Health health))
            {
                health.DeathEvent -= Show;
            }
        }

        runSetup = false;
    }
    
    private void Update()
    {
        float angle = Mathf.PerlinNoise(Time.time * shakeFrequency, 0.5f) * Mathf.PI * 2.0f;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        ((RectTransform)layout).anchoredPosition = direction * shakeStrength;
    }

    private void Show(DamageArgs ctx)
    {
        foreach (TeamPlayer player in playerTeam.players)
        {
            if (player.TryGetComponent(out Health health)) continue;
            if (health.currentHealth < 0.0f) continue;

            return;
        }

        gameObject.SetActive(true);

        string[] flavourTexts = flavourTextSource.text.Split('\n');
        flavourTextElement.text = flavourTexts[Random.Range(0, flavourTexts.Length)];
    }

    public void TryAgain ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
