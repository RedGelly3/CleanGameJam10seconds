using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public double time;
    public List<GameObject> transformations;
    public Transform checkpoint;
    public Transform player;
    public Camera cam;
    private Rigidbody2D animalBody;
    [SerializeField]
    private float walkingSpeed = 5.0f;
    [SerializeField]
    private float jumpPower = 3.50f;
    private float aircontrol = 0.5f;
    private bool isGrounded;

    public GameObject nextTransformation;
    public GameObject currentTransformation;
    public GameObject Logo;

    private void Awake()
    {
        player = gameObject.transform;

        gameObject.transform.position = player.position;

        animalBody = gameObject.GetComponent<Rigidbody2D>();

        isGrounded = false;

        time = 10f;
        Cursor.visible = false;
        StartCoroutine(LaunchTime());
        currentTransformation = FindTransformationActuelle(gameObject);
        nextTransformation = null;
        
    }


    IEnumerator LaunchTime()
    {
        float oldTime = Time.realtimeSinceStartup;
        while (true)
        {   
           
            time -= Time.realtimeSinceStartup - oldTime;
            oldTime = Time.realtimeSinceStartup;

            if (time <= 0.0f)
            {
                Transformation();
                time = 10.0f;
            }
            else if (time <= 5.0f && nextTransformation==null)
            {
                nextTransformation = TransformationRoulette();
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Transformation()
    {
        Destroy(currentTransformation);
        nextTransformation.transform.parent = gameObject.transform;
        nextTransformation.transform.localPosition = Vector3.zero;
        animalBody.gravityScale = 1; 
        animalBody.mass = nextTransformation.GetComponent<ValuesAnimal>().mass;
        jumpPower = nextTransformation.GetComponent<ValuesAnimal>().jumpForce;
        walkingSpeed = nextTransformation.GetComponent<ValuesAnimal>().speed;
        aircontrol = nextTransformation.GetComponent<ValuesAnimal>().aircontrol;
        gameObject.GetComponent<CapsuleCollider2D>().direction = nextTransformation.GetComponent<CapsuleCollider2D>().direction;
        gameObject.GetComponent<CapsuleCollider2D>().size = nextTransformation.GetComponent<CapsuleCollider2D>().size * nextTransformation.GetComponent<Transform>().localScale.x;
        gameObject.GetComponent<CapsuleCollider2D>().offset = nextTransformation.GetComponent<CapsuleCollider2D>().offset * nextTransformation.GetComponent<Transform>().localScale.y;
        currentTransformation = nextTransformation;
        nextTransformation = null;
        Logo.GetComponent<LogoTalisman>().nextTransformation = nextTransformation;
        Logo.GetComponent<LogoTalisman>().currentTransformation = currentTransformation;
        Logo.GetComponent<LogoTalisman>().updated = true;

    }
    GameObject TransformationRoulette()
    {   
        currentTransformation = FindTransformationActuelle(gameObject);
        Debug.Log(currentTransformation.name);
        GameObject newTransformation = Instantiate(transformations[0], gameObject.transform.position, Quaternion.identity);
        //Vector2 playerVelocity = transformationActuelle.GetComponent<Rigidbody2D>().velocity;
        do
        {
            Destroy(newTransformation);
            double dice = Random.Range(0f, 100f);
            // set new prefab et lui donner la transform player
            if (dice <= 1)
            {
                newTransformation = Instantiate(transformations[3], gameObject.transform.position, Quaternion.identity);
            }
            else if (dice <= 34)
            {
                newTransformation = Instantiate(transformations[0], gameObject.transform.position, Quaternion.identity);
            }
            else if (dice <= 77)
            {
                newTransformation = Instantiate(transformations[2], gameObject.transform.position, Quaternion.identity);
            }
            else
            {
                newTransformation = Instantiate(transformations[1], gameObject.transform.position, Quaternion.identity);
            }

            
        } while (GetObjectNameWithoutCareForClone(currentTransformation) == GetObjectNameWithoutCareForClone(newTransformation));
        newTransformation.transform.tag = "Transformation";
        newTransformation.transform.position =new Vector3(-1000,-1000,-1000);
        Logo.GetComponent<LogoTalisman>().nextTransformation = newTransformation;
        Logo.GetComponent<LogoTalisman>().currentTransformation = currentTransformation;
        Logo.GetComponent<LogoTalisman>().updated = true;
        return newTransformation;

    }

    public static string GetObjectNameWithoutCareForClone(GameObject gameObject) {
        string name = gameObject.transform.name;
        return name.Split('(')[0];    
    }
    public static GameObject FindTransformationActuelle(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.tag == "Transformation")
            {
                return parent.transform.GetChild(i).gameObject;
            }

        }
        print("error transformationActuelle not found");
        return null;
    }
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
                animalBody.AddForce(Vector3.right * aircontrol);
            }
            if (currentTransformation.GetComponent<SpriteRenderer>().flipX)
            {
                currentTransformation.GetComponent<SpriteRenderer>().flipX = false;
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
                animalBody.AddForce(-Vector3.right * aircontrol);
            }
            if (!currentTransformation.GetComponent<SpriteRenderer>().flipX)
            {
                currentTransformation.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(currentTransformation.GetComponent<ValuesAnimal>().Pouvoir(currentTransformation));

        }
        if ((currentTransformation.name == "Lapin") || (currentTransformation.name == "Lapin(Clone)"))
        {
            if (((Input.GetKey(KeyCode.D)) || ((Input.GetKey(KeyCode.A)))) && isGrounded)
            {
                currentTransformation.GetComponent<Animator>().SetBool("lapinMarche", true);
            }
            else
            {
                currentTransformation.GetComponent<Animator>().SetBool("lapinMarche", false);
            }

            
        }
        if (currentTransformation.name == "Sanglier(Clone)")
        {
            if (((Input.GetKey(KeyCode.D)) || ((Input.GetKey(KeyCode.A)))) && isGrounded)
            {
                currentTransformation.GetComponent<Animator>().SetBool("sanglierMarche", true);
            }
            else
            {
                currentTransformation.GetComponent<Animator>().SetBool("sanglierMarche", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Death")
        {   
            StartCoroutine(Death(null));
        }
        if (collision.gameObject.tag == "Ennemi")
        {
            collision.gameObject.GetComponent<EnnemiPaterne>().enabled = false;
            StartCoroutine(Death(collision));
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

    private IEnumerator Death(Collider2D coll)
    {
        currentTransformation.GetComponent<SpriteRenderer>().color = Color.gray;
        cam.GetComponent<FollowPlayer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        time = 10.0f;
        yield return new WaitForSeconds(0.5f);
        if (coll != null)
        {
            coll.gameObject.GetComponent<EnnemiPaterne>().enabled = true;
        }
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<Transform>().position = checkpoint.position;
        cam.GetComponent<FollowPlayer>().enabled = true;
        currentTransformation.GetComponent<SpriteRenderer>().color = Color.white;
    }
}