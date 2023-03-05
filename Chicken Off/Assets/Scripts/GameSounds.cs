using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSounds : MonoBehaviour
{
    
    [SerializeField] private AudioSource victory;       // Play when game is won
    [SerializeField] private AudioSource countdown;     // Play during 3 2 1 count down
    [SerializeField] private AudioSource start;         // Play right after count down ends
    // TODO potentially other major game sounds here not sure. 

    public void PlayVictorySound() { victory.Play(); }
    public void PlayCountdownSound() { countdown.Play(); }
    public void PlayStartSound() { start.Play(); }
}
