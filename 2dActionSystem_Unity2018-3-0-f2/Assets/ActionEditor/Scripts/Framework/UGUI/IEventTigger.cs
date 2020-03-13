using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace ActionGame.ActionEditor
{
    public class IEventTigger : EventTrigger
    {
        public static IEventTigger Get(GameObject obj)
        {
            IEventTigger tigger = obj.GetComponent<IEventTigger>();
            if (tigger == null)
                tigger = obj.AddComponent<IEventTigger>();
            return tigger;
        }

        public delegate void EventDataCallBack(PointerEventData data);
        public delegate void ClickCallBack(GameObject obj);
        public delegate void DlgOnClick(GameObject obj, float px, float py);
        public delegate void PressCallBack(GameObject obj, bool pressed);
        public delegate void DragCallBack(GameObject obj, float posx, float posy);

        public ClickCallBack OnClickCallBack;
        public DlgOnClick onClickCall;
        public PressCallBack OnPressCallBack;
        public DragCallBack OnDragCallBack;

        public EventDataCallBack OnPointerUpCallBack;
        public EventDataCallBack OnPointerDownCallBack;
        public EventDataCallBack OnPointerDragCallBack;
        public EventDataCallBack OnPointerClickCallBack;

        bool pressed = false, isCanClick = false;
        float press_time = 0, diff_time = 0, dis_curr = 0,
        limit_time = 0.2f, limit_dis_min = 0.1f * 0.1f, limit_dis_max = 70f * 70f;

        private ScrollRect scroll = null;
        private Vector2 v2Start;

        void OnDisable()
        {
            if (pressed && OnPressCallBack != null)
                OnPressCallBack(gameObject, false);
            pressed = false;
        }

        void OnEnable()
        {
            pressed = false;
            press_time = 0;
            diff_time = 0;
            v2Start = Vector2.zero;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!enabled)
                return;
            pressed = true;
            press_time = Time.realtimeSinceStartup;
            v2Start = eventData.position;
            scroll = GetRootScroll(this.gameObject);
            if (scroll != null)
            {
                scroll.OnBeginDrag(eventData);
            }
            if (OnPointerDownCallBack != null)
            {
                OnPointerDownCallBack(eventData);
            }
            if (OnPressCallBack != null)
            {
                OnPressCallBack(gameObject, true);
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (!enabled)
                return;
            pressed = false;
            if (press_time > 0)
            {
                diff_time = Time.realtimeSinceStartup - press_time;
                press_time = 0;
            }
            if (scroll != null)
            {
                scroll.OnEndDrag(eventData);
                scroll = null;
            }
            if (OnPointerUpCallBack != null)
            {
                OnPointerUpCallBack(eventData);
            }
            if (OnPressCallBack != null)
            {
                OnPressCallBack(gameObject, false);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (!enabled)
                return;
            if (press_time > 0)
            {
                diff_time = Time.time - press_time;
                press_time = 0;
            }

            dis_curr = (eventData.position - v2Start).sqrMagnitude;
            isCanClick = dis_curr <= limit_dis_min;
            if (!isCanClick)
            {
                isCanClick = dis_curr <= limit_dis_max && diff_time <= limit_time;
            }

            if (isCanClick)
            {
                diff_time = 0;
                if (OnPointerClickCallBack != null)
                    OnPointerClickCallBack(eventData);
                if (OnClickCallBack != null)
                    OnClickCallBack(eventData.pointerEnter);
                if (onClickCall != null)
                    onClickCall(eventData.pointerEnter, eventData.position.x, eventData.position.y);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (!enabled)
                return;

            if (pressed)
            {
                if (scroll != null)
                {
                    scroll.OnDrag(eventData);
                }
                if (OnDragCallBack != null)
                    OnDragCallBack(gameObject, eventData.position.x, eventData.position.y);
                if (OnPointerDragCallBack != null)
                    OnPointerDragCallBack(eventData);
            }
        }

        //增加监听事件
        public void AddListener(ClickCallBack fun)
        {
            OnClickCallBack += fun;
        }

        //移除监听事件
        public void RemoveListener(ClickCallBack fun)
        {
            OnClickCallBack -= fun;
        }

        ScrollRect GetRootScroll(GameObject obj)
        {
            if (obj.transform.parent == null)
                return null;
            if (obj.GetComponent<ScrollRect>() != null)
                return obj.GetComponent<ScrollRect>();
            return GetRootScroll(obj.transform.parent.gameObject);
        }
    }
}
