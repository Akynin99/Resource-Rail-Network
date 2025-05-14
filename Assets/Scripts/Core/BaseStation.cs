using UnityEngine;

namespace ResourceRailNetwork.Core
{
    /// <summary>
    /// Base station node handling resource multiplication in a rail network.
    /// Inherits core node functionality and tracks resource multiplier changes at runtime.
    /// </summary>
    public class BaseStation : Node
    {
        [SerializeField] private float resourceMult;
        
        private float _lastMult;
        
        public float ResourceMult => resourceMult;
        
        protected override void Awake()
        {
            base.Awake();

            _lastMult = resourceMult;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying) return;

            if (_lastMult != resourceMult)
            {
                _lastMult = resourceMult;

                SettingsChanged();
            }
        }
#endif
    }
}