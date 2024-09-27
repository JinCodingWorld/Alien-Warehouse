using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInjurious
{
    void OnInjury(float damage, Vector3 hitPoint, Vector3 hitNormal);
}

