using System;

namespace ResourceRailNetwork.Resource
{
    public interface IResourceWallet
    {
        void AddResource(float value);
        float GetAmount();
        public event Action<float> OnAmountChanged; 
    }
}