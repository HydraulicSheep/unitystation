﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using PlayGroup;

public partial class PlayerNetworkActions : NetworkBehaviour
{
	[HideInInspector]

	public bool isPulling = false;
	[Command]
	public void CmdPullObject(GameObject obj)
	{
		if (isPulling) {
			CmdStopPulling(gameObject.GetComponent<PlayerSync>().pullingObject);
		}
		
		ObjectActions pulled = obj.GetComponent<ObjectActions>();
		if (pulled.pulledBy != null) {
			pulled.GetComponent<PlayerNetworkActions>().CmdStopPulling(obj);
		}
        if (pulled != null)
        {
            PlayerSync pS = GetComponent<PlayerSync>();
            pS.pullObjectID = pulled.netId;
            isPulling = true;
        }
	}

	[Command]
	public void CmdStopPulling(GameObject obj)
	{
		if (!isPulling)
			return;
		
		isPulling = false;
		ObjectActions pulled = obj.GetComponent<ObjectActions>();
        if (pulled != null)
        {
			PlayerSync pS = gameObject.GetComponent<PlayerSync>();
            pS.pullObjectID = NetworkInstanceId.Invalid;
			pulled.pulledBy = null;
        }
	}

	[Command]
	public void CmdTryPush(GameObject obj, Vector3 pos){
		ObjectActions pushed = obj.GetComponent<ObjectActions>();
		if (pushed != null) {
			pushed.RpcPush(pos);
		}
	}
}