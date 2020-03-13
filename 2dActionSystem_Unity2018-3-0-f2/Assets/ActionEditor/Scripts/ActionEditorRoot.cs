using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionGame.ActionEditor
{
    /*
     * 编辑器启动类型，进行编辑器初始化工作
     */
    public class ActionEditorRoot : MonoBehaviour
    {

        private void Awake()
        {
        
        }

        private void Start()
        {
            ActionEditorUIMgr.Instance.OpenPanel("uihome", EditorUILayer.Bottom);
        }

        private void OnDestroy()
        {
        
        }
    }
}

