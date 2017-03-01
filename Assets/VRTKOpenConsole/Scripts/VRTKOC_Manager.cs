using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class VRTKOC_Manager : VRTKOC_Function
{



    public VRTK_ControllerEvents LeftControllerEvents;
    public VRTK_ControllerEvents RightControllerEvents;


    private List<VRTKOC_Function> m_registeredFunctions = new List<VRTKOC_Function>();
    private VRTKOC_Function m_activeFunction;
    private bool VRTKOC_Open;

    public override string FunctionName
    {
        get
        {
            return "Main Menu";
        }
    }


    // Use this for initialization
    protected override void Start()
    {
        //VRTK.VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(ButtonOnePressed);
        //VRTK.VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(ButtonOneReleased);
        LeftControllerEvents.ButtonTwoPressed += new ControllerInteractionEventHandler(ButtonOnePressed);
        RightControllerEvents.ButtonTwoPressed += new ControllerInteractionEventHandler(ButtonOnePressed);
        //LeftControllerEvents.
        VRTKOC_Function[] functions = GetComponents<VRTKOC_Function>();
        foreach (VRTKOC_Function function in functions)
        {
            if (function != this)
                RegisterFunction(function);
        }
        base.Start();
    }

    private void ButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("Button one pressed");
        if (m_activeFunction != null)
        {
            m_activeFunction.OnDeactivate();
            OnActivate();
            m_activeFunction = null;
        }
        else
        {
            if (VRTKOC_Open)
            {
                OnDeactivate();
            }
            else
            {
                OnActivate();
            }
            VRTKOC_Open = !VRTKOC_Open;
        }
    }

    private void ButtonOneReleased(object sender, ControllerInteractionEventArgs e)
    {

    }
    public void RegisterFunction(VRTKOC_Function function)
    {
        m_registeredFunctions.Add(function);
    }

    protected override void GenerateSpheres()
    {
        for (int i = 0; i < m_registeredFunctions.Count; i++)
        {
            VRTKOC_Function function = m_registeredFunctions[i];
            GameObject functionSphere = GenerateDefaultSphere(i,function.FunctionName, function.FunctionMat);
            
        }
    }

    protected override void OnIndexUsed(int i)
    {
        m_activeFunction = m_registeredFunctions[i];
        OnDeactivate();
        m_registeredFunctions[i].OnActivate();

    }
}
