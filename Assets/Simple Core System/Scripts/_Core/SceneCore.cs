using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public interface ISceneService
    {
    }

    public interface IInitializable
    {
        IEnumerator Initialize();
    }

    [DefaultExecutionOrder(-10)]
    public class SceneCore : MonoBehaviour
    {
        public List<ISceneService> Services => _cachedServices;

        [Header("Scene Services")]
        [SerializeField]
        private List<GameObject> initializeableServices = new List<GameObject>();

        public UnityEvent onFinishInitialize = new UnityEvent();

        private List<ISceneService> _cachedServices = new();
        
        private void Awake()
        {
            
        }

        [Button]
        private void CacheServices()
        {
            var servicesOnScene = GameObject.FindGameObjectsWithTag(SceneServiceProvider.SCENE_SERVICE_TAG);

            foreach (var serviceGO in servicesOnScene)
            {
                if (initializeableServices.Contains(serviceGO) == false)
                    initializeableServices.Add(serviceGO);
            }

            foreach (var serviceGO in servicesOnScene)
            {
                var service = serviceGO.GetComponent<ISceneService>();
                if (service == null)
                    continue;

                _cachedServices.Add(service);
            }
        }

        private IEnumerator Start()
        {
            foreach (var serviceGO in initializeableServices)
            {
                var service = serviceGO.GetComponent<IInitializable>();
                if (service == null)
                    continue;

                yield return service.Initialize();
            }

            onFinishInitialize?.Invoke();
        }
    }

    public static class SceneServiceProvider
    {
        public const string SCENE_CORE_TAG = "SceneCore";        
        public const string SCENE_SERVICE_TAG = "SceneService";

        private static SceneCore _cachedCore;

        public static SceneCore GetSceneCore()
        {
            if (_cachedCore != null)
                return _cachedCore;

            var gameObject = GameObject.FindGameObjectWithTag(SCENE_CORE_TAG);
            _cachedCore = gameObject.GetComponent<SceneCore>();

            if (_cachedCore == null)
                Debug.LogWarning("No scene core in this scene");

            return _cachedCore;
        }

        public static T GetService<T>() where T : ISceneService
        {
            var core = GetSceneCore();
            foreach (var service in core.Services)
            {
                if (service.GetType() == typeof(T))
                    return (T)service;
            }

            Debug.LogWarning($"No services found in the scene");
            return default(T);
        }
    }    
}