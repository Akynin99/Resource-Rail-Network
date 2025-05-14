using UnityEngine;

namespace ResourceRailNetwork.Graph
{
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