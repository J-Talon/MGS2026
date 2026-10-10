using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;


/// <summary>
/// 
/// Pool system is adapted from Unity's built in framework for pooling (uses stack to use older ones first (not sure reasoning))
/// </summary>
public class EntityManager
{


    /// <summary>
    /// (taken straight from talon) singleton constructor (private)
    /// </summary>
    private EntityManager() {

    }

    /// <summary>
    /// Singleton instance
    /// </summary>
    public static EntityManager instance = null;

    /// <summary>
    /// Returns the singleton instance.
    /// </summary>
    /// <returns></returns>
    public static EntityManager GetInstance()
    {
        if (instance == null)
            instance = new EntityManager();
        
        return instance;
    }

    #region Pooling

    #endregion Pooling
}
