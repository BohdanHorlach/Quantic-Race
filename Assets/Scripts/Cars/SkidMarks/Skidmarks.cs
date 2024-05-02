﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

// Skidmarks controller. Put one of these in a scene somewhere. Call AddSkidMark.
// Copyright 2017 Nition, BSD licence (see LICENCE file). http://nition.co

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Skidmarks : MonoBehaviour
{

    private static Skidmarks instance;
    public static Skidmarks Instance
    {
        get
        {
            if (instance == null)
                instance = new Skidmarks();
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    //private Skidmarks() { }

    [SerializeField] private MeshRenderer mr;
    [SerializeField] private MeshFilter mf;

    private const int MAX_MARKS = 2048; // Max number of marks total for everyone together
    private const float MARK_WIDTH = 0.35f; // Width of the skidmarks. Should match the width of the wheels
    private const float GROUND_OFFSET = 0.02f;  // Distance above surface in metres
    private const float MIN_DISTANCE = 0.25f; // Distance between skid texture sections in metres. Bigger = better performance, less smooth
    private const float MIN_SQR_DISTANCE = MIN_DISTANCE * MIN_DISTANCE;
    private const float MAX_OPACITY = 1.0f; // Max skidmark opacity


    private class MarkSection
    {
        public Vector3 Pos = Vector3.zero;
        public Vector3 Normal = Vector3.zero;
        public Vector4 Tangent = Vector4.zero;
        public Vector3 Posl = Vector3.zero;
        public Vector3 Posr = Vector3.zero;
        public Color32 Colour;
        public int LastIndex;
    };

    private int markIndex;
    private MarkSection[] skidmarks;
    private Mesh marksMesh;

    private Vector3[] vertices;
    private Vector3[] normals;
    private Vector4[] tangents;
    private Color32[] colors;
    private Vector2[] uvs;
    private int[] triangles;

    private bool meshUpdated;
    private bool haveSetBounds;

    private Color32 black = Color.black;


    protected void Awake()
    {
        instance = this;
        if (transform.position != Vector3.zero)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }
    }

    protected void Start()
    {
        // Generate a fixed array of skidmarks
        skidmarks = new MarkSection[MAX_MARKS];
        for (int i = 0; i < MAX_MARKS; i++)
        {
            skidmarks[i] = new MarkSection();
        }

        if (mr == null)
        {
            mr = gameObject.AddComponent<MeshRenderer>();
        }
        if (mf == null)
        {
            mf = gameObject.AddComponent<MeshFilter>();
        }

        marksMesh = new Mesh();
        marksMesh.MarkDynamic();
        mf.sharedMesh = marksMesh;

        vertices = new Vector3[MAX_MARKS * 4];
        normals = new Vector3[MAX_MARKS * 4];
        tangents = new Vector4[MAX_MARKS * 4];
        colors = new Color32[MAX_MARKS * 4];
        uvs = new Vector2[MAX_MARKS * 4];
        triangles = new int[MAX_MARKS * 6];

        mr.shadowCastingMode = ShadowCastingMode.Off;
        mr.receiveShadows = false;
        mr.lightProbeUsage = LightProbeUsage.Off;
    }

    protected void LateUpdate()
    {
        if (!meshUpdated) return;
        meshUpdated = false;

        // Reassign the mesh if it's changed this frame
        marksMesh.vertices = vertices;
        marksMesh.normals = normals;
        marksMesh.tangents = tangents;
        marksMesh.triangles = triangles;
        marksMesh.colors32 = colors;
        marksMesh.uv = uvs;

        if (!haveSetBounds)
        {
            // Could use RecalculateBounds here each frame instead, but it uses about 0.1-0.2ms each time
            // Save time by just making the mesh bounds huge, so the skidmarks will always draw
            // Not sure why I only need to do this once, yet can't do it in Start (it resets to zero)
            marksMesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(10000, 10000, 10000));
            haveSetBounds = true;
        }

        mf.sharedMesh = marksMesh;
    }


    // Function called by the wheel that's skidding. Sets the intensity of the skidmark section, in the default black.
    public int AddSkidMark(Vector3 pos, Vector3 normal, float opacity, int lastIndex)
    {
        if (opacity > 1) opacity = 1.0f;
        else if (opacity < 0) return -1;

        black.a = (byte)(opacity * 255);
        return AddSkidMark(pos, normal, black, lastIndex);
    }


    // Function called by the wheel that's skidding. Sets the colour and intensity of the skidmark section.
    public int AddSkidMark(Vector3 pos, Vector3 normal, Color32 colour, int lastIndex)
    {
        if (colour.a == 0) return -1; // No point in continuing if it's invisible	

        MarkSection lastSection = null;
        Vector3 distAndDirection = Vector3.zero;
        Vector3 newPos = pos + normal * GROUND_OFFSET;
        if (lastIndex != -1)
        {
            lastSection = skidmarks[lastIndex];
            distAndDirection = newPos - lastSection.Pos;
            if (distAndDirection.sqrMagnitude < MIN_SQR_DISTANCE)
            {
                return lastIndex;
            }
            // Fixes an awkward bug:
            // - Car draws skidmark, e.g. index 50 with last index 40.
            // - Skidmark markIndex loops around, and other car overwrites index 50
            // - Car draws skidmark, e.g. index 45. Last index was 40, but now 40 is different, changed by someone else.
            // This makes sure we ignore the last index if the distance looks wrong
            if (distAndDirection.sqrMagnitude > MIN_SQR_DISTANCE * 10)
            {
                lastIndex = -1;
                lastSection = null;
            }
        }

        colour.a = (byte)(colour.a * MAX_OPACITY);

        MarkSection curSection = skidmarks[markIndex];

        curSection.Pos = newPos;
        curSection.Normal = normal;
        curSection.Colour = colour;
        curSection.LastIndex = lastIndex;

        if (lastSection != null)
        {
            Vector3 xDirection = Vector3.Cross(distAndDirection, normal).normalized;
            curSection.Posl = curSection.Pos + xDirection * MARK_WIDTH * 0.5f;
            curSection.Posr = curSection.Pos - xDirection * MARK_WIDTH * 0.5f;
            curSection.Tangent = new Vector4(xDirection.x, xDirection.y, xDirection.z, 1);

            if (lastSection.LastIndex == -1)
            {
                lastSection.Tangent = curSection.Tangent;
                lastSection.Posl = curSection.Pos + xDirection * MARK_WIDTH * 0.5f;
                lastSection.Posr = curSection.Pos - xDirection * MARK_WIDTH * 0.5f;
            }
        }

        UpdateSkidmarksMesh();

        int curIndex = markIndex;
        if (lastIndex == -1)
        {
            markIndex = (markIndex - 1 + MAX_MARKS) % MAX_MARKS;
        }
        else
        {
            markIndex = ++markIndex % MAX_MARKS;
        }
        return curIndex;
    }


    // Update part of the mesh for the current markIndex
    private void UpdateSkidmarksMesh()
    {
        MarkSection curr = skidmarks[markIndex];

        // Nothing to connect to yet
        if (curr.LastIndex == -1) return;

        MarkSection last = skidmarks[curr.LastIndex];
        vertices[markIndex * 4 + 0] = last.Posl;
        vertices[markIndex * 4 + 1] = last.Posr;
        vertices[markIndex * 4 + 2] = curr.Posl;
        vertices[markIndex * 4 + 3] = curr.Posr;

        normals[markIndex * 4 + 0] = last.Normal;
        normals[markIndex * 4 + 1] = last.Normal;
        normals[markIndex * 4 + 2] = curr.Normal;
        normals[markIndex * 4 + 3] = curr.Normal;

        tangents[markIndex * 4 + 0] = last.Tangent;
        tangents[markIndex * 4 + 1] = last.Tangent;
        tangents[markIndex * 4 + 2] = curr.Tangent;
        tangents[markIndex * 4 + 3] = curr.Tangent;

        colors[markIndex * 4 + 0] = last.Colour;
        colors[markIndex * 4 + 1] = last.Colour;
        colors[markIndex * 4 + 2] = curr.Colour;
        colors[markIndex * 4 + 3] = curr.Colour;

        uvs[markIndex * 4 + 0] = new Vector2(0, 0);
        uvs[markIndex * 4 + 1] = new Vector2(1, 0);
        uvs[markIndex * 4 + 2] = new Vector2(0, 1);
        uvs[markIndex * 4 + 3] = new Vector2(1, 1);

        triangles[markIndex * 6 + 0] = markIndex * 4 + 0;
        triangles[markIndex * 6 + 2] = markIndex * 4 + 1;
        triangles[markIndex * 6 + 1] = markIndex * 4 + 2;

        triangles[markIndex * 6 + 3] = markIndex * 4 + 2;
        triangles[markIndex * 6 + 5] = markIndex * 4 + 1;
        triangles[markIndex * 6 + 4] = markIndex * 4 + 3;

        meshUpdated = true;
    }


    //// initialize
    //public IEnumerator Initialize()
    //{
    //    Debug.Log("Skidmarks script initialized");
    //    yield return null;
    //}
}
