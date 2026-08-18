using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private Transform missionContainer;

    private List<GameObject> missionItems = new List<GameObject>();

    void Start()
    {
        Debug.Log("✅ MissionUI inicializado");

        if (missionPrefab == null)
            Debug.LogError("❌ Mission Prefab está vazio!");

        if (missionContainer == null)
            Debug.LogError("❌ Mission Container está vazio!");
    }

    public void UpdateUI(List<Mission> missions)
    {
        // Limpa itens antigos
        foreach (GameObject item in missionItems)
        {
            Destroy(item);
        }
        missionItems.Clear();

        // Cria novos itens
        foreach (Mission mission in missions)
        {
            if (missionPrefab == null)
            {
                Debug.LogError("❌ Mission Prefab é NULL!");
                return;
            }

            GameObject newItem = Instantiate(missionPrefab, missionContainer);
            missionItems.Add(newItem);

            Text text = newItem.GetComponentInChildren<Text>();
            if (text != null)
            {
                if (mission.isCompleted)
                {
                    text.text = $"✅ {mission.missionName}";
                }
                else
                {
                    text.text = $"⬜ {mission.missionName}";
                }
            }
            else
            {
                Debug.LogError($"❌ Text component não encontrado no prefab!");
            }
        }
    }
}