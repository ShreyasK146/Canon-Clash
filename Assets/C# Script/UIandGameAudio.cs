

using UnityEngine;
using UnityEngine.Audio;

public class UIandGameAudio : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject playerWeaponSelect;
    [SerializeField] GameObject howToPlay;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] GameObject chooseDifficulty;
    [SerializeField] new AudioSource[] audio = new AudioSource[3];
    AudioSource mainMenuClip;
    AudioSource inGameClip;
    private float volumeOfGame;


    private void Start()
    {
        mainMenuClip = audio[0].GetComponent<AudioSource>();
        inGameClip = audio[1].GetComponent<AudioSource>();
        audioMixer.GetFloat("volume", out volumeOfGame);
    }
    private void Update()
    {
        if ((mainMenu.gameObject.activeSelf || settingsCanvas.gameObject.activeSelf || chooseDifficulty.gameObject.activeSelf || playerWeaponSelect.gameObject.activeSelf || howToPlay.gameObject.activeSelf ) && !mainMenuClip.isPlaying)
        {
            inGameClip.Stop();
            mainMenuClip.Play();

        }
        else if (!mainMenu.gameObject.activeSelf && !chooseDifficulty.gameObject.activeSelf && !settingsCanvas.gameObject.activeSelf && !playerWeaponSelect.gameObject.activeSelf && !howToPlay.gameObject.activeSelf && !inGameClip.isPlaying)
        {
            mainMenuClip.Stop();
            inGameClip.Play();
        }

        

    }

    public void Mute()
    {
        volumeOfGame = -80f;
        audioMixer.SetFloat("volume", volumeOfGame);
    }

    public void Unmute()
    {
        volumeOfGame = 0f;
        audioMixer.SetFloat("volume", volumeOfGame);
    }

   public void BackToPreviousMenu()
    {
        if(!GameObject.Find("GameMechanics").GetComponent<GameMechanics>().gameStarted) 
        {
            mainMenu.gameObject.SetActive(true);
        }
        else if(GameObject.Find("GameMechanics").GetComponent<GameMechanics>().gameStarted)
        {
            settings.gameObject.SetActive(true);
        }
    }

   
}
