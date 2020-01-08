using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EventManager : SingleTon<EventManager>
{
	
	public enum eGameEvents
	{
       PlayerAnimation_Done,
       Show_Card
    };
    

	void Awake ()
	{
	
	}

	public void OnInstanceCreated ()
	{
	}

	public delegate void GameEventDelegate (params object[] args);


	Dictionary<eGameEvents, GameEventDelegate> m_dicEventRegistry = new Dictionary<eGameEvents, GameEventDelegate> ();

	public void RegisterEvent (EventManager.eGameEvents a_eEvent, GameEventDelegate a_delListener)
	{
		if (!m_dicEventRegistry.ContainsKey (a_eEvent)) {
			m_dicEventRegistry.Add (a_eEvent, a_delListener);
			return;
		}
		
		m_dicEventRegistry [a_eEvent] -= a_delListener;
		m_dicEventRegistry [a_eEvent] += a_delListener;

	}

	public void DeRegisterEvent (EventManager.eGameEvents a_eEvent, GameEventDelegate a_delListener)
	{
		if (!m_dicEventRegistry.ContainsKey (a_eEvent))
			return;
		
			m_dicEventRegistry [a_eEvent] -= a_delListener;
	}

	public void DeRegisterAllEvent(){
		m_dicEventRegistry.Clear ();
	}

	string strEventKey;
	GameEventDelegate d;

	public void TriggerEvent (eGameEvents a_eEvent, params object[] args)
	{
		strEventKey = a_eEvent.ToString ();

		if (m_dicEventRegistry.TryGetValue (a_eEvent, out d)) {
            //Callback callback = d as Callback;
            if (d != null)
            {
                d(args);
            }
            else
            {
                	Debug.Log ("=================================Could not trigger event: " + strEventKey);
            }
		}
		d = null;
	}
    
	void OnDestroy ()
	{
		base.OnDestroy ();
		m_dicEventRegistry.Clear ();
	}
}
