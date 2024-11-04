// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using static JoltPhysicsSharp.JoltApi;

namespace JoltPhysicsSharp;

public static class Foundation
{
    /// <summary>
    /// If objects are closer than this distance, they are considered to be colliding (used for GJK) (unit: meter)
    /// </summary>
    public const float DefaultCollisionTolerance = 1.0e-4f;

    /// <summary>
    /// A factor that determines the accuracy of the penetration depth calculation. If the change of the squared distance is less than tolerance * current_penetration_depth^2 the algorithm will terminate. (unit: dimensionless)
    /// </summary>
    public const float DefaultPenetrationTolerance = 1.0e-4f;

    /// <summary>
    /// How much padding to add around objects
    /// </summary>
    public const float DefaultConvexRadius = 0.05f;

    /// <summary>
    /// Used by (Tapered)CapsuleShape to determine when supporting face is an edge rather than a point (unit: meter)
    /// </summary>
    public const float CapsuleProjectionSlop = 0.02f;

    /// <summary>
    /// Maximum amount of jobs to allow
    /// </summary>
    public const int MaxPhysicsJobs = 2048;

    /// <summary>
    /// Maximum amount of barriers to allow
    /// </summary>
    public const int MaxPhysicsBarriers = 8;

    public static bool Init(bool doublePrecision = false)
    {
        JoltApi.DoublePrecision = doublePrecision;
        return JPH_Init();
    }

    public static void Shutdown() => JPH_Shutdown();

    private static TraceDelegate? s_traceCallback;
    private static AssertFailedDelegate? s_assertCallback;

    public static unsafe void SetTraceHandler(TraceDelegate callback)
    {
        s_traceCallback = callback;

        JPH_SetTraceHandler(callback != null ? &OnNativeTraceCallback : null);
    }

    public static unsafe void SetAssertFailureHandler(AssertFailedDelegate callback)
    {
        s_assertCallback = callback;

        JPH_SetAssertFailureHandler(callback != null ? &OnNativeAssertCallback : null);
    }

    public delegate void TraceDelegate(string message);

    public delegate bool AssertFailedDelegate(string expression, string message, string file, uint line);

    [UnmanagedCallersOnly]
    private static unsafe void OnNativeTraceCallback(byte* messagePtr)
    {
        if (s_traceCallback != null)
        {
            string message = ConvertToManaged(messagePtr)!;
            s_traceCallback(message);
        }
    }

    [UnmanagedCallersOnly]
    private static unsafe Bool8 OnNativeAssertCallback(byte* expressionPtr, byte* messagePtr, byte* filePtr, uint line)
    {
        string expression = ConvertToManaged(expressionPtr)!;
        string message = ConvertToManaged(messagePtr)!;
        string file = ConvertToManaged(filePtr)!;

        if (s_assertCallback != null)
        {
            return s_assertCallback(expression, message, file, line);
        }

        return Bool8.True;
    }
}
