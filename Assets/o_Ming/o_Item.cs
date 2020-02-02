using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_Item : MonoBehaviour
{
    private Animator destroyEffectAnim;

    public Animator explosionAnim; 

    private void Awake()
    {
        destroyEffectAnim = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            destroyEffectAnim.SetTrigger("isDestroyed");
            Instantiate(explosionAnim, transform.position, Quaternion.identity);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
