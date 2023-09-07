using UnityEngine;

namespace Oikos.VFX.ProceduralWaterVolume {
    
    [AddComponentMenu("Oikos VFX/Procedural Water Volume (Box)")] public class ProceduralWaterVolumeBox : ProceduralWaterVolumeBase {

        #region Attributes

        [Header("Procedural Water Volume Box - Settings")]
        [SerializeField] private Vector3 dimensions = Vector3.zero;
        
        #endregion

        #region Properties

        /// <summary>
        /// The dimensions of this Procedural Water Volume Box
        /// </summary>
        public Vector3 Dimensions { get { return dimensions; } }

        #endregion

        #region ProceduralWaterVolumeBase's virtual methods

        protected override void GenerateTiles(ref bool[,,] _tiles) {
            //Calculate the box volume's (in tiles)
            int _maxX = Mathf.Clamp(Mathf.RoundToInt(dimensions.x / TileSize), 1, MAX_TILES_X);
            int _maxY = Mathf.Clamp(Mathf.RoundToInt(dimensions.y / TileSize), 1, MAX_TILES_Y);
            int _maxZ = Mathf.Clamp(Mathf.RoundToInt(dimensions.z / TileSize), 1, MAX_TILES_Z);

            //Populate the tile's with this box volume
            for (int x=0; x<_maxX; x++) {
                for (int y=0; y<_maxY; y++) {
                    for (int z=0; z<_maxZ; z++) {
                        _tiles[x, y, z] = true;
                    }
                }
            }
        }

        protected override void Validate() {
            //Keep the dimensions values clamped
            dimensions.x = Mathf.Clamp(dimensions.x, 1, MAX_TILES_X);
            dimensions.y = Mathf.Clamp(dimensions.y, 1, MAX_TILES_Y);
            dimensions.z = Mathf.Clamp(dimensions.z, 1, MAX_TILES_Z);
        }

        #endregion
        
    }
    
}