using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ProjectGame.Features.Player.Configs;
using ProjectGame.Features.Player.Interfaces;

namespace ProjectGame.Features.Weapons
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private AssetReference ProjectileReference;
        [SerializeField] private Transform FirePoint;

        // State
        private IPlayerInput _input;
        private PlayerSettingsSO _settings;
        private ObjectPool<Projectile> _pool;
        private float _nextFireTime;
        
        private GameObject _loadedPrefab; 
        private AsyncOperationHandle<GameObject> _prefabHandle;
        private bool _isReady = false;
        
        private Transform _poolContainer;

        // Dependency Injection
        public void Initialize(IPlayerInput input, PlayerSettingsSO settings)
        {
            _input = input;
            _settings = settings;
            
            LoadProjectileAsset();
        }
        
        private void LoadProjectileAsset()
        {
            ProjectileReference.LoadAssetAsync<GameObject>().Completed += OnPrefabLoaded;
        }
        
        private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedPrefab = handle.Result;
                _prefabHandle = handle;
                
                GameObject containerObj = new GameObject("--- [Projectile Pool] ---");
                _poolContainer = containerObj.transform;
                
                InitializePool();
                _isReady = true;
            }
            else
            {
                Debug.LogError($"Failed to load projectile: {handle.OperationException}");
            }
        }
        
        private void InitializePool()
        {
            _pool = new ObjectPool<Projectile>(
                createFunc: () => {
                    GameObject obj = Instantiate(_loadedPrefab, _poolContainer); 
                    return obj.GetComponent<Projectile>();
                },
                actionOnGet: (bullet) => bullet.gameObject.SetActive(true),
                actionOnRelease: (bullet) => bullet.gameObject.SetActive(false),
                actionOnDestroy: (bullet) => Destroy(bullet.gameObject),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 50
            );
        }

        private void Update()
        {
            if (_input == null || _settings == null || !_isReady) return;

            if (_input.IsFiring && Time.time >= _nextFireTime)
            {
                Fire();
            }
        }

        private void Fire()
        {
            _nextFireTime = Time.time + _settings.FireRate;
            
            Projectile bullet = _pool.Get();

            // Position it at the gun tip
            bullet.transform.position = FirePoint.position;
            bullet.transform.rotation = FirePoint.rotation;
            
            // We pass _pool.Release as the callback so the bullet handles its own return
            bullet.Initialize(
                direction: transform.up, 
                speed: _settings.BulletSpeed, 
                lifetime: 2.0f, // Hardcoded for now, or add to SettingsSO
                returnAction: _pool.Release 
            );
        }
        
        private void OnDestroy()
        {
            DestroyPoolContainer();
            ReleasePrefabHandle();
        }

        private void DestroyPoolContainer()
        {
            if (_poolContainer != null)
            {
                Destroy(_poolContainer.gameObject);
            }
        }

        private void ReleasePrefabHandle()
        {
            if (_prefabHandle.IsValid())
            {
                Addressables.Release(_prefabHandle);
            }
        }
    }
}