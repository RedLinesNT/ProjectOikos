<div align="center"> <img src="https://github.com/RedLinesNT/ProjectOikos/blob/main/Visual/Oikos_Wide.png"> </div>
Proof-Of-Concept of an educational game for young children about the importance of recycling.<br/>
Please note that this project only has the purpose of "showcasing" something somewhat playable, don't except anything clean :)

<hr>

Project realized by:<br/>
   * [Carla MALERGUE (3D Artist)](https://www.linkedin.com/in/carla-malergue-b492272a9/)<br/>
   * [Baptiste PELLETIER (3D Artist)](https://www.linkedin.com/in/baptiste-pelletier-269597233/)<br/>
   * [Vasgen VARDANIAN (Game Designer)](https://www.linkedin.com/in/vasguen-vardanian/)<br/>
   * [Léo GRIFFOULIERE (Programmer)](https://www.linkedin.com/in/l%C3%A9o-griffouli%C3%A8re/)<br/>

<hr>

## Summary

<!--ts-->
   * [Required programs](#required-programs)
   * [Project packages](#project-packages)
   * [Programming standards](#programming-standards)
   * [Documentation](#documentation)
<!--te-->

<hr>

## Required programs
  - Unity Hub
  - Unity Engine (2022.3.5f1 - LTS)

Programmers will need an IDE of their choice, these twos are recommended :
  - Visual Studio Community 2019/2022
  - JetBrains Rider 2023

### Missing the Unity version mentionned above ?
  To install the engine version required with Unity Hub, go to [this page from Unity](https://unity.com/releases/editor/whats-new/2022.3.5).<br>
  And at the top of this page, click on "<i>Install this version with Unity Hub</i>", then Unity Hub will deal with the rest.

<hr>

## Engine modules
  To be able to build the project, certain modules are required.<br>
  <i>Note that these modules are optional for members who do not wish to make deployable versions of the project.</i><br>
  To add modules to an already installed version of Unity, go to Unity Hub, then to "<i><strong>Install</strong></i>" > "<i><strong>2022.3.5f1</strong></i>" > "<i><strong>Add Modules</strong></i>". And select the following modules : 
  - Linux Build Support (IL2CPP) - (217 MB)
  - Mac Build Support (Mono) - (1.87 GB)
  - Windows Build Support (IL2CPP) - (418 MB)

<hr>

## Project packages

Here's the list of packages currently installed :
 - 2D Sprite (1.0.0)
 - Autodesk FBX SDK for Unity (4.2.1)
 - Burst (1.8.8)
 - Core RP Library (14.0.8)
 - Custom NUnit (1.0.6)
 - Editor Coroutines (1.0.0)
 - FBX Exporter (4.2.1)
 - JetBrains Rider Editor (3.0.24)
 - Mathematics (1.2.6)
 - ProBuilder (5.1.1)
 - Searcher (4.9.2)
 - Settings Manager (2.0.1)
 - Shader Graph (14.0.8)
 - Sysroot Base (2.0.7)
 - Sysroot Linux x64 (2.0.6)
 - Test Framework (1.1.33)
 - TextMeshPro (3.0.6)
 - Timeline (1.7.5)
 - Toolchain Win Linux x64 (2.0.6)
 - Unity UI (1.0.0)
 - Universal RP (14.0.8)
 - Visual Studio Editor (2.0.20)

<hr>

## Programming standards

  Class :
    ```
    CamelCase
    ```<br>
  Attributes :
    ```
    camelCase
    ```<br>
  Variables :
    ```
    _camelCase
    ```<br>
  Methods :
    ```
    CamelCase()
    ```<br>
  Enums :
    ```
    ENameOfEnum
    ```<br>
  Enum's Values :
    ```
    VALUE
    ```<br><br>
All attributes must be private, use Properties or Getters/Setters instead.<br/>
Every names/comments MUST be in English, no matter how broken yours is.

<hr>

## Documentation
  Useful docs :<br>
<!--ts-->
   * <a href="Docs/Import_Texture_Documentation.pdf">Export your textures from Substance to Unity.</a>
<!--te-->
  
<hr>

The project currently have <i>1'760</i> lines of code (C#)

<hr>  
