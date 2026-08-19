using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MissionUI_Manual : MonoBehaviour
{
    [System.Serializable]
    public class MissionText
    {
        public string missionName;
        public TextMeshProUGUI missionText;
    }

    [SerializeField] private List<MissionText> missionTexts = new List<MissionText>();

    void Start()
    {
        Debug.Log("🚀 MissionUI_Manual Start!");
        UpdateAllTexts();
    }

    public void UpdateAllTexts()
    {
        Debug.Log("📋 UpdateAllTexts CHAMADO!");

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

            // 🔥 APLICA O ESTILO CERTO
            if (isCompleted)
            {
                mt.missionText.text = $"<s>{mt.missionName}</s>";
                mt.missionText.color = new Color(0.3f, 0.3f, 0.3f); // Cinza escuro
                Debug.Log($"   ✅ {mt.missionName} - COMPLETA (riscado e cinza)");
            }
            else
            {
                mt.missionText.text = $"⬜ {mt.missionName}";
                mt.missionText.color = Color.white;
                Debug.Log($"   ⬜ {mt.missionName} - INCOMPLETA");
            }
        }
    }

    // 🔥 FORÇA ATUALIZAÇÃO
    public void ForceUpdate()
    {
        Debug.Log("🔥 ForceUpdate CHAMADO!");
        UpdateAllTexts();
    }
}