# SampleFNA
A browser port of the MonoGame Platformer2D sample, using FNA (an open-source implementation of the XNA libraries), SDL2 (compiled with emscripten), and JSIL.

## Building
* You'll need to have [JSIL](https://github.com/sq/JSIL) checked out in a folder *next to* this one, so that ```SampleFNA\..\JSIL``` is valid.
* You also need to clone the [FNA fork](https://github.com/sq/FNA) next to this repository, so that ```SampleFNA\..\FNA``` is valid. In the future we hope to merge this support into trunk FNA (and add it as a submodule to this sample).
* You will need the latest [MonoGame SDK](http://www.monogame.net/downloads/) installed if you want to compile the sample's content into the ```XNB``` files used by the sample. Once you have it installed, double-click the ```Platformer2D\Content\Platformer2d.mgcb``` file to open the content build tool and Build to create the necessary content files. This only needs to be done once.
* For music playback to work, you'll need to convert ```Music.wma``` into an ogg vorbis file called ```Music.ogg```, next to the ```Music.xnb``` file created by the content builder above. Sadly, the content builder cannot do this for us. (JSILc may acquire the ability to do this in the future.)
* To build ```SDL2.js``` and/or ```soft-oal.js``` yourself you'll need to install the latest Emscripten and *todo: instructions here*

## Running
Open ```index.html``` in a browser. **However, this will only work when loaded from a web server, because modern web browsers deny access to resources over the ```file://``` protocol.**
