# Animation System

* Scripts to improve animation workflow
* Unity minimum version: **6000.3**
* License: **MIT**

## Summary

Components to animate GameOjects at runtime.

In the Inspector, click on the **Add Component** button > **Animation** and select the category to start to animate.

## How To Use

### Using Ease Animation Curves

You can use Sine, Cosine, Elastic and Bounce (In, Out or both InOut) ease Animation Curves. To do so, first you'll need to add those curves into your project.

Find your Presets folder location by going into any AnimationCurve Window and doing the following:

![FindingPresetsFolder](/Docs~/FindingPresetsFolder.png)

Put the [Ease file](/Docs~/Ease.curves) inside this Presets folder. Back to Unity, select the Ease Presets:

![FindingPresetsFolder](/Docs~/SelecEaseAnimationCurves.png)

You gonna see a collection of Ease Animation curves:

![FindingPresetsFolder](/Docs~/EaseAnimationcurves.png)

You just need to do it once.

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
