using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileEffects : MonoBehaviour
{
    [SerializeField] public ParticleSystem explosionEffect;

    private PhotonView view;

    private AudioSource audio;


    /// <summary>
    /// code that deals with explosion effect and this is where the switching of turn happens 
    /// </summary>
    

    private void Awake()
    {
        view = gameObject.GetComponent<PhotonView>();
        audio = GameObject.Find("Explosion").GetComponent<AudioSource>();
        
         
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("colission happened");

        

        if (!collision.gameObject.CompareTag("EndofWorld"))
            audio.Play();

        ParticleSystem instantiateEffect = Instantiate(explosionEffect, collision.GetContact(0).point, Quaternion.identity);

        float duration = instantiateEffect.main.duration;

        Destroy(instantiateEffect.gameObject, duration);

       

        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {

            if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn && !GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersProjectileDestroyed)
            {
                
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn = false;
                Debug.Log(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn);
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn = true;
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().waitOver = false;
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysProjectileDestroyed = true;

            }
            else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn && !GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysProjectileDestroyed)
            {
              
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn = false;
                Debug.Log(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn);

                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn = true;

                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersProjectileDestroyed = true;

            }
            
            Destroy(gameObject);

            Projectile.Instance.playerProjectilePrefab = null;
            
            
            
           

        }
        else if (!GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {

            if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn)
            {
                Debug.Log("its player2 turn now ");
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn = false;

                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn = true;
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Moved = false;
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2ProjectileDestroyed = true;


            }
            else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn)
            {
                Debug.Log("its player1 turn now ");
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn = false;
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Moved = false;
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn = true;
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1ProjectileDestroyed = true;



            }

            Destroy(gameObject);

            Projectile.Instance.playerProjectilePrefab = null;

        }
    }
   
}