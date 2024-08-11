    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Photon.Pun;
    using TMPro;
    using Photon.Realtime;
using System.Linq;
using System.Threading;
using UnityEditor;

public class Launcher : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// code that basically deals with connection to server , create room, leave room , start multi game etc
    /// </summary>
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject weaponSelectUI;
    private PhotonView photonView;
    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
    }



    private void Update()
    {
        /*if(PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                photonView.RPC("ActivateStart", RpcTarget.All);

            }
            else
                photonView.RPC("DeactivateStart", RpcTarget.All);
        }*/
    }

    public void ConenctToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {

        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;


    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("joined lobby");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");

    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom()
    {

        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }


        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            photonView.RPC("ActivateStart", RpcTarget.All);

        }
        else
            photonView.RPC("DeactivateStart", RpcTarget.All);

    }

    [PunRPC]
    public void ActivateStart()
    {
        if (PhotonNetwork.IsMasterClient)
            startGameButton.SetActive(true);
    }

    [PunRPC]
    public void DeactivateStart()
    {
        if(PhotonNetwork.IsMasterClient)
            startGameButton.SetActive(false);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            photonView.RPC("ActivateStart", RpcTarget.All);

        }
        else
            photonView.RPC("DeactivateStart", RpcTarget.All);
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creation Failed = " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void StartGame()
    {
        MenuManager.Instance.OpenMenu("loading");
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ActivateWeaponSelectionUI", RpcTarget.All);

        }

    }





    [PunRPC]
    public void ActivateWeaponSelectionUI()
    {
        if (weaponSelectUI != null)
        {
            MenuManager.Instance.OpenMenu("loading");
            weaponSelectUI.gameObject.SetActive(true);
        }

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
        
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
       
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public void DisconnectFromPhoton()
    {
        if(!GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
            PhotonNetwork.Disconnect();
    }

}