using Particle.Base;


/*
 * @Author Talon J
 */
namespace Particle.Behaviour
{
    public class ParticleSimple: ParticleWrapperBase
    {
        
        //if you need custom behaviour, you can inherit this class and override the OnPlay and OnStop methods
        //to do something when the particle system starts and stops playing
        //and this does inherit monobehaviour too so you get access to the Monobehaviour functionality
        
        //please DO NOT listen for the OnParticleSystemStopped event function. Use the OnStop method instead. 
        protected override void OnPlay() {
        }

        protected override void OnStop() {
        }
        
    }
}