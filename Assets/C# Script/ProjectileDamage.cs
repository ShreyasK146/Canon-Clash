using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class ProjectileDamage : MonoBehaviour
{
    public int health1 = 2000;
    [SerializeField] public TextMeshProUGUI textHealth;
    [SerializeField] public GridItems grid;
    [SerializeField] public GameObject endMessageCanvas;
    [SerializeField] public GameObject endMessage;
    private bool explosionEffectDone = false;


    PhotonView view;

    private void Start()
    {
        health1 = GameObject.Find("GameMechanics").GetComponent<GameMechanics>().hp;
        view = GetComponent<PhotonView>();
    }
    //code that deals with removing health based on damage point
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        
        if(collision.gameObject.CompareTag("rock"))
        {
            
            health1 -= 100;
            textHealth.text = health1.ToString();
            
            
        }
        else if (collision.gameObject.CompareTag("Projectile"))
        {
            
            foreach (var item in grid.gridItems)
            {
                //Debug.Log(item.gameObject.GetComponent<ItemSelected>().item.icon.name +"\t" + collision.gameObject.GetComponent<SpriteRenderer>().sprite.name);

                if (item.gameObject.GetComponent<ItemSelected>().item.icon.name == collision.gameObject.GetComponent<SpriteRenderer>().sprite.name)
                {
                    
                    health1 -= item.gameObject.GetComponent<ItemSelected>().item.damagePoint;
                    Debug.Log(health1 + "-" + item.gameObject.GetComponent<ItemSelected>().item.damagePoint);
                    textHealth.text = health1.ToString();
                    

                }
               
            }
            

        }
       
    }
    // to  check if player or enemy is dead 
    private void Update()
    {
        if(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode && !GameObject.Find("GameMechanics").GetComponent<GameMechanics>().died) 
        {
            if(health1 <= 0)
            {
                StartCoroutine(Wait());
                if(explosionEffectDone)
                {
                    textHealth.text = "0";
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn = GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn;
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn = !GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn;

                    endMessageCanvas.gameObject.SetActive(true);
                    if (gameObject.name == "PlayerProjectileLauncher")
                    {
                        endMessage.gameObject.SetActive(true);
                        Time.timeScale = 0f;
                        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().died = true;

                    }
                    else if (gameObject.name == "EnemyProjectileLauncher")
                    {
                        endMessage.gameObject.SetActive(true);
                        Time.timeScale = 0f;
                        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().died = true;
                    }
                    explosionEffectDone = false;
                }
               
            }
            
        }
        
    }

   
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        explosionEffectDone = true;
    }

}
