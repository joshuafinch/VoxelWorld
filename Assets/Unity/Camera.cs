using System;
using UnityEngine;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using Aurayu.VoxelWorld.Voxel;
using Aurayu.VoxelWorld.Voxel.Block;
using JetBrains.Annotations;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Networking;

namespace Aurayu.VoxelWorld.Unity
{
    public class Camera : MonoBehaviour
    {
        private Vector2 _rotation;

        [SerializeField]
        private World _world;

        // Use this for initialization
        internal void Start()
        {
            Debug.Assert(_world != null, "_world != null");
        }

        // Update is called once per frame
        internal void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
                {
                    print("Hit: " + hit.point);
                    SetBlock(hit, new AirBlock());
                }
            }

            _rotation = new Vector2(
                _rotation.x + Input.GetAxis("Mouse X")*3,
                _rotation.y + Input.GetAxis("Mouse Y")*3);

            transform.localRotation = Quaternion.AngleAxis(_rotation.x, Vector3.up);
            transform.localRotation = Quaternion.AngleAxis(_rotation.y, Vector3.left);

            transform.position += transform.forward*3*Input.GetAxis("Vertical");
            transform.position += transform.right*3*Input.GetAxis("Horizontal");
        }

        private bool SetBlock(RaycastHit hit, IBlock block, bool adjacent = false)
        {
            var position = GetBlockPosition(hit, adjacent);

//            var chunk = hit.collider.GetComponent<Chunk>();
//            if (chunk == null)
//                return false;
//
//            chunk.ChunkData.SetBlock(position, block);

            _world.SetBlock(position, block);

            return true;
        }

        [CanBeNull]
        private IBlock GetBlock(RaycastHit hit, bool adjacent = false)
        {
            var position = GetBlockPosition(hit, adjacent);

            var block = _world.GetBlock(position);

//            var chunk = hit.collider.GetComponent<Chunk>();
//            if (chunk == null)
//                return null;
//
//            var block = chunk.ChunkData.GetBlock(position);
            return block;
        }

        private static Point3D GetBlockPosition(RaycastHit hit, bool adjacent = false)
        {
            var position = new Vector3(
                GetCenterOfBlockPosition(hit.point.x, hit.normal.x, adjacent),
                GetCenterOfBlockPosition(hit.point.y, hit.normal.y, adjacent),
                GetCenterOfBlockPosition(hit.point.z, hit.normal.z, adjacent)
                );

            
            return GetBlockPosition(position);
        }

        private static Point3D GetBlockPosition(Vector3 position)
        {
            var blockPosition = new Point3D(
                Mathf.RoundToInt(position.x),
                Mathf.RoundToInt(position.y),
                Mathf.RoundToInt(position.z)
                );

            Debug.Log("Got block position at x:" + blockPosition.X + ", y:" + blockPosition.Y + ", z:" + blockPosition.Z);
            return blockPosition;
        }

        private static float GetCenterOfBlockPosition(float position, float normal, bool adjacent = false)
        {
            var difference = position - (int)position;

            if (!(Math.Abs(Math.Abs(difference) - 0.5f) < 0.01f))
                return position;

            if (adjacent)
            {
                return position + normal / 2;
            }

            return position - normal / 2;
        }
    }
}
