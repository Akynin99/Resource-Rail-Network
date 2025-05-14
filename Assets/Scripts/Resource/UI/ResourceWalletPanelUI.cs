using TMPro;
using UnityEngine;
using Zenject;

namespace ResourceRailNetwork
{
    public class ResourceWalletPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmp;
        [SerializeField] private string prefix;

        [Inject] private IResourceWallet _resourceWallet;

        private void Start()
        {
            tmp.text = prefix + _resourceWallet.GetAmount();

            _resourceWallet.OnAmountChanged += OnAmountChanged;
        }

        private void OnAmountChanged(float amount)
        {
            tmp.text = prefix + amount;
        }

        private void OnDestroy()
        {
            _resourceWallet.OnAmountChanged -= OnAmountChanged;
        }
    }
}