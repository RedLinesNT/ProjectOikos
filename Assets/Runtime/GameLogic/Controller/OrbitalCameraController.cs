using System.Collections;
using System.Collections.Generic;
using Oikos.Core;
using UnityEngine;

namespace Oikos.GameLogic.Controller {

    public class OrbitalCameraController : ACameraEntity {

        #region ACameraEntity's MonoBehaviour's Methods

        private protected override void OnAwakeEntity() {
            CameraName = $"Orbital Camera Controller"; //Set the Camera name
            CameraType = E_CAMERA_ENTITY_TYPE.CONTROLLER; //Set the Camera type
        }

        #endregion
        
    }
    
}