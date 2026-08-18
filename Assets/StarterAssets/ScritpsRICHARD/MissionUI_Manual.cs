using UnityEngine;
using System.Collections.Generic;
using TMPro; // ← TextMeshPro

public class MissionUI_Manual : MonoBehaviour
{
    [System.Serializable]
    public class MissionText
    {
        public string missionName;  // Nome da missão
        public TextMeshProUGUI missionText; // ← TMP_UGUI (para UI)!
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

            // Verifica se a missão está completa
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

            // Atualiza o texto
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