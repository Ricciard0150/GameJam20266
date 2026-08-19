using UnityEngine;

public class FundoFlutuante : MonoBehaviour
{
    public float movimento = 3f;
    public float velocidade = 0.5f;

    private RectTransform rectTransform;
    private Vector2 posicaoInicial;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        posicaoInicial = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float movimentoAtual = Mathf.Sin(Time.time * velocidade) * movimento;

        rectTransform.anchoredPosition =
            posicaoInicial + new Vector2(movimentoAtual, 0);
    }
}