using System;
using UnityEngine;
using Aurayu.VoxelWorld.Voxel;
using Aurayu.VoxelWorld.Voxel.Block;
using JetBrains.Annotations;

namespace Aurayu.VoxelWorld.Unity
{
    public class SetBlockCamera : MonoBehaviour
    {
        private Vector2 _rotation;

        [SerializeField]
        private World _world = null;

        private int _environmentLayerMask;

        // Use this for initialization
        internal void Start()
        {
            Debug.Assert(_world != null, "_world != null");

            _environmentLayerMask = 1 << LayerMask.NameToLayer("Environment");
        }

        // Update is called once per frame
        internal void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.Log("Fire1");
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 100, _environmentLayerMask))
                {
                    Debug.DrawLine(transform.position, hit.point);
                    print("Hit: " + hit.point + ", collider: " + hit.collider);

                    if (!SetBlock(hit, new AirBlock()))
                    {
                        print("Couldn't convert block at" + hit.point + " to air block");
                    }
                }
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                Debug.Log("Fire1");

                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, 100, _environmentLayerMask))
                {
                    print("Hit: " + hit.point);
                    SetBlock(hit, new SolidBlock(), true);
                }
            }
        }

        private bool SetBlock(RaycastHit hit, IBlock block, bool adjacent = false)
        {
            var position = GetBlockPosition(hit, adjacent);
            return _world.SetBlock(position, block);
        }

        [CanBeNull]
        private IBlock GetBlock(RaycastHit hit, bool adjacent = false)
        {
            var position = GetBlockPosition(hit, adjacent);
            var block = _world.GetBlock(position);
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
