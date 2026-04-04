# Animation System

* Scripts to improve animation workflow
* Unity minimum version: **6000.3**
* Current version: **0.1.0**
* LitMotion Version: **2.0.1**
* License: **MIT**

## Summary

Components to animate GameOjects in runtime.

Also, this packges uses [LitMotion](https://annulusgames.github.io/LitMotion/) and [LitMotion.Animation](https://annulusgames.github.io/LitMotion/articles/en/litmotion-animation-overview.html) as the Tween Library.

## How To Use

### Using LitMotion

Add the LitMotionAnimation component into a gameobject and configure it. Check the [official documentation](https://annulusgames.github.io/LitMotion/articles/en/litmotion-animation.html).

## How to upgrade LitMotion

1. Go to the [LitMotion Release Page](https://github.com/annulusgames/LitMotion/releases)
2. Download the lasted release
3. Paste `LitMotion` and `LitMotion.Animation` folders inside the local [LitMotion folder](/LitMotion).
4. Redo the custom implementations:
    - [Set LitMotionAnimationComponent Targets when reseted](https://github.com/HyagoOliveira/AnimationSystem/commit/887a0b90964472d2aeff89ebf6240ec1dba64ff5)
    - [LitMotionAnimation implements AbstractAnimation](https://github.com/HyagoOliveira/AnimationSystem/commit/3d4762a3d6212440ec2113984a684db5a2137566)

## Installation

### Using the Package Registry Server

Follow the instructions inside [here](https://cutt.ly/ukvj1c8) and the package **ActionCode-Animation System** 
will be available for you to install using the **Package Manager** windows.

### Using the Git URL

You will need a **Git client** installed on your computer with the Path variable already set. 

- Use the **Package Manager** "Add package from git URL..." feature and paste this URL: `https://github.com/HyagoOliveira/AnimationSystem.git`

- You can also manually modify you `Packages/manifest.json` file and add this line inside `dependencies` attribute: 

```json
"com.actioncode.[package-name]":"https://github.com/HyagoOliveira/AnimationSystem.git"
```

---

**Hyago Oliveira**

[GitHub](https://github.com/HyagoOliveira) -
[LinkedIn](https://www.linkedin.com/in/hyago-oliveira/) -
<hyagogow@gmail.com>
