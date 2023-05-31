using System;
using UnityEngine;

namespace _Scripts.Player.Structure
{
    public class PlacableObject : MonoBehaviour
    {
        public bool Placed { get; private set; }
        public Vector3Int Size { get; private set; }
        private Vector3[] vertices;

        private void GetColliderVertexPositions()
        {
            BoxCollider box = gameObject.GetComponent<BoxCollider>();
            vertices = new Vector3[4];
            Vector3 size = box.size;
            Vector3 center = box.center;
            vertices[0] = center + new Vector3(-size.x, -size.y, -size.z) * 0.5f;
            vertices[1] = center + new Vector3(size.x, -size.y, -size.z) * 0.5f;
            vertices[2] = center + new Vector3(size.x, -size.y, size.z) * 0.5f;
            vertices[3] = center + new Vector3(-size.x, -size.y, size.z) * 0.5f;
        }

        private void CalculateSizeInCells()
        {
            Vector3Int[] verticesInWorld = new Vector3Int[vertices.Length];

            for (int i = 0; i < verticesInWorld.Length; i++)
            {
                Vector3 worldPos = transform.TransformPoint(vertices[i]);
                verticesInWorld[i] = BuildingHandler.instance.gridLayout.WorldToCell(worldPos);
            }

            Size = new Vector3Int(Math.Abs((verticesInWorld[0] - verticesInWorld[1]).x), Math.Abs((verticesInWorld[0] - verticesInWorld[3]).y), 1);
        }

        public Vector3 GetStartPosition()
        {
            return transform.TransformPoint(vertices[0]);
        }

        public void Rotate()
        {
            transform.Rotate(new Vector3(0, 90, 0));
            Size = new Vector3Int(Size.y, Size.x, 1);

            Vector3[] vertices = new Vector3[this.vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = this.vertices[(i + 1) % vertices.Length];
            }

            this.vertices = vertices;
        }

        void Start()
        {
            GetColliderVertexPositions();
            CalculateSizeInCells();
        }

        public virtual void Place()
        {
            Placed = true;

            // invoke events of placement
        }
    }
}