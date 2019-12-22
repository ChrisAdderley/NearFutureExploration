﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP.Localization;


namespace VisualDebugUtils
{

  public class DebugRenderer
  {
    protected Transform xFormHost;
    protected string debugShader = "GUI/Text Shader";
    protected int debugQueue = 4000;
    protected int debugLayer = 0;

    public Transform XForm
    {
      get { return xFormHost; }
    }
    public void SetVisibility(bool on)
    {
      xFormHost.gameObject.SetActive(on);
    }
    public virtual void SetColor(Color c)
    {
      
    }
    public DebugRenderer() { }
  }

  public class DebugAxisTripod : DebugRenderer
  {
    protected LineRenderer zAxis;
    protected LineRenderer yAxis;
    protected LineRenderer xAxis;

    public DebugAxisTripod(float size)
    {
      GameObject hostGO = new GameObject("Debug_AxisTripod");
      xFormHost = hostGO.transform;
      zAxis = CreateBasicRenderer("ZAxis", xFormHost, new Vector3[] { Vector3.zero, Vector3.forward * size }, Color.blue);
      yAxis = CreateBasicRenderer("YAxis", xFormHost, new Vector3[] { Vector3.zero, Vector3.up * size }, Color.green);
      xAxis = CreateBasicRenderer("XAxis", xFormHost, new Vector3[] { Vector3.zero, Vector3.left * size }, Color.red);
    }
    public void AssignTransform(Transform t)
    {
      xFormHost.SetParent(t, false);
      xFormHost.localPosition = Vector3.zero;
      xFormHost.localRotation = Quaternion.identity;
    }
    protected LineRenderer CreateBasicRenderer(string lineName, Transform parent, Vector3[] points, Color color)
    {
      GameObject child = new GameObject(lineName);
      child.transform.SetParent(parent, false);
      child.transform.localRotation = Quaternion.identity;
      child.transform.localPosition = Vector3.zero;

      LineRenderer lr = child.AddComponent<LineRenderer>();
      // Set up the material
      lr.useWorldSpace = false;
      lr.material = new Material(Shader.Find(debugShader));
      lr.material.color = color;
      lr.material.renderQueue = debugQueue;
      lr.gameObject.layer = debugLayer;
      lr.SetVertexCount(2);
      lr.SetWidth(0.05f, 0.05f);
      lr.SetPosition(0, points[0]);
      lr.SetPosition(1, points[1]);

      return lr;
    }

    public override void SetColor(Color c)
    {
      
    }
  }

  public class DebugPoint : DebugRenderer
  {
    public DebugPoint(float size, Color color)
    {
      GameObject hostGO = new GameObject("Debug_Dot");
      xFormHost = hostGO.transform;
      CreateBasicRenderer("Point", xFormHost, size, color);
    }

    protected void CreateBasicRenderer(string objName, Transform parent, float size, Color color)
    {
      GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);

      GameObject.Destroy(go.GetComponent<Collider>());
      go.transform.parent = parent;
      go.transform.localPosition = Vector3.zero;
      go.transform.localScale = Vector3.one * size;

      MeshRenderer m = go.GetComponent<MeshRenderer>();
      m.material = new Material(Shader.Find(debugShader));
      m.material.color = color;
      m.material.renderQueue = debugQueue;
    }
    public override void SetColor(Color c)
    {

    }
  }


  public class DebugLine : DebugRenderer
  {

    protected LineRenderer axis;

    public DebugLine(float size, Color color)
    {
      GameObject hostGO = new GameObject("Debug_Dot");
      xFormHost = hostGO.transform;
      axis = CreateBasicRenderer("Axis", xFormHost, new Vector3[] { Vector3.zero, Vector3.forward * size }, color);
    }

    public void AdjustSize(float size)
    {
      axis.SetPosition(0, Vector3.zero);
      axis.SetPosition(1, Vector3.forward * size);
    }

    protected LineRenderer CreateBasicRenderer(string lineName, Transform parent, Vector3[] points, Color color)
    {
      GameObject child = new GameObject(lineName);
      child.transform.SetParent(parent, false);
      child.transform.localRotation = Quaternion.identity;
      child.transform.localPosition = Vector3.zero;

      LineRenderer lr = child.AddComponent<LineRenderer>();
      // Set up the material
      lr.material = new Material(Shader.Find(debugShader));
      lr.startWidth = .02f;
      lr.endWidth = .02f;
      lr.material.color = Color.white;
      lr.material.renderQueue = debugQueue;
      lr.gameObject.layer = debugLayer;
      lr.positionCount = 2;
      lr.useWorldSpace = false;
      lr.SetPosition(0, points[0]);
      lr.SetPosition(1, points[1]);

      return lr;
    }

    public override void SetColor(Color c)
    {
      axis.endColor = c;
      axis.startColor = c;
    }
  }
}
