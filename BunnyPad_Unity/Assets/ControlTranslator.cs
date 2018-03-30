/*
 * GIT + Grenchen Institute of Technology
 *
 * BunnyController (a.k.a. JoyMouse)
 * 
 * by Benedict Jäggi @ 2018
 * based on JoyMouse from the same human beeing, from about 1999.
 * 
 * PLEASE INCLUDE THE FORMER TEXT IF YOU USE THIS WORK
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// new
//using System.Drawing;
using System.Runtime.InteropServices;

public class ControlTranslator : MonoBehaviour
{
	protected static float m_xMul = 5.0f;
	protected static float m_yMul = 5.0f;

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		public int X;
		public int Y;
	}

	[DllImport("user32.dll")]
	static extern bool GetCursorPos(out POINT lpPoint);

	public static POINT GetCursorPosition()
	{
		POINT lpPoint;
		GetCursorPos (out lpPoint);
		return lpPoint;
	}

	[DllImport("user32.dll")]
	static extern bool SetCursorPos(int x, int y);

	// Use this for initialization
	void Start ()
	{
		Log("Hallo, ich bin dein Hasenkontrollator (BunnyController).");
		Input.ResetInputAxes ();
		Log ("Verbundene Spielgeräte: ");
		string[] pads = Input.GetJoystickNames();
		string p = "";
		char enter='\n';
		foreach(string pad in pads) 
		{
			p += pad+enter;
		}
		Log (p);
	}
		
	// Update is called once per frame
	void Update () 
	{
		float px = Input.GetAxis ("X");
		float py = Input.GetAxis ("Y");

		// dead zone
		if (px < 0.1f && px > -0.1f)
			px = 0.0f;
		if (py < 0.1f && py > -0.1f)
			py = 0.0f;

		// get cursor position and convert it to float.
		POINT p = GetCursorPosition();
		float curX = (float)p.X;
		float curY = (float)p.Y;

		// calculate new position.
		curX=(float)(curX)+(px*m_xMul);
		curY=(float)(curY)+(py*m_yMul);

		// set new position.
		SetCursorPos ((int)curX, (int)curY);
	}

	// LOG Function
	public static void Log(string text) {Debug.Log(text);}
}
