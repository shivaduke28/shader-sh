using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace SH
{
    public class SHSerializer : MonoBehaviour
    {
        [SerializeField] SHData shData;

        public void Serialize()
        {
            if (shData == null)
            {
                shData = new SHData();
            }

            LightProbes.GetInterpolatedProbe(transform.position, null, out var sh);
            shData.FromUnity(sh);
        }

        public SphericalHarmonicsL2 Deserialize()
        {
            Assert.IsNotNull(shData);
            return shData.ToUnity();
        }
    }
}