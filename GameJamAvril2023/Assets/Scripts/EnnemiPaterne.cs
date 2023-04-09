using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code repris de TutoUnityFR (vidéo 4) 
public class EnnemiPaterne : MonoBehaviour
{
    public float vitesse;
    public Transform[] balises;
    public SpriteRenderer graphics; 
    private Transform cible;
    private int destPoint;
    void Start()
    {
        cible = balises[0];
    }


    void Update()
    {
        Vector3 dir = cible.position - transform.position;
        transform.Translate(dir.normalized * vitesse * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, cible.position) <0.3f)
        {
            destPoint = (destPoint + 1) % balises.Length;
            cible = balises[destPoint];
            graphics.flipX = !graphics.flipX;
        }
    }
}
