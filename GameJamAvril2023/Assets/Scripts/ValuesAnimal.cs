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

        ParticleSystem powerEffect;
        if (transformation.TryGetComponent<ParticleSystem>(component: out powerEffect))
        {
            if (!transformation.GetComponent<SpriteRenderer>().flipX)
            {
                //Debug.Log("flip particle ");
                powerEffect.shape.rotation.Set(0,0,-90);
                transformation.GetComponent<ParticleSystemRenderer>().flip = new Vector3(50, 0, -90);

            }
            else
            {
                transformation.GetComponent<ParticleSystemRenderer>().flip = new Vector3(0, 0, 90);
            } 
                powerEffect.Play();
        }
        if (transformation.name == "Sanglier(Clone)" && b_dash)
        {
            if (!transformation.GetComponent<SpriteRenderer>().flipX) {
                sens = 1;
            }
            else
            {
                sens = -1;
            }
            GameObject.Find("Player").GetComponent<Rigidbody2D>().AddForce(forceDash * sens);
            GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = 0;
            b_dash = false;
            yield return new WaitForSeconds(timeDash);
            GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = 1;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            GameObject.Find("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(freezeDash());
        }
        if (transformation.name == "Herisson(Clone)")
        {
            if (!boule_herisson)
            {
                boule_herisson = true;
                transformation.GetComponent<Animator>().SetBool("Boule", true);
                GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = 3;
            }
            else
            {
                boule_herisson = false;
                transformation.GetComponent<Animator>().SetBool("Boule", false);
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
