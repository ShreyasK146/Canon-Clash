using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public RoomInfo info;
    //setting room name to show in search lobby
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        text.text = info.Name;
    }
    public void OnClick()
    {
        Debug.Log("TEST");
        Launcher.Instance.JoinRoom(info);
    }
}
