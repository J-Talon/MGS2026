using System.IO;
using Unity.Collections;
using UnityEngine;

namespace Particle
{
    public class ParticleType
    {
        
        private string _assetPath;
        private GameObject _prefab;
        

        public ParticleType(string assetPath) {
            _assetPath = assetPath;
            _prefab = Resources.Load<GameObject>(assetPath);
            
            if (_prefab == null) 
                throw new FileNotFoundException("Could not find the particle system asset:",  assetPath);
            
            if (_prefab.GetComponent<ParticleSystem>() == null)
                throw new MissingComponentException("The particle asset "+assetPath+" must have a particle system component");

            if (_prefab.GetComponent<ParticleWrapper>() == null)
                throw new MissingComponentException("The particle asset "+assetPath+" must have a particle wrapper script component");
        }
        
        public string GetAssetPath() {
            return _assetPath;
        }
        
        
        public ParticleWrapper Create(float locationX, float locationY, float locationZ)
        {
            GameObject instance = Object.Instantiate(_prefab);
            ParticleWrapper wrapper = instance.GetComponent<ParticleWrapper>();
            wrapper.Initialize(_assetPath);

            instance.transform.position = new Vector3(locationX, locationY, locationZ);
            return wrapper;
        }

        
        //==============================

        
        
        public static readonly ParticleType X = new ParticleType("X");
        



        
        
        
    }
}