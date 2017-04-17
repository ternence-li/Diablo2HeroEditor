using System;
using System.Collections.Generic;
using Diablo2FileFormat;

namespace Diablo2HeroEditor.Helpers
{
    public class Mediator
    {
        static private MultiDictionary<string, Action<object>> s_actions = new MultiDictionary<string, Action<object>>();

        static public void Register(string token, Action<object> callback)
        {
            s_actions.AddValue(token, callback);
        }

        static public void Unregister(string token, Action<object> callback)
        {
            s_actions.RemoveValue(token, callback);
        }

        static public void NotifyColleagues(string token, object args)
        {
            List<Action<object>> actions;
            if (s_actions.TryGetValue(token, out actions))
            {
                foreach (var action in actions)
                {
                    action(args);
                }
            }
        }
    }
}
