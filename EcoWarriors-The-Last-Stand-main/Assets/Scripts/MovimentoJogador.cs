using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoJogador : MonoBehaviour
{
    private CharacterController controller;
    private Transform myCamera;
    private Animator animator;

    private bool estaNoChao;
    [SerializeField] private Transform peDoPersonagem;
    [SerializeField] private LayerMask colisaoLayer;
    private float forcaY;

    // Variáveis para queda
    private float alturaInicial;
    private bool caindo;

    // Referência ao script de Vida
    private Vida vidaScript;

void Start()
{
    controller = GetComponent<CharacterController>();
    myCamera = Camera.main.transform;
    animator = GetComponent<Animator>();

    // Força a associação do script Vida
    vidaScript = GetComponent<Vida>();

    if (vidaScript == null)
    {
        Debug.LogError("Script Vida não encontrado no GameObject.");
    }

    caindo = false;
}

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(horizontal, 0, vertical);
        movimento = myCamera.TransformDirection(movimento);
        movimento.y = 0;

        controller.Move(movimento * Time.deltaTime * 5);

        if (movimento != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), Time.deltaTime * 10);
        }

        animator.SetBool("Mover", movimento != Vector3.zero);

        estaNoChao = Physics.CheckSphere(peDoPersonagem.position, 0.3f, colisaoLayer);
        animator.SetBool("EstaNoChao", estaNoChao);

        // Detectar início da queda
        if (!estaNoChao && !caindo)
        {
            alturaInicial = transform.position.y;  // Captura a altura inicial da queda
            caindo = true;
        }

        // Detectar aterrissagem
        if (estaNoChao && caindo)
        {
            float alturaFinal = transform.position.y;
            float alturaQueda = alturaInicial - alturaFinal;

            // Se a queda for maior que 3 metros, aplica dano proporcional
            if (alturaQueda > 3f)
            {
                int dano = (int)(alturaQueda * 10);  // Cada metro de queda causa 10 de dano
                vidaScript.AplicarDano(dano);  // Aplica o dano baseado na altura da queda
            }

            caindo = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && estaNoChao)
        {
            forcaY = 5f;
            animator.SetTrigger("Saltar");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("AtaqueOne");
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            animator.SetTrigger("AtaqueTwo");
        }

        if (forcaY > -9.81f)
        {
            forcaY += -9.81f * Time.deltaTime;
        }

        controller.Move(new Vector3(0, forcaY, 0) * Time.deltaTime);
    }
}
