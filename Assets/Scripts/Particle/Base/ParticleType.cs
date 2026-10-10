using System.IO;
using UnityEngine;


/*
 * @Author Talon J
 */
namespace Particle.Base
{
    public class ParticleType
    {
        
        private readonly string _assetPath;
        private readonly GameObject _prefab;
        
        /**
         *
         * Each particle type should have a particle system and a script which inherits from ParticleWrapperBase
         * We throw exceptions to let you know that you're missing something
         */
        private ParticleType(string assetPath) {
            _assetPath = assetPath;
            _prefab = Resources.Load<GameObject>(assetPath);
            
            if (_prefab == null) 
                throw new FileNotFoundException("Could not find the particle system asset:"+assetPath);
            
            if (_prefab.GetComponent<ParticleSystem>() == null)
                throw new MissingComponentException("The particle asset "+assetPath+" must have a particle system component");

            if (_prefab.GetComponent<ParticleWrapperBase>() == null)
                throw new MissingComponentException("The particle asset "+assetPath+" must have a particle wrapper script component");
        }
        
        public string GetAssetPath() {
            return _assetPath;
        }
        
        
        
        /**
         *
         * Creates a new particle at a given location
         * @Return: A particle wrapper object which gives you access to the raw particle system component
         */
        public ParticleWrapperBase Play(float locationX, float locationY, float locationZ)
        {
            return this.Play(locationX, locationY, locationZ, Quaternion.identity);
        }
        
        
        /**
         *
         * Creates a new particle at a given location with an initial rotation
         * @Return: A particle wrapper object which gives you access to the raw particle system component
         */
        public ParticleWrapperBase Play(float locationX, float locationY, float locationZ, Quaternion rotation)
        {
            ParticleManager manager = ParticleManager.GetInstance();
            ParticleWrapperBase wrapperBase = manager.ExtractObject(this);

            GameObject body;
            
            if (wrapperBase == null) { 
                body = Object.Instantiate(_prefab);
                wrapperBase = body.GetComponent<ParticleWrapperBase>();
                wrapperBase.Initialize(_assetPath);
                manager.StartTracking(wrapperBase);
                body.transform.position = new Vector3(locationX, locationY, locationZ);
                body.transform.rotation = rotation;
                wrapperBase.StartPlaying();
                
                return wrapperBase;
            }
            
            body = wrapperBase.gameObject;
            body.transform.position = new Vector3(locationX, locationY, locationZ);
            body.transform.rotation = rotation;
            wrapperBase.StartPlaying();
            
            return wrapperBase;
        }
        

        //==============================
        //The base prefab for the particle variants can be found at:
        //"Assets/Prefab/ParticleSystem/BaseParticleBurst"
        //"Assets/Prefab/ParticleSystem/BaseParticleRepeat"
        //please put all particle prefabs in the resources folder under Resources/VFX/Particle
        
        //you are free to create your own base as well, so as long as it:
        // 1: has a script component which inherits from ParticleWrapperBase
        // 2: has a particle system component (The particle type class will check for these requirements and throw exceptions if they do not)
        // 3: For all particle types, you must ensure that the StopAction is set to CALLBACK
        
        

        
        /*
         @author Talon J
         How to create a new particle?
         
         1. Create a prefab (either a variant, or an entirely new prefab) which has:
           -> a particle system component
           -> a script which inherits from ParticleWrapperBase (use ParticleSimple if you just need something simple)
        Put this prefab in the resources folder.
           
         2. Create a new particle type below
         public static readonly ParticleType MY_PARTICLE = new ParticleType("path to the prefab");
         
         3. Call ParticleType.<Your particle here>.Play(...)
         
         Example:
           //How to spawn particles?
           //Simple: ParticleType.SPARK.Play(x, y, -1);
           //That's it. It's THAT easy. :P Enjoy.
         
         */
        
        
        //sample particle system that plays particles and then ends
        public static readonly ParticleType SPARK = new ParticleType("VFX/Particle/ParticleBurstSpark");
        
        //sample particle system that plays indefinitely
        public static readonly ParticleType FIRE = new ParticleType("VFX/Particle/ParticleFireRepeat");
        



        
        
        
    }
}