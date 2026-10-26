using UnityEngine;

// @author Wilson (1oopy)
// If the VFX prefab does not have a behaviour, it falls back to this
// Does the same thing as a persistent behaviour
public class VFXSimple : VFXBehaviour
{
    // Constructor
    public VFXSimple(){}

    public override void onPlay()
    {
        visualEffect.gameObject.SetActive(true);
    }

    public override void onStop()
    {
        visualEffect.gameObject.SetActive(false);
    }
}
