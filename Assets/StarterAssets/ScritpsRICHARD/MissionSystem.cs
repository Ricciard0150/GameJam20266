using UnityEngine;
using System.Collections.Generic;

public class MissionSystem : MonoBehaviour
{
    public static MissionSystem Instance;

    [SerializeField] private List<Mission> missions = new List<Mission>();
    [SerializeField] private MissionUI_Manual missionUIManual;

    [Header("Fim do Jogo")]
    [SerializeField] private string nextSceneName = "WinScreen"; // Nome da cena final
    [SerializeField] private float delayBeforeFade = 1.5f; // Espera antes do fade

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

                CheckAllCompleted();
                return;
            }
        }
    }

    public List<Mission> GetMissions() => missions;

    // 🔥 VERIFICA SE TODAS AS MISSÕES FORAM COMPLETADAS
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

    // 🔥 CORRUTINA PARA FINALIZAR O JOGO
    private System.Collections.IEnumerator CompleteGame()
    {
        // Espera um pouco antes de começar o fade
        yield return new WaitForSeconds(delayBeforeFade);

        // Faz o fade out
        if (SceneFader.Instance != null)
        {
            yield return StartCoroutine(SceneFader.Instance.FadeOut());
        }

        // Espera mais um pouco
        yield return new WaitForSeconds(0.5f);

        // Carrega a próxima cena
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ Nenhuma cena definida em 'Next Scene Name'!");
        }

        // Se tiver SceneFader, faz fade in na nova cena
        if (SceneFader.Instance != null)
        {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(SceneFader.Instance.FadeIn());
        }
    }
}