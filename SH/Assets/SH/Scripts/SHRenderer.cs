using UnityEngine;
using UnityEngine.Rendering;

namespace SH
{
    public class SHRenderer : MonoBehaviour
    {
        [SerializeField] Renderer[] renderers;
        [SerializeField] Light[] lights;
        [SerializeField] bool[] shEnable = { true, true, true, true, true, true, true, true, true, };

        MaterialPropertyBlock block;
        SphericalHarmonicsL2 sh;

        void Awake()
        {
            block = new MaterialPropertyBlock();
            sh = new SphericalHarmonicsL2();
        }

        private void LateUpdate()
        {
            block.Clear();
            sh.Clear();

            foreach (var l in lights)
            {
                var count = Mathf.Min(shEnable.Length, 9);
                for (var i = 0; i < count; i++)
                {
                    if (shEnable[i])
                    {
                        SHUtility.AddDirectionalLightSH(ref sh, i, -l.transform.forward, l.color, l.intensity);
                    }
                }
            }
            SHUtility.ApplySH(block, sh);
            foreach (var r in renderers)
            {
                r.SetPropertyBlock(block);
            }
        }
    }
}