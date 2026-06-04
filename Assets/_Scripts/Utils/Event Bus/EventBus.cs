using System;
using System.Collections.Generic;

namespace _Scripts.Utils.Event_Bus
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> Subs = new();

        public static void Subscribe<T>(Action<T> handler) where T : struct
        {
            Type type = typeof(T);
            if (!Subs.TryGetValue(type, out List<Delegate> list))
            {
                list = new();
                Subs[type] = list;
            }

            list.Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            Type type = typeof(T);
            if (Subs.TryGetValue(type, out List<Delegate> list))
            {
                list.Remove(handler);
                if (list.Count == 0) Subs.Remove(type);
            }
        }

        public static void Publish<T>(T message) where T : struct
        {
            Type type = typeof(T);
            if (Subs.TryGetValue(type, out List<Delegate> list))
            {
                Delegate[] copy = list.ToArray();
                foreach (Delegate d in copy)
                {
                    (d as Action<T>)?.Invoke(message);
                }
            }
        }
    }
}