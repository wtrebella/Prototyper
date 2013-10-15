using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class ParseTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ParseCloud.CallFunctionAsync<IDictionary<string, object>>("hello", new Dictionary<string, object>())
			.ContinueWith(t =>
			{
				IDictionary<string, object> result = t.Result;
				Debug.Log(result);
				foreach (object key in result.Keys) Debug.Log(key);
			});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
