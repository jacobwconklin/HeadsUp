using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    [SerializeField] private GameObject intact;
    [SerializeField] private GameObject cracked;
    [SerializeField] private GameObject crackedMore;
    [SerializeField] private AudioSource touchedSound;
    private float automaticDecay;

    private int timesTouched = 0;

    // Start is called before the first frame update
    void Start()
    {
        intact.SetActive(true);
        cracked.SetActive(false);
        crackedMore.SetActive(false);
        automaticDecay = Time.time + Random.Range(4.0f, 9.0f) + 5;
    }

    private void Update()
    {
        // Every 3 to 8 seconds have a decay so that Ice will crack by itself
        if (Time.time > automaticDecay)
        {
            // Call touched method and update timer
            touched();
            automaticDecay = Time.time + Random.Range(3.0f, 8.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // increase times touched
        touched();
    }

    private void touched()
    {
        // Make sound of being touched
        touchedSound.Play();
        // Increase times touched and check for thresholds to be passed to perform action
        timesTouched++;

        // TODO Ideally I want a texture for EACH stage 0 - 9

        // at 3 switch to cracked
        if (timesTouched == 3)
        {
            intact.SetActive(false);
            cracked.SetActive(true);
        }
        // at 7 switch to crackedMore
        else if (timesTouched == 7)
        {
            cracked.SetActive(false);
            crackedMore.SetActive(true);
        }
        // Finally at 10 break iceblock, could have iceblock descend or particle effect of it breaking
        else if (timesTouched == 10)
        {
            Destroy(gameObject);
        }
    }
}
