
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float speed = 10f;
    private float projectileSpeed;
    [SerializeField] Animator animator;
    [SerializeField] SelectProjectile dropDown;
    [SerializeField] GameObject inventoryImage;
    [SerializeField] GameObject playerInventoryUI;
    [SerializeField] GameObject launchPos;
    [SerializeField] PowerandAngle powerandAngle;
    [SerializeField] ParticleSystem smokeEffect;
    [SerializeField] ParticleSystem smokeEffect2;
    [SerializeField] GameObject launchEffect;
    [SerializeField] AudioSource fire;
    [SerializeField] AudioSource move;
    [SerializeField] GameObject arrowPointer;
    private Animator arrowAnimator;
    private bool moved = false;

    private Transform dummy;

    private void Start()
    {
        arrowAnimator = arrowPointer.GetComponent<Animator>();
    }


    private void Update()
    {
        //different different condition to check if its player turn and the time for player to select and launch an item
        
        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {
            if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersProjectileDestroyed && !moved)
            {
                
                arrowPointer.gameObject.SetActive(true);
                arrowAnimator.SetBool("Arrow", true);
                
            }

            if (!inventoryImage.gameObject.activeSelf && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersProjectileDestroyed)
            { 
                
                playerInventoryUI.gameObject.SetActive(true);
                //Debug.Log(playerInventoryUI.gameObject.activeSelf);

            }
            
            else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn && !inventoryImage.gameObject.activeSelf)
            {
                
                playerInventoryUI.gameObject.SetActive(false);
            }
            if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersProjectileDestroyed)
            {
                

                horizontalInput = Input.GetAxis("Horizontal");
             
                if (horizontalInput != 0)
                {
                    moved = true;
                    arrowPointer.gameObject.SetActive(false);
                    arrowAnimator.SetBool("Arrow", false) ;
                    animator.SetBool("move", true);
                    if (!move.isPlaying)
                        move.Play();
                    smokeEffect.Play(); smokeEffect2.Play();
                }

                else
                {
                    animator.SetBool("move", false);
                    if (move.isPlaying)
                        move.Stop();
                    smokeEffect.Stop(); smokeEffect2.Stop();
                }
                transform.Translate(Vector2.right * horizontalInput * speed * Time.deltaTime);
                if (Input.GetKeyDown(KeyCode.L) && dropDown.weaponIsSelected && !inventoryImage.gameObject.activeSelf && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersProjectileDestroyed)
                {
                    moved = false;
                    arrowPointer.gameObject.SetActive(false);
                    arrowAnimator.SetBool("Arrow", false);
                    GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersProjectileDestroyed = false;
                    animator.SetBool("move", false);
                    playerInventoryUI.gameObject.SetActive(false);
                    //Debug.Log(playerInventoryUI.gameObject.activeSelf);
                    LaunchProjectile();

                }


            }
        }
       


    }



    
    void LaunchProjectile()
    {

        StartCoroutine(StartLaunchProcess());
    }

    // here we call methods which make sure canon smoothly rotates to required angle and fires
    IEnumerator StartLaunchProcess()
    {
        float angle = powerandAngle.angle;
        yield return StartCoroutine(powerandAngle.PositionTheCanon(angle));
        projectileSpeed = powerandAngle.power;
        launchEffect.gameObject.SetActive(true);
        fire.Play();
        StartCoroutine(Wait());
        Projectile.Instance.InstantiateProjectile(launchPos.transform.position, angle, projectileSpeed, dummy);
    }

    IEnumerator Wait()
    {

        yield return new WaitForSeconds(0.05f);
        launchEffect.gameObject.SetActive(false);
    }
    
}





        
