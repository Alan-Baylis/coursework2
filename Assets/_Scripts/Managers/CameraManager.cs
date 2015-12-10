using UnityEngine;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class CameraManager : MonoBehaviour {
    [Serializable]
    public class CameraStringEntry
    {
        public string Name { get { return camera.name; } }
        public Camera camera;
    }

    public static CameraManager Instance { get; protected set; }

    protected CameraManager()
    {
    }

    public List<CameraStringEntry> cameras = new List<CameraStringEntry>();

    public CameraStringEntry currentCamera;

    [UsedImplicitly]
    void Awake()
    {
        Instance = this;
        if (cameras.Count == 0)
        {
            var allCameras = Resources.FindObjectsOfTypeAll(typeof(Camera)) as Camera[];

            if (allCameras == null) return;
            foreach (var i in allCameras)
            {
                cameras.Add(new CameraStringEntry { camera = i });
            }
        }
        else if(currentCamera != null)
        {
            SwitchCamera(currentCamera);
        }
    }

    public void SwitchCamera(CameraStringEntry newCurrentCamera)
    {
        currentCamera = newCurrentCamera;
        currentCamera.camera.gameObject.SetActive(true);
        cameras.ForEach(x =>
        {
            if (x.Name == currentCamera.Name) return;
            x.camera.gameObject.SetActive(false);
            Debug.LogFormat("deactivating {0}", x.camera);
        });
    }

    public void SwitchCamera()
    {
        SwitchCamera(cameras[cameras.Count.NextInRangeOrFirst(0, cameras.Count)]);
    }
}
