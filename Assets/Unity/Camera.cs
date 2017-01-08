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
        private Vector2 rotation;

        // Use this for initialization
        internal void Start()
        {

        }

        // Update is called once per frame
        internal void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
                {
                    print("Hit: " + hit);
                    SetBlock(hit, new AirBlock());
                }
            }

            rotation = new Vector2(
                rotation.x + Input.GetAxis("Mouse X")*3,
                rotation.y + Input.GetAxis("Mouse Y")*3);

            transform.localRotation = Quaternion.AngleAxis(rotation.x, Vector3.up);
            transform.localRotation = Quaternion.AngleAxis(rotation.y, Vector3.left);

            transform.position += transform.forward*3*Input.GetAxis("Vertical");
            transform.position += transform.right*3*Input.GetAxis("Horizontal");
        }

        static bool SetBlock(RaycastHit hit, IBlock block, bool adjacent = false)
        {
            var chunk = hit.collider.GetComponent<Chunk>();
            if (chunk == null)
                return false;

            var position = GetBlockPosition(hit, adjacent);

            chunk.ChunkData.SetBlock(position, block);
            return true;
        }

        [CanBeNull]
        static IBlock GetBlock(RaycastHit hit, bool adjacent = false)
        {
            var chunk = hit.collider.GetComponent<Chunk>();
            if (chunk == null)
                return null;

            var position = GetBlockPosition(hit, adjacent);

            var block = chunk.ChunkData.GetBlock(position);
            return block;
        }

        static Point3D GetBlockPosition(Vector3 position)
        {
            var blockPosition = new Point3D(
                Mathf.RoundToInt(position.x),
                Mathf.RoundToInt(position.y),
                Mathf.RoundToInt(position.z)
                );

            return blockPosition;
        }

        static Point3D GetBlockPosition(RaycastHit hit, bool adjacent = false)
        {
            var position = new Vector3(
                GetCenterOfBlockPosition(hit.point.x, hit.normal.x, adjacent),
                GetCenterOfBlockPosition(hit.point.y, hit.normal.y, adjacent),
                GetCenterOfBlockPosition(hit.point.z, hit.normal.z, adjacent)
                );

            return GetBlockPosition(position);
        }

        static float GetCenterOfBlockPosition(float position, float normal, bool adjacent = false)
        {
            var difference = position - (int) position;

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
