using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoints : MonoBehaviour
{
    //private Vector3[,] positions = new Vector3[17, 9];

    [SerializeField] public Transform[] corners = new Transform[4];
    //[SerializeField] private GameObject bomb;

    private void Awake()
    {
        /*float dz = 0f;
        float dx = 0f;
        dx = (corners[1].position.x - corners[0].position.x) / 17;
        dz = (corners[0].position.z - corners[2].position.z) / 9;
        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                positions[i, j] = new Vector3(corners[0].position.x -dx + (dx * (i+1)), 0f, corners[0].position.z - (dz * (j+1)));
            }
        }

        for (int i = 0; i < 17; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                Instantiate(bomb, positions[i, j], Quaternion.Euler(new Vector3(45, 0, 0)));
            }
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}