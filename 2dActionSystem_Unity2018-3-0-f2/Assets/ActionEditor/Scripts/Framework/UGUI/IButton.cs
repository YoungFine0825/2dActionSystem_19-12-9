using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace ActionGame.ActionEditor
{
    [RequireComponent(typeof(IEventTigger))]
    public class IButton : MonoBehaviour
    {
        private bool m_bable = true;
        public float clicktime = 0.5f;
        private float m_lastclicktime = 0.0f;

        // 是否冻结所有的Button对象
        static public bool m_isAllFreezed = false;
        static public int m_exceptID00 = 0;
        static public int m_exceptID01 = 0;
        static public int m_exceptID02 = 0;

        int _selfID = -1;
        Vector3 oldScale;
        public string pressAudio = "";
        public string clickAudio = "anniudianji";
        public bool GrayOnDisable = true;
        public bool PressLight = false;
        [Range(0.0f, 2.0f)]
        public float PressLightValue = 1.2f;
        public bool PressScale = true;
        [Range(0.5f, 1.5f)]
        public float ScaleValue = 0.98f;
        public Transform[] otherNeedScaleTf;
        [HideInInspector]
        public IEventTigger.ClickCallBack clickCallBack;
        [HideInInspector]
        public IEventTigger.PressCallBack pressCallBack;
        [HideInInspector]
        public IEventTigger.EventDataCallBack pointerDownCallBack;
        [HideInInspector]
        public IEventTigger.EventDataCallBack pointerUpCallBack;

        public PointerEventData data;
        public RectTransform rectTransform
        {
            get
            {
                return transform as RectTransform;
            }
        }

        IEventTigger tigger;

        bool _isAppQuit = false;
        public System.Action callOnDestroy = null;

        public Image img = null;

        public Button m_btn = null;
        void Reset()
        {
        }

        public static IButton Get(GameObject obj)
        {
            if (obj == null)
                throw new System.Exception("Create Button Error: GameObject Is Null!");
            IButton btn = obj.GetComponent<IButton>();
            if (btn == null)
            {
                btn = obj.AddComponent<IButton>();
            }
            //默认img 打开射线检测
            btn.img = obj.GetComponent<Image>();
            if (btn.img != null)
            {
                btn.img.raycastTarget = true;
            }
            btn.m_btn = obj.GetComponent<Button>();
            if (btn.m_btn)
            {
                btn.m_btn.interactable = true;
            }
            return btn;
        }
        //按钮点击
        public void SetTouchEnable(bool able)
        {
            if (img != null)
            {
                img.raycastTarget = able;
            }
            if (m_btn)
            {
                m_btn.interactable = able;
            }
            if (tigger != null)
            {
                tigger.enabled = able;
            }
            m_bable = false;
        }

        void Awake()
        {
            oldScale = transform.localScale;
            tigger = IEventTigger.Get(this.gameObject);
            tigger.OnClickCallBack = OnClick;
            tigger.OnPressCallBack = OnPress;
            tigger.OnPointerDownCallBack = OnPointerDownCallBack;
            tigger.OnPointerUpCallBack = OnPointeUpCallBack;
            _selfID = gameObject.GetInstanceID();
        }

        void OnDisable()
        {
            tigger.enabled = false;
        }

        void OnEnable()
        {
            tigger.enabled = m_bable;
        }

        void OnApplicationQuit()
        {
            _isAppQuit = true;
        }

        void OnDestroy()
        {
            if (_isAppQuit)
                return;

            if (callOnDestroy != null)
            {
                callOnDestroy();
            }
            callOnDestroy = null;

            clickCallBack = null;
            pressCallBack = null;
        }


        void PlayAudio()
        {
            if (!string.IsNullOrEmpty(clickAudio))
            {
                //AudioManager.PlayAudio("audio/" + clickAudio + ".audio");
            }
        }

        void OnClick(GameObject obj)
        {
            if (IsAllFreezed())
                return;

            if (clickCallBack != null)
            {
                if (clicktime > 0.1f && Time.unscaledTime - m_lastclicktime < clicktime)
                {
                    return;
                }
                m_lastclicktime = Time.unscaledTime;
                clickCallBack(obj);
            }
        }

        void OnPress(GameObject obj, bool press)
        {
            if (IsAllFreezed())
                return;

            if (pressCallBack != null)
            {
                pressCallBack(this.gameObject, press);
            }

            if (press)
            {
                if (PressScale)
                {
                    transform.localScale = oldScale * ScaleValue;
                    if (otherNeedScaleTf != null && otherNeedScaleTf.Length > 0)
                    {
                        for (int i = 0; i < otherNeedScaleTf.Length; i++)
                        {
                            otherNeedScaleTf[i].localScale = oldScale * ScaleValue;
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(pressAudio))
                //{
                //AudioManager.PlayAudio("audio/ui/" + pressAudio + ".audio");
                //}
            }
            else
            {
                if (PressScale)
                {
                    transform.localScale = oldScale;
                    if (otherNeedScaleTf != null && otherNeedScaleTf.Length > 0)
                    {
                        for (int i = 0; i < otherNeedScaleTf.Length; i++)
                        {
                            otherNeedScaleTf[i].localScale = oldScale;
                        }
                    }
                }
                //if (!string.IsNullOrEmpty(clickAudio))
                //{
                //    AudioManager.PlayAudio("audio/" + clickAudio + ".audio");
                //}
            }
        }

        void OnPointerDownCallBack(PointerEventData data)
        {
            if (IsAllFreezed())
                return;

            PlayAudio();
            if (pointerDownCallBack != null)
                pointerDownCallBack(data);
        }

        void OnPointeUpCallBack(PointerEventData data)
        {
            if (IsAllFreezed())
                return;

            if (pointerUpCallBack != null)
                pointerUpCallBack(data);
        }

        bool IsAllFreezed()
        {
            return m_isAllFreezed && !IsExcept();
        }

        bool IsExcept()
        {
            if ((m_exceptID00 == 0 || m_exceptID00 == -1) && (m_exceptID01 == 0 || m_exceptID01 == -1) && (m_exceptID02 == 0 || m_exceptID02 == -1))
                return false;
            return m_exceptID00 == _selfID || m_exceptID01 == _selfID || m_exceptID02 == _selfID;
        }
    }
}
