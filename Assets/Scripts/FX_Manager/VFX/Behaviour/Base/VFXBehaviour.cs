using UnityEngine;

// @author Wilson (1oopy)
// Base class for VFX behaviour
// Needed for onPlay and onStop functions. You can do stuff like fade in on play!
public abstract class VFXBehaviour : MonoBehaviour
{

    // Reference to the visual effect
    protected VisualEffectBase visualEffect;

    // Get component
    void Start()
    {
        visualEffect = GetComponent<VisualEffectBase>();
    }

    // Behaviour on play
    public abstract void onPlay();

    // Behaviour on stop
    public abstract void onStop();

    // Tick function
    public abstract void Tick();

    // Do something like this in a vlass that needs it
    // void Update()
    // {
    //     Tick();
    // }
}
