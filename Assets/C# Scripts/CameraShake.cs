using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * This class is used to create the 
 * camera shake effect and the phone vibration
 */
public class CameraShake : MonoBehaviour
{
    public Transform CameraTransform;
    public float ShakeDuration = 0f;
    public float ShakeAmount = 0.1f;
    public float DecreaseFactor = 1.0f;
    public Vector3 OriginalPosition;
    public bool PhoneVibrating = false;

    // Awake function
    private void Awake()
    {
        if (CameraTransform == null) 
        {
            CameraTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    //OnEnable Function
    private void OnEnable()
    {
        OriginalPosition = CameraTransform.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ShakeDuration > 0)
        {
            CameraTransform.localPosition = OriginalPosition + Random.insideUnitSphere * ShakeAmount;
            ShakeDuration -= Time.deltaTime * DecreaseFactor;
            if (!PhoneVibrating)
            {
                Handheld.Vibrate();
            }
        }
        else
        {
            ShakeDuration = 0f;
            CameraTransform.localPosition = OriginalPosition;
            PhoneVibrating = false;
        }
        
    }
}
