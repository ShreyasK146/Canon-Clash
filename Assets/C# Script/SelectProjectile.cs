

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;



public class SelectProjectile : MonoBehaviour
{
    public TMP_Dropdown projectileSelected;
    [SerializeField] GridItems grid;
    [SerializeField] Image imageDisplay;
    [SerializeField] Projectile projectile;
    [SerializeField] Toggle confirmWeapon;
    [SerializeField] GameObject inventoryImage;
    [HideInInspector] public bool weaponIsSelected = false;
    [HideInInspector] public static bool player1WeaponIsSelected = false;
    [HideInInspector] public static bool player2WeaponIsSelected = false;
    [HideInInspector] public int dropDownNumber;
    [SerializeField] GameObject rock;
    private PhotonView view;
    [SerializeField] Sprite rockSprite;

    //[SerializeField] GameObject chooseWeapon;
    //[SerializeField] Image arrow;
    private void Start()
    {
        //code that deals with dropdown where projectile is selected for launch
        view = GetComponent<PhotonView>();
        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {
            PopulateDropDown();
            //imageDisplay.gameObject.SetActive(false);
            projectileSelected.onValueChanged.AddListener(OnDropdownValueChanged);
            confirmWeapon.onValueChanged.AddListener(ToggleWeaponSelected);
            //confirmWeapon.gameObject.SetActive(false);
        }
        else
        {
                if(PhotonNetwork.IsMasterClient) 
                {
                    PopulateDropDown();
                    Debug.Log("masterdropdown called");
                    projectileSelected.onValueChanged.AddListener(OnDropdownValueChanged);
                    confirmWeapon.onValueChanged.AddListener(ToggleWeaponSelected);
                }
                else if(!PhotonNetwork.IsMasterClient) 
                {
                    PopulateDropDown();
                    Debug.Log("slavedropdown called");
                    projectileSelected.onValueChanged.AddListener(OnDropdownValueChanged);
                    confirmWeapon.onValueChanged.AddListener(ToggleWeaponSelected);
                }

        }

    }
    //code basically make sure correct weapon is launched only when confirm weapon is selected(also based on their turns)
    private void ToggleWeaponSelected(bool arg0)
    {
        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {

            if (confirmWeapon.isOn)
            {

                weaponIsSelected = true;

            }
            else if (!confirmWeapon.isOn)
            {
                if (inventoryImage.gameObject.activeSelf)
                {
                    weaponIsSelected = false;
                }
            }
        }
        else
        {

            if (PhotonNetwork.IsMasterClient)
            {

                if (confirmWeapon.isOn)
                {
                    
                    player1WeaponIsSelected = true;
                    Debug.Log("weaponIsSelected? = " + player1WeaponIsSelected);

                }
                else if (!confirmWeapon.isOn)
                {
                    if (inventoryImage.gameObject.activeSelf)
                    {
                        
                        player1WeaponIsSelected = false;
                    }
                }
            }
            else if (!PhotonNetwork.IsMasterClient)
            {

                if (confirmWeapon.isOn)
                {
                    Debug.Log("confirm weapon is on player2");
                    player2WeaponIsSelected = true;
                    Debug.Log("weaponIsSelected? = " + player2WeaponIsSelected);

                }
                else if (!confirmWeapon.isOn)
                {
                    Debug.Log("confirm weapons is not on for palyer2");
                    if (inventoryImage.gameObject.activeSelf)
                    {
                        Debug.Log("inventoryimage is active so player2weaponselected is false");
                        player2WeaponIsSelected = false;
                    }
                }
            }
        
        }
        

    }

