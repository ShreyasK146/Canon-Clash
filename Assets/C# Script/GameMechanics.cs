
using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMechanics : MonoBehaviour
{
 
    [SerializeField] GameObject canvasForWeaponSelect;
    [SerializeField] GameObject playerWeaponSelect;
    [SerializeField] GameManagers gameManagers;
    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject healthBar;
    [SerializeField] GameObject playerWins;
    [SerializeField] GameObject enemyWins;
    [SerializeField] GameObject EndMessageCanvas;
    //[SerializeField] ProjectileDamage projectileDamage;

    [HideInInspector] public bool waitOver = false;

    public GameObject restartToggle;
    public GameObject mainMenu;
    public GameObject restart;

    //[HideInInspector] public bool someoneLeft = true;


    [HideInInspector] public bool gameStarted = false;

    [HideInInspector] public bool waitEnd = false;
    [HideInInspector] public bool offlineMode;

    [HideInInspector] public bool playersTurn = false;
    [HideInInspector] public bool enemysTurn = false;

    [HideInInspector] public bool player1Turn = true;
    [HideInInspector] public bool player2Turn = false;

    [HideInInspector] public bool died = false;

    public int hp = 2000;
    [HideInInspector] public static bool wantsToRestart = false;

    [HideInInspector] public bool player1Moved = false;
    [HideInInspector] public bool player2Moved = true;

    private Toggle toggle;
    [SerializeField] Launcher launcher;
    private PhotonView view;
    [SerializeField] GameObject pvp;
    [SerializeField] GameStart gameStart;

    [HideInInspector] public bool player1ProjectileDestroyed = true;
    [HideInInspector] public bool player2ProjectileDestroyed = false;

    [HideInInspector] public bool playersProjectileDestroyed = false;
    [HideInInspector] public bool enemysProjectileDestroyed = false;

    [HideInInspector] public float enemyCanonAngle1 = 0f;
    [HideInInspector] public float enemyCanonAngle2 = 1f;
    [HideInInspector] public float enemyRightHand = 60f;
    [HideInInspector] public float enemyLeftHand = 61f;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        
        

    }

    private void Start()
    {

        if(offlineMode)
        {
            
            mainMenu.gameObject.SetActive(true);
        }
        else if(!offlineMode)
        {

            
            toggle = restartToggle.GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(AllowRestart);
        }
    }

    public void SetGameTypeToOffine()
    {
        offlineMode = true;
    }

    public void SetGameTypeToOnline()
    {
        offlineMode = false;
    }

    // i dont think below method is used anywhere cuz feature was discarded
           
    private void AllowRestart(bool arg0)
    {
        if (toggle.isOn)
        {
            wantsToRestart = true;
        }    
            
        else
        {
            wantsToRestart = false;
        }
            
    }

    public void MainMenuScene()
    {
        if (offlineMode)
        {   
            Time.timeScale = 1.0f;
            //PhotonNetwork.Destroy(PhotonView.Find(100));
            gameStarted = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
           
        else if (!offlineMode)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Disconnect();
                PhotonNetwork.DestroyAll();
            }
            
            Time.timeScale = 1.0f;
            gameStarted = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
           
        }
    }
    public void ReloadScene()
    {
       if(offlineMode)
       {
            gameStarted = false;
            Reload();
            
        }
       else if(!offlineMode)
       {
            gameStarted = false;
            Reload2();
       }    

        
    }
    private void Reload()
    {
        
        playerWeaponSelect.SetActive(false);
        Time.timeScale = 0f;
        enemy.gameObject.SetActive(false);
        player.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
        gameManagers.Restart();

    }
    private void Reload2()
    {

        if (PhotonNetwork.IsMasterClient)
        {

            view.RPC("RestartGame", RpcTarget.All);
        }
        
    }   

    [PunRPC]

    public void RestartGame()
    {
        playerWins.SetActive(false);
        enemyWins.SetActive(false);
        EndMessageCanvas.SetActive(false);
        if(PhotonNetwork.IsMasterClient && gameStart.player1Instance != null)
        {
            PhotonNetwork.Destroy(gameStart.player1Instance);
        }



        if (!PhotonNetwork.IsMasterClient && gameStart.player2Instance != null)
        {
            PhotonNetwork.Destroy(gameStart.player2Instance);
        }




        pvp.gameObject.SetActive(true);
        //playerWeaponSelect.SetActive(false);
        //healthBar.gameObject.SetActive(false);

        canvasForWeaponSelect.SetActive(true);

        

        player1Turn = true;
        player2Turn = false;
        died = false;

        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Moved = false;

        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Moved = true;


        player1ProjectileDestroyed = true;
        player2ProjectileDestroyed = false;

        //someoneLeft = true;

        Time.timeScale = 1.0f;


    }

    public void SetEasyDifficulty()
    {
        enemyCanonAngle1 = 0f;
        enemyCanonAngle2 = 20f;
        enemyLeftHand = 20f;
        enemyRightHand = 100f;
    }

    public void SetMediumDifficulty()
    {
        enemyCanonAngle1 = 0f;
        enemyCanonAngle2 = 45f;
        enemyLeftHand = 20f;
        enemyRightHand = 100f;
    }

    public void SetHardDifficulty()
    {
        enemyCanonAngle1 = 0f;
        enemyCanonAngle2 = 60f;
        enemyLeftHand = 20f;
        enemyRightHand = 100f;
    }

    public void SetInsaneDifficulty()
    {
        enemyCanonAngle1 = 52f;
        enemyCanonAngle2 = 67f;
        enemyLeftHand = 20f;
        enemyRightHand = 109f;
    }
  
}
