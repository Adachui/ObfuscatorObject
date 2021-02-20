using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendBlock : AppAdvisoryHelper {

	public int DefendBlockId = 0;
	public GameObject lockObj;

	public bool _isLock = false;
	public bool isLock
	{
		get
		{
			return _isLock;
		}
		set
		{
			_isLock = value;
			initBlock ();
		}
	}

	public void initBlock(){
		if (_isLock) {
			lockObj.SetActive (true);
			transform.GetComponent<BoxCollider> ().enabled = false;
		} else {
			lockObj.SetActive (false);
			transform.GetComponent<BoxCollider> ().enabled = true;
		}
	}

}
