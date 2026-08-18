using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CachorroIA : MonoBehaviour
{
    public Transform player;
    public Transform[] pontosPatrulha;

    public float distanciaVisao = 12f;
    public float anguloVisao = 100f;
    public float distanciaPerda = 18f;
    public float distanciaPonto = 1f;

    public float distanciaAtaque = 2f;

    private NavMeshAgent agent;
    private int pontoAtual;
    private bool perseguindo;
    private bool carregandoJumpscare;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("ERRO: O cachorro não possui NavMeshAgent!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("ERRO: O cachorro não está sobre o NavMesh!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("ERRO: O campo Player do cachorro está vazio!");
            return;
        }

        if (pontosPatrulha != null && pontosPatrulha.Length > 0)
        {
            pontoAtual = 0;
            agent.SetDestination(pontosPatrulha[pontoAtual].position);
        }
        else
        {
            Debug.LogWarning("AVISO: O cachorro não possui pontos de patrulha.");
        }
    }

    void Update()
    {
        if (carregandoJumpscare)
            return;

        if (player == null)
            return;

        float distancia = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distancia <= distanciaAtaque)
        {
            Debug.Log("CACHORRO CHEGOU PERTO DO GATO!");
            CarregarJumpscare();
            return;
        }

        if (PodeVerPlayer())
        {
            perseguindo = true;
        }

        if (perseguindo)
        {
            PerseguirPlayer();
        }
        else
        {
            Patrulhar();
        }
    }

    void Patrulhar()
    {
        if (pontosPatrulha == null || pontosPatrulha.Length == 0)
            return;

        if (!agent.pathPending &&
            agent.remainingDistance <= distanciaPonto)
        {
            pontoAtual++;

            if (pontoAtual >= pontosPatrulha.Length)
                pontoAtual = 0;

            agent.SetDestination(
                pontosPatrulha[pontoAtual].position
            );
        }
    }

    void PerseguirPlayer()
    {
        float distancia = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distancia <= distanciaPerda)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            perseguindo = false;

            if (pontosPatrulha != null &&
                pontosPatrulha.Length > 0)
            {
                agent.SetDestination(
                    pontosPatrulha[pontoAtual].position
                );
            }
        }
    }

    bool PodeVerPlayer()
    {
        Vector3 direcao = player.position - transform.position;
        float distancia = direcao.magnitude;

        if (distancia > distanciaVisao)
            return false;

        float angulo = Vector3.Angle(
            transform.forward,
            direcao
        );

        if (angulo > anguloVisao / 2f)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(
            transform.position + Vector3.up,
            direcao.normalized,
            out hit,
            distancia
        ))
        {
            if (hit.transform == player)
                return true;
        }

        return false;
    }

    void CarregarJumpscare()
    {
        if (carregandoJumpscare)
            return;

        carregandoJumpscare = true;

        if (agent != null)
            agent.isStopped = true;

        Debug.Log("=================================");
        Debug.Log("JUMPSCARE ATIVADO!");
        Debug.Log("Tentando carregar: JumpScare");
        Debug.Log("=================================");

        SceneManager.LoadScene("JumpScare");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            distanciaVisao
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            distanciaPerda
        );

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(
            transform.position,
            distanciaAtaque
        );
    }
}