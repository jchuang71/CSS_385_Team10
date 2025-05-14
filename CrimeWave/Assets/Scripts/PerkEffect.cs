using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public abstract class PerkEffect : ScriptableObject
{
    public string perkName;
    public string perkDescription;
    public float perkDuration;
    public bool isPermanent;

    public abstract void Apply(GameObject player);
    public abstract IEnumerator Duration(GameObject player, GameObject perkTextInstance);
}

[Serializable]
public class PerkEffectList
{
    public List<PerkEffect> list;
}