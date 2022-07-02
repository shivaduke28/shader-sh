using System;
using UnityEngine;

namespace SH
{
    public class SHOverrideSample : MonoBehaviour
    {
        [SerializeField] SHSerializer[] shSerializers;
        int index = 0;

        MaterialPropertyBlock block;
        [SerializeField] Renderer[] renderers;

        void Start()
        {
            block = new MaterialPropertyBlock();
        }

        [ContextMenu("Apply")]
        void Apply()
        {
            var shSerializer = shSerializers[index];
            var sh = shSerializer.Deserialize();
            foreach (var renderer in renderers)
            {
                renderer.GetPropertyBlock(block);
                SHApplier.Apply(block, sh);
                renderer.SetPropertyBlock(block);
            }

            index = (index + 1) % shSerializers.Length;
        }
    }
}