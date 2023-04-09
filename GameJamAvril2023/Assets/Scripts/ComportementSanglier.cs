using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementSanglier : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D animalBody;
    [SerializeField]
    private float walkingSpeed = 5.0f;
    [SerializeField]
    private float jumpPower = 3.50f;
    private bool isGrounded;

    private void Awake()
    {
        player = gameObject.transform;
        if (transform.parent is not null)
        {
            player = gameObject.transform.parent.transform;
        }

        gameObject.transform.position = player.position;

        animalBody = gameObject.GetComponent<Rigidbody2D>();

        isGrounded = false;
    }
    // Update is called once per frame
    void Update()
    {

        Mouvement();

    }

    void Mouvement()
    {
        //vitesse - InputVelocity 

        if (Input.GetKeyDown(KeyCode.W)) //up key  
        {
            if (isGrounded)
            {
                animalBody.AddForce(Vector3.up * jumpPower * 100);
            }
        }
        if (Input.GetKey(KeyCode.S)) //down key 
        {
            //Nothing To Do
        }
        if (Input.GetKey(KeyCode.D)) // Right key 
        {
            if (isGrounded)
            {
                player.transform.Translate(Vector3.right * walkingSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                animalBody.AddForce(Vector3.right * walkingSpeed);
            }
        }
        if (Input.GetKey(KeyCode.A)) // Left key
        {
            if (isGrounded)
            {
                player.transform.Translate(-Vector3.right * walkingSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                animalBody.AddForce(Vector3.right * walkingSpeed);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}