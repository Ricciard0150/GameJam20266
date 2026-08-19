using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeCanvas : MonoBehaviour
{
    public float duracao = 0.3f;

    private Image painel;

    void Awake()
    {
        painel = GetComponent<Image>();

        Color cor = painel.color;
        cor.a = 0f;
        painel.color = cor;
    }

    void OnEnable()
    {
        StartCoroutine(FadeParaPreto());
    }

    IEnumerator FadeParaPreto()
    {
        Color cor = painel.color;
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;

            cor.a = Mathf.Lerp(0f, 1f, tempo / duracao);
            painel.color = cor;

            yield return null;
        }

        cor.a = 1f;
        painel.color = cor;
    }
}