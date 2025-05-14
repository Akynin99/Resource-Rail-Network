using System;

namespace ResourceRailNetwork
{
    public class ResourceWallet : IResourceWallet
    {
        private float _amount;

        public float GetAmount()
        {
            return _amount;
        }

        public event Action<float> OnAmountChanged; 
        
        public void AddResource(float value)
        {
            if (value <= 0) return;

            _amount += value;
            
            OnAmountChanged?.Invoke(_amount);
        }
    }
}