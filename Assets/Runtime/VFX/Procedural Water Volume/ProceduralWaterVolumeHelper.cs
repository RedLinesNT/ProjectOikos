using UnityEngine;

namespace Oikos.VFX.ProceduralWaterVolume {

    public class ProceduralWaterVolumeHelper : MonoBehaviour {

        #region Attributes

        [SerializeField] private ProceduralWaterVolumeBase waterVolume = null;

        #endregion

        #region Properties

        public ProceduralWaterVolumeBase WaterVolume { get { return waterVolume; } }

        #endregion

        #region ProceduralWaterVolumeHelper's methods

        public float? GetHeight(Vector3 _position) {
            //Don't go anywhere if there's no ProceduralWaterVolumeBase assigned (Just return 0)
            if (!waterVolume) return 0f;

            // ensure a material
            MeshRenderer _meshRen = waterVolume.gameObject.GetComponent<MeshRenderer>(); //Get the MeshRender of the ProceduralWaterVolumeBase
            if (!GetComponent<Renderer>() || !GetComponent<Renderer>().sharedMaterial)
            {
                return 0f;
            }

            // replicate the shader logic, using parameters pulled from the specific material, to return the height at the specified position
            var waterHeight = WaterVolume.GetHeight(_position);
            if (!waterHeight.HasValue)
            {
                return null;
            }
            var _WaveFrequency = GetComponent<Renderer>().sharedMaterial.GetFloat("_WaveFrequency");
            var _WaveScale = GetComponent<Renderer>().sharedMaterial.GetFloat("_WaveScale");
            var _WaveSpeed = GetComponent<Renderer>().sharedMaterial.GetFloat("_WaveSpeed");
            var time = Time.time * _WaveSpeed;
            var shaderOffset = (Mathf.Sin(_position.x * _WaveFrequency + time) + Mathf.Cos(_position.z * _WaveFrequency + time)) * _WaveScale;
            return waterHeight.Value + shaderOffset;
        }

        #endregion
        
    }
    
}