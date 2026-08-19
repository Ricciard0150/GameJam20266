using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class CachorroIA : MonoBehaviour
{
    public Transform player;
    public Transform[] pontosPatrulha;

    public float distanciaVisao = 15f;
    public float anguloVisao = 120f;
    public float distanciaPonto = 1f;

    private NavMeshAgent agent;
    private int pontoAtual;
    private bool perseguindo;
    private bool carregandoJumpscare;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("O cachorro não possui NavMeshAgent!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("O cachorro não está sobre o NavMesh!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("O Player não foi colocado no campo Player!");
            return;
        }

        agent.speed = 3.5f;
        agent.angularSpeed = 360f;
        agent.acceleration = 8f;
        agent.stoppingDistance = 0f;

        if (pontosPatrulha != null && pontosPatrulha.Length > 0)
        {
            pontoAtual = 0;
            agent.SetDestination(pontosPatrulha[pontoAtual].position);
        }
    }

    void Update()
    {
        if (carregandoJumpscare)
            return;

        if (player == null)
            return;

        if (PodeVerPlayer())
        {
            perseguindo = true;

            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            if (perseguindo)
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

            Patrulhar();
        }
    }

    void Patrulhar()
    {
        if (pontosPatrulha == null ||
            pontosPatrulha.Length == 0)
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

    bool PodeVerPlayer()
    {
        Vector3 origem = transform.position + Vector3.up * 1.2f;
        Vector3 destino = player.position + Vector3.up * 0.5f;

        Vector3 direcao = destino - origem;
        float distancia = direcao.magnitude;

        if (distancia > distanciaVisao)
            return false;

        float angulo = Vector3.Angle(
            transform.forward,
            direcao
        );

        if (angulo > anguloVisao / 2f)
            return false;

        if (Physics.Linecast(
            origem,
            destino,
            out RaycastHit hit
        ))
        {
            if (hit.transform == player ||
                hit.transform.IsChildOf(player))
            {
                return true;
            }

            return false;
        }

        return true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == player ||
            collision.transform.IsChildOf(player))
        {
            CarregarJumpscare();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player ||
            other.transform.IsChildOf(player))
        {
            CarregarJumpscare();
        }
    }

    void CarregarJumpscare()
    {
        if (carregandoJumpscare)
            return;

        carregandoJumpscare = true;

        if (agent != null)
            agent.isStopped = true;

        Debug.Log("JUMPSCARE! O cachorro colidiu com o gato.");

        SceneManager.LoadScene("Jumpscare");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            distanciaVisao
        );
    }
}   