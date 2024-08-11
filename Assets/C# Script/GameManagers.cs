using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManagers : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] GameStart gameStart;
    [SerializeField] SelectProjectile dropDown;
    [SerializeField] GameObject playerPivot;
    [SerializeField] GameObject enemyPivot;
    [SerializeField] GameObject weaponSelectUI;


    

    [SerializeField] GridItems grid;


  
    
    public void Restart()
    {
        weaponSelectUI.SetActive(true);
        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().died = false;
        Time.timeScale = 1.0f;

        grid.ClearItems();

        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn = false;
        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn = false;

        player.GetComponent<ProjectileDamage>().health1 = 2000;
        enemy.GetComponent<ProjectileDamage>().health1 = 2000;

        player.GetComponent<ProjectileDamage>().textHealth.text = "2000"; 
        enemy.GetComponent<ProjectileDamage>().textHealth.text = "2000";

        //gameStart.healthBar.gameObject.SetActive(true);

        player.transform.localPosition = new Vector3(-77.8f, -61f, 0f);
        playerPivot.transform.localRotation = Quaternion.identity;


        //player.SetActive(true);


        enemy.transform.localPosition = new Vector3(62.2f, -61f, 0f);
        enemyPivot.transform.localRotation = Quaternion.identity;


        //enemy.SetActive(true);

        
    }

    


}
