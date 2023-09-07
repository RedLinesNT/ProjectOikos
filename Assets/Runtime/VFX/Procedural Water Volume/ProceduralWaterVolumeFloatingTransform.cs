using System;
using UnityEngine;

namespace Oikos.VFX.ProceduralWaterVolume {
    
    [AddComponentMenu("Oikos VFX/Procedural Water Volume Floating Transform Point")] public class ProceduralWaterVolumeFloatingTransform : MonoBehaviour{

        #region Attributes

        [SerializeField] ProceduralWaterVolumeHelper waterVolumeHelper = null;

        #endregion

        #region Properties

        public ProceduralWaterVolumeHelper WaterVolumeHelper { get { return waterVolumeHelper; } }

        #endregion

        #region MonoBehaviour's methods

        private void Update() {
            if(waterVolumeHelper == null || waterVolumeHelper?.WaterVolume == null) return; //Don't execute anything if there's no ProceduralWaterVolumeHelper assigned on this instance
            
            transform.position = new Vector3(transform.position.x, waterVolumeHelper.GetHeight(transform.position) ?? transform.position.y, transform.position.z);
        }

        #endregion
        
    }
}