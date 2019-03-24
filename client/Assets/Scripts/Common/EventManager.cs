/// ==================================================================
/// 作者：代飞
/// 邮箱：289562468@qq.com
/// 手机：18210172108
/// 时间：2014/02/21
/// 版本：1.2
/// 作用:
/// 	事件管理器
/// ==================================================================

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;

public delegate void Handler (object[] args);

public interface ISubscriber
{
	void UnSubscribe ();

	Handler Handler { set; }
}

/// by daifei
public static class EventManager
{
	static Dictionary<SCEvent, List<Subscriber>> m_subscribers = new Dictionary<SCEvent, List<Subscriber>>();

    private class Subscriber : ISubscriber
	{
		SCEvent m_name;
		Handler m_handler;
		
		public Handler Handler { set { m_handler = value; } get { return m_handler; } }

		public Subscriber(SCEvent key, Handler handler)
        {
            m_name = key;
			m_handler = handler;
        }

        public void UnSubscribe()
        {
            List<Subscriber> sublist = null;
            if (m_subscribers.TryGetValue(m_name, out sublist))
            {
				if (sublist.Remove(this))
				{
					if (sublist.Count == 0)
					{
						m_subscribers.Remove(m_name);
					}
				}
            }
		}
		
		public void Notify(object[] args)
		{
			try 
			{
				if (m_handler != null)
					m_handler (args);
			}
			catch (MissingReferenceException)
			{
				Debug.Log("<color=red>事件未注销: " + m_name + "</color>");
			}
			catch (Exception e)
			{
				Debug.Log("<color=yellow>事件通知出错: " + m_name + "</color>\n<color=red>"+e+"</color>");
			}
		}
    }

	/// <summary>
	/// subscribe an event.
	/// </summary>
	/// <param name="name">Event Name.</param>
	/// <param name="handler">Event Handler.</param>
	public static ISubscriber Subscribe(SCEvent name, Handler handler = null)
    {
        List<Subscriber> sublist;
        if (!m_subscribers.TryGetValue (name, out sublist))
        {
            sublist = new List<Subscriber> ();
            m_subscribers.Add(name, sublist);
        }
		// 重复注册
		if (handler != null)
		{
			Subscriber sub = null;
			for (int n = 0; n < sublist.Count; ++n)
			{
				sub = sublist[n];

				if (sub.Handler == handler)
				{
					Debug.LogWarning ("重复注册事件: " + name);
					return sub;
				}
			}
		}
		// 新注册
		Subscriber newSub = new Subscriber(name, handler);
		sublist.Add(newSub);
		return newSub;
	}
	
	public static void UnSubscribe (SCEvent name, Handler handler)
	{
		if (handler == null)
			return;
		List<Subscriber> sublist;
		if (m_subscribers.TryGetValue(name, out sublist))
		{
			for (int i = sublist.Count; i > 0;)
			{
				if (sublist[--i].Handler == handler)
				{
					sublist.RemoveAt(i);
				}
			}
			if (sublist.Count == 0)
			{
				m_subscribers.Remove(name);
			}
		}
	}

	public static void Notify(SCEvent name, params object[] args)
    {
		List<Subscriber> sublist = null;
        if (m_subscribers.TryGetValue(name, out sublist))
        {
			//Debug.Log ("Notify:" + name);
			Subscriber[] subs = sublist.ToArray();
			Subscriber sub = null;
			for (int n = 0; n < subs.Length; ++n)
			{
				sub = subs[n];
				sub.Notify(args);
			}
        }
    }
}