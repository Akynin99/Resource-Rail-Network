using ResourceRailNetwork.Attributes;
using UnityEngine;

namespace ResourceRailNetwork.Train
{
    public class TrainDebugInfo : MonoBehaviour
    {
        [ReadOnly]
        public string readOnlyText;

        public void SetText(string text)
        {
            readOnlyText = text;
        }
    }
}