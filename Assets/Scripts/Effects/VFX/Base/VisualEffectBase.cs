using UnityEngine;

/*
Abstract base class for vfx
Should not allow other functions to run in its entirety without Setup being ran once
TODO: Movement behaviours

@author Wilson (1oopy)
*/
public abstract class VisualEffectBase : MonoBehaviour
{
    protected VFXType _effectType;
    protected Vector2 _initPosition;
    protected VFXBehaviour vfxBehaviour;

    protected bool playing = false;
    protected bool ready = false;
    protected int _vfxID;

    public int GetVFXID() {return _vfxID;}
    public VFXType GetVFXType() {return _effectType;}

    public void Start()
    {
        // assign a unique id on start
        _vfxID = VisualEffectsManager.GetUniqueID();
    }

    public virtual void Setup(VFXType effectType, Vector2 initPosition)
    {
        _effectType = effectType;
        _initPosition = initPosition; // stored just in case

        // Get the behaviour of the VFX
        vfxBehaviour = GetComponent<VFXBehaviour>();
        if (vfxBehaviour == null) // fallback
        {
            vfxBehaviour = gameObject.AddComponent<VFXSimple>();
        }

        gameObject.transform.position = initPosition;

        ready = true;
    }

    public virtual void Play()
    {
        if (!ready) {return;}
        playing = true;
        vfxBehaviour.onPlay();
    }

    public virtual void Stop()
    {
        if (!ready) {return;}
        playing = false;
        vfxBehaviour.onStop();
    }

    // Im sorry. I will not implement all movement types.
    // TODO: Add movement types (lerp etc.)

    // Moves the VFX to a new position instantaneously
    public virtual void MoveToSnappy(Vector2 newPos)
    {
        if (!ready) {return;}
        gameObject.transform.position = newPos;
    }
}
