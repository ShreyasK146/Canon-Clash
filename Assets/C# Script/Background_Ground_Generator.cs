using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class Background_Ground_Generator : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjects = new List<GameObject>();
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
       
        if(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {
            int i = Random.Range(0, gameObjects.Count);
            gameObjects[i].gameObject.SetActive(true);
        }
        else
        {
            if(PhotonNetwork.IsMasterClient) 
            {
                //passing different seed for randomness
                int seed = (int)System.DateTime.Now.Ticks;
                view.RPC("GenerateWorld", RpcTarget.All,seed);
            }
           
        }
        
    }

    [PunRPC]
    private void GenerateWorld(int seed)
    {
        
        Random.InitState(seed);//same seed for the both players
        int i = Random.Range(0, gameObjects.Count);
        gameObjects[i].gameObject.SetActive(true);
    }
}
