using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestructibleObject", menuName = "Scriptable Objects/DestructibleObject")]
public class DestructibleObject : ScriptableObject
{
    public string objectName;
    public Sprite sprite;
    public float maxHealth;
    public float lootDropAmount;
}
