using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    // Generic script so that all characters audios can be tied to the specific characters
    // but accessed the same by the main scripts.

    // Changing these will be the key to personalizing each player
    [SerializeField] private AudioSource jumpSound;     // just a little something not too loud or remarkable. 
    [SerializeField] private AudioSource hitSound;      // Should be most specific / iconic sound of each player
    [SerializeField] private AudioSource getHitSound;   // like an "ow"
    [SerializeField] private AudioSource dieSound;      // Quick and noticable
    [SerializeField] private AudioSource winSound;      // should be especially funny and a motivation to win to hear your character gloat
    [SerializeField] private AudioSource tiltSound;     // like a "wooah" if steadying onself or close to touching the ground maybe
    [SerializeField] private AudioSource runningSound;  // like a heavier breathing, probably want to keep this pretty quiet. 

    // Public players to be accessed by Player methods
    public void playJumpSound() { if (PersistentValues.persistentValues.gameIsStarted) jumpSound.Play(); }
    public void playHitSound() { if (PersistentValues.persistentValues.gameIsStarted) hitSound.Play(); }
    public void playGetHitSound() { getHitSound.Play(); }
    public void playDieSound() { if (PersistentValues.persistentValues.gameIsStarted) dieSound.Play(); }
    public void playWinSound() { winSound.Play(); }
    public void playTiltSound() { tiltSound.Play(); }
    public void playRunningSound() { runningSound.Play(); }
}
