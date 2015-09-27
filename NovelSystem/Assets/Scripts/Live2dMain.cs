using UnityEngine;
using System;
using System.Collections;
using live2d;
using UnityEngine.UI;

public class Live2dMain : MonoBehaviour {
    public TextAsset mocFile;
    public Texture2D[] textures;

    private Live2DModelUnity live2DModel;

    private Camera live2Dcam;

    public RawImage image;

	// Use this for initialization
	void Start () {
        Live2D.init();

        live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);

        live2DModel.setRenderMode(Live2D.L2D_RENDER_DRAW_MESH);

        for (int i = 0; i < textures.Length; i++)
        {
            live2DModel.setTexture(i, textures[i]);
        }
        setCamera();
        setRenderTexture();
	}

    void Update()
    {
        live2DModel.update();
        if (live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH) Render();
        //live2DModel.update();
    }

    void OnRenderObject()
    {
        if (live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW) Render();
        float modelWidth = live2DModel.getCanvasWidth();
        Matrix4x4 m1 = Matrix4x4.Ortho(
            0,
            modelWidth,
            modelWidth,
            0,
            -50.0f, 50.0f);
        Matrix4x4 m2 = transform.localToWorldMatrix;
        Matrix4x4 m3 = m2 * m1;

        live2DModel.setMatrix(m3);
        live2DModel.draw();
    }

    void Render()
    {
        live2DModel.draw();
    }

    void setCamera()
    {
        live2Dcam = gameObject.AddComponent<Camera>();
        live2Dcam.orthographic = true;
        live2Dcam.orthographicSize = 1f;
        live2Dcam.cullingMask = 0;
        live2Dcam.nearClipPlane = 0;
    }

    void setRenderTexture()
    {
        var w = live2DModel.getCanvasWidth();
        var h = live2DModel.getCanvasHeight();
        var target = new RenderTexture((int)w, (int)h, (int)Screen.dpi, RenderTextureFormat.ARGB32);
        live2Dcam.targetTexture = target;

        image.rectTransform.sizeDelta = new Vector2(w / 4f, h / 4f);
    }
}
