using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{

    /// <summary>
    /// code that does nothing (cancelled the plan)
    /// </summary>
    [SerializeField] Button inventoryButton;
    [SerializeField] Button closeInventoryButton;
    [SerializeField] GridItems grid;
    private int playerProjectileId;


    private void Start()
    {
        inventoryButton.onClick.AddListener(RefreshDropDown);
    }

    private void RefreshDropDown()
    {
        
    }
}
