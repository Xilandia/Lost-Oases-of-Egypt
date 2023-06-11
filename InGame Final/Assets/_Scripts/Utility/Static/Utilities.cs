using UnityEngine;

namespace _Scripts.Utility.Static
{
    public class Utilities
    {
        public static bool IsInLayerMask(int layer, LayerMask layerMask)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}