using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoJogador : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController controller;
    private Transform myCamera;
    private Animator animator;

    private bool estaNoChao;
    [SerializeField]private Transform peDoPersonagem;
    [SerializeField]private LayerMask colisaoLayer;
    private float forcaY;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        myCamera = Camera.main.transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimento = new Vector3(horizontal, 0, vertical);
        movimento = myCamera.TransformDirection(movimento);
        movimento.y = 0;

        controller.Move(movimento * Time.deltaTime * 5);

        if(movimento != Vector3.zero) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movimento), Time.deltaTime * 10);
        }

        animator.SetBool("Mover", movimento != Vector3.zero);

        estaNoChao = Physics.CheckSphere(peDoPersonagem.position, 0.3f, colisaoLayer);
        animator.SetBool("EstaNoChao", estaNoChao);

        if(Input.GetKeyDown(KeyCode.Space) && estaNoChao) {
            forcaY = 5f;
            animator.SetTrigger("Saltar");
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            animator.SetTrigger("AtaqueOne");
        }

        if(Input.GetKeyDown(KeyCode.Mouse1)) {
            animator.SetTrigger("AtaqueTwo");
        }

        if(forcaY > -9.81f) {
            forcaY += -9.81f * Time.deltaTime;
        }

        controller.Move(new Vector3(0, forcaY,0) * Time.deltaTime);
    }
}
