﻿using GameDevHQ.Scripts;
using System.Collections;
using System.Collections.Generic;
using GameDevHQ.Scripts.Managers;
using UnityEngine;

public class RangeColour : MonoBehaviour
{
    [SerializeField]
    Material _material;

    [SerializeField]
    GameObject _range;

    bool _isOver = false;
    // Start is called before the first frame update
    void OnEnable()
    {
        TowerPlacementZone.onMouseOver += SetColour;
        TowerManager.onPlaceTower += DisableRangeFX;
        _material.SetColor("_Color", Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _material.SetColor("_Color", Color.yellow);
        }
    }

    public void SetColour ()
    {
        _isOver = !_isOver;
        if (_isOver)
            _material.SetColor("_Color", Color.yellow);
        else
            _material.SetColor("_Color", Color.red);
    }

    public void SetRange (bool isActive)
    {
        _range.SetActive(isActive);
    }

    void DisableRangeFX()
    {
        SetRange(false);
    }

    private void OnDisable()
    {
        TowerPlacementZone.onMouseOver -= SetColour;
        TowerManager.onPlaceTower -= DisableRangeFX;
    }
}
