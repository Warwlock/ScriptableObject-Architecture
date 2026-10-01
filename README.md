# ScriptableObject-Architecture

**Note:** This is a forked version of [DanielEverland/ScriptableObject-Architecture](https://github.com/DanielEverland/ScriptableObject-Architecture) project. It was an archived project and I need some new features to support my game projects.

* Makes using Scriptable Objects as a fundamental part of your architecture in Unity super easy.

* Based on Ryan Hipple's 2017 Unite talk https://www.youtube.com/watch?v=raQ3iHhE_Kk

* Reading the [Wiki Page](https://github.com/Warwlock/ScriptableObject-Architecture/wiki) is recommended!

**IMPORTANT:** UI Toolkit is more performant and I want performance when I am using Editor. That's why I completely ditched the IMGUI part of this package. I could have kept it for backwards compability but it makes the editor code very messy. If you use any IMGUI based Editor Tool, then they will not work.

# Features
- Automatic Script Generation
- Variables - All C# primitives
- Clamped Variables
- Variable References
- Typed Events
- Runtime Sets
- Custom Icons
- Optimised Event Stack Tracing
- UI Toolkit is used for Editor UI

# Known Problems / Limitations

- You can't use it with [Naught Attributes](https://github.com/dbrizov/NaughtyAttributes) or similar packages that are using IMGUI! Use [Saints Field](https://github.com/TylerTemp/SaintsField/) or similar packages that are using UIToolkit.
- There is Custom Vector4 Property Drawer. It can affect all of the scripts in the project. But it makes the field nice and more useful.
- Using SceneReference with "UseConstant" inside inspector causes problems. No problem with "UseVariable". I didn't bother to fix it for now.

# Installation

There is only .git and manual installation ways:

* Open package manager and Install Package from Git URL: `https://github.com/Warwlock/ScriptableObject-Architecture.git`
* Manually download this repo and add it to your `Assets` or `Packages` folder.

# Showcase

Visual debugging of events

![](https://i.imgur.com/GPP3aVR.gif)

Full stacktrace and editor invocation for events

![](https://i.imgur.com/S90VUWI.png)

Custom icons

![](https://i.imgur.com/simB0mK.png)

Easy and automatic script generation

![](https://i.imgur.com/xm2gNmo.png)


# ToDo

- [ ] VFX Bindings