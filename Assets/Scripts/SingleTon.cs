using UnityEngine;
using System.Collections;

public class SingleTon<T> : MonoBehaviour where T: MonoBehaviour
{
	private static T _instance;
	private static object _lock = new object ();
	private static bool _isApplicationQuitting;
	private static bool _doNotDestroyOnLoad = false;

	public static bool DoNotDestroyOnLoad {
		get {
			return _doNotDestroyOnLoad;
		}
		set {
			_doNotDestroyOnLoad = value;
		}
	}

	public static T Instance {
		get {
			if (_isApplicationQuitting) {
//				Debug.Log ("[Singleton] Instance '" + typeof(T) +
//				"' already destroyed on application quit." +
//				" Won't create again - returning null.");
				return null;
			}

			lock (_lock) {
				if (_instance == null) {
					_instance = (T)FindObjectOfType (typeof(T));
					
					if (FindObjectsOfType (typeof(T)).Length > 1) {
//						Debug.Log ("[Singleton] Something went really wrong " +
//						" - there should never be more than 1 singleton!" +
//						" Reopenning the scene might fix it." + FindObjectOfType (typeof(T)).name);
						return _instance;
					}
					
					if (_instance == null) {
						GameObject singleton = new GameObject ();
						_instance = singleton.AddComponent<T> ();
						singleton.name = "(singleton) " + typeof(T).ToString ();

						//if(DoNotDestroyOnLoad == true)
						DontDestroyOnLoad (singleton);
						
//						DebugUtils.Log ("[Singleton] An instance of " + typeof(T) +
//						" is needed in the scene, so '" + singleton +
//						"' was created with DontDestroyOnLoad.");
					} else {
						Debug.Log ("[Singleton] Using instance already created: " +
						_instance.gameObject.name);
					}
				}
				
				return _instance;
			}
		}
	}

	public void OnDestroy ()
	{
		_isApplicationQuitting = true;	
	}
}
