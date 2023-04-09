using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ValuesAnimal : MonoBehaviour
{
    public float jumpForce;
    public float mass;
    public float speed;
    public float aircontrol;
    private int initDash = 100;
    public Vector2 forceDash;
    public float freshDash;
    private int sens = 1;
    public float timeDash; 
    private bool b_dash = true;
    private bool boule_herisson = false; 

    private void Awake()
    {
        forceDash = forceDash * initDash;
    }

    public IEnumerator Pouvoir(GameObject transformation)
    {
        if(transformation.name == "Sanglier(Clone)" && b_dash)
        {
            if(!transformation.GetComponent<SpriteRenderer>().flipX){
                sens = 1;
            }
            else
            {
                sens = -1;
            }
            GameObject.Find("Player").GetComponent<Rigidbody2D>().AddForce(forceDash*sens);
            GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = 0; 
            b_dash = false;
            yield return new WaitForSeconds(timeDash);
            GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = 1;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(freezeDash());
        }
        if(transformation.name == "Herisson(Clone)")
        {
            if (!boule_herisson)
            {
                boule_herisson = true;
                GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = 3; 
            }
            else
            {
                boule_herisson = false;
                GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = 1; 
            }
        }
    }
    private IEnumerator freezeDash()
    {
        yield return new WaitForSeconds(freshDash);
        b_dash = true;
    }
}
