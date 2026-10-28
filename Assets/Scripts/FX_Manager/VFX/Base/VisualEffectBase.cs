using UnityEngine;

/*
Abstract base class for vfx
Should not allow other functions to run in its entirety without Setup being ran once
TODO: Materials, Movement behaviours

@author Wilson (1oopy)
*/
public abstract class VisualEffectBase : MonoBehaviour
{
    // References
    protected VFXBehaviour vfxBehaviour;

    // Materials
    protected Renderer[] activeRenderers;
    protected Renderer[] selfRenderers; // if you want to change the effect materials go ahead
    protected MaterialPropertyBlock _propBlock;

    // Stats
    protected VFXType _effectType;
    protected Vector2 _initPosition;
    protected bool playing = false;
    protected bool ready = false;
    protected int _vfxID;

    public int GetVFXID() {return _vfxID;}
    public VFXType GetVFXType() {return _effectType;}

    protected virtual void Awake()
    {
        // Set up reference to self renderers
        selfRenderers = GetComponentsInChildren<Renderer>();
        // Make a new material property block
        _propBlock = new MaterialPropertyBlock();
    }

    public virtual void Setup(VFXType effectType, Vector2 initPosition)
    {
        // assign a unique id on setup
        _vfxID = VisualEffectsManager.GetUniqueID();

        _effectType = effectType;
        _initPosition = initPosition; // stored just in case

        // Get the behaviour of the VFX
        vfxBehaviour = GetComponent<VFXBehaviour>();
        if (vfxBehaviour == null) // fallback
        {
            vfxBehaviour = gameObject.AddComponent<VFXSimple>();
        }

        // Default to setting its on materials
        activeRenderers = selfRenderers;

        gameObject.transform.position = initPosition;

        ready = true;
    }

    // Target some other objects materials (overloaded)
    public virtual void Setup(VFXType effectType, GameObject otherObject)
    {
        // assign a unique id on setup
        _vfxID = VisualEffectsManager.GetUniqueID();

        _effectType = effectType;
        _initPosition = otherObject.transform.position;

         // Get the behaviour of the VFX
        vfxBehaviour = GetComponent<VFXBehaviour>();
        if (vfxBehaviour == null) // fallback
        {
            vfxBehaviour = gameObject.AddComponent<VFXSimple>();
        }

        activeRenderers = otherObject.GetComponentsInChildren<Renderer>();

        gameObject.transform.position = _initPosition;

        ready = true;
    }

    public virtual void Play()
    {
        if (!ready) {return;}
        vfxBehaviour.onPlay();
        playing = true;
    }

    public virtual void Stop()
    {
        if (!ready) {return;}
        vfxBehaviour.onStop();
        Reset();
    }

    public virtual void Reset()
    {
        _vfxID = -1;
        playing = false;
        ready = false;

        if (activeRenderers == null) return;
        
        foreach (var renderer in activeRenderers)
        {
            renderer.SetPropertyBlock(null); // this should definetly reset it to default instead.
        }

        activeRenderers = null;
    }

    // TODO
    // Applies mater
    // public void ApplyMaterial()

    // Im sorry. I will not implement all movement types.
    // TODO: Add movement types (lerp etc.)

    // Moves the VFX to a new position instantaneously
    public virtual void MoveToSnappy(Vector2 newPos)
    {
        if (!ready) {return;}
        gameObject.transform.position = newPos;
    }
}
