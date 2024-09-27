using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreenController : MonoBehaviour
{
    public FixedTouchField fixedTouchField;
    public PlayerCamera playerCamera;
    void Update()
    {
        playerCamera.LookAxis = fixedTouchField.TouchDist;
    }
}
