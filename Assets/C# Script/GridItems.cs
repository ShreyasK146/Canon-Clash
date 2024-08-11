
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class GridItems : MonoBehaviour
{

    /// <summary>
    /// repeated same code as in methods twice or thrice for multiplayer  
    /// </summary>
    private ItemSelected item;
    public GridLayoutGroup itemGrid;
    public GridLayoutGroup playerItems;
    public GridLayoutGroup enemyItems;
    public List<GameObject> gridItems = new List<GameObject>();
    [HideInInspector] public bool playerTurnToSelect = true;
    [HideInInspector] public bool enemyTurnToSelect = false;
    [HideInInspector] public bool player1TurnToSelect = true;
    [HideInInspector] public bool player2TurnToSelect = false;
    [HideInInspector] public bool player1buttonselected = false;
    [HideInInspector] public bool player2buttonselected = false;
    public Button randomize;
    private PhotonView photonView;
    [SerializeField] private GameMechanics game;
    private int i=0, j = 0;
    [HideInInspector] public bool moving = false;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        photonView = GetComponent<PhotonView>();
        if(game.offlineMode)
        {
            randomize.onClick.AddListener(RandomSelection);
            
            PopulateMainGrid();
            
        }
        else
        {
            // to synchronize randomize selection
            #if PHOTON_UNITY_NETWORKING
                randomize.onClick.AddListener(() => RandomSelection2());
            #endif
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    int seed = GenerateSeedForPlayer(PhotonNetwork.LocalPlayer.ActorNumber);

                    Random.InitState(seed);
                    if (photonView != null)
                    {
                        photonView.RPC("SynchronizeSeed", RpcTarget.All, seed);
                        
                    }
                    else
                    {
                        Debug.LogError("Start: PhotonView is null!");
                    }

                }

            }
        }

    }

    

   

    private int GenerateSeedForPlayer(int playerID)
    {
        int seed = (int)System.DateTime.Now.Ticks + playerID;
        return seed;
    }

#if PHOTON_UNITY_NETWORKING
    [PunRPC]
    private void SynchronizeSeed(int seed)
    {
        
        Random.InitState(seed);

        if (photonView != null)
        {
            photonView.RPC("SynchronizeMainGrid", RpcTarget.All);
        }
        else
        {
            Debug.LogError("SynchronizeSeed: PhotonView is null!");
        }
    }


    [PunRPC]
    private void SynchronizeMainGrid()
    {
        
        PopulateMainGrid();
        //Debug.Log("helloimclient ithink");
    }
