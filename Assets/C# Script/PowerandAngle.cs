
using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerandAngle : MonoBehaviour
{
    [SerializeField] public Slider sliderForPower;
    public Slider circleSliderForPower;
    [SerializeField] public  TextMeshProUGUI textOfPower;
    [HideInInspector]public float angle;
    [HideInInspector]public float power;
    [SerializeField] public Slider sliderForAngle;
    public Slider circleSliderForAngle;
    [SerializeField] public TextMeshProUGUI textOfAngle;
    //[HideInInspector] public bool canonSet = false;
   // [SerializeField] GameObject enemyPivot;
    [SerializeField] public GameObject pivot;
    [SerializeField] public GameObject pivotEnemy;
    [SerializeField] public AudioSource crank;
    [SerializeField] public GameObject player1Pivot;
    [SerializeField] public GameObject player2Pivot;
    [SerializeField] public AudioSource fire;
    [SerializeField] public EnemyAI enemyAI;
    [SerializeField] public AudioSource move;
    float currentAngle;
    float previousAngle;
    int count = 0;

    float currentAngleEnemy;
    float previousAngleEnemy;
    int countEnemy = 0;
    [HideInInspector] public PhotonView view;
    GameMechanics gameMechanics;
    private void Start()
    {
        //setting inital angle and lots checks for whose turn it is 
        textOfAngle.text = "23°";
        gameMechanics = GameObject.Find("GameMechanics").GetComponent<GameMechanics>();
        view = GetComponent<PhotonView>();
        if (gameMechanics.offlineMode)
        {
            if (sliderForPower != null)
            {
                sliderForPower.onValueChanged.AddListener(HandlePower);
            }

            if (sliderForAngle != null)
            {
                sliderForAngle.onValueChanged.AddListener(HandleAngle);
            }
        }
        else
        {
            if (PhotonNetwork.IsMasterClient )
            {
                if (sliderForPower != null)
                {
                    sliderForPower.onValueChanged.AddListener(HandlePower);
                }

                if (sliderForAngle != null)
                {
                    sliderForAngle.onValueChanged.AddListener(HandleAngle);
                }
            }
            else if (!PhotonNetwork.IsMasterClient )
            {
                if (sliderForPower != null)
                {
                    sliderForPower.onValueChanged.AddListener(HandlePower);
                }

                if (sliderForAngle != null)
                {
                    sliderForAngle.onValueChanged.AddListener(HandleAngle);
                }
            }
        }
        
    }
   

    //methods to handle the power and angle provide by slider value

    private void HandlePower(float arg0)
    {
        if (gameMechanics.offlineMode ||
           (PhotonNetwork.IsMasterClient && gameMechanics.player1Turn) ||
           (!PhotonNetwork.IsMasterClient && gameMechanics.player2Turn))
        {
            circleSliderForPower.value = sliderForPower.value;
            power = sliderForPower.value;
            textOfPower.text = Mathf.RoundToInt(sliderForPower.value) + "%";
        }
    }

    private void HandleAngle(float arg0)
    {
        if (gameMechanics.offlineMode ||
         (PhotonNetwork.IsMasterClient && gameMechanics.player1Turn) ||
         (!PhotonNetwork.IsMasterClient && gameMechanics.player2Turn))
        {
            circleSliderForAngle.value = sliderForAngle.value;
            angle = sliderForAngle.value;
            textOfAngle.text = Mathf.RoundToInt(sliderForAngle.value + 23) + "°";
        }
    }
    //enemy and player handles the angling of canon
    public IEnumerator PositionTheCanon(float angle)
    {
        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {



            if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn)
            {
                crank.Play();
                if (count == 0)
                {
                    currentAngle = pivot.transform.localRotation.eulerAngles.z; 
                    count++;
                }
                else
                {
                    currentAngle = previousAngle;
                }
                float newAngle = angle;
                //Debug.Log("currentAngle = " + currentAngle + "\n" + "newAngle = " + newAngle + "\n" + "shortestDistancebetweenthem = " + (newAngle - currentAngle));
                float duration = 2f;
                float elapsedDuration = 0f;

                while (elapsedDuration < duration)
                {
                    float lerpAngle = Mathf.Lerp(currentAngle, angle, elapsedDuration / duration);
                    pivot.transform.localRotation = Quaternion.Euler(0f, 0f, lerpAngle);
                    elapsedDuration += Time.deltaTime;
                    yield return null;
                }
                crank.Stop();
                pivot.transform.localRotation = Quaternion.Euler(0f, 0f, newAngle);
                previousAngle = newAngle;

            }
            else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().enemysTurn)
            {
                crank.Play();
                if (count == 0)
                {
                    currentAngleEnemy = pivotEnemy.transform.localRotation.eulerAngles.z;
                    count++;
                }
                else
                {
                    currentAngleEnemy = previousAngleEnemy;
                }
                float newAngle = angle;
                //Debug.Log("currentAngle = " + currentAngle + "\n" + "newAngle = " + newAngle + "\n" + "shortestDistancebetweenthem = " + (newAngle - currentAngle));
                float duration = 2f;
                float elapsedDuration = 0f;

                while (elapsedDuration < duration)
                {
                    float lerpAngle = Mathf.Lerp(currentAngleEnemy, angle, elapsedDuration / duration);
                    pivotEnemy.transform.localRotation = Quaternion.Euler(0f, 0f, lerpAngle);
                    elapsedDuration += Time.deltaTime;
                    yield return null;
                }
                crank.Stop();
                pivotEnemy.transform.localRotation = Quaternion.Euler(0f, 0f, newAngle);
                previousAngleEnemy = newAngle;
                enemyAI.launchEffect.gameObject.SetActive(true);
                enemyAI.fire.Play();
                StartCoroutine(enemyAI.Wait(angle));

            }
        }
        
    }

   
    
   
        
}
       
     

    

