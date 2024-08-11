
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class ToggleSelect : MonoBehaviour
{
    private Toggle manualToggle;
    [SerializeField] GameObject SelectButton;
    [SerializeField] GameObject randomize;
    [SerializeField] GridItems grid;
    private PhotonView view;
    [SerializeField] GameMechanics game;
    /// <summary>
    /// code that is basically used to switch between random selection and manual seltion of weapons 
    /// </summary>
    private void Start()
    {   
        view = GetComponent<PhotonView>();  
        manualToggle = GetComponent<Toggle>();
        manualToggle.onValueChanged.AddListener(HandleToggle);
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                manualToggle.gameObject.SetActive(false);
                randomize.gameObject.SetActive(false);
            }
        }
        
    }
    
    private void HandleToggle(bool arg0)
    {
        if (manualToggle.isOn)
        {
            randomize.gameObject.SetActive(false);
            SelectButton.gameObject.SetActive(true);
            if(game.offlineMode) 
            {
                PopulateLocalGrid();
            }
            else
            {
                #if PHOTON_UNITY_NETWORKING
                    SynchronizeGrid();
                #endif
            }


        }
        else if (!manualToggle.isOn)
        {

            SelectButton.gameObject.SetActive(false);
            randomize.gameObject.SetActive(true);

            if (game.offlineMode)
            {
                PopulateLocalGrid();
            }
            else
            {
                grid.player1TurnToSelect = true;
                grid.player2TurnToSelect = false;
                grid.player1buttonselected = false;
                grid.player2buttonselected = false;
                #if PHOTON_UNITY_NETWORKING
                    SynchronizeGrid();
                #endif
            }
        }
    }

#if PHOTON_UNITY_NETWORKING

    
    private void SynchronizeGrid()
    {
       
        view.RPC("SynchronizeSeed",RpcTarget.All,(int)System.DateTime.Now.Ticks);
    }

    [PunRPC]

    private void SynchronizeSeed(int seed)
    {
        Random.InitState(seed);
        view.RPC("SynchronizeMainGrid", RpcTarget.All);
    }

    [PunRPC]
    private void SynchronizeMainGrid()
    {
        grid.PopulateMainGrid();
        grid.ClearItems();
    }
#endif
    private void PopulateLocalGrid()
    {
        
        grid.PopulateMainGrid();
        grid.ClearItems();
    }
}
