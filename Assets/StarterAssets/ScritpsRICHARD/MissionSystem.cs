using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events; // Adicione isso

public class MissionSystem : MonoBehaviour
{
    public static MissionSystem Instance;

    [SerializeField] private List<Mission> missions = new List<Mission>();
    [SerializeField] private MissionUI_Manual missionUIManual;

    // 🔥 NOVO: Evento para notificar quando o progresso muda
    public UnityEvent OnProgressChanged;

    [Header("Fim do Jogo")]
    [SerializeField] private string nextSceneName = "WinScreen";
    [SerializeField] private float delayBeforeFade = 1.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (missionUIManual != null)
        {
            missionUIManual.UpdateAllTexts();
        }

        // 🔥 NOTIFICA O PROGRESSO INICIAL
        OnProgressChanged?.Invoke();
    }

    public void CompleteMission(string missionName)
    {
        foreach (Mission mission in missions)
        {
            if (mission.missionName == missionName && !mission.isCompleted)
            {
                mission.isCompleted = true;
                Debug.Log($"✅ Missão completada: {missionName}");

                if (missionUIManual != null)
                {
                    missionUIManual.UpdateAllTexts();
                }

                // 🔥 NOTIFICA QUE O PROGRESSO MUDOU
                OnProgressChanged?.Invoke();

                CheckAllCompleted();
                return;
            }
        }
    }

    public List<Mission> GetMissions() => missions;

    // Método para obter o progresso
    public float GetProgress()
    {
        int total = missions.Count;
        if (total == 0) return 0f;

        int completed = 0;
        foreach (Mission mission in missions)
        {
            if (mission.isCompleted)
                completed++;
        }

        return (float)completed / total;
    }

    // Método para obter o progresso como texto
    public string GetProgressText()
    {
        int total = missions.Count;
        if (total == 0) return "0/0";

        int completed = 0;
        foreach (Mission mission in missions)
        {
            if (mission.isCompleted)
                completed++;
        }

        return $"{completed}/{total}";
    }

    private void CheckAllCompleted()
    {
        int total = missions.Count;
        int completed = 0;

        foreach (Mission mission in missions)
        {
            if (mission.isCompleted)
                completed++;
        }

        if (completed >= total && total > 0)
        {
            Debug.Log("🎉 TODAS AS MISSÕES COMPLETADAS!");
            StartCoroutine(CompleteGame());
        }
    }

    private System.Collections.IEnumerator CompleteGame()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        if (SceneFader.Instance != null)
        {
            yield return StartCoroutine(SceneFader.Instance.FadeOut());
        }

        yield return new WaitForSeconds(0.5f);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ Nenhuma cena definida em 'Next Scene Name'!");
        }

        if (SceneFader.Instance != null)
        {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(SceneFader.Instance.FadeIn());
        }
    }
}