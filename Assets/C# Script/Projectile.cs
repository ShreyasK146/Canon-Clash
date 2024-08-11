using System.Collections;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;



public class Projectile : MonoBehaviour
{
    private static Projectile instance;
    [HideInInspector] public GameObject playerProjectilePrefab;
    private GameObject enemyProjectilePrefab;
    [HideInInspector] public GameObject playerPreviousProjectile;
    private GameObject enemyPreviousProjectile;
    [HideInInspector] public bool projectileLaunched = false;
    [SerializeField] public GridItems grid;
    [SerializeField] public GameObject weaponSelectCanvas;
    [HideInInspector] public int playerItemCurrentNumber = 100;
    [HideInInspector] public static int player1ItemCurrentNumber = 100;
    [HideInInspector] public static int player2ItemCurrentNumber = 100;
    private int enemyItemCurrentNumber;
    [SerializeField] public SelectProjectile playerDropdown;
    [SerializeField] EnemyAI enemyItem;
    [SerializeField] public GameObject mainMenu;
    [SerializeField] public GameObject rockItemPrefab;
    [SerializeField] public GameObject rock;
    public bool rockLaunched = false;
    private PhotonView view;
    private float gravity = 39.2f;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }
    // to make sure only one instance of project is present at a scene at a time 
    public static Projectile Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Projectile>();

                if(instance != null) 
                {
                    Debug.Log("no instances found");
                }
            }
            return instance;
        }

    }

    private void Awake()
    {
   
        if(instance!= null && instance!= this) 
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;    
            //DontDestroyOnLoad(gameObject);
        }
        
        
    }

    /*public void InstantiateProjectile(Vector3 position, float angle, float projectileSpeed)
    {
      

            if (playerPreviousProjectile != null)
            {
                Destroy(playerPreviousProjectile);
            }
            if (enemyPreviousProjectile != null)
            {
                Destroy(enemyPreviousProjectile);
            }



            if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn)
            {
                int i = Random.Range(0, enemyItem.items.Count);

                if (enemyItem.items[i] == 100)
                {
                    float angleInRadians = angle * Mathf.Deg2Rad;

                    float horizontalVelocity = -projectileSpeed * Mathf.Cos(angleInRadians);
                    float verticalVelocity = projectileSpeed * Mathf.Sin(angleInRadians);


                    GameObject projectile = Instantiate(rock, position, Quaternion.identity);
                    projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalVelocity, verticalVelocity);



                    enemyPreviousProjectile = projectile;
                    projectileLaunched = false;
                    rockLaunched = true;
                }

                else
                {
                    foreach (Transform child in grid.enemyItems.transform)
                    {


                        if (child.transform.gameObject.GetComponent<ItemSelected>().item.id == enemyItem.items[i])
                        {
                            Projectile.Instance.enemyProjectilePrefab = child.transform.gameObject.GetComponent<ItemSelected>().item.prefab;
                            Debug.Log("Selected Enemy Projectile: " + enemyProjectilePrefab);
                            enemyItemCurrentNumber = child.transform.gameObject.GetComponent<ItemSelected>().item.id;
                        }
                    }

                    if (enemyProjectilePrefab != null)
                    {

                        float angleInRadians = angle * Mathf.Deg2Rad;
                        Debug.Log("angle is = " + angle);
                        Debug.Log("calculatedAngle = " + angleInRadians);
                        float horizontalVelocity = -projectileSpeed * Mathf.Cos(angleInRadians);
                        float verticalVelocity = projectileSpeed * Mathf.Sin(angleInRadians);


                        GameObject projectile = Instantiate(enemyProjectilePrefab, position, Quaternion.identity);
                        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalVelocity, verticalVelocity);

                        enemyPreviousProjectile = projectile;
                        projectileLaunched = false;
                        rockLaunched = false;
                    }
                }

            }

            else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn)
            {
                if (playerItemCurrentNumber == 100)
                {
                    Projectile.Instance.playerProjectilePrefab = rockItemPrefab.gameObject.GetComponent<ItemSelected>().item.prefab;
                }
                //Debug.Log(playerDropdown.weaponIsSelected + " " + playerProjectilePrefab);
                if (playerProjectilePrefab != null && playerDropdown.weaponIsSelected)
                {
                    //launchEffect.gameObject.SetActive(true);
                    Debug.Log("playerprojectilelaunched");
                    GameObject projectile = Instantiate(playerProjectilePrefab, position, Quaternion.identity);
                    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                    Vector2 launchDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                    Debug.Log("projectilespeed " + projectileSpeed);
                    //launchEffect.gameObject.SetActive(false);
                    rb.AddForce(launchDirection * projectileSpeed, ForceMode2D.Impulse);

                    playerPreviousProjectile = projectile;
                    playerDropdown.weaponIsSelected = false;

                }
            }



            if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn)
            {
                //Debug.Log("!");
                //GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn = false;

                WaitForPlayerTurnToEnd();
            }
            else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn)
            {
                //Debug.Log("$");
                //GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn = false;
                WaitForEnemyTurnToEnd();
            }

        
    }
    */
    public void InstantiateProjectile(Vector3 position, float angle, float projectileSpeed, Transform playerTransform)
    {


        if (playerPreviousProjectile != null)
        {
            Destroy(playerPreviousProjectile);
        }
        if (enemyPreviousProjectile != null)
        {
            Destroy(enemyPreviousProjectile);
        }



        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn)
        {
            int i = Random.Range(0, enemyItem.items.Count);

            if (enemyItem.items[i] == 100)
            {
               
                

                GameObject projectile = Instantiate(rock, position, Quaternion.identity);
             

                 Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

                 /*if(rb != null)
                    Destroy(rb);*/

                StartCoroutine(SimulateProjectile(projectile,angle,position,playerTransform));

            


                

                enemyPreviousProjectile = projectile;
                projectileLaunched = false;
                rockLaunched = true;
            }

            else
            {
                foreach (Transform child in grid.enemyItems.transform)
                {


                    if (child.transform.gameObject.GetComponent<ItemSelected>().item.id == enemyItem.items[i])
                    {
                        Projectile.Instance.enemyProjectilePrefab = child.transform.gameObject.GetComponent<ItemSelected>().item.prefab;
                        Debug.Log("Selected Enemy Projectile: " + enemyProjectilePrefab);
                        enemyItemCurrentNumber = child.transform.gameObject.GetComponent<ItemSelected>().item.id;
                    }
                }

                if (enemyProjectilePrefab != null)
                {
                    GameObject projectile = Instantiate(enemyProjectilePrefab, position, Quaternion.identity);

                    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

                    /*if (rb != null)
                        Destroy(rb);*/

                    StartCoroutine(SimulateProjectile(projectile, angle, position, playerTransform));

                    enemyPreviousProjectile = projectile;
                    projectileLaunched = false;
                    rockLaunched = false;
                }
            }

        }

        else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn)
        {
            if (playerItemCurrentNumber == 100)
            {
                Projectile.Instance.playerProjectilePrefab = rockItemPrefab.gameObject.GetComponent<ItemSelected>().item.prefab;
            }
            //Debug.Log(playerDropdown.weaponIsSelected + " " + playerProjectilePrefab);
            if (playerProjectilePrefab != null && playerDropdown.weaponIsSelected)
            {
                //launchEffect.gameObject.SetActive(true);
                Debug.Log("playerprojectilelaunched");
                GameObject projectile = Instantiate(playerProjectilePrefab, position, Quaternion.identity);
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                Vector2 launchDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                Debug.Log("projectilespeed " + projectileSpeed);
                //launchEffect.gameObject.SetActive(false);
                rb.AddForce(launchDirection * projectileSpeed, ForceMode2D.Impulse);

                playerPreviousProjectile = projectile;
                playerDropdown.weaponIsSelected = false;

            }
        }



        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn)
        {
            //Debug.Log("!");
            //GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn = false;

            WaitForPlayerTurnToEnd();
        }
        else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn)
        {
            //Debug.Log("$");
            //GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn = false;
            WaitForEnemyTurnToEnd();
        }


    }
    //part of the code that i didnt clearly understand 
    IEnumerator SimulateProjectile(GameObject Projectile,float firingAngle, Vector3 launchPosition, Transform playerTransform)
    {
        //yield return new WaitForSeconds(1.5f);

        Transform transform = Projectile.GetComponent<Transform>();
        Rigidbody2D rb = Projectile.GetComponent<Rigidbody2D> ();

        rb.gravityScale = 4f;

        //Projectile.position = launchPosition.position + new Vector3(0, 0.0f, 0);

        float target_Distance = Vector2.Distance(transform.position, playerTransform.position);

        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);


        float flightDuration = target_Distance / Vx;

        float elapse_time = 0;

        Debug.Log(Vx);
        Debug.Log(Vy);

        rb.velocity = new Vector2(-Vx, Vy - (gravity * elapse_time));

        /* while (elapse_time < flightDuration)
         {
             if(transform!=null)
             {
                 transform.Translate(new Vector3(-Vx * Time.deltaTime, (Vy - (gravity * elapse_time)) * Time.deltaTime, 0));

                 elapse_time += Time.deltaTime;

                 yield return null;
             }
             else
             {
                 break;
             }

         }*/
        while (Projectile != null)
        {
            yield return null;
        }

        //Projectile.AddComponent<Rigidbody2D>();
    }


    /*IEnumerator WaitForPlayerTurnToEnd()
    {
        yield return new WaitForSeconds(5f);

        //playerDropdown.defaultDropDownOption.gameObject.SetActive(true);
        if(playerItemCurrentNumber != 100)
        {
            foreach (Transform child in grid.playerItems.transform)
            {
                if (child.transform.gameObject.GetComponent<ItemSelected>().item.id == playerItemCurrentNumber)
                {
                    Destroy(child.gameObject);
                }

            }
            playerDropdown.projectileSelected.options.RemoveAt(playerDropdown.dropDownNumber);
            //Debug.Log(playerDropdown.dropDownNumber + "--" + playerDropdown.projectileSelected.options.Count);
            playerDropdown.PopulateDropDown();
        }

        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn = true;
        //Debug.Log(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn + " enemy");


    }
    IEnumerator WaitForEnemyTurnToEnd()
    {
        yield return new WaitForSeconds(5f);
        if(!rockLaunched)
        {
            RemovetheLaunchedProjectile(enemyItemCurrentNumber);
            enemyItem.items.Remove(enemyItemCurrentNumber);
        }

        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn = true;
        //Debug.Log(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn + " player");
    }
    private void RemovetheLaunchedProjectile(int number)
    {
        foreach(Transform child in grid.enemyItems.transform)
        {
            if(number == child.transform.gameObject.GetComponent<ItemSelected>().item.id)
            {
                Destroy(child.gameObject);
            }

        }
    }*/



    void WaitForPlayerTurnToEnd()
    {
        
        
        //playerDropdown.defaultDropDownOption.gameObject.SetActive(true);
        if(playerItemCurrentNumber != 100)
        {
            foreach (Transform child in grid.playerItems.transform)
            {
                if (child.transform.gameObject.GetComponent<ItemSelected>().item.id == playerItemCurrentNumber)
                {
                    Destroy(child.gameObject);
                }

            }
            playerDropdown.projectileSelected.options.RemoveAt(playerDropdown.dropDownNumber);
            //Debug.Log(playerDropdown.dropDownNumber + "--" + playerDropdown.projectileSelected.options.Count);
            playerDropdown.PopulateDropDown();
        }
       
       // GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn = true;
        //Debug.Log(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn + " enemy");

        
    }
    void WaitForEnemyTurnToEnd()
    {
        
        if(!rockLaunched)
        {
            RemovetheLaunchedProjectile(enemyItemCurrentNumber);
            enemyItem.items.Remove(enemyItemCurrentNumber);
        }
            
       // GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn = true;
        //Debug.Log(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn + " player");
    }
    private void RemovetheLaunchedProjectile(int number)
    {
        foreach(Transform child in grid.enemyItems.transform)
        {
            if(number == child.transform.gameObject.GetComponent<ItemSelected>().item.id)
            {
                Destroy(child.gameObject);
            }
                
        }
    }
}
