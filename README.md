Instalação realizada via [Package Manager](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.0/manual/index.html).

# XR_UIElements 0.1 Alpha

XRUI_Elements é um pacote de elementos para interação em Realidade Virtual (e futuramente Realidade Aumentada) para a Unity.

Os elementos deste pacote foram desenvolvidos para funcionar integrados ao package [XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@0.9/manual/index.html).

Atualmente é possível baixar e adicionar a XRUI_Elements em um projeto da unity a partir do sistema de custom packages [Instalation](#installation).

Este projeto faz parte do projeto de pesquisa de Mestrado do @PedroRossa em parceria com o Vizlab | X-Reality and GeoInformatics Lab @vizlab-uni.

<p align="center">
<img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/capaProvisoriaXRUIElements.png"  width="700" align="..." alt="Provisory Image">
</p>

# Table of contents
- [Installation](#installation)
- [Usage](#usage)
- [License](#license)
- [Credits](#credits)
- [How to cite](#how-to-cite)

## Installation

Softwares necessários para utilização do pacote XRUI_Elements

* [Visual Studio](https://visualstudio.microsoft.com/vs/community/) ou [Visual Code](https://code.visualstudio.com/docs/other/unity)
* [Unity 2019.4 LTS](https://unity3d.com/unity/qa/lts-releases?_ga=2.178802020.1167371567.1592846982-112079466.1585313065).

# Configuring XR Interaction Toolkit

1. Create a new Unity Project or open an existent one (Unity 2019.4).
2. On Menu Bar, access "Window->PackageManager"
<img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/PackageManager_01.png" alt="PackageManager Unity" width="350">
3. Check the option to show PREVIEW Packages
<img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/PackageManager_02.png" alt="Preview Packages Option" width="350"">
4. Install the ** XR Interaction Toolkit ** Package
<img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/PackageManager_03.png" alt="XR Interaction Toolkit Install" width="350">

That is it! Now the project can use the Unity package that deal with the basics on Virtual and Augmented Reality.

The next step is get and configure the XRUI_Elements.

<img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/PackageManager_04.png" alt="XR Interaction Toolkit Instaled." width="350">

# Get XRUI_Elements Pacakge

Access [XRUI_Elements 0.1 Alpha](Link Release aqui) and download the XRUI_Elements_01_alpha.unitypackage
On the opened Unity project, go to "Assets -> Import Package -> Custom Package".

<img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/CustomPackage.png" alt="Import Custom Package." width="350">

Because of a internal dependecy of the elements, it's necessary import the TextMeshPro package from Unity.
To do that, goes to "Window -> TextMeshPro -> Import TMP Essential Resource"

<img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ImportTextMeshPro.png" alt="Import TextMeshPro Package." width="350">

Select the XRUI_Elements_01_alpha.unitypackage and import the files to Project.

Congrats!!!! 
Now you already have all the basics to star te amazing journey on the development to Virtual Reality using this simple but awesome elements created with love, tears and dedication.

<img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/XRUI_ElementsMenu.png" alt="XRUI_Elements Menu Bar." width="450">

## Usage

# 2D Elements

| Element       | Sample |
| ------------- |:------:|
| Button Sprite | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/2DButtonSprite.png" alt="2D Button Sprite" height="128"> |
| Button Text   | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/2DButtonText.png" alt="2D Button Text" height="128" > |
| Progress Bar  | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/2DprogressBar.png" alt="2D ProgressBar" height="128" > |
| Toggle        | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/2DToggle.png" alt="2D Toggle" height="128" > |


# 3D Elements

| Element       | Sample |
| ------------- |:------:|
| Button Sprite | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/3DButtonSprite.png" alt="3D Button Sprite" height="128"> |
| Button Text   | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/3DButtonText.png" alt="3D Button Text" height="128" > |
| Progress Bar  | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/3DProgressBar.png" alt="3D ProgressBar" height="128" > |
| Toggle        | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/3DToggle.png" alt="3D Toggle" height="128" > |
| Slider        | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/3DSlider.png" alt="3D Slider" height="128" > |
| Box Slider    | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/3DBoxSlider.png" alt="3D BoxSlider" height="128" > |


# General Elements

| Element       | Sample |
| ------------- |:------:|
| Manipulables  | <img src="https://github.com/PedroRossa/XR_UIElements/blob/master/_ReadMe_Resources/ElementImages/Manipulables.png" alt="Manipulables" height="128" > |
| Feedback System | - |

# Demo Scene

The package comes with a Demo Scene where the users can interact with all the existent elements created on XRUI_Elements.

## Credits
 
- [Pedro Rossa](http://lattes.cnpq.br/8600200220209812)
 
 Special thanks to Research and Development Vizlab's Team for all the support, help and tips on development of this package.

## License

```
MIT Licence (https://mit-license.org/)
```

# How to cite

Not published yet

```


```

