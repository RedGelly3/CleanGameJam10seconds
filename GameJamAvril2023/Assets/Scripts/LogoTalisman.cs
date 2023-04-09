using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoTalisman : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject player;
    public GameObject nextTransformation;
    public GameObject currentTransformation;
    public bool updated;
    private string nextTransformationName;
    private string currentTransformationName;
    private SpriteRenderer sprite;

    void Awake()
    {
        updated = false;
        sprite = gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {   
        if (updated)
        {
            //print("logo update");
            if (currentTransformation is not null && nextTransformation is not null)
            {
                StopAllCoroutines();
                currentTransformationName = GetObjectNameWithoutCareForClone(currentTransformation);
                nextTransformationName = GetObjectNameWithoutCareForClone(nextTransformation);
                ChangeSpriteTo(nextTransformationName, sprites,sprite);
            }
            else if (currentTransformation is not null) {
                currentTransformationName = GetObjectNameWithoutCareForClone(currentTransformation);
                StartCoroutine(AnimationTirage(currentTransformationName));
            }


            updated = false;
        }
        
    }
    private static string GetObjectNameWithoutCareForClone(GameObject gameObject)
    {
        string name = gameObject.transform.name;
        return name.Split('(')[0];
    }
    public static void ChangeSpriteTo(string animalName, Sprite[] sprites, SpriteRenderer sprite)
    {
        for(int i = 0; i < sprites.Length ; i++)
        {
            if(sprites[i].name == "Logo" + animalName)
            {
                sprite.sprite = sprites[i];
                return;
            }
        }
    }
    IEnumerator AnimationTirage(string currentForm) {
        Sprite tempSprite = sprites[3];
        string lastForm = currentForm;
        while (true)
        {
            float oldTime = Time.realtimeSinceStartup;
            
            
            do
            {   
                lastForm = tempSprite.name;
                float dice = Random.Range(0f, 100f);
                
                // set new prefab et lui donner la transform player
                if (dice <= 1)
                {
                    tempSprite = sprites[3];
                }
                else if(dice <=34)
                {
                    tempSprite = sprites[2];
                }else if(dice <= 77)
                {
                    tempSprite = sprites[1];
                }
                else
                {
                    tempSprite = sprites[0];
                }
                print(lastForm);

            } while (/*"Logo" +currentForm == tempSprite.name ||*/tempSprite.name == lastForm );
            sprite.sprite = tempSprite;
            yield return new WaitForSeconds(0.2f - Time.realtimeSinceStartup + oldTime);
        }
    
    
    }
}
