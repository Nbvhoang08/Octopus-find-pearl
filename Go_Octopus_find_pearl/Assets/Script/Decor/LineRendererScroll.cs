using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererScroll : MonoBehaviour
{
    public LineRenderer lineRenderer; // Gắn LineRenderer vào đây trong Inspector
    public float scrollSpeed = 1f;   // Tốc độ giảm giá trị z (_MainTex_ST.z)
    [SerializeField] private Material lineMaterial;
    private float offsetZ;

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        if (lineRenderer != null)
        {
            lineMaterial = lineRenderer.material;

            if (lineMaterial.HasProperty("_MainTex_ST"))
            {
                // Lấy giá trị z hiện tại
                offsetZ = lineMaterial.GetVector("_MainTex_ST").z;
            }
            else
            {
                Debug.LogError("Material không có thuộc tính _MainTex_ST");
            }
        }
    }

    void Update()
    {
        if (lineMaterial != null && lineMaterial.HasProperty("_MainTex_ST"))
        {
            // Giảm giá trị z theo thời gian
            offsetZ -= scrollSpeed * Time.deltaTime;

            // Cập nhật giá trị _MainTex_ST.z
            Vector4 textureST = lineMaterial.GetVector("_MainTex_ST");
            textureST.z = offsetZ;
            lineMaterial.SetVector("_MainTex_ST", textureST);
        }
    }
}
