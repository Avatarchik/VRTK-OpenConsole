using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTKOC_LocationTeleporter : VRTK.VRTK_InteractableObject {

    public Transform Location;

    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        Teleport();
    }

   
    protected override void Awake()
    {
        base.Awake();
        this.isUsable = true;
        this.touchHighlightColor = new Color(0.56f, 1, 1, 0.3f);
        //this.pointerActivatesUseAction = true;
    }

    private void Teleport()
    {
        VRTK.VRTK_DeviceFinder.PlayAreaTransform().transform.position = Location.position;
        VRTK.VRTK_DeviceFinder.PlayAreaTransform().transform.eulerAngles = Location.eulerAngles;
        Debug.Log("[VRTKOC_LocationTeleporter] Teleporting player");
        transform.parent.gameObject.SetActive(false);
    }
}
