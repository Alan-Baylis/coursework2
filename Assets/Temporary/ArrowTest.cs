using UnityEngine;
using System.Collections;

public class ArrowTest : MonoBehaviour {
    public Arrow arrow;
    private Texture2D texture;
	// Use this for initialization
	void Start () {
	    arrow = new Arrow(0, new Vector2(200, 200), 100);
        texture = CreateLineTexture(100, 100);
        StartCoroutine(IncreaseAngle());
        texture.
	}

    static Texture2D CreateLineTexture(int width, int height)
    {
        var texture = new Texture2D(width, height);
        for (var i = 0; i < texture.width; ++i)
        {
            for (var j = 0; j < texture.height; ++j)
            {
                var logi = i == j || i - 1 == j || j - 1 == i;
                texture.SetPixel(i, j, logi ? Color.black : Color.clear);
            }
        }
        return texture;
    }
	
	// Update is called once per frame
	void Update () {
        arrow.position = Input.mousePosition;
	}

    IEnumerator IncreaseAngle()
    {
        for(var i = 0; i < 180; ++i) {
            arrow.angle++;
            Debug.Log(arrow.angle + " " + arrow.GetArrow());
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OnGUI()
    {
        GUI.Label(arrow.GetArrow(), new GUIContent(texture));
    }
}
