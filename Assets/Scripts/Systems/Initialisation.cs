using System;
using System.Collections.Generic;
using UnityEngine;

public class Initialisation : MonoBehaviour
{
    public HUDController hudController;
    // Start is called before the first frame update
    void Start()
    {
        hudController = FindObjectOfType<HUDController>();

        hudController.readables = new List<Readable>();
        hudController.readables.Add(new Readable()
        {
            id = 0,
            text = "Specimin 35L has responded positively to splicing the microgenome with that of substance of the carbon neutralizer." + Environment.NewLine +
            "Specimin 35L - IR Test 11 - tank levels of CO2 are non-detectable."
        }); ;
        hudController.readables.Add(new Readable()
        {
            id = 1,
            text = "Specimin 35L has conjoined with the carbon neutralizer in unexpected ways."
        });
        hudController.readables.Add(new Readable()
        {
            id = 2,
            text = "I've been repeatedly getting headaches lately but only whilst in the lab, probably due to stress the deadlines on this aren't exactly movable. Nearly done then I can kick back."
        });
        hudController.readables.Add(new Readable()
        {
            id = 3,
            text = "It's 35L, it's registered us as carbon carrying entities and is trying to refine us."
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
