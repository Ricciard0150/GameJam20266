using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float aumento = 1.08f;
    public float velocidade = 10f;

    private RectTransform rectTransform;
    private Vector3 escalaOriginal;
    private Vector3 escalaAlvo;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Guarda o tamanho original do botão
        escalaOriginal = rectTransform.localScale;
        escalaAlvo = escalaOriginal;
    }

    void Update()
    {
        rectTransform.localScale = Vector3.Lerp(
            rectTransform.localScale,
            escalaAlvo,
            Time.deltaTime * velocidade
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Aumenta baseado no tamanho ORIGINAL
        escalaAlvo = escalaOriginal * aumento;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Volta exatamente para o tamanho original
        escalaAlvo = escalaOriginal;
    }
}