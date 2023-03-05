using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdController : MonoBehaviour
{

    // Controls one member of the crowd. Set up to receive signals to tell them when to clap and cheer. 
    // TODO maybe even add booing and shock / gasps for further fun
    private Animator animator;

    // TODO have an audiocomponent too for each action

    private void Start()
    {
        // At the start of a scene with this crowd member add itself to persistentValue's List of 
        // active crowd members. Take itself out at end of life. 
        PersistentValues.persistentValues.crowd.Add(this);
        // Find animator in child
        animator = GetComponentInChildren<Animator>();
    }

    public void beginClap()
    {
        StartCoroutine(clap());
    }

    IEnumerator clap()
    {
        // hesistate a random split second so that claps aren't exaclty synched and
        // look more natural. 
        float delay = Random.Range(0.0f, 1.0f);
        yield return new WaitForSeconds(delay);
        //After delay signal animator to clap
        animator.SetTrigger("Clap");
    }

    public void beginCheer()
    {
        StartCoroutine(cheer());
    }

    IEnumerator cheer()
    {
        // hesistate a random split second so that jumps aren't exaclty synched and
        // look more natural, but wait less than the claps
        float delay = Random.Range(0.0f, 0.5f);
        yield return new WaitForSeconds(delay);
        //After delay signal animator to clap
        animator.SetTrigger("Cheer");
    }

    private void OnDestroy()
    {
        // Remove itself from persistent value's list of crowd members. 
        PersistentValues.persistentValues.crowd.Remove(this);
    }
}
