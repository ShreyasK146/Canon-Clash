
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{   
    [SerializeField]  Button startButton;
    [SerializeField]  public GridItems gridItems;
    [SerializeField] GameObject weaponSelect;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject enemy;
    public GameObject player1;
    public GameObject player2;
    public GameObject player1Instance;
    public GameObject player2Instance;
    [SerializeField] GameObject weaponDropDown;
    [SerializeField] public GameObject healthBar;
    [SerializeField] Menu menu;
    [SerializeField] GameObject grounds;
    [SerializeField] public GameObject inventoryButton;
    [SerializeField] public GameObject image;

    [SerializeField] GameManagers gameManagers;

    private PhotonView view;

    private void Start()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    private void Update()
    {    // checks if offline mode or not and if playeritem and enemy item are full then they can start the game
        if(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {
            if (weaponSelect.gameObject.activeSelf)
            {
                if (gridItems.playerItems.transform.childCount == 15 && gridItems.enemyItems.transform.childCount == 15)
                {
                    if (!startButton.gameObject.activeSelf)
                    {
                        startButton.gameObject.SetActive(true);
                        startButton.onClick.AddListener(SaveGridItems);
                        grounds.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (startButton.gameObject.activeSelf)
                    {
                        startButton.gameObject.SetActive(false);
                        startButton.onClick.RemoveAllListeners();
                    }
                }
            }
        }
        else
        {
            if(PhotonNetwork.IsMasterClient && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn)
            {
                if (weaponSelect.gameObject.activeSelf)
                {
                    if (gridItems.playerItems.transform.childCount == 15 && gridItems.enemyItems.transform.childCount == 15)
                    {
                        if (!startButton.gameObject.activeSelf)
                        {
                            startButton.gameObject.SetActive(true);
                            startButton.onClick.AddListener(SaveGridItems);
                            
                        }
                    }
                    else
                    {
                        if (startButton.gameObject.activeSelf)
                        {
                            startButton.gameObject.SetActive(false);
                            startButton.onClick.RemoveAllListeners();
                        }
                    }
                }
            }
           
            
        }
        
    }

    public void SaveGridItems()
    {
        if(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {
            GameObject.Find("WeaponSelectionUI").gameObject.SetActive(false);
        }
        else
        {
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().gameStarted = true;
            view.RPC("ActivateGround", RpcTarget.All);
            view.RPC("ActivateUI", RpcTarget.All);
        }
         
    }


    [PunRPC]

    private void ActivateGround()
    {
        grounds.gameObject.SetActive(true) ;
    }

    public void StartingTheGame()
    {
        if(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode) 
        {
            Turn();
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().gameStarted = true;
            healthBar.gameObject.SetActive(true);
            player.gameObject.SetActive(true);
            enemy.gameObject.SetActive(true);
            weaponDropDown.gameObject.SetActive(true);

        }

    }

    private void Turn()
    {
        int randomNumber = Random.Range(0, 2);
        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn = randomNumber == 1 ? true : false;
        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn = randomNumber == 0 ? true : false;
        if(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn)
        {
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersProjectileDestroyed = true;
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysProjectileDestroyed = false;
        }   
           
        else if(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn)
        {
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysProjectileDestroyed = true;
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersProjectileDestroyed = false;
        }
           

    }

    [PunRPC]
    private void ActivateUI()
    {
        MenuManager.Instance.CloseMenu(menu);
        GameObject.Find("WeaponSelectionUI").gameObject.SetActive(false);
        healthBar.gameObject.SetActive(true);
        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn = false;
        if (PhotonNetwork.IsMasterClient)
        {
            
            view.RPC("ActiveThePlayers", RpcTarget.All);
        }
        weaponDropDown.gameObject.SetActive(true);  
            
    }
        
   

    [PunRPC]

    private void ActiveThePlayers()
    {
        if(PhotonNetwork.IsMasterClient) 
        {
            player1Instance = PhotonNetwork.Instantiate(player1.name, player1.transform.position, Quaternion.identity);
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().gameStarted = true;
        }
        else
        {
            player2Instance = PhotonNetwork.Instantiate(player2.name , player2.transform.position, Quaternion.Euler(0,180,0));
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().gameStarted = true;
        }

    }



}
