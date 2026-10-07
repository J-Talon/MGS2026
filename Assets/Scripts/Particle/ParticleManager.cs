using System.Collections.Generic;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;



//please read the task outlines people!!! We want an API, not a game object with a particle system component!!!


namespace Particle
{
    
    
    public class ParticleManager: MonoBehaviour
    {
        
        private static ParticleManager _instance = null;
        private Dictionary<string, List<ParticleWrapper>> _objectPool;
        private HashSet<ParticleWrapper> _activeSystems;
        
        
        public void Awake()
        {
            if (_instance != this && _instance != null)
                throw new DuplicateNameException("There can only be 1 particle manager!");

            _instance = this;
            _objectPool = new Dictionary<string, List<ParticleWrapper>>();
        }
        
        public static ParticleManager GetInstance()
        {
            return _instance;
        }


        [CanBeNull]
        public ParticleWrapper Extract(ParticleType particleType)
        {
            //since each particle is a different asset, all paths should be unique
            string assetPath = particleType.GetAssetPath();
            if (!_objectPool.ContainsKey(assetPath))
                throw new KeyNotFoundException("The particle type "+assetPath+" was not loaded properly");

            List<ParticleWrapper> list;
            _objectPool.TryGetValue(assetPath, out list);
            
            //this case really shouldn't be possible but here we are
            if (list == null) {
                list = new List<ParticleWrapper>();
                _objectPool[assetPath] = list;
                return null;
            }

            if (list.Count == 0) 
                return null;
            
            ParticleWrapper obj = list[0];
            list.RemoveAt(0);
            return obj;
        }


        
        
        public void Return(ParticleWrapper wrapper)
        {
            string path = wrapper.GetAssetPath();
            _objectPool.TryGetValue(path, out List<ParticleWrapper> list);
            
            //this also shouldn't happen in theory
            if (list == null)
                throw new KeyNotFoundException("Particle "+path+" could not be returned to object pool!");
            
            list.Add(wrapper);
        }
    }
}