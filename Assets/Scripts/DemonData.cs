using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/DemonData", fileName = "Demon Data")]
public class DemonData : ScriptableObject
{
    public float health = 100f;
    public float damage = 20f;
    public float speed = 2f;

}
