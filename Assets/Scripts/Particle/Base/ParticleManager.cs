
using System.Collections.Generic;
using JetBrains.Annotations;


namespace Particle.Base
{
    
    /**
     *
     * Particle manager which is scene agnostic (usable anywhere without dependencies)
     * @author Talon J
     */
    public class ParticleManager
    {
     
        //there are benefits to not having monobehaviour objects. 
        //you decrease the amount of coupling, and can allow the object to be scene independent
        
        //object pools for active and reserved objects
        private static ParticleManager _instance = null;
        private readonly Dictionary<string, List<ParticleWrapperBase>> _reservePool;
        private readonly Dictionary<string, HashSet<ParticleWrapperBase>> _activePool;
        
        
        //True singleton pattern using private constructor
        private ParticleManager() {
            _reservePool = new Dictionary<string, List<ParticleWrapperBase>>();
            _activePool = new Dictionary<string, HashSet<ParticleWrapperBase>>();
        }
        
        
        public static ParticleManager GetInstance()
        {
            if (_instance == null)
                _instance = new  ParticleManager();
            
            return  _instance;
        }

        
        //start tracking a particle system wrapper
        public void StartTracking(ParticleWrapperBase wrapperBase) {
                string path = wrapperBase.GetAssetPath();
                
                _activePool.TryGetValue(path, out HashSet<ParticleWrapperBase> list);
                if (list == null)
                    _activePool.Add(path, new HashSet<ParticleWrapperBase>());
                
                _activePool[path].Add(wrapperBase);
             
        }


        
        /**
        Try to get a particle system object from the reserved pool if available
        since we may have many particles, reusing them is a good idea where possible
        */
        [CanBeNull]
        public ParticleWrapperBase ExtractObject(ParticleType particleType)
        {
            //since each particle is a different asset, all paths should be unique
            string assetPath = particleType.GetAssetPath();
            
            if (!_reservePool.ContainsKey(assetPath)) {
                _reservePool.Add(assetPath, new List<ParticleWrapperBase>());
                return null;
            }

            _reservePool.TryGetValue(assetPath, out List<ParticleWrapperBase> list);
            
            //this case really shouldn't be possible but here we are
            if (list == null) {
                list = new List<ParticleWrapperBase>();
                _reservePool[assetPath] = list;
                return null;
            }

            if (list.Count == 0) 
                return null;
            
            ParticleWrapperBase obj = list[0];
            list.RemoveAt(0);
            this.StartTracking(obj);
            
            return obj;
        }

        
        /**
         * Returns an object to the pool
         * Use this if you want to force a particle to stop playing
         */
        public void ReturnObject(ParticleWrapperBase wrapperBase)
        {
            string path = wrapperBase.GetAssetPath();
            
            if (_activePool.ContainsKey(path))
                _activePool[path].Remove(wrapperBase);
            
            wrapperBase.StopPlaying();
            
            _reservePool.TryGetValue(path, out List<ParticleWrapperBase> list);
            //this also shouldn't happen in theory
            if (list == null)
                throw new KeyNotFoundException("Particle "+path+" could not be returned to object pool!");
            
            list.Add(wrapperBase);
        }

        /**
         * Forcibly stops all particles of a certain type to stop playing.
         *
         */
        public bool ClearParticlesOfType(ParticleType particleType) {
            return this.ClearParticlesOfType(particleType.GetAssetPath());
        }
        
        
        //Don't use this function. It should stay private. Use the particle type instead
        //It prevents you needing to fumble around with strings
        private bool ClearParticlesOfType(string assetPath)
        {
            _activePool.TryGetValue(assetPath, out HashSet<ParticleWrapperBase> set);
                
            if (set == null)
                return false;
                
            List<ParticleWrapperBase> list = new List<ParticleWrapperBase>();
            foreach (ParticleWrapperBase wrapper in set) {
                wrapper.StopPlaying();
                list.Add(wrapper);
            }
            set.Clear();

            _reservePool.TryGetValue(assetPath, out List<ParticleWrapperBase> reserveList);
            if (reserveList == null)
                throw new KeyNotFoundException("Particle "+assetPath+" could not be returned to object pool!");
                
            reserveList.AddRange(list);
            
            return true;
        }

        
        /**
         * Stops all particles from playing
         */
        public void ClearAllParticles()
        {
            foreach (string key in _activePool.Keys) {
                ClearParticlesOfType(key);
            }
        }
    }
}