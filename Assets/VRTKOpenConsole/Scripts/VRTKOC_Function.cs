using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public abstract class VRTKOC_Function : MonoBehaviour
{

    protected static Transform m_masterParent;
    protected static Transform MasterParent
    {
        get
        {
            if (m_masterParent == null)
            {
                m_masterParent = new GameObject("[VRTKOC]").transform; 
            }
            return m_masterParent;
        }
    }

    public abstract string FunctionName
    {
        get;
    }
    public Material FunctionMat;

    public virtual void OnActivate()
    {
        for (int i=0;i<m_spheres.Count;i++)
        {
            m_spheres[i].gameObject.SetActive(true);
            m_spheres[i].position = getArcWorldPosition(i, m_spheres.Count, 0.5f, 30, 140);
            m_spheres[i].LookAt(VRTK_DeviceFinder.HeadsetCamera());
        }
    }
    public virtual void OnDeactivate()
    {
        foreach (Transform t in m_spheres)
        {
            t.gameObject.SetActive(false);
        }
    }

    private Vector3 GetArcPosition(int i, int length, float r, float minAngle, float maxAngle)
    {
        //                   ^^^ Floor((minAngle*i)/MaxAngle) *MinAngle
        // -maxAngle/2 <----- | -----> maxAngle/2
        float horizontalAngle = 0;
        if (length * minAngle < maxAngle)
        {
            horizontalAngle = ((minAngle * i) -(length/2.0f*minAngle));
        }
        else
        {
            horizontalAngle = (minAngle * i) % maxAngle - maxAngle / 2;
        }
        float verticalAngle = Mathf.Floor((minAngle * i) / maxAngle) * 0.5f;

        float angle = horizontalAngle ; //Extra multiplication for float conversion
                                                 //var angle = i * Mathf.PI * 2;
        float x = r * Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = r * Mathf.Cos(angle * Mathf.Deg2Rad);
        return new Vector3(x, verticalAngle, y);
    }

    private Vector3 getArcWorldPosition(int i, int length, float r, float minAngle, float maxAngle)
    {
        return VRTK_DeviceFinder.HeadsetCamera().TransformPoint(GetArcPosition(i,length,r,minAngle,maxAngle));
    }

    protected List<Transform> m_spheres = new List<Transform>();
    bool _generated = false;

    protected virtual void Start()
    {
        if (!_generated)
        {
            GenerateSpheres();
            _generated = true;
        }
        
    }

    protected abstract void GenerateSpheres();

    protected GameObject GenerateDefaultSphere(int i, string text = "", Material mat = null)
    {
        GameObject locationSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        locationSphere.transform.localScale = Vector3.one * 0.25f;

        //Set material
        if (mat == null)
        {
            mat = new Material(Shader.Find("Standard"));
        }
        locationSphere.GetComponent<Renderer>().material = mat;
        //Set text
        if (text != "")
        {
            GameObject textPrefab = Resources.Load("VRTKOC_TeleportText", typeof(GameObject)) as GameObject;
            GameObject.Instantiate(textPrefab, locationSphere.transform, false).GetComponentInChildren<Text>().text = text;
        }
        VRTK_InteractableObject interact = locationSphere.AddComponent<VRTK_InteractableObject>();
        interact.InteractableObjectUsed += new InteractableObjectEventHandler(OnObjectUsed);
        interact.isUsable = true;
        interact.touchHighlightColor = new Color(0.56f, 1, 1, 0.3f);

        locationSphere.transform.parent = MasterParent;
        locationSphere.SetActive(false);
        m_spheres.Add(locationSphere.transform);
        return locationSphere;
    }

    protected virtual void OnObjectUsed(object sender, InteractableObjectEventArgs e)
    {
        VRTK_InteractableObject interactable = sender as VRTK_InteractableObject;
        for(int i = 0; i < m_spheres.Count; i++)
        {
            if(m_spheres[i].GetInstanceID() == interactable.transform.GetInstanceID())
            {
                OnIndexUsed(i);
            }
        }
    }

    protected abstract void OnIndexUsed(int i);
}
