using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SwitchState : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOn = true;
    [SerializeField] private UnityEvent OnTurnOff;
    [SerializeField] private UnityEvent OnCompleteMission;

    private Outline outline;
    private bool isCompleted = false;

    public void HideOutline()
    {
        if (outline != null)
            outline.enabled = false;
    }

    public void ShowOutline()
    {
        if (outline != null)
            outline.enabled = true;
    }

    public void Interact()
    {
        if (isCompleted)
        {
            Debug.Log("⚠️ Já foi desligado!");
            return;
        }

        if (isOn)
        {
            Debug.Log("🔴 Desligando: " + gameObject.name);
            OnTurnOff.Invoke();
            OnCompleteMission.Invoke();
            isOn = false;
            isCompleted = true;
        }
        else
        {
            Debug.Log("⚠️ Já está desligado!");
        }
    }

    private void Start()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;

        isOn = true;
        Debug.Log($"✅ SwitchState inicializado em: {gameObject.name}");
    }
}