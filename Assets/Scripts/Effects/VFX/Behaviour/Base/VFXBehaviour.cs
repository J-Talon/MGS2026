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

    public abstract void onPlay();

    public abstract void onStop();
}
