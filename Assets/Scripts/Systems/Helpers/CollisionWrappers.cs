using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public struct PadCollisionInfo : ICollisionInfo
    {
        public Collision2D collision2D { get; private set; }
        public Vector2 additionalForce { get; private set; }
        public Vector2 otherVelocity { get; private set; }

        public PadCollisionInfo(Collision2D collision2D, Vector2 additionalForce, Vector2 otherVelocity)
            : this()
        {
            this.collision2D = collision2D;
            this.additionalForce = additionalForce;
            this.otherVelocity = otherVelocity;
        }
    }
    
    public struct BlockCollisionInfo : ICollisionInfo
    {
        public Collision2D collision2D { get; private set; }
        public Vector2 additionalForce { get; private set; }
        public Vector2 otherVelocity { get; private set; }

        public BlockCollisionInfo(Collision2D collision2D, Vector2 otherVelocity)
            : this()
        {
            this.collision2D = collision2D;

            this.additionalForce = Vector2.zero;
            this.otherVelocity = otherVelocity;
        }
    }

    public interface ICollisionInfo
    {
        Collision2D collision2D {get;}
        Vector2 additionalForce {get;}
        Vector2 otherVelocity {get;}
    }
}
