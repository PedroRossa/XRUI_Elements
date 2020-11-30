using UnityEngine;

namespace Vizlab.Package.Support
{
    /// <summary>
    /// Central utilities class
    /// </summary>
    public class Helper : MonoBehaviour
    {
        /// <summary>
        /// Add Line Renderer On Object
        /// </summary>
        /// <param name="go">Gameobject to add line renderer </param>
        /// <param name="startWidth">initial line witdth</param>
        /// <param name="endWidth">Final line width</param>
        /// <param name="startColor">Initial line color </param>
        /// <param name="endColor">Final line color </param>
        /// <returns></returns>
        public static LineRenderer CreateLineRendererOnObject(GameObject go, float startWidth, float endWidth, Color startColor, Color endColor)
        {
            LineRenderer lineRenderer = go.AddComponent<LineRenderer>();

            lineRenderer.useWorldSpace = true;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);

            return lineRenderer;
        }
    }
}
