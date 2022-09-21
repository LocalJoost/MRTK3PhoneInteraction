using System;
using TMPro;
using UnityEngine;

namespace MRTKExtensions.Utilities
{
    public static class TextUtilities
    {
        /// <summary>
        /// Wraps text in a mesh to a certain width.
        /// Nicked and adapted from https://answers.unity.com/questions/223906/textmesh-wordwrap.html
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="maxLength"></param>
        public static void WordWrap(this TextMesh mesh, float maxLength)
        {
            var oldQ = mesh.gameObject.transform.rotation;
            mesh.gameObject.transform.rotation = Quaternion.identity;
            var renderer = mesh.GetComponent<Renderer>();
            string[] parts = mesh.text.Split(' ');
            mesh.text = "";
            for (int i = 0; i < parts.Length; i++)
            {
                var tmp = mesh.text;

                mesh.text += parts[i] + " ";
                if (renderer.bounds.size.x > maxLength)
                {
                    tmp += Environment.NewLine;
                    tmp += parts[i] + " ";
                    mesh.text = tmp;
                }
            }

            mesh.text = mesh.text.TrimEnd();
            mesh.gameObject.transform.rotation = oldQ;
        }

        public static void WordWrap(this TextMeshPro mesh, float maxLength)
        {
            var oldQ = mesh.gameObject.transform.rotation;
            mesh.gameObject.transform.rotation = Quaternion.identity;
            var renderer = mesh.GetComponent<Renderer>();
            string[] parts = mesh.text.Split(' ');
            mesh.text = "";
            for (int i = 0; i < parts.Length; i++)
            {
                var tmp = mesh.text;

                mesh.text += parts[i] + " ";
                if (renderer.bounds.size.x > maxLength)
                {
                    tmp += Environment.NewLine;
                    tmp += parts[i] + " ";
                    mesh.text = tmp;
                }
            }

            mesh.text = mesh.text.TrimEnd();
            mesh.gameObject.transform.rotation = oldQ;
        }
    }
}
