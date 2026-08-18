using UnityEngine;
using TMPro;

public class ContadorFPS : MonoBehaviour
{
    public TMP_Text textoFPS;

    public float intervaloAtualizacao = 0.5f;

    private float tempo;
    private int frames;

    void Update()
    {
        frames++;
        tempo += Time.unscaledDeltaTime;

        if (tempo >= intervaloAtualizacao)
        {
            float fps = frames / tempo;

            textoFPS.text = "FPS: " + Mathf.RoundToInt(fps);

            frames = 0;
            tempo = 0f;
        }
    }
}