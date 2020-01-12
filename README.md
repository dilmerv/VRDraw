# VR Draw (A Draw Oculus Experience)

An open source project demonstrating how to draw in virtual reality. This project contains initial implementation of networking TCP server and client where clients can send what each draws to other clients.

As a way to give back please consider subscribing to my [YouTube channel](https://www.youtube.com/c/dilmervalecillos?sub_cofirmation=1) thank you !

## VR Draw Main Scene (With Hands)

***Important Be sure to select "Hands Only Or Controller And Hands" in the OVRCameraRig Hand Tracking Support***
VRHandDrawing.unity is the main scene for hand drawing implementation and few examples created while running with the Oculus Quest are shown below:

<img src="https://github.com/dilmerv/VRDraw/blob/master/docs/images/handdraw_1.gif" width="300">

<img src="https://github.com/dilmerv/VRDraw/blob/master/docs/images/handdraw_2.gif" width="300">

<img src="https://github.com/dilmerv/VRDraw/blob/master/docs/images/handdraw_3.gif" width="300">

## VR Draw Main Scene (With Controller)

***Important Be sure to select "Hands Only Or Controller And Hands" in the OVRCameraRig Hand Tracking Support***

VRDrawing.unity is the main scene for controller drawing implementation and few examples created while running with the Oculus Quest are shown below:

<img src="https://github.com/dilmerv/VRDraw/blob/master/docs/images/demo_1.gif" width="300">

<img src="https://github.com/dilmerv/VRDraw/blob/master/docs/images/demo_2.gif" width="300">

<img src="https://github.com/dilmerv/VRDraw/blob/master/docs/images/demo_3.gif" width="300">

<img src="https://github.com/dilmerv/VRDraw/blob/master/docs/images/demo_4.gif" width="300">

## VR Draw testing in the editor

It always takes time to run and test in the Oculus device so instead I provide you with a few tools to help you test your changes within Unity.

1- Enable AllowEditorControls by searching for VRDrawLeft and VRDrawRight game objects in the hierarchy and updating the property in the VRDraw.cs inspector. 

<img src="https://github.com/dilmerv/VRDraw/blob/master/docs/images/instructions_1.png" width="300">

2- Hit Play in the editor and use the following keyboard keys to move around, draw, and bring draw UI options:

### Movement and drawing
* Left arrow, right arrow, down arrow, up arrow - Movement
* Press and hold K - Emulates drawing with left controller
* Press and hold L - Emulates drawing with right controller

### VR Draw Options
* Press X - Opens left controller drawing options
* Press Z - Opens right controller drawing options
* Left arrow, right arrow, down arrow, up arrow - To move through drawing options
* Press L - Select drawing option on left controller
* Press R - Select drawing option on right controller