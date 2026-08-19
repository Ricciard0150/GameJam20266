using UnityEngine;
using UnityEngine.InputSystem;

public class GatoFPS : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidadeAndar = 3f;
    public float velocidadeCorrer = 5.5f;
    public float aceleracao = 12f;
    public float desaceleracao = 18f;

    [Header("Mouse")]
    public float sensibilidadeMouse = 0.15f;
    public float limiteVertical = 85f;

    [Header("Câmera")]
    public Transform cameraJogador;

    [Header("Character Controller")]
    public CharacterController controller;

    [Header("Gravidade e Pulo")]
    public float gravidade = -20f;
    public float alturaPulo = 1f;

    private float rotacaoX;
    private float velocidadeAtual;
    private float velocidadeVertical;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (controller == null)
            controller = GetComponent<CharacterController>();

        // Começa com velocidade vertical zerada.
        velocidadeVertical = 0f;
    }

    void Update()
    {
        Movimento();
        Camera();
    }

    void Movimento()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            float horizontal = 0f;
            float vertical = 0f;

            if (Keyboard.current.aKey.isPressed)
                horizontal -= 1f;

            if (Keyboard.current.dKey.isPressed)
                horizontal += 1f;

            if (Keyboard.current.sKey.isPressed)
                vertical -= 1f;

            if (Keyboard.current.wKey.isPressed)
                vertical += 1f;

            input = new Vector2(horizontal, vertical).normalized;
        }

        Vector3 direcao =
            transform.right * input.x +
            transform.forward * input.y;

        bool correndo =
            Keyboard.current != null &&
            Keyboard.current.leftShiftKey.isPressed &&
            input.y > 0;

        float velocidadeAlvo =
            correndo ? velocidadeCorrer : velocidadeAndar;

        if (direcao.magnitude > 0.1f)
        {
            velocidadeAtual = Mathf.MoveTowards(
                velocidadeAtual,
                velocidadeAlvo,
                aceleracao * Time.deltaTime
            );
        }
        else
        {
            velocidadeAtual = Mathf.MoveTowards(
                velocidadeAtual,
                0f,
                desaceleracao * Time.deltaTime
            );
        }

        Vector3 movimento = direcao * velocidadeAtual;

        // GRAVIDADE
        if (controller.isGrounded)
        {
            // Mantém o jogador encostado no chão.
            if (velocidadeVertical < 0f)
                velocidadeVertical = -2f;

            // PULO
            if (Keyboard.current != null &&
                Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                velocidadeVertical = Mathf.Sqrt(
                    alturaPulo * -2f * gravidade
                );
            }
        }
        else
        {
            velocidadeVertical += gravidade * Time.deltaTime;
        }

        movimento.y = velocidadeVertical;

        controller.Move(movimento * Time.deltaTime);
    }

    void Camera()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouse = Mouse.current.delta.ReadValue();

        float mouseX = mouse.x * sensibilidadeMouse;
        float mouseY = mouse.y * sensibilidadeMouse;

        // Olhar para os lados
        transform.Rotate(Vector3.up * mouseX);

        // Olhar para cima e para baixo
        rotacaoX -= mouseY;

        rotacaoX = Mathf.Clamp(
            rotacaoX,
            -limiteVertical,
            limiteVertical
        );

        // SOMENTE ROTACIONA A CÂMERA.
        // A POSIÇÃO DELA NÃO É ALTERADA.
        cameraJogador.localRotation = Quaternion.Euler(
            rotacaoX,
            0f,
            0f
        );
    }
}