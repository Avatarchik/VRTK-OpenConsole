using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

[RequireComponent(typeof(VRTK.VRTK_ControllerEvents))]
public class VRTKOC_ControllerLocationTeleporter : MonoBehaviour {

    //[SerializeField]
    //VRTK_PolicyList TeleportLocations;
    public string Tag = "TeleportLocation";
    static List<Cubemap> m_cubeMaps = new List<Cubemap>();
    static bool active;
    static GameObject LocationHolder;
    static bool _generated = false;
	// Use this for initialization
	void Start () {
        if (!_generated)
        {
            GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(ButtonOnePressed);
            GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(ButtonOneReleased);
            GenerateTeleportPreviews();
            LocationHolder.SetActive(false);
            _generated = true;
        }
    }

    private void GenerateTeleportPreviews()
    {
        GameObject tempCam = new GameObject("CubemapCamera");
        tempCam.AddComponent<Camera>();

        //GameObject[] locations = new GameObject[0];
        /*if(TeleportLocations.checkType == VRTK_PolicyList.CheckTypes.Layer)
        {

        }*/
        GameObject[] locations = GameObject.FindGameObjectsWithTag(Tag);
        LocationHolder = new GameObject("LocationHolder");
        LocationHolder.transform.position = Vector3.zero;

        GameObject particlesPrefab = Resources.Load("VRTKOC_TeleportParticles", typeof(GameObject)) as GameObject;
        GameObject textPrefab = Resources.Load("VRTKOC_TeleportText", typeof(GameObject)) as GameObject;

        for (int i=0; i<locations.Length;i++)
        {
            //GenerateSphere
            GameObject locationSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            locationSphere.transform.parent = LocationHolder.transform;
            locationSphere.transform.localPosition = GetCirclePosition(i, locations.Length, 0.5f);
            AddInteraction(locationSphere, locations[i].transform);
            locationSphere.transform.localScale = Vector3.one * 0.25f;
            Material mat = new Material(Shader.Find("Custom/Cubemap"));
            locationSphere.GetComponent<Renderer>().material = mat;

            //GeneratePreview
            Cubemap cubeMap = new Cubemap(128, TextureFormat.RGBA32, false);
            tempCam.transform.position = locations[i].transform.position + Vector3.up;
            tempCam.transform.rotation = Quaternion.identity;
            tempCam.GetComponent<Camera>().RenderToCubemap(cubeMap);
            mat.SetTexture("_node_3243", cubeMap);
            m_cubeMaps.Add(cubeMap);
            if(particlesPrefab!=null)
                GameObject.Instantiate(particlesPrefab, locationSphere.transform,false);
            if (textPrefab != null)
            {
                GameObject.Instantiate(textPrefab, locationSphere.transform, false).GetComponentInChildren<Text>().text = locations[i].name;
            }

        }
        // destroy temporary camera
        DestroyImmediate(tempCam);
    }

    private void AddInteraction(GameObject locationSphere, Transform Location)
    {
        locationSphere.AddComponent<VRTKOC_LocationTeleporter>().Location = Location;
    }

    private Vector3 GetCirclePosition(int i, int length, float r)
    {
        float angle = 360 * (i * 1.0f) / length; //Extra multiplication for float conversion
                                                 //var angle = i * Mathf.PI * 2;
       float x = r* Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = r * Mathf.Cos(angle * Mathf.Deg2Rad);
        return new Vector3(x, 0, y);
    }

    private void ButtonOneReleased(object sender, ControllerInteractionEventArgs e)
    {
        
    }

    private void ButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        active = !active;
        LocationHolder.SetActive(active);
        LocationHolder.transform.position = VRTK.VRTK_DeviceFinder.HeadsetTransform().transform.position;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
