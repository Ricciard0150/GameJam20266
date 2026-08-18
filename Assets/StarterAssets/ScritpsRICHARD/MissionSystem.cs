using UnityEngine;
using System.Collections.Generic;

public class MissionSystem : MonoBehaviour
{
    public static MissionSystem Instance;

    [SerializeField] private List<Mission> missions = new List<Mission>();
    [SerializeField] private MissionUI_Manual missionUIManual; // ← UI MANUAL

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("✅ MissionSystem criado!");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Atualiza a UI no começo
        if (missionUIManual != null)
        {
            missionUIManual.UpdateAllTexts();
            Debug.Log($"📋 UI atualizada com {missions.Count} missões");
        }
        else
        {
            Debug.LogWarning("⚠️ MissionUI_Manual não está conectado!");
        }
    }

    public void CompleteMission(string missionName)
    {
        Debug.Log($"🔍 Procurando missão: {missionName}");

        foreach (Mission mission in missions)
        {
            if (mission.missionName == missionName && !mission.isCompleted)
            {
                mission.isCompleted = true;
                Debug.Log($"✅ Missão completada: {missionName}");

                // 🔥 ATUALIZA A UI MANUAL
                if (missionUIManual != null)
                {
                    missionUIManual.UpdateAllTexts();
                    Debug.Log("📋 UI atualizada!");
                }

                CheckAllCompleted();
                return;
            }
        }

        Debug.LogWarning($"⚠️ Missão não encontrada: {missionName}");
    }

    public List<Mission> GetMissions()
    {
        return missions;
    }

    public bool IsMissionCompleted(string missionName)
    {
        foreach (Mission mission in missions)
        {
            if (mission.missionName == missionName)
            {
                return mission.isCompleted;
            }
        }
        return false;
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
            // Aqui você pode ativar algo, tipo: portal.SetActive(true);
        }
    }
}