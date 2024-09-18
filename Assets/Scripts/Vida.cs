using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour
{
    private int vidaAtual;
    private int vidaTotal = 100;

    [SerializeField] private BarraDeVida barraDeVida; 


    private void Start()
    {
        vidaAtual = vidaTotal;
        barraDeVida.AlterarBarraDeVida(vidaAtual, vidaTotal);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AplicarDano(10);
        }
    }

    private void AplicarDano(int dano)
    {
        vidaAtual -= 10;
        barraDeVida.AlterarBarraDeVida(vidaAtual, vidaTotal);
    }
    
}
