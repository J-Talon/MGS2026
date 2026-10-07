
using System.Collections.Generic;
using JetBrains.Annotations;


namespace Particle.Base
{
    
    
    public class ParticleManager
    {
        
        private static ParticleManager _instance = null;
        private readonly Dictionary<string, List<ParticleWrapperBase>> _reservePool;
        private readonly Dictionary<string, HashSet<ParticleWrapperBase>> _activePool;
        


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

        
        public void StartTracking(ParticleWrapperBase wrapperBase) {
                string path = wrapperBase.GetAssetPath();
                
                _activePool.TryGetValue(path, out HashSet<ParticleWrapperBase> list);
                if (list == null)
                    _activePool.Add(path, new HashSet<ParticleWrapperBase>());
                
                _activePool[path].Add(wrapperBase);
             
        }


        
        
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

        
        public bool ClearParticlesOfType(ParticleType particleType) {
            return this.ClearParticlesOfType(particleType.GetAssetPath());
        }


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

        

        public void ClearAllParticles()
        {
            foreach (string key in _activePool.Keys) {
                ClearParticlesOfType(key);
            }
        }
    }
}