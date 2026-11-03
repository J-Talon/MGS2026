using UnityEngine;

/*
Abstract base class for vfx
Should not allow other functions to run in its entirety without Setup being ran once

TODO: Materials, Movement behaviours

@author Wilson (1oopy)
*/
namespace FX_Manager.VFX.Base
{
    public abstract class VisualEffectBase : MonoBehaviour
    {
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
        public Renderer[] GetActiveRenderers() {return activeRenderers;}
        public void SetActiveRenderers(Renderer[] renderers) {activeRenderers = renderers;} 

        protected virtual void Awake()
        {
            // Set up reference to self renderers
            selfRenderers = GetComponentsInChildren<Renderer>();
            // Make a new material property block
            _propBlock = GetPropBlock();
        }

        // Set up
        // Pass an object reference through to set the vfx active renderer to the objects.
        public virtual void Setup(VFXType effectType, Vector2 initPosition, GameObject otherObject = null)
        {
            // reset
            Reset();

            // assign a unique id on setup
            _vfxID = VisualEffectsManager.GetUniqueID();

            _effectType = effectType;
            _initPosition = initPosition; // stored just in case

            if (otherObject != null)
            {
                // Set to change other object material
                activeRenderers = otherObject.GetComponentsInChildren<Renderer>();
            }
            else
            {
                // Default to setting its on materials
                activeRenderers = selfRenderers;  
            }
        
            gameObject.transform.position = initPosition;

            ready = true;
        }

        public void Play()
        {
            if (!ready) {return;}
            playing = true;
            OnPlay();
        }
        public void Stop()
        {
            if (!ready) {return;}
            playing = false;
            OnStop();
        }

        protected abstract void OnPlay();
        protected abstract void OnStop();

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

        protected virtual MaterialPropertyBlock GetPropBlock()
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            return propBlock;
        }

        // TODO
        // Applies material
        // public void ApplyMaterial()

        // Im sorry. I will not implement all movement types.
        // TODO: Add movement types (lerp etc.)

        // Moves the VFX to a new position instantaneously
        public virtual void MoveToSnappy(Vector2 newPos)
        {
            if (!ready) {return;}
            gameObject.transform.position = newPos;
        }

        // Tick function
        protected virtual void Tick(){}
    }
}
