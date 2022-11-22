Alpha Version (!) no warrantries given.

Control your computer with a gamepad.
by ben0bi @ ~2002, 2018, 2022

e.G. play the next movie in the browser or
	play your most loved adventure (Monkey Island :) ) with the gamepad.

Optimized for use with modern games like Splitgate.

Back in 2002 I wrote the program JoyMouse for using the joystick as a mouse.
In that time, programs where called programs and not apps. Times of wonder
and prosperity. :) But nowadays this program was way outdated, and so I 
created a new one.

This one.

I began to write BunnyPad with the Unity engine, 
but it is way more effective to make it with pure C#.

Majestic_11 is the Visual Studio Project.
I deleted the other ones.

You will find the executables in this folder:
Majestic_11/Majestic_11/bin/

It's intended to use with XBox Gamepads but the newer versions
also support standard pc gamepads.

It will search for a controller until it found one.
If you press "Hide Me" on the main window without a controller, 
it will minimize. With a controller, it will hide completely and not 
be visible in the taskbar or somewhere else - running in the background.
You can get back the main menu by pressing the Start MAIN MENU* button
or by disconnecting the controller.

You can do the following stuff right now:
(Versions below 1.0.0, like 0.1.2, are alpha- and beta-versions "in development").
(Each beta version has another focus. 0.5.x has the focus on the configuration.)

v0.9.0 Working now with standard pc gamepads (DirectInput)
	 Optimized for use with modern games.

v0.8.0 With virtual keyboard now but it won't work sometimes when 
		the main menu gets hidden and it had the focus before. It will not give the focus back to the underlying app then, 
		even when you click on it and it really *has* the focus - then you turn VK on and it does not work anymore.
		Just press the main menu button then, click on your app and then again on the main menu button to hide it without focus.

v0.7.x Uh, I forgot the virtual keyboard. Working on it.

v0.7.0 RC1 Release Candidate 1 => See publish folder.

v0.6.x

Save and load the configuration from 0.5.x. 
Load the last saved or loaded config on startup.
Prepare RC1.

v0.5.x

Configure each button for yourself in the configuration window.
You can also make double button configs and stuff, like you want.
*All it needs to have configured is the MAIN MENU button.

There are several functions which you can assign to each button. 
Keyboard actions can be repeated, and the volume-up and -down actions.
Every button can have the FN flag so you need to press the FN-button for it.
Configure it like you want.

<= v0.4

+ Move the mouse cursor with the left thumbstick.
+ Simulate the mouse wheel with the right thumbstick.
+ Use the arrow keys with the digital pad. 
	It waits 500ms after the first keystroke, then it will do it faster.

+ Adjust the system volume with FN + DPad-Up/-Down/-Left for volume up, down and mute.

(Main Buttons)
+ Left click with A.
+ Right click with B.
+ Press ENTER with Y.
+ Press Backspace with X

+ FN: Select another config by holding the left shoulder button (LS).
+ Ctrl-C with FN+A.
+ Ctrl-V with FN+B.
+ Ctrl-Y with FN+Y.
+ Ctrl-Z with FN+X.

(Secondary Buttons)
+ Press ESC with the Back button. (Left middle button)
+ Press TABulator with the right shoulder button.

+ Slow down mouse/scrollwheel speed with the left trigger (LT).
+ Speed up mouse/scrollwheel speed with the right trigger (RT).

+ Show the main menu with the Start button.

+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
