using UnityEngine;

namespace _Scripts
{
    public class Helper
    {
        public static bool IsInLayerMask(int layer, LayerMask layerMask)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}