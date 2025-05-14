using UnityEngine;

namespace ResourceRailNetwork.Graph
{
    public class Mine : Node
    {
        [SerializeField] private float timeMult;

        private float _lastMult;
            
        public float TimeMult => timeMult;

        protected override void Awake()
        {
            base.Awake();

            _lastMult = timeMult;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying) return;

            if (_lastMult != timeMult)
            {
                _lastMult = timeMult;

                SettingsChanged();
            }
        }
#endif
    }
}