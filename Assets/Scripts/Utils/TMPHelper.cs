using TMPro;
using UnityEngine;

namespace Utils
{
    public static class TMPHelper
    {

        /**
         * <summary>
         * Creates a new GameObject with a TextMeshProUGUI Component, places it inside the Canvas and applies
         * the options passed as parameter.
         *
         * Returns a struct containing both the GameObject and the TextMeshProUGUI instance.
         * </summary>
         */
        public static TMPBundle CreateTextObject(string name, TextOptions options)
        {
            var gameObject = new GameObject(name)
            {
                transform =
                {
                    localPosition = options.Position,
                    localScale = Vector3.one
                }
            };
            gameObject.transform.SetParent(GlobalCanvas.CanvasGameObject.transform, false);
            var tmpText = gameObject.AddComponent<TextMeshProUGUI>();
            gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10000f);
            tmpText.fontSize = options.FontSize;
            tmpText.alignment = options.Alignment;
            tmpText.fontStyle = options.FontStyle;
            tmpText.text = options.Text;
            return new TMPBundle()
            {
                GameObject = gameObject,
                TMPText = tmpText
            };
        }
        
    }

    public struct TMPBundle
    {
        public GameObject GameObject;
        public TextMeshProUGUI TMPText;
    }

    public class TextOptions
    {
        public Vector3 Position = Vector3.zero;
        public float FontSize = 20;
        public TextAlignmentOptions Alignment = TextAlignmentOptions.Center;
        public FontStyles FontStyle = FontStyles.Normal;
        public string Text = "";
    }
}