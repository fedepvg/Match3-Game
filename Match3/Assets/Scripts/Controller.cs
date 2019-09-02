using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Model GridModel;
    public View GridView;
    
    void Start()
    {
        GridModel = new Model();
        GridModel.CreateGrid(7, 10);
        GridModel.SetGridPositions();
    }
    
    void Update()
    {
        
    }
}
