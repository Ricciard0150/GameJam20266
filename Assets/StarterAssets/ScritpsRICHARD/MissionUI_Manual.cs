using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissionUI_Manual : MonoBehaviour
{
    [System.Serializable]
    public class MissionText
    {
        public string missionName;  // Nome da missão
        public Text missionText;    // O texto na UI
    }

    [SerializeField] private List<MissionText> missionTexts = new List<MissionText>();

    void Start()
    {
        UpdateAllTexts();
    }

    public void UpdateAllTexts()
    {
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
            }
            else
            {
                mt.missionText.text = $"⬜ {mt.missionName}";
                mt.missionText.color = Color.white;
            }
        }
    }
}