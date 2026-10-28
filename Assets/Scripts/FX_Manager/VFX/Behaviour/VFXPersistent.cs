using UnityEngine;

// @author Wilson (1oopy)
// This VFX does nothing fancy, it just shows up and stays.
// This script is pretty simple, it is needed for effects that will stay for long periods of time (Player grid crosshair)
public class VFXPersistent : VFXBehaviour
{
    /*
    // Update not needed here, commented out in case someone wants to make a change.
    void Update()
    {    
        // Make the effect do funky stuff here
    }
    */

    public override void onPlay()
    {
        visualEffect.gameObject.SetActive(true);
    }

    public override void onStop()
    {
        visualEffect.gameObject.SetActive(false);
    }

    public override void Tick()
    {
        // nothing
    }
}