    //again repeatation of code for populating dropdown
    [PunRPC]
    public void PopulateDropDown()
    {
        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {
            projectileSelected.ClearOptions();
            projectile.playerItemCurrentNumber = 100;
            //Debug.Log(rock.gameObject.GetComponent<ItemSelected>().item.icon);
            List<TMP_Dropdown.OptionData> dropDownOptions = new List<TMP_Dropdown.OptionData>();
            TMP_Dropdown.OptionData Rock = new TMP_Dropdown.OptionData("Rock");
            dropDownOptions.Add(Rock);
            //defaultDropDownOption.gameObject.SetActive(true);
            foreach (Transform child in grid.playerItems.transform)
            {
                Transform backgroundProjectileTransform = child.Find("BackgroundProjectile");

                //Debug.Log("BackgroundProjectile: " + backgroundProjectileTransform.name);

                if (backgroundProjectileTransform != null)
                {
                    Transform textContainerTransform = backgroundProjectileTransform.Find("ProjectileText");
                    Transform projectile = backgroundProjectileTransform.Find("ProjectileSprite");
                    Image projectileImage = projectile.GetComponentInChildren<Image>();
                    //Debug.Log("imageContainerTransform: " + imageContainerTransform.name);

                    if (textContainerTransform != null && projectileImage != null)
                    {
                        TextMeshProUGUI projectileText = textContainerTransform.GetComponentInChildren<TextMeshProUGUI>();
                        //Debug.Log("Image: " + (image != null ? image.name : "No Image"));

                        if (projectileText != null && projectileText.text != null)
                        {
                            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(projectileText.text);
                            optionData.image = projectileImage.sprite;
                            dropDownOptions.Add(optionData);


                            /*Image itemImage = Instantiate(imageDisplay, imageDisplay.transform.parent);
                            itemImage.sprite = projectileImage.sprite;
                            itemImages.Add(itemImage);*/

                        }
                    }
                    else
                    {
                        Debug.Log("TextContainer or Image not found under BackgroundProjectile");
                    }

                }
            }

            projectileSelected.AddOptions(dropDownOptions);
            //imageDisplay.sprite = projectileSelected.options[0].image;
        }
        else if(!GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {

           
            if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn && PhotonNetwork.IsMasterClient)
            {
                Debug.Log("populating player1 dropdown");
                projectileSelected.ClearOptions();
                Projectile.player1ItemCurrentNumber = 100;

                List<TMP_Dropdown.OptionData> dropDownOptions = new List<TMP_Dropdown.OptionData>();
                TMP_Dropdown.OptionData Rock = new TMP_Dropdown.OptionData("Rock");
                dropDownOptions.Add(Rock);

                foreach (Transform child in grid.playerItems.transform)
                {
                    Transform backgroundProjectileTransform = child.Find("BackgroundProjectile");



                    if (backgroundProjectileTransform != null)
                    {
                        Transform textContainerTransform = backgroundProjectileTransform.Find("ProjectileText");
                        Transform projectile = backgroundProjectileTransform.Find("ProjectileSprite");
                        Image projectileImage = projectile.GetComponentInChildren<Image>();


                        if (textContainerTransform != null && projectileImage != null)
                        {
                            TextMeshProUGUI projectileText = textContainerTransform.GetComponentInChildren<TextMeshProUGUI>();


                            if (projectileText != null && projectileText.text != null)
                            {
                                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(projectileText.text);
                                optionData.image = projectileImage.sprite;
                                dropDownOptions.Add(optionData);

                            }
                        }
                        else
                        {
                            Debug.Log("TextContainer or Image not found under BackgroundProjectile");
                        }

                    }
                }

                projectileSelected.AddOptions(dropDownOptions);

            }
            else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn && !PhotonNetwork.IsMasterClient)

            {
                
                projectileSelected.ClearOptions();
                Projectile.player2ItemCurrentNumber = 100;

                List<TMP_Dropdown.OptionData> dropDownOptions = new List<TMP_Dropdown.OptionData>();
                TMP_Dropdown.OptionData Rock = new TMP_Dropdown.OptionData("Rock");
                dropDownOptions.Add(Rock);

                foreach (Transform child in grid.playerItems.transform)
                {
                    Transform backgroundProjectileTransform = child.Find("BackgroundProjectile");



                    if (backgroundProjectileTransform != null)
                    {
                        Transform textContainerTransform = backgroundProjectileTransform.Find("ProjectileText");
                        Transform projectile = backgroundProjectileTransform.Find("ProjectileSprite");
                        Image projectileImage = projectile.GetComponentInChildren<Image>();


                        if (textContainerTransform != null && projectileImage != null)
                        {
                            TextMeshProUGUI projectileText = textContainerTransform.GetComponentInChildren<TextMeshProUGUI>();


                            if (projectileText != null && projectileText.text != null)
                            {
                                TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(projectileText.text);
                                optionData.image = projectileImage.sprite;
                                dropDownOptions.Add(optionData);

                            }
                        }
                        else
                        {
                            Debug.Log("TextContainer or Image not found under BackgroundProjectile");
                        }

                    }
                }
                projectileSelected.AddOptions(dropDownOptions);
            }
        }
    }
    // when drop down weapon is changed make sure the selected weapon also changes and other things related to selection
    [PunRPC]
    public void OnDropdownValueChanged(int arg0)
    {
        Projectile.Instance.playerProjectilePrefab = null;
        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {

            //chooseWeapon.gameObject.SetActive(false);
            Debug.Log("Dropdown Value =" + arg0);
            if (arg0 == 0)
            {
                imageDisplay.sprite = rockSprite;
                projectile.playerItemCurrentNumber = 100;
            }
            //defaultDropDownOption.gameObject.SetActive(false);
            else if (projectileSelected.options[arg0].text != "Rock")
            {
                //confirmWeapon.gameObject.SetActive(true);
                imageDisplay.gameObject.SetActive(true);

               

                if (arg0 > 0 && arg0 < projectileSelected.options.Count)
                {
                    Sprite selectedImage = projectileSelected.options[arg0].image;

                    if (selectedImage != null)
                    {
                        imageDisplay.sprite = selectedImage;
                    }

                    if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().playersTurn)
                    {
                        foreach (Transform child in grid.playerItems.transform)
                        {
                            if (child.transform.gameObject.GetComponent<ItemSelected>().item.icon.name == projectileSelected.options[arg0].image.name)
                            {
                                Projectile.Instance.playerProjectilePrefab = child.transform.gameObject.GetComponent<ItemSelected>().item.prefab;
                                //Debug.Log("projectile -" + Projectile.Instance.playerProjectilePrefab.name);

                                projectile.playerItemCurrentNumber = child.transform.gameObject.GetComponent<ItemSelected>().item.id;
                                dropDownNumber = arg0;
                                //confirmWeapon.gameObject.SetActive(true);
                            }

                        }
                    }

                }
            }
        }
        else if(!GameObject.Find("GameMechanics").GetComponent<GameMechanics>().offlineMode)
        {
            if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn && PhotonNetwork.IsMasterClient)
            {
                Debug.Log("1stplayerdropdownvaluechanged");

                Debug.Log("Dropdown Value =" + arg0);

                if (arg0 == 0)
                {
                    imageDisplay.sprite = rockSprite;
                    Projectile.player1ItemCurrentNumber = 100;
                }


                else if (projectileSelected.options[arg0].text != "Rock")
                {

                    imageDisplay.gameObject.SetActive(true);

                    if (arg0 > 0 && arg0 < projectileSelected.options.Count)
                    {
                        Sprite selectedImage = projectileSelected.options[arg0].image;

                        if (selectedImage != null)
                        {
                            imageDisplay.sprite = selectedImage;
                        }

                        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player1Turn)
                        {
                            foreach (Transform child in grid.playerItems.transform)
                            {
                                if (child.transform.gameObject.GetComponent<ItemSelected>().item.icon.name == projectileSelected.options[arg0].image.name)
                                {
                                    Projectile.Instance.playerProjectilePrefab = child.transform.gameObject.GetComponent<ItemSelected>().item.prefab;


                                    Projectile.player1ItemCurrentNumber = child.transform.gameObject.GetComponent<ItemSelected>().item.id;
                                    dropDownNumber = arg0;

                                }

                            }
                        }

                    }
                }
            }
            else if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn && !PhotonNetwork.IsMasterClient)
            {
                Debug.Log("2ndplayerdropdownvaluechanged");

                Debug.Log("Dropdown Value =" + arg0);


                if (arg0 == 0)
                {
                    imageDisplay.sprite = rockSprite;
                    Projectile.player2ItemCurrentNumber = 100;
                }

                else if (projectileSelected.options[arg0].text != "Rock")
                {

                    imageDisplay.gameObject.SetActive(true);

                    if (arg0 > 0 && arg0 < projectileSelected.options.Count)
                    {
                        Sprite selectedImage = projectileSelected.options[arg0].image;

                        if (selectedImage != null)
                        {
                            imageDisplay.sprite = selectedImage;
                        }

                        if (GameObject.Find("GameMechanics").GetComponent<GameMechanics>().player2Turn)
                        {
                            foreach (Transform child in grid.playerItems.transform)
                            {
                                if (child.transform.gameObject.GetComponent<ItemSelected>().item.icon.name == projectileSelected.options[arg0].image.name)
                                {
                                    Projectile.Instance.playerProjectilePrefab = child.transform.gameObject.GetComponent<ItemSelected>().item.prefab;


                                    Projectile.player2ItemCurrentNumber = child.transform.gameObject.GetComponent<ItemSelected>().item.id;
                                    dropDownNumber = arg0;

                                }

                            }
                        }

                    }
                }
            }
        }
    }
   
    
}






