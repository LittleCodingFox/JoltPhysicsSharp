﻿// Copyright (c) Amer Koleci and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.

using System.Runtime.InteropServices;
using static JoltPhysicsSharp.JoltApi;

namespace JoltPhysicsSharp;

public abstract class ObjectLayerFilter : NativeObject
{
    private readonly JPH_ObjectLayerFilter_Procs _procs;

    public ObjectLayerFilter()
    {
        nint listenerContext = DelegateProxies.CreateUserData(this, true);
        _procs = new JPH_ObjectLayerFilter_Procs
        {
            ShouldCollide = &ShouldCollideCallback,
        };
        Handle = JPH_ObjectLayerFilter_Create(in _procs, listenerContext);
    }


    protected override void DisposeNative()
    {
        JPH_ObjectLayerFilter_Destroy(Handle);
    }

    protected abstract bool ShouldCollide(ObjectLayer layer);

    [UnmanagedCallersOnly]
    private static Bool8 ShouldCollideCallback(IntPtr context, ObjectLayer layer)
    {
        ObjectLayerFilter listener = DelegateProxies.GetUserData<ObjectLayerFilter>(context, out _);
        return listener.ShouldCollide(layer);
    }
}
