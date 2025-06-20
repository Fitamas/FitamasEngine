using ClipperLib;
using Fitamas.ECS;
using Fitamas.Math;
using Microsoft.Xna.Framework;
using System;

namespace Fitamas.Animation.Rigging
{
    public abstract class Rig
    {
        //private RigController controller;

        ////public AnimationSkeleton Skeleton => controller.Skeleton;
        //public Vector2 Target { get; set; }

        //public Rig(RigController controller)
        //{
        //    this.controller = controller;
        //    controller.AddRig(this);
        //}

        //public abstract void Update(float deltaTime);
    }

    //public class TwoBoneRig : Rig
    //{
    //    public Vector2 direction = new Vector2(1, 1);

    //    private string root;
    //    private string mid;
    //    private string tip;

    //    public TwoBoneRig(RigController controller, string root, string mid, string tip) : base(controller)
    //    {
    //        this.root = root;
    //        this.mid = mid;
    //        this.tip = tip;
    //    }

    //    public override void Update(float deltaTime)
    //    {
    //        Transform transformRoot = Skeleton.GetTransform(root);
    //        Transform transformMid = Skeleton.GetTransform(mid);
    //        Transform transformTip = Skeleton.GetTransform(tip);

    //        float weightRoot = Skeleton.GetWeight(root);
    //        float weightMid = Skeleton.GetWeight(tip);

    //        float r1 = Vector2.Distance(transformRoot.Position, transformMid.Position);
    //        float r2 = Vector2.Distance(transformMid.Position, transformTip.Position);
    //        Vector2 targetPosition = Target - transformRoot.Position;

    //        if (targetPosition.Length() > r1 + r2)
    //        {
    //            targetPosition = targetPosition.NormalizeF() * (r1 + r2) * 0.999f; //TODO fix this
    //        }

    //        float A = -2 * targetPosition.X;
    //        float B = -2 * targetPosition.Y;
    //        float C = targetPosition.X * targetPosition.X + targetPosition.Y * targetPosition.Y + r1 * r1 - r2 * r2;
    //        float X0 = -A * C / (A * A + B * B);
    //        float Y0 = -B * C / (A * A + B * B);
    //        float D = MathF.Sqrt(r1 * r1 - C * C / (A * A + B * B));
    //        float mult = MathF.Sqrt(D * D / (A * A + B * B));

    //        Vector2 point1 = new Vector2(X0 + B * mult, Y0 - A * mult);
    //        Vector2 point2 = new Vector2(X0 - B * mult, Y0 + A * mult);

    //        Vector2 result;
    //        if (Vector2.DistanceSquared(point1, direction) > Vector2.DistanceSquared(point2, direction))
    //        {
    //            result = point2;
    //        }
    //        else
    //        {
    //            result = point1;
    //        }

    //        float angleRoot = MathV.SignedAngle(new Vector2(0, 1), -result);
    //        float angleMid = MathV.SignedAngle(new Vector2(0, 1), result - targetPosition);

    //        transformRoot.Rotation = MathV.Lerp(transformRoot.Rotation, angleRoot, weightRoot);
    //        transformMid.Rotation = MathV.Lerp(transformMid.Rotation, angleMid, weightMid);
    //    }
    //}

    //public class AimRig : Rig
    //{
    //    private string bone;
    //    private Vector2 vectorUp;
    //    private Vector2 vectorRight;

    //    public AimRig(RigController controller, string bone, Vector2 vectorUp, Vector2 vectorRight) : base(controller)
    //    {
    //        this.bone = bone;
    //        this.vectorUp = vectorUp;
    //        this.vectorRight = vectorRight;
    //    }

    //    public override void Update(float deltaTime)
    //    {
    //        Transform transform = Skeleton.GetTransform(bone);

    //        float weightBone = Skeleton.GetWeight(bone);

    //        Vector2 direction = Target - transform.Position;
    //        float angle = MathV.SignedAngle(vectorRight, direction);

    //        if (MathV.SignedAngle(vectorUp, direction) > 0)
    //        {
    //            angle += MathF.PI;
    //        }

    //        transform.Rotation = MathV.Lerp(transform.Rotation, angle, weightBone);
    //    }
    //}

    //public class MoveRig : Rig
    //{
    //    private string bone;

    //    public float angle = 0;
    //    public Vector2 localPosition = Vector2.Zero;

    //    public MoveRig(RigController controller, string bone) : base(controller)
    //    {
    //        this.bone = bone;
    //    }

    //    public override void Update(float deltaTime)
    //    {
    //        Transform transform = Skeleton.GetTransform(bone);
    //        float weight = Skeleton.GetWeight(bone);

    //        transform.LocalPosition = Vector2.Lerp(transform.LocalPosition, localPosition, weight);
    //        transform.Rotation = MathV.Lerp(transform.Rotation, angle, weight);
    //    }
    //}
}
