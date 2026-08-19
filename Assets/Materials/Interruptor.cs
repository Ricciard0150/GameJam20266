using UnityEngine;

public class Interruptor : MonoBehaviour
{
    public float rotacaoAtivado = -45f;
    public float velocidade = 8f;

    private Quaternion rotacaoDesligado;
    private Quaternion rotacaoLigado;

    private bool ativado = false;

    void Start()
    {
        rotacaoDesligado = transform.localRotation;

        rotacaoLigado = rotacaoDesligado * Quaternion.Euler(rotacaoAtivado, 0, 0);
    }

    void Update()
    {
        Quaternion alvo = ativado ? rotacaoLigado : rotacaoDesligado;

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            alvo,
            Time.deltaTime * velocidade
        );
    }

    public void Ativar()
    {
        ativado = true;
    }

    public void Desativar()
    {
        ativado = false;
    }
}