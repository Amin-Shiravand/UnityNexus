using ClientUtilities.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.ClientUtilities.ScheduleSystem
{
    public class ScheduleObj
    {
        public bool IsDone
        {
            get
            {
                return OnComplete == null;
            }
        }

        private Action OnComplete;
        private float delay = 0.0F;
        private float deliverTime = 0.0F;

        public ScheduleObj(Action onComplete, float delay = 0.0F)
        {
            OnComplete = onComplete;
            this.delay = delay;
            this.deliverTime = Time.time + delay;
        }

        public void CancelSchedule()
        {
            OnComplete = null;
        }

        public void Update()
        {
            if (Time.time < deliverTime)
                return;
            try
            {
                OnComplete?.Invoke();
                OnComplete = null;
            }catch(Exception e)
            {
                Debug.LogError(e);
                OnComplete = null;
            }
        }


    }

    public class ScheduleManager : MonoBehaviorSingleton<ScheduleManager>
    {
        private class ThreadedSchedule
        {
            public Action Action;
            public bool IsCompleted;
            public Action OnComplete;
        }

        private List<ScheduleObj> scheduleList = new List<ScheduleObj>();
        private List<ThreadedSchedule> threadedScheduleList = new List<ThreadedSchedule>();

        public ScheduleObj AddSchedule(Action Action, float Delay = 0.0f)
        {
            ScheduleObj obj = new ScheduleObj(Action, Delay);
            scheduleList.Add(obj);
            return obj;
        }

        public void AddThreadedSchedule(Action Action, Action OnComplete = null)
        {
            ThreadedSchedule info = new ThreadedSchedule() { Action = Action, IsCompleted = false, OnComplete = OnComplete };

            threadedScheduleList.Add(info);

            ThreadPool.QueueUserWorkItem((state) =>
            {
                ThreadedSchedule info1 = (ThreadedSchedule)state;

                info1?.Action.Invoke();

                lock (info)
                {
                    info1.IsCompleted = true;
                }

            }, info);
        }

        private void Update()
        {
            for (int i = 0; i < scheduleList.Count; ++i)
            {
                ScheduleObj obj = scheduleList[i];

                obj.Update();
                if (obj.IsDone)
                    scheduleList.RemoveAt(i--);
            }

            for (int i = 0; i < threadedScheduleList.Count; ++i)
            {
                ThreadedSchedule info = threadedScheduleList[i];

                lock (info)
                {
                    if (!info.IsCompleted)
                        continue;
                }

                try
                {
                    info.OnComplete?.Invoke();

                    threadedScheduleList.RemoveAt(i--);
                }catch(Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
