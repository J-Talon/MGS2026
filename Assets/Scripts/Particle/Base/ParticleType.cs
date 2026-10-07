using System.IO;
using UnityEngine;

namespace Particle.Base
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

            if (_prefab.GetComponent<ParticleWrapperBase>() == null)
                throw new MissingComponentException("The particle asset "+assetPath+" must have a particle wrapper script component");
        }
        
        public string GetAssetPath() {
            return _assetPath;
        }
        
        
        public ParticleWrapperBase Create(float locationX, float locationY, float locationZ)
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
                wrapperBase.StartPlaying();
                
                return wrapperBase;
            }
            
            body = wrapperBase.gameObject;
            body.transform.position = new Vector3(locationX, locationY, locationZ);
            body.SetActive(true);
            wrapperBase.StartPlaying();
            
            return wrapperBase;
        }

        
        //==============================

        
        
        public static readonly ParticleType X = new ParticleType("X");
        



        
        
        
    }
}