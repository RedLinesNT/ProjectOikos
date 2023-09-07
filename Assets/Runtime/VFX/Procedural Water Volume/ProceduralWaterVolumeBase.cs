using System;
using System.Collections;
using System.Collections.Generic;
using Oikos.Core;
using UnityEngine;
using UnityEngine.UIElements;
using Logger = Oikos.Core.Logger;

namespace Oikos.VFX.ProceduralWaterVolume {
    
    [ExecuteInEditMode, RequireComponent(typeof(MeshFilter))] public class ProceduralWaterVolumeBase : MonoBehaviour {

        #region Constants

        /// <summary>
        /// Max tiles X
        /// </summary>
        private protected const int MAX_TILES_X = 100;
        /// <summary>
        /// Max tiles Y
        /// </summary>
        private protected const int MAX_TILES_Y = 50;
        /// <summary>
        /// Max tiles Z
        /// </summary>
        private protected const int MAX_TILES_Z = 100;

        #endregion

        #region TileFace Flags list

        [Flags] public enum E_TILE_FACE : int {
            NegX = 1,
            PosX = 2,
            NegZ = 4,
            PosZ = 8
        }

        #endregion

        #region Attributes

        [Header("Procedural Water Volume Base")]
        [SerializeField, FlagEnum] private E_TILE_FACE includeFaces = E_TILE_FACE.NegX | E_TILE_FACE.NegZ | E_TILE_FACE.PosX | E_TILE_FACE.PosZ;
        [SerializeField, FlagEnum] private E_TILE_FACE includeFoam = E_TILE_FACE.NegX | E_TILE_FACE.NegZ | E_TILE_FACE.PosX | E_TILE_FACE.PosZ;        
        [Space(10)]
        [SerializeField, Range(0.1f, 100f)] private float tileSize = 1f;
        [Space(10)]
        [SerializeField] private protected bool showDebug = true;
        [SerializeField] private protected bool realtimeUpdates = false;
        
        #endregion
        
        #region Runtime values

        /// <summary>
        /// Is this WaterVolume dirty
        /// </summary>
        protected bool isDirty = true;
        
        /// <summary>
        /// WaterVolume's mesh
        /// </summary>
        private Mesh mesh = null;
        /// <summary>
        /// WaterVolume's MeshFilter
        /// </summary>
        private MeshFilter meshFilter = null;

        /// <summary>
        /// WaterVolume's tiles
        /// </summary>
        private bool[,,] tiles = null;
        
        #endregion

        #region Properties

        public E_TILE_FACE IncludeFaces { get { return includeFaces; } private protected set { includeFaces = value; } }

        public E_TILE_FACE IncludeFoam { get { return includeFoam; } private protected set { includeFoam = value; } }

        public float TileSize { get { return tileSize; } private protected set { tileSize = value; } }
        
        public bool ShowDebug { get { return showDebug; } private protected set { showDebug = value; } }
        
        public bool RealtimeUpdates { get { return realtimeUpdates; } private protected set { realtimeUpdates = value; } }
        
        #endregion

        #region ProceduralWaterVolumeBase's private methods

        /// <summary>
        /// Check the references placed right next to this component.
        /// </summary>
        private void CheckReferences() {
            //Check the MeshFilter
            if(meshFilter == null) { //If there's no MeshFilter component assigned on this instance
                mesh = null; //Remove the mesh reference
                meshFilter = gameObject.GetComponent<MeshFilter>(); //Get the MeshFilter component
                
                if(meshFilter == null) { //If no MeshFilter component is placed next to this component
                    meshFilter = gameObject.AddComponent<MeshFilter>(); //Auto-add a MeshFilter component
                }
            }
            
            //Check the Mesh
            if(mesh == null) { //If there's no Mesh component assigned on this instance
                mesh = meshFilter.sharedMesh; //Try to get the Mesh from the MeshFilter component of this instance
                
                if(mesh == null || mesh.name != $"ProceduralWaterVolume ({gameObject.GetInstanceID()})") { //If there's no mesh or the name is incorrect (Incorrect mesh's name => Not a ProceduralWaterVolume's generated mesh)
                    mesh = new Mesh(); //Create a new Mesh
                    mesh.name = $"ProceduralWaterVolume ({gameObject.GetInstanceID()})"; //Set the name of this Mesh instance
                }
            }
            
            //Finally, set the MeshFilter's shared mesh
            meshFilter.sharedMesh = mesh;
        }

        /// <summary>
        /// Rebuild the Procedural mesh of this WaterVolume
        /// </summary>
        private protected void Rebuild() {
            Logger.Trace($"Procedural Water Volume ({name})", $"Rebuilding the procedural mesh of this ProceduralWaterMesh");
            
            CheckReferences(); //Call this method to check and assign reference before using them
            
            mesh.Clear(); //Delete the existing mesh (It'll be recreated here)
            
            //Allow child classes to generate the tiles
            tiles = new bool[MAX_TILES_X, MAX_TILES_Y, MAX_TILES_Z];
            GenerateTiles(ref tiles); //Call this virtual method to make the child classes to rebuild their own custom ProceduralWaterVolume
            
            //Create and prepare buffers for the generated mesh data
            List<Vector3> _vertices = new List<Vector3>(); //The generated mesh's vertices
            List<Vector3> _normals = new List<Vector3>(); //The generated mesh's normals
            List<Vector2> _uvs = new List<Vector2>(); //The generated mesh's UVs
            List<Color> _colors = new List<Color>(); //The generated mesh's colors
            List<int> _indices = new List<int>(); //The generated mesh's indices
            
            //Iterate on each tiles
            for(int x=0; x<MAX_TILES_X; x++) { //Loop on the tile's X
                for(int y=0; y<MAX_TILES_Y; y++) { //Loop on the tile's Y
                    for(int z=0; z<MAX_TILES_Z; z++) { //Loop on the tile's Z
                        
                        //Don't go anywhere if there's no water on this position
                        if(!tiles[x, y, z]) continue;
                        
                        //Calculate the tile's position
                        float _x0 = x * TileSize - 0.5f; //Negative X position
                        float _x1 = _x0 + TileSize; //Positive X position
                        float _y0 = y * TileSize - 0.5f; //Negative Y position
                        float _y1 = _y0 + TileSize; //Positive Y position
                        float _z0 = z * TileSize - 0.5f; //Negative Z position
                        float _z1 = _z0 + TileSize; //Positive Z position
                        float _ux0 = _x0 + transform.position.x; //Negative X UVs
                        float _ux1 = _x1 + transform.position.x; //Positive X UVs
                        float _uy0 = _y0 + transform.position.y; //Negative Y UVs
                        float _uy1 = _y1 + transform.position.y; //Positive Y UVs
                        float _uz0 = _z0 + transform.position.z; //Negative Z UVs
                        float _uz1 = _z1 + transform.position.z; //Positive Z UVs

                        //Check for the tile's edges
                        bool _negX = x == 0 || !tiles[x - 1, y, z]; //Negative X edge's position
                        bool _posX = x == MAX_TILES_X - 1 || !tiles[x + 1, y, z]; //Positive X edge's position
                        bool _negY = y == 0 || !tiles[x, y - 1, z]; //Negative Y edge's position
                        bool _posY = y == MAX_TILES_Y - 1 || !tiles[x, y + 1, z]; //Positive Y edge's position
                        bool _negZ = z == 0 || !tiles[x, y, z - 1]; //Negative Z edge's position
                        bool _posZ = z == MAX_TILES_Z - 1 || !tiles[x, y, z + 1]; //Positive Z edge's position
                        bool _negXnegZ = !_negX && !_negZ && x > 0 && z > 0 && !tiles[x - 1, y, z - 1]; //Negative X/Z edge's position
                        bool _negXposZ = !_negX && !_posZ && x > 0 && z < MAX_TILES_Z && !tiles[x - 1, y, z + 1]; //Negative X/Positive Y edge's position
                        bool _posXposZ = !_posX && !_posZ && x < MAX_TILES_X && z < MAX_TILES_Z && !tiles[x + 1, y, z + 1]; //Positive X/Positive Z edge's position
                        bool _posXnegZ = !_posX && !_negZ && x < MAX_TILES_X && z > 0 && !tiles[x + 1, y, z - 1]; //Positive X/Negative Z edge's position
                        bool _faceNegX = _negX && (IncludeFaces & E_TILE_FACE.NegX) == E_TILE_FACE.NegX; //Negative X face's position
                        bool _facePosX = _posX && (IncludeFaces & E_TILE_FACE.PosX) == E_TILE_FACE.PosX; //Positive X face's position
                        bool _faceNegZ = _negZ && (IncludeFaces & E_TILE_FACE.NegZ) == E_TILE_FACE.NegZ; //Negative Z face's position
                        bool _facePosZ = _posZ && (IncludeFaces & E_TILE_FACE.PosZ) == E_TILE_FACE.PosZ; //Positive Z face's position
                        bool _foamNegX = _negX && (IncludeFoam & E_TILE_FACE.NegX) == E_TILE_FACE.NegX; //Negative X foam's position
                        bool _foamPosX = _posX && (IncludeFoam & E_TILE_FACE.PosX) == E_TILE_FACE.PosX; //Positive X foam's position
                        bool _foamNegZ = _negZ && (IncludeFoam & E_TILE_FACE.NegZ) == E_TILE_FACE.NegZ; //Negative Z foam's position
                        bool _foamPosZ = _posZ && (IncludeFoam & E_TILE_FACE.PosZ) == E_TILE_FACE.PosZ; //Positive Z foam's position
                        bool _foamNegXnegZ = _negXnegZ && ((IncludeFoam & E_TILE_FACE.NegX) == E_TILE_FACE.NegX || (IncludeFoam & E_TILE_FACE.NegZ) == E_TILE_FACE.NegZ); //Negative X/Negative Z foam's position
                        bool _foamNegXposZ = _negXposZ && ((IncludeFoam & E_TILE_FACE.PosX) == E_TILE_FACE.PosX || (IncludeFoam & E_TILE_FACE.PosZ) == E_TILE_FACE.PosZ); //Negative X/Positive Z foam's position
                        bool _foamPosXposZ = _posXposZ && ((IncludeFoam & E_TILE_FACE.NegZ) == E_TILE_FACE.NegZ || (IncludeFoam & E_TILE_FACE.PosZ) == E_TILE_FACE.PosZ); //Positive X/Positive Z foam's position
                        bool _foamPosXnegZ = _posXnegZ && ((IncludeFoam & E_TILE_FACE.PosZ) == E_TILE_FACE.PosZ || (IncludeFoam & E_TILE_FACE.NegZ) == E_TILE_FACE.NegZ); //Positive X/Negative Z foam's position

                        //Create the procedural mesh's top faces
                        if (y == MAX_TILES_Y - 1 || !tiles[x, y + 1, z]) {
                            //Set the face's Vertices
                            _vertices.Add(new Vector3(_x0, _y1, _z0));
                            _vertices.Add(new Vector3(_x0, _y1, _z1));
                            _vertices.Add(new Vector3(_x1, _y1, _z1));
                            _vertices.Add(new Vector3(_x1, _y1, _z0));
                            //Set the face's Normals
                            _normals.Add(new Vector3(0, 1, 0));
                            _normals.Add(new Vector3(0, 1, 0));
                            _normals.Add(new Vector3(0, 1, 0));
                            _normals.Add(new Vector3(0, 1, 0));
                            //Set the face's UVs
                            _uvs.Add(new Vector2(_ux0, _uz0));
                            _uvs.Add(new Vector2(_ux0, _uz1));
                            _uvs.Add(new Vector2(_ux1, _uz1));
                            _uvs.Add(new Vector2(_ux1, _uz0));
                            //Set the face's Colors
                            _colors.Add(_foamNegX || _foamNegZ || _foamNegXnegZ ? Color.red : Color.black);
                            _colors.Add(_foamNegX || _foamPosZ || _foamNegXposZ ? Color.red : Color.black);
                            _colors.Add(_foamPosX || _foamPosZ || _foamPosXposZ ? Color.red : Color.black);
                            _colors.Add(_foamPosX || _foamNegZ || _foamPosXnegZ ? Color.red : Color.black);
                            int _verticesCount = _vertices.Count - 4; //The count of vertices registered
                            if (_foamNegX && _foamPosZ || _foamPosX && _foamNegZ) { //Set the foam's indices
                                _indices.Add(_verticesCount + 1);
                                _indices.Add(_verticesCount + 2);
                                _indices.Add(_verticesCount + 3);
                                _indices.Add(_verticesCount + 3);
                                _indices.Add(_verticesCount);
                                _indices.Add(_verticesCount + 1);
                            } else {
                                _indices.Add(_verticesCount);
                                _indices.Add(_verticesCount + 1);
                                _indices.Add(_verticesCount + 2);
                                _indices.Add(_verticesCount + 2);
                                _indices.Add(_verticesCount + 3);
                                _indices.Add(_verticesCount);
                            }
                        }
                        
                        //Create the procedural mesh's side faces
                        if (_faceNegX) {
                            //Set the face's Vertices
                            _vertices.Add(new Vector3(_x0, _y0, _z1));
                            _vertices.Add(new Vector3(_x0, _y1, _z1));
                            _vertices.Add(new Vector3(_x0, _y1, _z0));
                            _vertices.Add(new Vector3(_x0, _y0, _z0));
                            //Set the face's Normals
                            _normals.Add(new Vector3(-1, 0, 0));
                            _normals.Add(new Vector3(-1, 0, 0));
                            _normals.Add(new Vector3(-1, 0, 0));
                            _normals.Add(new Vector3(-1, 0, 0));
                            //Set the face's UVs
                            _uvs.Add(new Vector2(_uz1, _uy0));
                            _uvs.Add(new Vector2(_uz1, _uy1));
                            _uvs.Add(new Vector2(_uz0, _uy1));
                            _uvs.Add(new Vector2(_uz0, _uy0));
                            //Set the face's Colors
                            _colors.Add(Color.black);
                            _colors.Add(_posY ? Color.red : Color.black);
                            _colors.Add(_posY ? Color.red : Color.black);
                            _colors.Add(Color.black);
                            int _verticesCount = _vertices.Count - 4; //The count of vertices registered
                            //Set the face's indices
                            _indices.Add(_verticesCount);
                            _indices.Add(_verticesCount + 1);
                            _indices.Add(_verticesCount + 2);
                            _indices.Add(_verticesCount + 2);
                            _indices.Add(_verticesCount + 3);
                            _indices.Add(_verticesCount);
                        }
                        
                        if (_facePosX) {
                            //Set the face's Vertices
                            _vertices.Add(new Vector3(_x1, _y0, _z0));
                            _vertices.Add(new Vector3(_x1, _y1, _z0));
                            _vertices.Add(new Vector3(_x1, _y1, _z1));
                            _vertices.Add(new Vector3(_x1, _y0, _z1));
                            //Set the face's Normals
                            _normals.Add(new Vector3(1, 0, 0));
                            _normals.Add(new Vector3(1, 0, 0));
                            _normals.Add(new Vector3(1, 0, 0));
                            _normals.Add(new Vector3(1, 0, 0));
                            //Set the face's UVs
                            _uvs.Add(new Vector2(_uz0, _uy0));
                            _uvs.Add(new Vector2(_uz0, _uy1));
                            _uvs.Add(new Vector2(_uz1, _uy1));
                            _uvs.Add(new Vector2(_uz1, _uy0));
                            //Set the face's Colors
                            _colors.Add(Color.black);
                            _colors.Add(_posY ? Color.red : Color.black);
                            _colors.Add(_posY ? Color.red : Color.black);
                            _colors.Add(Color.black);
                            int _verticesCount = _vertices.Count - 4; //The count of vertices registered
                            //Set the face's indices
                            _indices.Add(_verticesCount);
                            _indices.Add(_verticesCount + 1);
                            _indices.Add(_verticesCount + 2);
                            _indices.Add(_verticesCount + 2);
                            _indices.Add(_verticesCount + 3);
                            _indices.Add(_verticesCount);
                        }
                        
                        if (_faceNegZ) {
                            //Set the face's Vertices
                            _vertices.Add(new Vector3(_x0, _y0, _z0));
                            _vertices.Add(new Vector3(_x0, _y1, _z0));
                            _vertices.Add(new Vector3(_x1, _y1, _z0));
                            _vertices.Add(new Vector3(_x1, _y0, _z0));
                            //Set the face's Normals
                            _normals.Add(new Vector3(0, 0, -1));
                            _normals.Add(new Vector3(0, 0, -1));
                            _normals.Add(new Vector3(0, 0, -1));
                            _normals.Add(new Vector3(0, 0, -1));
                            //Set the face's UVs
                            _uvs.Add(new Vector2(_ux0, _uy0));
                            _uvs.Add(new Vector2(_ux0, _uy1));
                            _uvs.Add(new Vector2(_ux1, _uy1));
                            _uvs.Add(new Vector2(_ux1, _uy0));
                            //Set the face's Colors
                            _colors.Add(Color.black);
                            _colors.Add(_posY ? Color.red : Color.black);
                            _colors.Add(_posY ? Color.red : Color.black);
                            _colors.Add(Color.black);
                            int _verticesCount = _vertices.Count - 4; //The count of vertices registered
                            //Set the face's indices
                            _indices.Add(_verticesCount);
                            _indices.Add(_verticesCount + 1);
                            _indices.Add(_verticesCount + 2);
                            _indices.Add(_verticesCount + 2);
                            _indices.Add(_verticesCount + 3);
                            _indices.Add(_verticesCount);
                        }
                        
                        if (_facePosZ) {
                            //Set the face's Vertices
                            _vertices.Add(new Vector3(_x1, _y0, _z1));
                            _vertices.Add(new Vector3(_x1, _y1, _z1));
                            _vertices.Add(new Vector3(_x0, _y1, _z1));
                            _vertices.Add(new Vector3(_x0, _y0, _z1));
                            //Set the face's Normals
                            _normals.Add(new Vector3(0, 0, 1));
                            _normals.Add(new Vector3(0, 0, 1));
                            _normals.Add(new Vector3(0, 0, 1));
                            _normals.Add(new Vector3(0, 0, 1));
                            //Set the face's UVs
                            _uvs.Add(new Vector2(_ux1, _uy0));
                            _uvs.Add(new Vector2(_ux1, _uy1));
                            _uvs.Add(new Vector2(_ux0, _uy1));
                            _uvs.Add(new Vector2(_ux0, _uy0));
                            //Set the face's Colors
                            _colors.Add(Color.black);
                            _colors.Add(_posY ? Color.red : Color.black);
                            _colors.Add(_posY ? Color.red : Color.black);
                            _colors.Add(Color.black);
                            int _verticesCount = _vertices.Count - 4; //The count of vertices registered
                            //Set the face's indices
                            _indices.Add(_verticesCount);
                            _indices.Add(_verticesCount + 1);
                            _indices.Add(_verticesCount + 2);
                            _indices.Add(_verticesCount + 2);
                            _indices.Add(_verticesCount + 3);
                            _indices.Add(_verticesCount);
                        }
                    }
                }
            }
            
            //Apply the buffers of the generated water's mesh to the mesh instance
            mesh.SetVertices(_vertices); //Set the Vertices
            mesh.SetNormals(_normals); //Set the Normals
            mesh.SetUVs(0, _uvs); //Set the UVs
            mesh.SetColors(_colors); //Set the Colors
            mesh.SetTriangles(_indices, 0); //Set the Triangles (Indices)
            
            mesh.RecalculateBounds(); //Refresh the mesh's bounds
            mesh.RecalculateTangents(); //Refresh the mesh's tangents
            
            //Set the generated mesh to the MeshFilter of this instance
            meshFilter.sharedMesh = mesh;
            
            //Flag this component as dirty
            isDirty = false;
            
        }
        
        #endregion

        #region ProceduralWaterVolumeBase's methods

        /// <summary>
        /// Take a position and return the height of this position in the WaterVolume.
        /// </summary>
        /// <param name="_position">The position to check the height</param>
        /// <returns>The height of this point in the WaterVolume.
        /// The result can be null if out of bounds!</returns>
        public float? GetHeight(Vector3 _position) {
            //Convert the position given to a WaterVolume's tile
            int _x = Mathf.FloorToInt((_position.x - transform.position.x + 0.5f) / TileSize);
            int _z = Mathf.FloorToInt((_position.z - transform.position.z + 0.5f) / TileSize);
            
            //Check if the tile's coordinates are out of bounds
            if(_x < 0 || _x >= MAX_TILES_X || _z < 0 || _z >= MAX_TILES_Z) {
                return null; //Return null, the coords are out of bounds!
            }
            
            //If there's no tile bool list, return null
            if(tiles == null) return null;
            
            //Find the highest active WaterVolume's block in the current column
            //TODO: Rework this part to taking in account the WaterVolume's gaps
            for(int y=MAX_TILES_Y - 1; y >= 0; y--) {
                if(tiles[_x, y, _z]) { //If the tile is the highest one and not out of bounds
                    return transform.position.y + y * TileSize; //Return the height of the point given in this WaterVolume
                }
            }
            
            //There's no water (Out of bound)
            return null;
        }
        
        #endregion

        #region ProceduralWaterVolumeBase's virtual methods

        protected virtual void GenerateTiles(ref bool[,,] _tiles) {}
        protected virtual void Validate() {}
        
        #endregion
        
        #region MonoBehaviour's Editor method

        private void OnValidate() {
            //Clamp the TileSize
            TileSize = Mathf.Clamp(TileSize, 0.1f, 100f);
            
            //Allow child classes to perform the ProceduralWaterVolume's validation
            Validate();
            
            //Set the IsDirty flag as needing Procedural mesh's rebuilding
            isDirty = true;
        }

        #endregion

        #region MonoBehaviour's method

        private void Update() {
            if(isDirty || (!Application.isPlaying && realtimeUpdates)) { //Rebuild the procedural mesh's if needed and not in play mode
                Rebuild();
            }
        }

        #endregion
        
    }
    
}