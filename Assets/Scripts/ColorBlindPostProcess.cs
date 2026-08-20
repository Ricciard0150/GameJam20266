using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorblindPostProcess : MonoBehaviour
{
    public Volume volume;

    private ColorAdjustments colorAdjustments;

    private void Awake()
    {
        // Não deixa duplicar quando trocar de cena
        DontDestroyOnLoad(gameObject);

        if (volume == null)
        {
            Debug.LogError("ColorblindPostProcess: coloque o Global Volume no campo Volume.");
            return;
        }

        if (!volume.profile.TryGet(out colorAdjustments))
        {
            Debug.LogError("ColorblindPostProcess: Color Adjustments não encontrado.");
            return;
        }

        Normal();
    }

    public void Normal()
    {
        Aplicar(0f, 0f, 0f, Color.white);
    }

    public void Protanopia()
    {
        Aplicar(0f, 5f, -10f, new Color(1f, 0.75f, 0.75f));
    }

    public void Deuteranopia()
    {
        Aplicar(0f, 5f, 10f, new Color(0.80f, 1f, 0.75f));
    }

    public void Tritanopia()
    {
        Aplicar(0f, 5f, 20f, new Color(0.75f, 0.85f, 1f));
    }

    private void Aplicar(
        float exposure,
        float contrast,
        float hue,
        Color filter)
    {
        colorAdjustments.postExposure.value = exposure;
        colorAdjustments.contrast.value = contrast;
        colorAdjustments.hueShift.value = hue;
        colorAdjustments.saturation.value = -20f;
        colorAdjustments.colorFilter.value = filter;
    }
}