using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public float speed = 1f; // Velocidade de perseguição
    public float rotationSpeed = 5f; // Velocidade de rotação para virar na direção do jogador

    public GameObject playerRef;

    public bool canSeePlayer;
    public float attackDistance = 2f; // Distância mínima para iniciar o ataque

    private Animator animator;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>(); // Obtém o componente Animator
        StartCoroutine(FOVRoutine());
    }

    private void Update()
{
    if (canSeePlayer)
    {
        // Calcula a direção para o jogador
        Vector3 direction = playerRef.transform.position - transform.position;

        // Ignora a componente vertical da direção
        direction.y = 0;

        // Normaliza o vetor direção para obter um vetor unitário
        direction.Normalize();

        // Move o objeto na direção do jogador
        transform.position += direction * speed * Time.deltaTime;

        // Gira gradualmente para olhar na direção do jogador
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Verifica a distância até o jogador e altera a animação conforme necessário
        float distanceToPlayer = Vector3.Distance(transform.position, playerRef.transform.position);
        if (distanceToPlayer < attackDistance)
        {
            animator.SetBool("isAttacking", true); // Muda para a animação de ataque
        }
        else
        {
            animator.SetBool("isAttacking", false); // Volta para a animação inicial
        }
    }
    else
    {
        if (animator.GetBool("isAttacking"))
        {
            animator.SetBool("isAttacking", false); // Assegura que a animação de ataque é desativada
        }
    }
}


    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        // Encontra todos os Colliders em um raio ao redor do inimigo
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius);

        if (rangeChecks.Length != 0)
        {
            // Procura pelo jogador na lista de objetos detectados
            foreach (Collider rangeCheck in rangeChecks)
            {
                if (rangeCheck.CompareTag("Player"))
                {
                    Transform target = rangeCheck.transform;
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    // Verifica se o jogador está dentro do campo de visão (ângulo)
                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);

                        // Verifica se há algum objeto com a tag "Obstruct" entre o inimigo e o jogador
                        if (!IsObstructed(target))
                        {
                            canSeePlayer = true;
                            playerRef = target.gameObject; // Define o jogador como alvo
                        }
                        else
                        {
                            canSeePlayer = false;
                        }
                    }
                    else
                    {
                        canSeePlayer = false;
                    }

                    // Encontrando o jogador, sai do loop
                    break;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
        }
        else
        {
            canSeePlayer = false;
        }
    }

    // Função que verifica se há algum obstáculo entre o inimigo e o jogador
    private bool IsObstructed(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Faz o Raycast e retorna verdadeiro se houver um objeto com a tag "Obstruct" entre o inimigo e o jogador
        if (Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, distanceToTarget))
        {
            if (hit.collider.CompareTag("Obstruct"))
            {
                return true; // Há um obstáculo
            }
        }

        return false; // Nenhum obstáculo
    }
}
