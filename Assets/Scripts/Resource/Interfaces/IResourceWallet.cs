using System;

namespace ResourceRailNetwork.Interfaces
{
    public interface IResourceWallet
    {
        void AddResource(float value);
        float GetAmount();
        public event Action<float> OnAmountChanged; 
    }
}