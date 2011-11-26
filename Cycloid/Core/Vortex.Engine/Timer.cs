using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace vortexWin.Engine
{
    public class Timer
    {
        public class TimerAction
        {
            double _initialTime;
            double _remainingTime;

            public double Time { 
                get { return _remainingTime; }
                set
                {
                    _initialTime = value;
                    _remainingTime = value;
                }
            }
            public Action Handler;
            public Action<int> Notifer;
            public bool Repeat = false;
            public int RepeatCount = -1;
            public object Tag = null;

            public object Clone()
            {
                return new TimerAction()
                {
                    Time = _initialTime,
                    Handler = this.Handler, 
                    Notifer = this.Notifer,
                    Repeat = this.Repeat,
                    RepeatCount = this.RepeatCount,
                    Tag = this.Tag
                };
            }
        }

        static List<TimerAction> actions = new List<TimerAction>();
        public static bool Paused = false;

        public static void Update(GameTime gameTime)
        {
            if (!Paused)
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    actions[i].Time = actions[i].Time - gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (actions[i].Notifer != null)
                        actions[i].Notifer((int)actions[i].Time);
                }

                List<TimerAction> itemsToAdd = new List<TimerAction>();

                foreach (TimerAction action in
                    actions.Where<TimerAction>(x => x.Time < 0))
                {
                    action.Handler();
                    if (action.Repeat)
                        if (action.RepeatCount == -1 || action.RepeatCount > 0)
                        {
                            if(action.RepeatCount>0)
                                action.RepeatCount--;

                            itemsToAdd.Add((TimerAction)action.Clone());
                        }
                }

                foreach (TimerAction action in itemsToAdd)
                    Add(action);

                actions.RemoveAll(x => x.Time < 0);
            }
        }

        private static void Add(TimerAction action)
        {
            actions.Add(action);
        }

        public static void Clear()
        {
            actions.Clear();
        }

        public static void Add(double item, Action handler)
        {
            Add(item, handler, null);
        }

        public static void Add(double time, Action handler,Action<int> progresshandler)
        {
            TimerAction action = CreateAction(time, handler,progresshandler);
            actions.Add(action);
        }

        private static TimerAction CreateAction(double time, Action handler)
        {
            return CreateAction(time, handler, null);
        }

        private static TimerAction CreateAction(double time, Action handler,Action<int> progresshandler)
        {
            TimerAction action = new TimerAction();
            action.Time = time;
            action.Handler = handler;
            action.Notifer = progresshandler;
            return action;
        }

        public static void Add(double time, Action handler,bool repeat)
        {
            TimerAction action = CreateAction(time, handler);
            action.Repeat = repeat;
            actions.Add(action);
        }

        public static void Add(double time, Action handler,int repeatcount)
        {
            TimerAction action = CreateAction(time, handler);
            action.Repeat = true;
            action.RepeatCount = repeatcount;
            actions.Add(action);
        }

        public static int Count
        {
            get { return actions.Count; }
        }

        public static double RemainsTime
        {
            get{

                foreach (TimerAction action in actions)
                    if (action.Repeat)
                        return -1;

                double result = actions.Max<TimerAction>(value => value.Time);
                return result;
            }
        }
    }
}
