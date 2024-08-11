
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    //[SerializeField] Toggle fullScreenToggle;

    private void Update()
    {
     
            if (Input.GetKeyDown(KeyCode.Escape) && GameObject.Find("GameMechanics").GetComponent<GameMechanics>().gameStarted)
            {
                if (!settingsMenu.gameObject.activeSelf)
                {
                    Time.timeScale = 0f;
                    settingsMenu.gameObject.SetActive(true);
                }
                else if (settingsMenu.gameObject.activeSelf)
                {
                    Time.timeScale = 1f;
                    settingsMenu.gameObject.SetActive(false);
                }
            }
           

        }
        
    }

   

