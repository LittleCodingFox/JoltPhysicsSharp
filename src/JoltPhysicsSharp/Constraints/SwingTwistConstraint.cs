// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using static JoltPhysicsSharp.JoltApi;

namespace JoltPhysicsSharp;

public sealed class SwingTwistConstraintSettings : TwoBodyConstraintSettings
{
    public SwingTwistConstraintSettings()
        : base(JPH_SwingTwistConstraintSettings_Create())
    {
    }

    public override TwoBodyConstraint CreateConstraint(in Body body1, in Body body2)
    {
        return new SliderConstraint(JPH_SwingTwistConstraintSettings_CreateConstraint(Handle, body1.Handle, body2.Handle));
    }
}

public sealed class SwingTwistConstraint : TwoBodyConstraint
{
    internal SwingTwistConstraint(nint handle)
        : base(handle)
    {
    }

    public float GetNormalHalfConeAngle() => JPH_SwingTwistConstraint_GetNormalHalfConeAngle(Handle);
}
