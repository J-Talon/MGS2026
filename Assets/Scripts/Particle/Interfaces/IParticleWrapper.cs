
/*
 * @Author Talon J
 */
namespace Particle.Interfaces
{
    public interface IParticleWrapper
    {
        public string GetAssetPath();
        
        public void StartPlaying();
        
        public void StopPlaying();
        
        public void Initialize(string particleType);
    }
}