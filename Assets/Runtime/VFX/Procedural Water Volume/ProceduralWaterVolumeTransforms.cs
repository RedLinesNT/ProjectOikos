using System;
using UnityEngine;

namespace Oikos.VFX.ProceduralWaterVolume {
    
    [AddComponentMenu("Oikos VFX/Procedural Water Volume (Transforms)")] public class ProceduralWaterVolumeTransforms : ProceduralWaterVolumeBase {

        #region MonoBehaviour's Editor methods

        private void OnDrawGizmos() {
            if(showDebug) return; //Don't show anything if the debug attribute is set to true
            
            //Iterate on every children transform points
            for(int i=0; i<transform.childCount; i++) {
                Vector3 _position = transform.GetChild(i).localPosition; //Get the current child position
                Vector3 _scale = transform.GetChild(i).localScale / TileSize; //Get the current child scale
                
                //Fix the tile position (Based on this current transform child) to a virtual grid
                int _x = Mathf.RoundToInt(_position.x / TileSize);
                int _y = Mathf.RoundToInt(_position.y / TileSize);
                int _z = Mathf.RoundToInt(_position.z / TileSize);
                
                Vector3 _drawPosition = new Vector3(_x, _y, _z) * TileSize; //Position to draw
                Vector3 _drawScale = new Vector3(Mathf.RoundToInt(_scale.x), Mathf.RoundToInt(_scale.y), Mathf.RoundToInt(_scale.z)) * TileSize; //Scale to draw
                
                //Correct the Draw position according to the scale and virtual grid
                _drawPosition += _drawScale / 2f;
                _drawPosition += transform.position;
                _drawPosition -= new Vector3(TileSize, TileSize, TileSize);
                
                //Render this transform position as a wired cube
                Gizmos.DrawWireCube(_drawPosition, _drawScale);
            }
        }

        private void OnTransformChildrenChanged() {
            Rebuild(); //Rebuild this ProceduralWaterVolume when a child transform's position has been modified
        }
        
        #endregion

        #region ProceduralWaterVolumeBase's virtual methods

        protected override void GenerateTiles(ref bool[,,] _tiles) {
            //Iterate on every children transform points
            for (int i=0; i<transform.childCount; i++) {
                Vector3 _position = transform.GetChild(i).localPosition; //Get the current child position
                Vector3 _scale = transform.GetChild(i).localScale / TileSize; //Get the current child scale
                
                //Fix the tile position (Based on this current transform child) to a virtual grid
                int _x = Mathf.RoundToInt(_position.x / TileSize);
                int _y = Mathf.RoundToInt(_position.y / TileSize);
                int _z = Mathf.RoundToInt(_position.z / TileSize);

                // iterate the size of the transform
                for (int ix = _x; ix < _x + Mathf.RoundToInt(_scale.x); ix++) {
                    for (int iy = _y; iy < _y + Mathf.RoundToInt(_scale.y); iy++) {
                        for (int iz = _z; iz < _z + Mathf.RoundToInt(_scale.z); iz++) {
                            //If this transform's position hasn't been changed, this position is valid and doesn't need a be recalculated
                            if (ix < 0 || ix >= MAX_TILES_X || iy < 0 | iy >= MAX_TILES_Y || iz < 0 || iz >= MAX_TILES_Z) {
                                continue;
                            }

                            //If this transform's position has been changed, add a new tile
                            _tiles[ix, iy, iz] = true;
                        }
                    }
                }
            }
        }

        #endregion
        
    }
    
}