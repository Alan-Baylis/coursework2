using UnityEngine;
using System;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {
    [Serializable]
    public class CameraStringEntry
    {
        public string Name { get { return camera.name; } }
        public Camera camera;

        /*public static bool operator ==(CameraStringEntry a, CameraStringEntry b)
        {
            return a.Name == b.Name;
        }
        public static bool operator !=(CameraStringEntry a, CameraStringEntry b)
        {
            return !(a == b);
        }*/
    }

    public List<CameraStringEntry> cameras = new List<CameraStringEntry>();

    public CameraStringEntry currentCamera;

    void Awake()
    {
        if (cameras.Count == 0)
        {
            var cameras_cam = Resources.FindObjectsOfTypeAll(typeof(Camera)) as Camera[];

            foreach (var i in cameras_cam)
            {
                cameras.Add(new CameraStringEntry() { camera = i });
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
        cameras.ForEach(x => { if (x.Name != currentCamera.Name) { x.camera.gameObject.SetActive(false); Debug.LogFormat("deactivating {0}", x.camera); } });
    }
}
