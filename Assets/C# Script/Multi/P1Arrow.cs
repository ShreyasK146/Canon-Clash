using Mono.Cecil;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1Arrow : MonoBehaviour
{
    [SerializeField] GameObject arrowPointer;

    [SerializeField] Animator arrowAnimator;


    private PhotonView view;

    private void Start()
    {
        

        view = GetComponent<PhotonView>(); 
    }

    private void Update()
    {
        if(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn)
        {
            view.RPC("ShowPointer", RpcTarget.All);
        }
        else
        {
            arrowPointer.gameObject.SetActive(false);
            
        }
    
    }

    [PunRPC]

    private void ShowPointer()
    {
        if (!GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Moved)
        {
           
                arrowPointer.gameObject.SetActive(true);
                arrowAnimator.SetBool("Arrow", true);
           

        }

        else
        {
            arrowPointer.gameObject.SetActive(false);

        }
    }

  


}
