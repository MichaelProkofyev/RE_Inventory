using UnityEngine;
using System.Collections;

public class ObjectShake : MonoBehaviour
{

    public bool debugMode = false;//Test-run/Call ShakeCamera() on start

    public float shakeAmount = 1;
    public float shakeDuration = .2f;

    float shakePercentage;//A percentage (0-1) representing the amount of shake to be applied when setting rotation.
    float shakeTimeLeft;//The initial shake duration, set when ShakeCamera is called.

    bool isRunning = false; //Is the coroutine running right now?

    public bool smooth;//Smooth rotation?
    public float smoothAmount = 5f;//Amount to smooth

    void Start()
    {

        if (debugMode) ShakeObject();
    }


    public void ShakeObject()
    {
        shakeTimeLeft = shakeDuration;//Set default (start) values

        if (!isRunning) StartCoroutine(Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
    }

    public void ShakeObject(float amount, float duration)
    {
        shakeAmount = amount;
        shakeDuration = duration;//Add to the current time.
        shakeTimeLeft = shakeDuration;//Reset the start time.

        if (!isRunning) StartCoroutine(Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShakeObject();
        }
    }


    IEnumerator Shake()
    {
        isRunning = true;

        while (shakeTimeLeft > 0.01f)
        {
            float shakeAmountThisFrame = shakeAmount * (shakeTimeLeft / shakeDuration);
            Vector3 shiftAmount = Random.insideUnitCircle * shakeAmountThisFrame;

            shakeTimeLeft -= Time.deltaTime;


            //if (smooth)
            //    transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(shiftAmount), Time.deltaTime * smoothAmount);
            //else
            //    transform.localRotation = Quaternion.Euler(shiftAmount);//Set the local rotation the be the rotation amount.
            transform.localPosition = Vector3.Lerp(transform.localPosition, shiftAmount, Time.deltaTime * smoothAmount);

            yield return null;
        }
        transform.localPosition = Vector3.zero;//Set the local rotation to 0 when done, just to get rid of any fudging stuff.
        isRunning = false;
    }

}