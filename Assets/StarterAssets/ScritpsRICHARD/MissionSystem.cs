using UnityEngine;
using System.Collections.Generic;

public class MissionSystem : MonoBehaviour
{
    public static MissionSystem Instance;

    [SerializeField] private List<Mission> missions = new List<Mission>();
    [SerializeField] private MissionUI missionUI;

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
        if (missionUI != null)
        {
            missionUI.UpdateUI(missions);
            Debug.Log($"📋 UI atualizada com {missions.Count} missões");
        }
        else
        {
            Debug.LogError("❌ MissionUI não está conectado no MissionSystem!");
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

                if (missionUI != null)
                {
                    missionUI.UpdateUI(missions);
                }

                CheckAllCompleted();
                return;
            }
        }

        Debug.LogWarning($"⚠️ Missão não encontrada: {missionName}");
    }

    public List<Mission> GetMissions() => missions;

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
        }
    }
}