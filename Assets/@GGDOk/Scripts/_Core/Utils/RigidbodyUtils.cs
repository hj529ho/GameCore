using System;
using UnityEngine;

namespace Core.Utils
{
    public static class RigidbodyUtils
    {
        public static void Teleport(this Rigidbody rigid, Vector3 position)
        {
            // TODO
            throw new NotImplementedException();
        }
        public static void Teleport(this Rigidbody2D rigid, Vector2 position)
        {
            rigid.simulated = false;
            rigid.transform.position = position;
            rigid.simulated = true;
        }
    }
}