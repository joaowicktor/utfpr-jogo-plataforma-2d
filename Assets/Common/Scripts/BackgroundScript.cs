using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    private List<GameObject> backgroundSlices = new List<GameObject>();

    void Start()
    {
        GetBackgroundSlices();
    }

    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");

        if (IsCameraOutsideNextToBackgroundSlice(xAxis))
        {
            GameObject lastSlice = GetLastBackgroundSlice(xAxis);
            float newSlicePosition = GetBackgroundSlicePosition(lastSlice, xAxis);

            ReplaceBackgroundSlice(newSlicePosition, xAxis);
        }
    }

    private void ReplaceBackgroundSlice(float newPosition, float horizontalAxis)
    {
        bool shouldInsertAtBeginning = horizontalAxis < 0;
        GameObject slice = GetFirstBackgroundSlice(horizontalAxis);
        slice.transform.position = new Vector3(newPosition, slice.transform.position.y, slice.transform.position.z);

        backgroundSlices.Remove(slice);
        if (shouldInsertAtBeginning)
            backgroundSlices.Insert(0, slice);
        else
            backgroundSlices.Add(slice);
    }

    private void GetBackgroundSlices()
    {
        for (int i = 1; i <= 7; i++)
        {
            backgroundSlices.Add(transform.Find(i.ToString()).gameObject);
        }
    }

    private GameObject GetFirstBackgroundSlice(float horizontalAxis)
    {
        int position = horizontalAxis < 0 ? backgroundSlices.Count - 1 : 0;
        return backgroundSlices[position];
    }

    private GameObject GetLastBackgroundSlice(float horizontalAxis)
    {
        int position = horizontalAxis < 0 ? 0 : backgroundSlices.Count - 1;
        return backgroundSlices[position];
    }

    private float GetBackgroundSliceBound(GameObject slice, float horizontalAxis)
    {
        bool shouldGetLeftBound = horizontalAxis < 0;

        if (shouldGetLeftBound)
            return slice.transform.position.x - slice.GetComponent<SpriteRenderer>().bounds.size.x / 2;
        else
            return slice.transform.position.x + slice.GetComponent<SpriteRenderer>().bounds.size.x / 2;
    }

    private float GetBackgroundSlicePosition(GameObject slice, float horizontalAxis)
    {
        bool shouldCalculateLeftPosition = horizontalAxis < 0;
        float sliceWidth = slice.GetComponent<SpriteRenderer>().bounds.size.x;

        if (shouldCalculateLeftPosition)
            return slice.transform.position.x - sliceWidth;
        else
            return slice.transform.position.x + sliceWidth;
    }

    private bool IsCameraOutsideNextToBackgroundSlice(float horizontalAxis)
    {
        bool shouldCheckLeftBound = horizontalAxis < 0;

        float cameraBound = GetCameraBound(shouldCheckLeftBound);
        float backgroundSliceBound = GetBackgroundSliceBound(GetLastBackgroundSlice(horizontalAxis), horizontalAxis);

        return shouldCheckLeftBound ? cameraBound < backgroundSliceBound : cameraBound > backgroundSliceBound;
    }

    private float GetCameraBound(bool checkLeftBound)
    {
        if (checkLeftBound)
            return Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect;
        else
            return Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect;
    }
}
