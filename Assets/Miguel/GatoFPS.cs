using UnityEngine;
using UnityEngine.InputSystem;

public class GatoFPS : MonoBehaviour
{
    public float velocidadeAndar = 3f;
    public float velocidadeCorrer = 5.5f;
    public float aceleracao = 12f;
    public float desaceleracao = 18f;

    public float sensibilidadeMouse = 0.15f;
    public float limiteVertical = 85f;

    public Transform cameraJogador;
    public CharacterController controller;

    public float gravidade = -20f;
    public float alturaPulo = 1f;

    public float intensidadeBalanço = 0.04f;
    public float velocidadeBalanço = 8f;

    private float rotacaoX;
    private float velocidadeAtual;
    private float velocidadeVertical;
    private float tempoBalanço;

    private Vector3 posicaoInicialCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (controller == null)
            controller = GetComponent<CharacterController>();

        posicaoInicialCamera = cameraJogador.localPosition;
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

        Vector3 direcao = transform.right * input.x + transform.forward * input.y;

        bool correndo = Keyboard.current != null &&
                        Keyboard.current.leftShiftKey.isPressed &&
                        input.y > 0;

        float velocidadeAlvo = correndo ? velocidadeCorrer : velocidadeAndar;

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
                0,
                desaceleracao * Time.deltaTime
            );
        }

        Vector3 movimento = direcao * velocidadeAtual;

        if (controller.isGrounded)
        {
            velocidadeVertical = -2f;

            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                velocidadeVertical = Mathf.Sqrt(
                    alturaPulo * -2f * gravidade
                );
            }
        }

        velocidadeVertical += gravidade * Time.deltaTime;

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

        transform.Rotate(Vector3.up * mouseX);

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(
            rotacaoX,
            -limiteVertical,
            limiteVertical
        );

        cameraJogador.localRotation = Quaternion.Euler(
            rotacaoX,
            0f,
            0f
        );

        Vector3 posicaoCamera = posicaoInicialCamera;

        if (velocidadeAtual > 0.2f && controller.isGrounded)
        {
            tempoBalanço += Time.deltaTime *
                            velocidadeBalanço *
                            (velocidadeAtual / velocidadeAndar);

            float movimentoX =
                Mathf.Cos(tempoBalanço) * intensidadeBalanço;

            float movimentoY =
                Mathf.Sin(tempoBalanço * 2f) * intensidadeBalanço;

            posicaoCamera += new Vector3(
                movimentoX,
                movimentoY,
                0
            );
        }

        cameraJogador.localPosition = Vector3.Lerp(
            cameraJogador.localPosition,
            posicaoCamera,
            Time.deltaTime * 10f
        );
    }
}   