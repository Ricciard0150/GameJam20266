using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MissionProgressBar : MonoBehaviour
{
    [Header("UI Referências")]
    [SerializeField] private Image progressFillImage; // A imagem que vai preencher
    [SerializeField] private GameObject missionCompleteIcon; // Opcional: ícone de conclusão

    [Header("Configurações")]
    [SerializeField] private float fillSpeed = 2f; // Velocidade da animação de preenchimento

    private float currentFill = 0f;
    private float targetFill = 0f;

    void Start()
    {
        if (progressFillImage == null)
        {
            Debug.LogError("❌ Progress Fill Image não atribuída!");
            return;
        }

        // Configura a imagem como Filled (horizontal)
        progressFillImage.type = Image.Type.Filled;
        progressFillImage.fillMethod = Image.FillMethod.Horizontal;
        progressFillImage.fillOrigin = (int)Image.OriginHorizontal.Left;

        // Atualiza o progresso inicial
        UpdateProgress();

        // Se tiver ícone de conclusão, desativa
        if (missionCompleteIcon != null)
            missionCompleteIcon.SetActive(false);
    }

    void Update()
    {
        // Anima o preenchimento suavemente
        if (Mathf.Abs(currentFill - targetFill) > 0.001f)
        {
            currentFill = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * fillSpeed);
            progressFillImage.fillAmount = currentFill;
        }
        else
        {
            progressFillImage.fillAmount = targetFill;
        }

        // Mostra ícone de conclusão se todas as missões foram completadas
        if (missionCompleteIcon != null)
        {
            bool allComplete = targetFill >= 1f;
            missionCompleteIcon.SetActive(allComplete);
        }
    }

    public void UpdateProgress()
    {
        if (MissionSystem.Instance == null)
        {
            Debug.LogWarning("⚠️ MissionSystem.Instance é null!");
            return;
        }

        List<Mission> missions = MissionSystem.Instance.GetMissions();

        if (missions == null || missions.Count == 0)
        {
            targetFill = 0f;
            return;
        }

        int total = missions.Count;
        int completed = 0;

        foreach (Mission mission in missions)
        {
            if (mission.isCompleted)
                completed++;
        }

        targetFill = (float)completed / total;

        Debug.Log($"📊 Progresso: {completed}/{total}");
    }

    // Chamado quando uma missão é completada
    public void OnMissionCompleted()
    {
        UpdateProgress();
    }

    // Reseta a barra
    public void ResetProgress()
    {
        targetFill = 0f;
        currentFill = 0f;
        progressFillImage.fillAmount = 0f;

        if (missionCompleteIcon != null)
            missionCompleteIcon.SetActive(false);
    }
}