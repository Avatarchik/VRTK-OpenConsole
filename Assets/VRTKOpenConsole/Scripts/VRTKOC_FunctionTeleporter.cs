using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class VRTKOC_FunctionTeleporter : VRTKOC_Function {

    //[SerializeField]
    //VRTK_PolicyList TeleportLocations;
    public string Tag = "TeleportLocation";
    List<Cubemap> m_cubeMaps = new List<Cubemap>();
    private List<Transform> m_worldLocations = new List<Transform>();

    public override string FunctionName
    {
        get
        {
            return "Teleporter";
        }
    }

   

    protected override void GenerateSpheres()
    {
        GameObject tempCam = new GameObject("CubemapCamera");
        tempCam.AddComponent<Camera>();

        GameObject[] locations = GameObject.FindGameObjectsWithTag(Tag);
        GameObject particlesPrefab = Resources.Load("VRTKOC_TeleportParticles", typeof(GameObject)) as GameObject;
        GameObject textPrefab = Resources.Load("VRTKOC_TeleportText", typeof(GameObject)) as GameObject;

        for (int i = 0; i < locations.Length; i++)
        {
            m_worldLocations.Add(locations[i].transform);
            //GeneratePreview
            Material mat = new Material(Shader.Find("Custom/Cubemap"));
            Cubemap cubeMap = new Cubemap(128, TextureFormat.RGBA32, false);
            tempCam.transform.position = locations[i].transform.position + Vector3.up;
            tempCam.transform.rotation = Quaternion.identity;
            tempCam.GetComponent<Camera>().RenderToCubemap(cubeMap);
            mat.SetTexture("_node_3243", cubeMap);
            m_cubeMaps.Add(cubeMap);
            //Generate sphere
            GameObject functionSphere = GenerateDefaultSphere(i, locations[i].name, mat);
            if (particlesPrefab != null)
                GameObject.Instantiate(particlesPrefab, functionSphere.transform, false);
        }
        // destroy temporary camera
        DestroyImmediate(tempCam);
    }

    protected override void OnIndexUsed(int i)
    {
        VRTK.VRTK_DeviceFinder.PlayAreaTransform().transform.position = m_worldLocations[i].position;
        //VRTK.VRTK_DeviceFinder.PlayAreaTransform().transform.eulerAngles = m_spheres[i].eulerAngles;
        Debug.Log("[VRTKOC_LocationTeleporter] Teleporting player");
    }
}
