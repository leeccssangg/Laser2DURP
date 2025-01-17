using System.Collections;
using System.Collections.Generic;
using TW.Utility.DesignPattern;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [field: SerializeField] public bool IsActive { get; private set; }
    [field: SerializeField] public bool IsTutorialBlock { get; private set; }
    [field: SerializeField] public LayerMask WhatIsDynamicUnit { get; private set; }
    [field: SerializeField] public float MoveThreshold { get; private set; }
    [field: SerializeField] public GameUnit SelectGameUnit { get; set; }
    private Plane Plane { get; set; }
    private Vector3 StartPoint { get; set; }
    private Vector3 EndPoint { get; set; }

    public int Move { get; set; }
    protected override void Awake()
    {
        base.Awake();
        Plane = new Plane(Vector3.up, Vector3.zero);
    }
    public void SetActive(bool value)
    {
        IsActive = value;
        SelectGameUnit = null;
    }
    public void SetTutorialBlock(bool value)
    {
        IsTutorialBlock = value;
    }

    private void Update()
    {
        if (!IsActive) return;
        HandleInput();
    }

    private void HandleInput()
    {
        if (IsTutorialBlock) return;
        if (SelectGameUnit == null && Input.GetMouseButtonDown(0))
        {
            Vector2 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero);
            if (hit.collider != null)
            {
                // Output the name of the object that was clicked/touched
                Debug.Log("Selected object: " + hit.collider.name);
                RoateableObject selectedObject = hit.collider.GetComponent<RoateableObject>();
                if (selectedObject != null)
                {
                    SelectGameUnit = selectedObject;
                    (SelectGameUnit as RoateableObject).Rotate();
                }

                // You can add your logic here to handle the object selection
            }
            //Vector3 screenPoint = Input.mousePosition;
            //Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);
            //worldPoint.z = 0; // Ensure the Z position is 0 for 2D

            //// Check for overlap with any 2D colliders at the world point
            //RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            //SelectGameUnit = hit.collider.GetDynamicUnit();
            //if (hitCollider != null)
            //{
            //    if (SelectGameUnit.IsMoving)
            //    {
            //        SelectGameUnit = null;
            //    }
            //    else
            //    {
            //        SelectGameUnit.OnInteract();
            //        if (Plane.Raycast(ray, out float enter))
            //        {
            //            StartPoint = ray.GetPoint(enter);
            //        }
            //    }
            //}
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out RaycastHit hit, 10000, WhatIsDynamicUnit))
            //{
            //    SelectGameUnit = hit.collider.GetRotateableObject();
            //    if (SelectGameUnit.IsRotate)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        SelectGameUnit.Rotate();
            //    }
            //    Debug.Log("Hit");
            //}
        }
    }
}
