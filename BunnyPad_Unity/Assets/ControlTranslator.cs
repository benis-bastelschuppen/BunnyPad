/*
 * GIT + Grenchen Institute of Technology
 *
 * BunnyPad (a.k.a. JoyMouse)
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
// THREADING STUFF FOR THE FOCUS
	#region Public data
	public float timeWaiting = 5000000.0f;
	public string labelInitialText = "I`m the console here!";
	#endregion

	#region Private data
	private string _label;
	private Thread _t1;
	private Thread _t2;
	private bool _t1Paused = false;
	private bool _t2Paused = false;
	#endregion

	#region Start
	void Start () {
		_label = labelInitialText;
		_t1 = new Thread(_func1);
		_t2 = new Thread(_func2);
	}
	#endregion

	#region Threads
	private void _func1()
	{
		if(_label == labelInitialText)
			_label = "";
		while(true)
		{
			_label += 1;
			for(int i = 0; i < timeWaiting; i ++)
				while(_t1Paused){}
		}
	}
	*/
	// ENDOF THREADS

	protected static float m_xMul = 5.0f;
	protected static float m_yMul = 5.0f;

	private static bool m_leftClickDown = false;
	private static bool m_rightClickDown = false;

	// create the point structure for the windows getcursorpos function.
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		public int X;
		public int Y;
	}

// WINDOWS SPECIFIC		
	// import getcursorpos from user32.dll
	[DllImport("user32.dll")]
	static extern bool GetCursorPos(out POINT lpPoint);

	// return the cursor position as POINT.
	public static POINT GetCursorPosition()
	{
		POINT lpPoint;
		GetCursorPos (out lpPoint);
		return lpPoint;
	}

	// import setcursorpos from user32.dll
	[DllImport("user32.dll")]
	static extern bool SetCursorPos(int x, int y);

	// import mouse_event from user32.dll
	[DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
	public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
	//Mouse actions
	private const int MOUSEEVENTF_LEFTDOWN = 0x02;
	private const int MOUSEEVENTF_LEFTUP = 0x04;
	private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
	private const int MOUSEEVENTF_RIGHTUP = 0x10;
// ENDOF WINDOWS SPECIFIC

// Use this for initialization
	void Start ()
	{
		Application.runInBackground = true;
		Log("Hallo, ich bin dein Hasenkontrollator (BunnyController).");
		Input.ResetInputAxes ();
		string p = "Verbundene Spielgeräte:\n";
		string[] pads = Input.GetJoystickNames();
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
		// get cursor position and convert it to float.
		POINT p = GetCursorPosition();
		UpdateMousePosition ((float)p.X, (float)p.Y);
		TranslatePadButtons ((uint)p.X, (uint)p.Y);
	}

	// translate the pad buttons to the mouse.
	protected void TranslatePadButtons(uint X, uint Y)
	{
		// click happens after release of button.
		if (Input.GetButton ("LeftMouseButton")) {
			if (!m_leftClickDown) {
				mouse_event (MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
				Log ("Left Mouse Down");
			}
			m_leftClickDown = true;
		} else {
			if (m_leftClickDown) {
				mouse_event (MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
				Log ("Left Mouse Up");
			}
			m_leftClickDown = false;
		}

		// click happens after release of button.
		if (Input.GetButton ("RightMouseButton")) {
			if (!m_rightClickDown) {
				mouse_event (MOUSEEVENTF_RIGHTDOWN, X, Y, 0, 0);
				Log ("Right Mouse Down");
			}
			m_rightClickDown = true;
		} else {
			if (m_rightClickDown) {
				mouse_event (MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
				Log ("Right Mouse Up");
			}
			m_rightClickDown = false;
		}
	}

	// get the joystick axis and set new mouse position.
	protected void UpdateMousePosition(float curX, float curY)
	{
		float px = Input.GetAxis ("X");
		float py = Input.GetAxis ("Y");

		// dead zone
		if (px < 0.1f && px > -0.1f)
			px = 0.0f;
		if (py < 0.1f && py > -0.1f)
			py = 0.0f;

		// calculate new position.
		curX=(float)(curX)+(px*m_xMul);
		curY=(float)(curY)+(py*m_yMul);

		// set new position.
		SetCursorPos ((int)curX, (int)curY);

	}

	// LOG Function
	public static void Log(string text) {Debug.Log(text);}
}
