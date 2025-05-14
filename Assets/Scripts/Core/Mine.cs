using UnityEngine;

namespace ResourceRailNetwork.Core
{
    /// <summary>
    /// Mining node specialization handling time multiplier logic. Inherits Node functionality,
    /// ensures non-negative time values, and tracks runtime multiplier changes in editor.
    /// </summary>
    public class Mine : Node
    {
        [SerializeField] private float timeMult;

        private float _lastMult;
            
        public float TimeMult
        {
            get
            {
                if (timeMult < 0) return 0;
                return timeMult;
            }
        }

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