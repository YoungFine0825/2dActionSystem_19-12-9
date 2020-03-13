using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace ActionGame.ActionEditor
{
    public enum EditorUILayer {
        None = 0,
        Bottom = 1,
        Normal = 2,
        Pop = 3,
        Message = 4
    }

    public class ActionEditorUIMgr : MonoBehaviour
    {
        public static ActionEditorUIMgr Instance
        {
            get
            {
                return _instance;
            }
        }

        private static ActionEditorUIMgr _instance;

        public GameObject uiCanvasGo;

        public string uiFolder = "Assets/ActionEditor/Resources/prefabs/ui/";

        private Dictionary<string, GameObject> _uiPanels = new Dictionary<string, GameObject>();

        private void Awake()
        {
            _instance = this;
        }

        void Start()
        {
        
        }

        public bool OpenPanel(string panelName, EditorUILayer layerType)
        {
            bool ret = false;
            StringBuilder sb = new StringBuilder();
            sb.Append(uiFolder);
            sb.Append(panelName);
            sb.Append(".prefab");
            string uiPath = sb.ToString();
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(uiPath);
            GameObject uiGo = GameObject.Instantiate(prefab);
            if (uiGo != null && uiCanvasGo != null)
            {
                string layerGoName = this._UILayerEnum2Name(layerType);
                if (layerGoName != string.Empty)
                {
                    Transform parent = uiCanvasGo.transform.Find(layerGoName);
                    if (parent != null)
                    {
                        RectTransform rectTrans = uiGo.GetComponent<RectTransform>();
                        rectTrans.SetParent(parent);
                        rectTrans.localPosition = Vector2.zero;
                        rectTrans.localRotation = Quaternion.identity;
                        rectTrans.localScale = Vector2.one;
                        ret = true;
                    }
                }
            }
            return ret;
        }


        private string _UILayerEnum2Name(EditorUILayer layerType)
        {
            string ret = string.Empty;
            if (layerType == EditorUILayer.Bottom)
            {
                ret = "Layer_Bottom";
            }
            else if (layerType == EditorUILayer.Normal)
            {
                ret = "Layer_Normal";
            }
            else if (layerType == EditorUILayer.Pop)
            {
                ret = "Layer_Pop";
            }
            else if (layerType == EditorUILayer.Message)
            {
                ret = "Layer_Message";
            }
            return ret;
        }

    }
}