#endif


    private void Update()
    {
        if (game.offlineMode && enemyTurnToSelect && itemGrid.transform.childCount != 0)
        {
            enemyTurnToSelect = false;
            Transform itemSelectedByEnemyAI = itemGrid.transform.GetChild(Random.Range(0, itemGrid.transform.childCount));
            GameObject itemSelectedbyEnemy = itemSelectedByEnemyAI.gameObject;
            PopulateEnemyGrid(itemSelectedbyEnemy);
        }
    }

    public void PopulateMainGrid()
    {

        foreach (Transform child in itemGrid.transform)
        {
            Destroy(child.gameObject);
        }



        int count = 0;
        HashSet<int> usedIndices = new HashSet<int>();
        while (count < 30)
        {
            int i = Random.Range(0, gridItems.Count);
            if (!usedIndices.Contains(i))
            {
                GameObject newItem = Instantiate(gridItems[i], itemGrid.transform);
                usedIndices.Add(i);
                count++;
            }
        }




    }
    public void PopulatePlayerGrid(GameObject playerItem)
    {
        if(game.offlineMode) 
        {
            StartCoroutine(MoveGameObjectToPlayerVerticalLayout(playerItem));
        }
        else
        {
            StartCoroutine(MoveGameObjectToPlayerVerticalLayout2(playerItem));
        }    
    }

    private void PopulateEnemyGrid(GameObject enemyItem)
    {
        
        StartCoroutine(MoveGameObjectToEnemyVerticalLayout(enemyItem));
    }
    
    IEnumerator MoveGameObjectToPlayerVerticalLayout2(GameObject item)
    {
        
       
        if(PhotonNetwork.IsMasterClient && player1TurnToSelect)
        {
            
            Vector2 newPosition = new Vector2(playerItems.transform.position.x, playerItems.transform.position.y);
            Vector2 initialPos = item.transform.position;
       
            float duration = 1f;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {

                item.transform.position = Vector2.Lerp(initialPos, newPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;

            }
            GameObject newItem = Instantiate(item, playerItems.transform);
            int index = -1;
            
            foreach (Transform child in itemGrid.transform)
            {
                index++;
                if (child == item.transform)
                {
                    
                    photonView.RPC("SyncForPlayer2", RpcTarget.Others, index);
                }
            }
            
            
            

           
            
        }
        else if(!PhotonNetwork.IsMasterClient && player2TurnToSelect) 
        {
            
            Vector2 newPosition = new Vector2(playerItems.transform.position.x, playerItems.transform.position.y);
            Vector2 initialPos = item.transform.position;
            
            float duration = 1f;
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {

                item.transform.position = Vector2.Lerp(initialPos, newPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;

            }
            GameObject newItem = Instantiate(item, playerItems.transform);
            int index = -1;
            
            foreach (Transform child in itemGrid.transform)
            {
                index++;
                if (child == item.transform)
                {
                    photonView.RPC("SyncForPlayer1", RpcTarget.Others, index);
                }
            }



            
        }
        
    }
    [PunRPC]
    private void SyncForPlayer1(int i)
    {
        


        GameObject newItem = Instantiate(itemGrid.transform.GetChild(i).gameObject, enemyItems.transform);

        player2TurnToSelect = false;
        player1TurnToSelect = true;
        player1buttonselected = false;

        photonView.RPC("DestroyItem", RpcTarget.All, i);
        
       
    }
    [PunRPC]
    private void SyncForPlayer2(int i)
    {

        GameObject newItem = Instantiate(itemGrid.transform.GetChild(i).gameObject, enemyItems.transform);
       
       
        player1TurnToSelect = false;
        player2TurnToSelect = true;
        player2buttonselected = false;
        

        photonView.RPC("DestroyItem", RpcTarget.All,i);
    }

    
    

    [PunRPC]

    private void DestroyItem(int i)
    {
        Destroy(itemGrid.transform.GetChild(i).gameObject);
    }

   // code to have a smooth movement of selected item from itemgrid to player or enemygrid
    IEnumerator MoveGameObjectToPlayerVerticalLayout(GameObject item)
    {
        Vector2 newPosition = new Vector2(playerItems.transform.position.x, playerItems.transform.position.y);
        Vector2 initialPos = item.transform.position;
        float duration = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {

            item.transform.position = Vector2.Lerp(initialPos, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;

        }
        GameObject newItem = Instantiate(item, playerItems.transform);

        Destroy(item);
        enemyTurnToSelect = true;
        playerTurnToSelect = false;
    }
    IEnumerator MoveGameObjectToEnemyVerticalLayout(GameObject item)
    {
        Vector2 newPosition = new Vector2(enemyItems.transform.position.x, enemyItems.transform.position.y);
        Vector2 initialPos = item.transform.position;
        float duration = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {

            item.transform.position = Vector2.Lerp(initialPos, newPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;


        }
        GameObject newItem = Instantiate(item, enemyItems.transform);
        Destroy(item);
        playerTurnToSelect = true;
    }


    //random selection of elements from main grid
    private void RandomSelection()
    {

        
        ClearItems();
        int count1 = 0, count2 = 0;
        List<int> playerList = new List<int>();
        List<int> enemyList = new List<int>();
        HashSet<int> usedIndices = new HashSet<int>();


        //making sure same elements are not choosed again 

        while (count1 < 15 && count1 < itemGrid.transform.childCount)
        {
            int i = Random.Range(0, itemGrid.transform.childCount);
            if (!playerList.Contains(i) && usedIndices.Add(i))
            {
                playerList.Add(i);
                count1++;
                GameObject newItem = Instantiate(itemGrid.transform.GetChild(i).gameObject, playerItems.transform);

            }

        }


        ////making sure same elements are not choosed again and that is not in playerlist
        while (count2 < 15 && count2 < itemGrid.transform.childCount)
        {
            int i = Random.Range(0, itemGrid.transform.childCount);
            if (!enemyList.Contains(i) && !playerList.Contains(i) && usedIndices.Add(i))
            {
                enemyList.Add(i);
                count2++;
                GameObject newItem = Instantiate(itemGrid.transform.GetChild(i).gameObject, enemyItems.transform);

            }

        }
    }
    //clear enemy and player items
    public void ClearItems()
    {
        if (playerItems.transform.childCount != 0)
        {
            foreach (Transform child in playerItems.transform)
            {
                Destroy(child.gameObject);
            }
        }
        if (enemyItems.transform.childCount != 0)
        {
            foreach (Transform child in enemyItems.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    [PunRPC]
    public void ClearItems2()
    {
        if (playerItems.transform.childCount != 0)
        {
            foreach (Transform child in playerItems.transform)
            {
                Destroy(child.gameObject);
            }
        }
        if (enemyItems.transform.childCount != 0)
        {
            foreach (Transform child in enemyItems.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    [PunRPC]
    private void RandomSelection2()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            PopulatePlayerAndEnemyInventory(playerItems.transform, enemyItems.transform);


            //StartCoroutine(DelayedSyncForPlayer2());
        }
    }


    /* ... */

    [PunRPC]
    private void SyncInventoryForPlayer2(int[] playerItemIndices, int[] enemyItemIndices)
    {
        

        // Use the indices to access the items in the itemGrid 
        foreach (int index in enemyItemIndices)
        {
            GameObject newItem = Instantiate(itemGrid.transform.GetChild(index).gameObject, playerItems.transform);
            
        }

        foreach (int index in playerItemIndices)
        {
            GameObject newItem = Instantiate(itemGrid.transform.GetChild(index).gameObject, enemyItems.transform);
            
        }
    }
    /* ... */
    [PunRPC]
    private void PopulatePlayerAndEnemyInventory(Transform playerInventory, Transform enemyInventory)
    {
        photonView.RPC("ClearItems2", RpcTarget.All);

        int count1 = 0, count2 = 0;
        List<int> playerList = new List<int>();
        List<int> enemyList = new List<int>();
        HashSet<int> usedIndices = new HashSet<int>();

        while (count1 < 15 && count1 < itemGrid.transform.childCount)
        {
            int i = Random.Range(0, itemGrid.transform.childCount);
            if (!playerList.Contains(i) && usedIndices.Add(i))
            {
                playerList.Add(i);
                count1++;
                GameObject newItem = Instantiate(itemGrid.transform.GetChild(i).gameObject, playerInventory);
            }
        }

        while (count2 < 15 && count2 < itemGrid.transform.childCount)
        {
            int i = Random.Range(0, itemGrid.transform.childCount);
            if (!enemyList.Contains(i) && !playerList.Contains(i) && usedIndices.Add(i))
            {
                enemyList.Add(i);
                count2++;
                GameObject newItem = Instantiate(itemGrid.transform.GetChild(i).gameObject, enemyInventory);
            }
        }
        int[] playerItemIndices = playerList.ToArray();
        int[] enemyItemIndices = enemyList.ToArray();
        photonView.RPC("SyncInventoryForPlayer2", RpcTarget.Others, playerItemIndices, enemyItemIndices);
    }
    /*
     [PunRPC]
     private void RandomSelection2(int playerPerspective)
     {
         Debug.Log("number " + PhotonNetwork.LocalPlayer.ActorNumber);
         string s = "";
         string s1 = "";
         photonView.RPC("ClearItems2", RpcTarget.AllBuffered);
         int count1 = 0, count2 = 0;
         List<int> playerList = new List<int>();
         List<int> enemyList = new List<int>();
         HashSet<int> usedIndices = new HashSet<int>();
         Debug.Log($"Player Items ViewID: {playerItems.GetComponent<PhotonView>().ViewID}");

         while (count1 < 15 && count1 < itemGrid.transform.childCount)
         {
             int i = Random.Range(0, itemGrid.transform.childCount);
             if (!playerList.Contains(i) && usedIndices.Add(i))
             {
                 playerList.Add(i);
                 count1++;
                 photonView.RPC("InstantiateItem", RpcTarget.All, i, playerItems.GetComponent<PhotonView>().ViewID, playerPerspective);
             }
         }
         Debug.Log($"Enemy Items ViewID: {enemyItems.GetComponent<PhotonView>().ViewID}");

         while (count2 < 15 && count2 < itemGrid.transform.childCount)
         {
             int i = Random.Range(0, itemGrid.transform.childCount);
             if (!enemyList.Contains(i) && !playerList.Contains(i) && usedIndices.Add(i))
             {
                 enemyList.Add(i);
                 count2++;
                 photonView.RPC("InstantiateItem", RpcTarget.All, i, enemyItems.GetComponent<PhotonView>().ViewID, playerPerspective);
             }
         }
         foreach(Transform child in playerItems.transform)
         {
             s += child.gameObject.GetComponent<ItemSelected>().item.name + "    ";   
         }
         Debug.Log(s);
         foreach (Transform child in enemyItems.transform)
         {
             s1 += child.gameObject.GetComponent<ItemSelected>().item.name + "   ";
         }
         Debug.Log(s1);
         LogToFile("Player Items: " + s);
         LogToFile("Enemy Items: " + s1);

     }

    private void LogToFile(string v)
    {
        string path = "D:\\New folder (3)\\log.txt";
        using (StreamWriter writer = new StreamWriter(path,true)) 
        {
            writer.WriteLine(v);
            writer.WriteLine("number " + PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    [PunRPC]
    private void InstantiateItem(int itemIndex, int parentViewID, int playerPerspective)
    {
        GameObject newItem = Instantiate(itemGrid.transform.GetChild(itemIndex).gameObject);
        PhotonView parentView = PhotonView.Find(parentViewID);
        newItem.transform.SetParent(parentView.gameObject.transform);
        

    }

    */

}


