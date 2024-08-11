
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;


public class EnemyAI : MonoBehaviour
{
    private float projectileSpeed = 100f;
    [SerializeField] GameObject player;
    //private Transform topOfWall;

    //private Rigidbody2D rb;
    private bool isMoving = false;
  
    [SerializeField] GridItems grid;
    private bool itemsAdded = false;
    public List<int> items = new List<int>();
    [SerializeField] Projectile projectile;
    [SerializeField] Animator animator;
    [SerializeField] GameObject weaponSelectionUI;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject launchPos;
    [SerializeField] GameObject pivot;
    [SerializeField] ParticleSystem smokeEffect;
    [SerializeField] ParticleSystem smokeEffect2;
    public GameObject launchEffect;
    public AudioSource fire;
    [SerializeField] AudioSource crank;
    [SerializeField] AudioSource move;
    [SerializeField] GameObject wall;
    [SerializeField] PowerandAngle powerandAngle;
    private Animator arrowAnimator;
    [SerializeField] GameObject arrowPointer;


    [SerializeField] Transform playerTransform;


    private void Start()
    {
        arrowAnimator = arrowPointer.GetComponent<Animator>();
    }

    private void Update()
    {
        //enemy items
        if (grid.enemyItems.transform.childCount == 15 && !itemsAdded)
        {
            foreach (Transform child in grid.enemyItems.transform)
            {
                items.Add(child.transform.gameObject.GetComponent<ItemSelected>().item.id);
            }
            items.Add(100);
            itemsAdded = true;
        }
        
        //if enemy turn show the pointer for sometime and then start the movement of enemy
        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn && !GameObject.Find("GameMechanics").GetComponent<GameMechanics>().died && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysProjectileDestroyed)
        {
            if (!GameObject.Find("GameMechanics").GetComponent<GameMechanics>().waitOver)
            {
                arrowPointer.gameObject.SetActive(true);
                arrowAnimator.SetBool("Arrow", true);
            }
            

            StartCoroutine(WaitaMin());

            if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().waitOver)
            {

                GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysProjectileDestroyed = false;
                projectile.projectileLaunched = true;
                if (!isMoving)
                {
                    StartCoroutine(MoveEnemySmoothly());
                }


            }    
            
        }
        else
        {
            animator.SetBool("move1", false);
            
        }
    }

    
    IEnumerator MoveEnemySmoothly()
    {
      
        smokeEffect.Play(); smokeEffect2.Play(); move.Play();
        isMoving = true;
        float newPos = Random.Range(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemyLeftHand, GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemyRightHand);
        Vector2 newPosition = new Vector2(newPos, transform.position.y);
        Vector2 initialPos = transform.position;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector2.Lerp(initialPos, newPosition, elapsedTime / duration);
            animator.SetBool("move1", true);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = newPosition;
        isMoving = false;
        smokeEffect.Stop(); smokeEffect2.Stop(); move.Stop();
        animator.SetBool("move1", false);

        PrepareForTheLaunch();
    }

    IEnumerator WaitaMin()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Find("GameMechanics").GetComponent<GameMechanics>().waitOver = true;
        arrowPointer.gameObject.SetActive(false);
        arrowAnimator.SetBool("Arrow", false);
    }

    private void PrepareForTheLaunch()
    {


            float randomAngle = Random.Range(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemyCanonAngle1, GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemyCanonAngle2);
            Debug.Log(randomAngle);
            StartCoroutine(powerandAngle.PositionTheCanon(randomAngle));
        
      
        
    }
    /*private void PrepareForTheLaunch2()
    {

        Vector2 direction = playerTransform.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x);
        StartCoroutine(powerandAngle.PositionTheCanon(angle * Mathf.Rad2Deg));
       // StartCoroutine(Wait(angle * Mathf.Rad2Deg));

    }*/

    /*IEnumerator PositionTheCanon(float angle)
    {
        crank.Play();
        float currentAngle = pivot.transform.localRotation.eulerAngles.z;

        float targetAngleMin = -23f;
        float targetAngleMax = 67f;
        float newAngle = (angle * Mathf.Rad2Deg + 360) % 360;

        float targetAngle = Mathf.Clamp(newAngle, targetAngleMin, targetAngleMax);

        Debug.Log("Current Angle: " + currentAngle + " ---- New Angle: " + newAngle);

        float duration = 2f;
        float elapsedDuration = 0f;

        while (elapsedDuration < duration)
        {
            float lerpAngle = Mathf.Lerp(currentAngle, targetAngle, elapsedDuration / duration);

            if (!float.IsNaN(lerpAngle))
            {
                pivot.transform.localRotation = Quaternion.Euler(0f, 0f, lerpAngle);
                elapsedDuration += Time.deltaTime;
                yield return null;
            }
            else
            {
                break; // Break if lerpAngle is invalid
            }
        }

        crank.Stop();
        pivot.transform.localRotation = Quaternion.Euler(0f, 0f, targetAngle);
        launchEffect.gameObject.SetActive(true);
        fire.Play();
        StartCoroutine(Wait());
        Projectile.Instance.InstantiateProjectile(launchPos.transform.position, angle, projectileSpeed);
    }*/

    public IEnumerator Wait(float angle)
    {
        yield return new WaitForSeconds(0.05f);
        launchEffect.gameObject.SetActive(false);
        Projectile.Instance.InstantiateProjectile(launchPos.transform.position, angle, projectileSpeed,playerTransform);//calling instantiate the projectile
    }
}