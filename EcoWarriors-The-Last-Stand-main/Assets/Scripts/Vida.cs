using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour
{
    private int vidaAtual;
    private int vidaTotal = 100;

    [SerializeField] private BarraDeVida barraDeVida;// Alterar o nome para evitar conflito
    private Color corOriginal;

    public float knockbackForce = 7f; // Intensidade do empurrão
    private Vector3 knockbackDirection; // Para armazenar a direção do empurrão
    private bool sendoEmpurrado = false; // Controle se está sendo empurrado

    private CharacterController characterController; // Para aplicar o knockback com CharacterController

    private SkinnedMeshRenderer meuRenderer; // Alterar para SkinnedMeshRenderer

private void Start()
{
    vidaAtual = vidaTotal;
    barraDeVida.AlterarBarraDeVida(vidaAtual, vidaTotal);

    // Obtém o componente SkinnedMeshRenderer do objeto para alterar sua cor
    meuRenderer = GetComponentInChildren<SkinnedMeshRenderer>(); // Para personagens humanoides, geralmente o SkinnedMeshRenderer está em um objeto filho

    if (meuRenderer != null)
    {
        corOriginal = meuRenderer.material.color;
    }
    else
    {
        Debug.LogError("SkinnedMeshRenderer não encontrado!");
    }

    // Obtém o CharacterController do jogador
    characterController = GetComponent<CharacterController>();
}

    // Altere o método AplicarDano de private para public
    public void AplicarDano(int dano)
    {
        vidaAtual -= dano;

        barraDeVida.AlterarBarraDeVida(vidaAtual, vidaTotal);

        // Mudar a cor para vermelho ao tomar dano
        StartCoroutine(MudarCorTemporariamente());
    }

    private IEnumerator MudarCorTemporariamente()
    {
        meuRenderer.material.color = Color.red; // Mudar para vermelho
        yield return new WaitForSeconds(0.2f); // Esperar 0.2 segundos
        meuRenderer.material.color = corOriginal; // Voltar para a cor original
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Inimigo"))
        {
            AplicarDano(10);
            EmpurrarJogador(other.ClosestPoint(transform.position)); // Aplicar o empurrão
        }
    }

    // Aplica o efeito de empurrão no jogador ao colidir com um inimigo
    private void EmpurrarJogador(Vector3 pontoDeContato)
    {
        // Calcula a direção do empurrão a partir do ponto de contato
        Vector3 direcaoEmpurrao = (transform.position - pontoDeContato).normalized;

        // Define a direção e ativa o empurrão
        knockbackDirection = direcaoEmpurrao * knockbackForce;
        sendoEmpurrado = true;

        // Inicia a coroutine para parar o empurrão após um tempo
        StartCoroutine(PararEmpurrao());
    }

    private IEnumerator PararEmpurrao()
    {
        // Aguarda 0.2 segundos (ajustável conforme necessário)
        yield return new WaitForSeconds(0.2f);
        sendoEmpurrado = false;
    }

    private void Update()
{
    if (sendoEmpurrado && characterController != null)
    {
        // Aplica o movimento de empurrão usando o CharacterController
        characterController.Move(knockbackDirection * Time.deltaTime);
    }
}

}
