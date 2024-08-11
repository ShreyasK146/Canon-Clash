using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;


public class MultiPlayerController : MonoBehaviour
{
    /// <summary>
    /// most of the code here are based on the player 1 or player 2 turn they execute 
    /// </summary>

    private float horizontalInput;
    private float speed = 10f;
    private GameStart game;
    [SerializeField] Animator animator;
    [SerializeField] GameObject launchPos;
    [SerializeField] GameObject playerPivot;
    private PowerandAngle powerandAngle;
    [SerializeField] ParticleSystem smokeEffect;
    [SerializeField] ParticleSystem smokeEffect2;
    [SerializeField] GameObject launchEffect;
    private ProjectileDamage projectileDamage;
    private TextMeshProUGUI player1Health;
    private TextMeshProUGUI player2Health;
    private SelectProjectile dropDown;
    private float projectileSpeed;
    float currentPlayerAngle;
    float previousPlayerAngle = -23;
    static int countPlayer = 0;
    float currentPlayer2Angle;
    float previousPlayer2Angle = -23;
    static int countPlayer2 = 0;
    private int player1HP = 2000;
    private int player2HP = 2000;
    private PhotonView view;
    private GridItems grid;
    private GameObject endMessageCanvas;
    private GameObject endMessageText;
    private int playerPreviousProjectileNumber;

    

    private void Start()
    {
       
        player1HP = GameObject.Find("GameMechanics").GetComponent<GameMechanics>().hp;
        player2HP = GameObject.Find("GameMechanics").GetComponent<GameMechanics>().hp;
        view = gameObject.GetComponent<PhotonView>();
        
        powerandAngle = GameObject.Find("PlayerWeaponsSelect").GetComponent<PowerandAngle>();
        dropDown = GameObject.Find("PlayerWeaponsSelect").GetComponent<SelectProjectile>();
        game = GameObject.Find("GameMechanics").GetComponent<GameStart>();
        player1Health = GameObject.Find("PlayerHealthText").GetComponent<TextMeshProUGUI>();
        player1Health.text = "2000";
        player2Health = GameObject.Find("EnemyHealthText").GetComponent<TextMeshProUGUI>();
        player2Health.text = "2000";
        endMessageCanvas = GameObject.Find("ProjectileMechanics").GetComponent<ProjectileDamage>().endMessageCanvas;
        endMessageText = GameObject.Find("ProjectileMechanics").GetComponent<ProjectileDamage>().endMessage;

    }

