using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPathMarker : MonoBehaviour {

    [SerializeField]
    Color mMarkerColor = Color.white;
    [SerializeField]
    float mIntervalLength = 1.0f;
    private List<GameObject> mMarkers = null;

    void Start()
    {
        mMarkers = new List<GameObject>();
    }

    

    public void SetPath( Vector3[] path )
    {
        ClearPath();
        if (path.Length < 2)
            return;

        for (int i = 0; i < path.Length - 1; ++i)
        {
            CreateLine(path[i], path[i + 1]);
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        Vector3 dir = (end - start);
        float distance = dir.magnitude;
        dir.Normalize();
        int intervals = Mathf.RoundToInt((distance / mIntervalLength));
        Vector3 currentPosition = start;
        for (int i = 0; i < intervals; ++i)
        {
            GameObject marker = PoolManager.Instance.GetPoolElement(PoolManager.PoolType.MARKER);
            marker.SetActive(true);
            marker.GetComponentInChildren<Renderer>().material.color = mMarkerColor;
            marker.transform.position = currentPosition;
            currentPosition += mIntervalLength * dir;
            mMarkers.Add(marker);
        }
    }

    public void ClearPath()
    {
        foreach (GameObject g in mMarkers)
        {
            g.GetComponentInChildren<Renderer>().material.color = Color.white;
            PoolManager.Instance.ReturnPoolElement(PoolManager.PoolType.MARKER, g);
        }
        mMarkers.Clear();
    }
}
