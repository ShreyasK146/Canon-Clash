using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] Menu[] menus;
    /// <summary>
    /// code to make sure when in multiplayer only one menu is displayed at once 
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }
    //only one menu should be active if menuname matches then if something is already open close
    public void OpenMenu(string menuName)
    {
        
        for(int i = 0; i < menus.Length; i++) 
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }
    //only one menu should be open
    public void OpenMenu(Menu menu)
    {
        Debug.Log("Test");
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        
        menu.Open();
    }
    public void CloseMenu(Menu menu)
    {
        
        menu.Close();
    }

    
}
