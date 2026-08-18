using UnityEngine;
using TMPro; // ← IMPORTANTE!
using System.Collections.Generic;

public class MissionUI_Manual : MonoBehaviour
{
    [System.Serializable]
    public class MissionText
    {
        public string missionName;
        public TextMeshProUGUI missionText; // ← TMP!
    }

    [SerializeField] private List<MissionText> missionTexts = new List<MissionText>();

    void Start()
    {
        UpdateAllTexts();
    }

    public void UpdateAllTexts()
    {
        Debug.Log("📋 Atualizando textos da UI...");

        foreach (MissionText mt in missionTexts)
        {
            if (mt.missionText == null)
            {
                Debug.LogError($"❌ Texto vazio para missão: {mt.missionName}");
                continue;
            }

            bool isCompleted = false;

            if (MissionSystem.Instance != null)
            {
                foreach (Mission mission in MissionSystem.Instance.GetMissions())
                {
                    if (mission.missionName == mt.missionName)
                    {
                        isCompleted = mission.isCompleted;
                        break;
                    }
                }
            }

            if (isCompleted)
            {
                mt.missionText.text = $"<s>{mt.missionName}</s>";
                mt.missionText.color = Color.gray;
                Debug.Log($"   ✅ {mt.missionName} - COMPLETA");
            }
            else
            {
                mt.missionText.text = $"⬜ {mt.missionName}";
                mt.missionText.color = Color.white;
                Debug.Log($"   ⬜ {mt.missionName} - INCOMPLETA");
            }
        }
    }
}