    private void Update()
    {

        

        if (!GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {

            if (PhotonNetwork.IsMasterClient && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn)
            {


                if (!game.inventoryButton.gameObject.activeSelf && !game.image.gameObject.activeSelf)
                    game.inventoryButton.gameObject.SetActive(true);
                PlayerMovementandLaunch();

            }

            else if (!PhotonNetwork.IsMasterClient && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn)
            {


                if (!game.inventoryButton.gameObject.activeSelf && !game.image.gameObject.activeSelf)
                    game.inventoryButton.gameObject.SetActive(true);
                PlayerMovementandLaunch();


            }

            if (!GameObject.Find("GameMechanics").GetComponent<GameMechanics>().died)
            {
                view.RPC("ShowMessage", RpcTarget.All);
            }

        }


    }




    private void PlayerMovementandLaunch()
    {
        if (view.IsMine)
        {
             
            if (PhotonNetwork.IsMasterClient && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn)
            {
               

                horizontalInput = Input.GetAxis("Horizontal");

                if (horizontalInput != 0)
                {


                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Moved = true;
                    view.RPC("SyncParticleEffects", RpcTarget.All, horizontalInput);
                    animator.SetBool("move", true);
                    if (!powerandAngle.move.isPlaying)
                        powerandAngle.move.Play();


                }
                else
                {
                    view.RPC("SyncParticleEffects", RpcTarget.All, horizontalInput);
                    animator.SetBool("move", false);
                    if (powerandAngle.move.isPlaying)
                        powerandAngle.move.Stop();

                }

                transform.Translate(Vector2.right * horizontalInput * speed * Time.deltaTime);

                Debug.Log(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1ProjectileDestroyed);
                if (Input.GetKeyDown(KeyCode.L) && SelectProjectile.player1WeaponIsSelected && !game.image.gameObject.activeSelf && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1ProjectileDestroyed)
                {
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Moved = true;
                    Debug.Log("HEYYYYYYYYYYYYYY");
                    animator.SetBool("move", false);
                    game.inventoryButton.gameObject.SetActive(false);
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1ProjectileDestroyed = false;
                    view.RPC("LaunchProjectile", RpcTarget.All);
                }

            }
            else if (!PhotonNetwork.IsMasterClient && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn)
            {
                

                horizontalInput = Input.GetAxis("Horizontal");

                if (horizontalInput != 0)
                {
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Moved = true;
                    view.RPC("SyncParticleEffects", RpcTarget.All, horizontalInput);
                    animator.SetBool("move", true);
                    if (!powerandAngle.move.isPlaying)
                        powerandAngle.move.Play();

                }
                else
                {
                    view.RPC("SyncParticleEffects", RpcTarget.All, horizontalInput);
                    animator.SetBool("move", false);
                    if (powerandAngle.move.isPlaying)
                        powerandAngle.move.Stop();

                }

                transform.Translate(Vector2.right * -horizontalInput * speed * Time.deltaTime);


                if (Input.GetKeyDown(KeyCode.L) && SelectProjectile.player2WeaponIsSelected && !game.image.gameObject.activeSelf && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2ProjectileDestroyed)
                {
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Moved = true;
                    Debug.Log("HEYYYYYYYYYYYYYY2");
                    animator.SetBool("move", false);
                    game.inventoryButton.gameObject.SetActive(false);
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2ProjectileDestroyed = false;
                    view.RPC("LaunchProjectile", RpcTarget.All);
                }
                else
                {
                    Debug.Log(SelectProjectile.player2WeaponIsSelected + "---" + !game.image.gameObject.activeSelf + "-----" + !PhotonNetwork.IsMasterClient);

                }

            }
        }

    }



  
    [PunRPC]
    private void SyncParticleEffects(float horizontalInput)
    {
        if (horizontalInput != 0)
        {
            smokeEffect.Play();
            smokeEffect2.Play();
        }
        else
        {
            smokeEffect.Stop();
            smokeEffect2.Stop();
        }
    }
    [PunRPC]
    void LaunchProjectile()
    {
        float angle = powerandAngle.angle;
        if(view.IsMine)
            view.RPC("PositionCanon", RpcTarget.All, angle);

    }
    [PunRPC]

    void PositionCanon(float angle)
    {
        StartCoroutine(PositioningTheCanon(angle, playerPivot));
    }


    [PunRPC]
    public IEnumerator PositioningTheCanon(float angle, GameObject pivot)
    {
        
        
            powerandAngle.crank.Play();

            if (countPlayer == 0)
            {
                currentPlayerAngle = 0;
                countPlayer++;
            }
            else
            {
                currentPlayerAngle = previousPlayerAngle;
            }

            float newAngle = angle;

            float duration = 2f;
            float elapsedDuration = 0f;

            while (elapsedDuration < duration)
            {
                float lerpAngle = Mathf.Lerp(currentPlayerAngle, angle, elapsedDuration / duration);
                pivot.transform.localRotation = Quaternion.Euler(0f, 0f, lerpAngle);
                elapsedDuration += Time.deltaTime;
                yield return null;
            }

            powerandAngle.crank.Stop();
            pivot.transform.localRotation = Quaternion.Euler(0f, 0f, newAngle);
            previousPlayerAngle = newAngle;

            projectileSpeed = powerandAngle.power;
            powerandAngle.fire.Play();
            StartCoroutine(Wait());

            //LogDebug("currentPlayerAngle = " + currentPlayerAngle + "     previousPlayerAngle = " + previousPlayerAngle + " angle = " + angle + "   pivot = " + pivot.transform.localRotation.z + "     master? = " + PhotonNetwork.IsMasterClient + "\n");
            view.RPC("InstantiateProjectile", RpcTarget.All, launchPos.transform.position, angle, projectileSpeed);

        
        
        
        


    }
    [PunRPC]
    IEnumerator Wait()
    {
        if(view.IsMine)
        {

            launchEffect.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            launchEffect.gameObject.SetActive(false);
        }
        
    }

    [PunRPC]
    public void InstantiateProjectile(Vector3 position, float angle, float projectileSpeed)
    {

        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn)
        {
            if(Projectile.Instance.playerProjectilePrefab == null)
            {

                if (Projectile.player1ItemCurrentNumber == 100)
                {

                    Projectile.Instance.playerProjectilePrefab = Projectile.Instance.rockItemPrefab.gameObject.GetComponent<ItemSelected>().item.prefab;
                    

                }
            }
            if (Projectile.Instance.playerProjectilePrefab != null && SelectProjectile.player1WeaponIsSelected)
            {
               
                string name = Projectile.Instance.playerProjectilePrefab.name;
                if (PhotonNetwork.IsMasterClient)
                {

                    GameObject projectile = PhotonNetwork.Instantiate(name, position, Quaternion.identity);

                    playerPreviousProjectileNumber = projectile.GetComponent<PhotonView>().ViewID;

                    view.RPC("SyncProjectileState", RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, position, angle, projectileSpeed);
                }
            }
            
        }
        else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn)
        {

            if (Projectile.Instance.playerProjectilePrefab == null)
            {

                if (Projectile.player2ItemCurrentNumber == 100)
                {

                    Projectile.Instance.playerProjectilePrefab = Projectile.Instance.rockItemPrefab.gameObject.GetComponent<ItemSelected>().item.prefab;


                }
            }
            if (Projectile.Instance.playerProjectilePrefab != null && SelectProjectile.player2WeaponIsSelected)
            {
                string name = Projectile.Instance.playerProjectilePrefab.name;
                if (!PhotonNetwork.IsMasterClient)
                {

                    GameObject projectile = PhotonNetwork.Instantiate(name, position, Quaternion.identity);
                    playerPreviousProjectileNumber = projectile.GetComponent<PhotonView>().ViewID;

                    view.RPC("SyncProjectileState2", RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, position, angle, projectileSpeed);
                }
            }
        }


        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn)
        {
            //GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn = false;

            WaitForPlayer1TurnToEnd(playerPreviousProjectileNumber) ;

        }
        else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn && !GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn)
        {

            //GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn = false;

            WaitForPlayer2TurnToEnd(playerPreviousProjectileNumber);

        }

    }


    //sync the projectile on both player screens
    [PunRPC]
    public void SyncProjectileState(int viewID, Vector3 position, float angle, float projectileSpeed)
    {
        // Find the projectile by its viewID
        PhotonView projectileView = PhotonView.Find(viewID);
        if (projectileView != null)
        {
            // Update the state of the projectile on other clients
            GameObject projectile = projectileView.gameObject;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Vector2 launchDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // Apply force on the networked rigidbody
            rb.AddForce(launchDirection * projectileSpeed, ForceMode2D.Impulse);

            Projectile.Instance.playerPreviousProjectile = projectile;
            SelectProjectile.player1WeaponIsSelected = false;
        }
    }


    [PunRPC]
    public void SyncProjectileState2(int viewID, Vector3 position, float angle, float projectileSpeed)
    {
        // Find the projectile by its viewID
        PhotonView projectileView = PhotonView.Find(viewID);
        if (projectileView != null)
        {
            // Update the state of the projectile on other clients
            GameObject projectile = projectileView.gameObject;

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Vector2 launchDirection = new Vector2(-Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            
            // Apply force on the networked rigidbody
            rb.AddForce(launchDirection * projectileSpeed, ForceMode2D.Impulse);

            Projectile.Instance.playerPreviousProjectile = projectile;
            SelectProjectile.player2WeaponIsSelected = false;
        }
    }

    // refresh dropdown once their turn ends 
   void WaitForPlayer1TurnToEnd(int playerPreviousProjectileNumber)
     {

         if (PhotonNetwork.IsMasterClient)
         {
             if (Projectile.player1ItemCurrentNumber != 100)
             {
                 
                 foreach (Transform child in Projectile.Instance.grid.playerItems.transform)
                 {
                     if (child.transform.gameObject.GetComponent<ItemSelected>().item.id == Projectile.player1ItemCurrentNumber)
                     {
                        
                         Destroy(child.gameObject);
                     }

                 }
                 dropDown.projectileSelected.options.RemoveAt(dropDown.dropDownNumber);

                 dropDown.PopulateDropDown();
                 
            }
         }

       

        // view.RPC("DestroyPrefab", RpcTarget.All, playerPreviousProjectileNumber);

    }



     void WaitForPlayer2TurnToEnd(int playerPreviousProjetile)
     {
         

         if (!PhotonNetwork.IsMasterClient)
         {
             if (Projectile.player2ItemCurrentNumber != 100)
             {
                 
                 foreach (Transform child in Projectile.Instance.grid.playerItems.transform)
                 {
                     if (child.transform.gameObject.GetComponent<ItemSelected>().item.id == Projectile.player2ItemCurrentNumber)
                     {
                        
                         Destroy(child.gameObject);
                     }

                 }
                 dropDown.projectileSelected.options.RemoveAt(dropDown.dropDownNumber);

                 dropDown.PopulateDropDown();
             }
         }
   

        //view.RPC("DestroyPrefab", RpcTarget.All, playerPreviousProjectileNumber);


    }


/*
     [PunRPC]
     public void DestroyPrefab(int playerPreviousProjectileNumber)
     {
         PhotonView projectileView = PhotonView.Find(playerPreviousProjectileNumber);
         if (projectileView != null)
         {
             Destroy(projectileView.gameObject);
             Projectile.Instance.playerProjectilePrefab = null;
         }
     }*/
     /*


    IEnumerator WaitForPlayer1TurnToEnd(int playerPreviousProjectileNumber)
    {


        if (PhotonNetwork.IsMasterClient)
        {
            if (Projectile.player1ItemCurrentNumber != 100)
            {
                Debug.Log("~~~~+++++");
                foreach (Transform child in Projectile.Instance.grid.playerItems.transform)
                {
                    if (child.transform.gameObject.GetComponent<ItemSelected>().item.id == Projectile.player1ItemCurrentNumber)
                    {
                        Debug.Log("~~~~");
                        Destroy(child.gameObject);
                        yield return null;
                    }

                }
                dropDown.projectileSelected.options.RemoveAt(dropDown.dropDownNumber);

                dropDown.PopulateDropDown();
            }
        }

        

        view.RPC("DestroyPrefab", RpcTarget.All, playerPreviousProjectileNumber);




    }



    IEnumerator WaitForPlayer2TurnToEnd(int playerPreviousProjetile)
    {
        

        if (!PhotonNetwork.IsMasterClient)
        {
            if (Projectile.player2ItemCurrentNumber != 100)
            {
                Debug.Log("~~~~+++++");
                foreach (Transform child in Projectile.Instance.grid.playerItems.transform)
                {
                    if (child.transform.gameObject.GetComponent<ItemSelected>().item.id == Projectile.player2ItemCurrentNumber)
                    {
                        Debug.Log("~~~~");
                        Destroy(child.gameObject);
                        yield return null;
                    }

                }
                dropDown.projectileSelected.options.RemoveAt(dropDown.dropDownNumber);

                dropDown.PopulateDropDown();
            }
        }

       


        view.RPC("DestroyPrefab2", RpcTarget.All, playerPreviousProjectileNumber);


    }



    [PunRPC]
    public void DestroyPrefab(int playerPreviousProjectileNumber)
    {
        PhotonView projectileView = PhotonView.Find(playerPreviousProjectileNumber);
        if (projectileView != null)
        {
            while(true)
            {
                if(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1ProjectileDestroyed && PhotonNetwork.IsMasterClient && !GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn)
                {
                    Destroy(projectileView.gameObject);
                    Projectile.Instance.playerProjectilePrefab = null;
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn = true;
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1ProjectileDestroyed = false;
                    break;
                }
                    
            }
            
        }

        

    }

    [PunRPC]
    public void DestroyPrefab2(int playerPreviousProjectileNumber)
    {
        PhotonView projectileView = PhotonView.Find(playerPreviousProjectileNumber);
        if (projectileView != null)
        {
            while (true)
            {
                if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2ProjectileDestroyed && !PhotonNetwork.IsMasterClient && !GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn)
                {
                    Destroy(projectileView.gameObject);
                    Projectile.Instance.playerProjectilePrefab = null;
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn = true;
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2ProjectileDestroyed = false;
                    break;
                }

            }

        }

        
    }*/
    //code to deal the damage based on which player projectile hits 
    private void OnCollisionEnter2D(Collision2D collision)

    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (collision.gameObject.CompareTag("rock") && IsLocalPlayerObject(collision.gameObject) && view.IsMine)
            {
                player1HP -= 100;
                view.RPC("UpdateHealth", RpcTarget.All,player1HP);
                //Destroy(collision.gameObject);
               // GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn = true;

            }
            else if (collision.gameObject.CompareTag("Projectile") && IsLocalPlayerObject(collision.gameObject) && view.IsMine)
            {
                foreach (var item in game.gridItems.gridItems)
                {
                    //Debug.Log(item.gameObject.GetComponent<ItemSelected>().item.icon.name +"\t" + collision.gameObject.GetComponent<SpriteRenderer>().sprite.name);

                    if (item.gameObject.GetComponent<ItemSelected>().item.icon.name == collision.gameObject.GetComponent<SpriteRenderer>().sprite.name)
                    {
                        player1HP -= item.gameObject.GetComponent<ItemSelected>().item.damagePoint;
                        view.RPC("UpdateHealth", RpcTarget.All, player1HP);

                        Debug.Log(player1HP + "-" + item.gameObject.GetComponent<ItemSelected>().item.damagePoint);
                        //Destroy(collision.gameObject);
                        //GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn = true;

                    }

                }

            }
            

        }
        else if(!PhotonNetwork.IsMasterClient)
        {
            if (collision.gameObject.CompareTag("rock") && IsLocalPlayerObject(collision.gameObject) && view.IsMine)
            {
                player2HP -= 100;
                
                view.RPC("UpdateHealth2", RpcTarget.All, player2HP);
               // Destroy(collision.gameObject);
                //GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn = true;

            }
            else if (collision.gameObject.CompareTag("Projectile") && IsLocalPlayerObject(collision.gameObject) && view.IsMine)
            {
                foreach (var item in game.gridItems.gridItems)
                {
                    //Debug.Log(item.gameObject.GetComponent<ItemSelected>().item.icon.name +"\t" + collision.gameObject.GetComponent<SpriteRenderer>().sprite.name);

                    if (item.gameObject.GetComponent<ItemSelected>().item.icon.name == collision.gameObject.GetComponent<SpriteRenderer>().sprite.name)
                    {
                        player2HP -= item.gameObject.GetComponent<ItemSelected>().item.damagePoint;
                        
                        view.RPC("UpdateHealth2", RpcTarget.All, player2HP);

                        Debug.Log(player2HP + "-" + item.gameObject.GetComponent<ItemSelected>().item.damagePoint);
                        //Destroy(collision.gameObject);
                        //GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn = true;

                    }

                }

            }
            
        }
       
    }

    [PunRPC]
    private void UpdateHealth(int hp)
    {
        if (hp <= 0)
        {
            player1Health.text = "0";
        }
        else
        {
            player1Health.text = hp.ToString();
        }
        
    }

    [PunRPC]
    private void UpdateHealth2(int hp)
    {
        if (hp <= 0)
        {
            player2Health.text = "0";
        }
        else
        {
            player2Health.text = hp.ToString();
        }
    }

    private bool IsLocalPlayerObject(GameObject obj)
    {
        PhotonView photonView = obj.GetComponent<PhotonView>();
        return photonView != null && photonView.Owner.ActorNumber == photonView.CreatorActorNr;
    }
    // code to show who won or  code to show the message when player leaves the game
    [PunRPC]

    public void ShowMessage()
    {
        if (view == null || !view.IsMine)
        {
         
            return;
        }

        if (player1HP <= 0)
        {
            //Find("GameMechanics").GetComponent<GameMechanics>().someoneLeft = false;

            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().died = true;
            if (PhotonNetwork.IsMasterClient)
            {
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().restartToggle.gameObject.SetActive(false);
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().mainMenu.gameObject.SetActive(true);
                endMessageCanvas.gameObject.SetActive(true);
                endMessageText.gameObject.SetActive(true);
                endMessageText.gameObject.GetComponent<TextMeshProUGUI>().text = "YOU LOST! \n PLAYER2 WINS";
                endMessageText.gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
                view.RPC("ShowEndMessage", RpcTarget.Others);
                player1HP = 2000;
                player2HP = 2000;
            }

        }
        else if (player2HP <= 0)
        {
            //GameObject.Find("GameMechanics").GetComponent<GameMechanics>().someoneLeft = false;

            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().died = true;
            if (!PhotonNetwork.IsMasterClient)
            {
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().restart.gameObject.SetActive(false);
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().restartToggle.gameObject.SetActive(true);
                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().mainMenu.gameObject.SetActive(true);
                endMessageCanvas.gameObject.SetActive(true);
                endMessageText.gameObject.SetActive(true);
                endMessageText.gameObject.GetComponent<TextMeshProUGUI>().text = "YOU LOST! \n PLAYER1 WINS";
                endMessageText.gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;
                view.RPC("ShowEndMessage", RpcTarget.Others);
                player2HP = 2000;
                player1HP = 2000;

            }

        }

        if (PhotonNetwork.PlayerList.Length < 2 && player1HP > 0 && player2HP > 0)
        {
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().restart.gameObject.SetActive(false);
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().mainMenu.gameObject.SetActive(true);
            endMessageCanvas.gameObject.SetActive(true);
            endMessageText.gameObject.SetActive(true);
            endMessageText.gameObject.GetComponent<TextMeshProUGUI>().text = "OPPONENT FLED \n YOU WIN";
            endMessageText.gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().died = true;
        }


    }
    [PunRPC]
    public void ShowEndMessage()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().restartToggle.gameObject.SetActive(false);
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().mainMenu.gameObject.SetActive(true);
        }
        else if (!PhotonNetwork.IsMasterClient)
        {

            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().restart.gameObject.SetActive(false);
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().restartToggle.gameObject.SetActive(true);
            GameObject.Find("GameMechanics").GetComponent<GameMechanics>().mainMenu.gameObject.SetActive(true);
        }

        endMessageCanvas.gameObject.SetActive(true);
        endMessageText.gameObject.SetActive(true);
        endMessageText.gameObject.GetComponent<TextMeshProUGUI>().text = "WELL DONE!!! \n YOU WIN";
        endMessageText.gameObject.GetComponent<TextMeshProUGUI>().color = Color.green;
        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().died = true;


    }

    /*private void LogDebug(string info)
    {
        string filePath = "D://New folder (3)//Game//debug.txt";

        try
        {
            using(StreamWriter writer = new StreamWriter(filePath,true))
            {
                writer.WriteLine(info);
            }
        }
        catch(Exception e) 
        {
            Debug.LogError("Error writing to log file: " + e.Message);
        }
    }*/
}





        


