using UnityEngine;

public class MissionLightSwitch : MonoBehaviour
{
    [SerializeField] private string missionName;

    public void OnTurnOn()
    {
        Debug.Log($"💡 LightSwitch ligado! Mission Name: {missionName}");

        if (MissionSystem.Instance == null)
        {
            Debug.LogError("❌ MissionSystem.Instance é NULL!");
            return;
        }

        if (string.IsNullOrEmpty(missionName))
        {
            Debug.LogError("❌ Mission Name está vazio!");
            return;
        }

        MissionSystem.Instance.CompleteMission(missionName);
    }
}