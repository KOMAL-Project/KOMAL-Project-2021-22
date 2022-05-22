using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class umbrellaScript : MonoBehaviour
{
    public float launchSpeed;
    [Range (0.0f, 1.0f)]
    [SerializeField] public float maintainVelocityPercent;
}
