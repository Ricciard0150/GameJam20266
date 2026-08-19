using UnityEngine;
using System.Collections;

public class BotaoEntrada : MonoBehaviour
{
    public float atraso = 0f;
    public float duracao = 0.4f;

    private RectTransform rectTransform;
    private Vector2 posicaoFinal;
    private CanvasGroup canvasGroup;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        posicaoFinal = rectTransform.anchoredPosition;

        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        StartCoroutine(Entrar());
    }

    IEnumerator Entrar()
    {
        // Espera o tempo definido
        yield return new WaitForSeconds(atraso);

        Vector2 posicaoInicial = posicaoFinal + new Vector2(0, -100);

        rectTransform.anchoredPosition = posicaoInicial;
        canvasGroup.alpha = 0;

        float tempo = 0;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            float t = tempo / duracao;

            // Suavização
            t = 1 - Mathf.Pow(1 - t, 3);

            rectTransform.anchoredPosition =
                Vector2.Lerp(posicaoInicial, posicaoFinal, t);

            canvasGroup.alpha = t;

            yield return null;
        }

        rectTransform.anchoredPosition = posicaoFinal;
        canvasGroup.alpha = 1;
    }
}