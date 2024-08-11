
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public int damagePoint;
    public Sprite icon;
    public GameObject prefab;
}

