using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov : MonoBehaviour
{
    public float velocidade = 0.1f;
    public Animator characterAnimator;
    // Start is called before the first frame update
    void Start()
    {
        characterAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)) {
            this.gameObject.transform.position = this.gameObject.transform.position + Vector3.right * this.velocidade;
            
        }
        
        if (Input.GetKey(KeyCode.LeftArrow)) {
            this.gameObject.transform.position = this.gameObject.transform.position + Vector3.right * -(this.velocidade);
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            this.gameObject.transform.position = this.gameObject.transform.position + Vector3.forward * (this.velocidade);
            characterAnimator.SetFloat("vertical", Input.GetAxis("Vertical"));
            //characterAnimator.SetTrigger();
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            this.gameObject.transform.position = this.gameObject.transform.position + Vector3.forward * -(this.velocidade);
        }
    }
}
