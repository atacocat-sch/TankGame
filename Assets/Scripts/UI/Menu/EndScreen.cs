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
    public Signal allPlayersDeadSignal;

    bool runSetup = false;

    private void Start()
    {
        if (runSetup) return;

        allPlayersDeadSignal.OnRaise += Show;

        runSetup = true;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        allPlayersDeadSignal.OnRaise -= Show;

        runSetup = false;
    }
    
    private void Update()
    {
        float angle = Mathf.PerlinNoise(Time.time * shakeFrequency, 0.5f) * Mathf.PI * 2.0f;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        ((RectTransform)layout).anchoredPosition = direction * shakeStrength;
    }

    private void Show()
    {
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
        SceneManager.LoadScene(0);
    }
}
