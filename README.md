# SampleFNA
A browser port of the MonoGame Platformer2D sample, using [FNA](https://github.com/flibitijibibo/FNA) (an open-source implementation of the XNA libraries), [SDL2](https://github.com/emscripten-ports/SDL2), [Emscripten](http://emscripten.org/), and [JSIL](http://jsil.org/).

## Building
* You'll need to have [JSIL](https://github.com/sq/JSIL) checked out in a folder *next to* this one, so that ```SampleFNA\..\JSIL``` is valid.
* You also need to clone the [FNA fork](https://github.com/sq/FNA) next to this repository, so that ```SampleFNA\..\FNA``` is valid. In the future we hope to merge this support into trunk FNA (and add it as a submodule to this sample).
* Building the project in Visual Studio will automatically invoke ```JSILc``` to generate the appropriate JavaScript files. Check the build output window to see the results. (You can also run the ```buildJSIL.bat``` file directly.)
* You will need the latest [MonoGame SDK](http://www.monogame.net/downloads/) installed if you want to compile the sample's content into the ```XNB``` files used by the sample. Once you have it installed, double-click the ```Platformer2D\Content\Platformer2d.mgcb``` file to open the content build tool and Build to create the necessary content files. This only needs to be done once.
* For Song playback to work, you'll need to convert ```Music.wma``` into an ogg vorbis file called ```Music.ogg```, next to the ```Music.xnb``` file created by the content builder above. Sadly, the content builder cannot do this for us. (JSILc may acquire the ability to do this in the future.)
* To build ```SDL2.js``` install the [latest Emscripten SDK](https://kripken.github.io/emscripten-site/docs/getting_started/downloads.html) and run ```buildSDL.bat```.
* To build ```soft_oal.js``` install the [latest Emscripten SDK](https://kripken.github.io/emscripten-site/docs/getting_started/downloads.html) and run ```buildOpenAL.bat```
  * For Song playback to work you will need to hand-edit ```soft_oal.js``` by searching for ```var AL={contexts:``` and replacing it with ```var AL=Module["OpenAL"]={contexts:```. This is a workaround for an [open Emscripten issue](https://github.com/kripken/emscripten/issues/3599).

## Running
Open ```index.html``` in a browser. **However, this will only work when loaded from a web server, because modern web browsers deny access to resources over the ```file://``` protocol.**

## Authors & Copyright
* The FNA and Platformer2D ports are the work of [Katelyn Gadd](https://github.com/kg) and [Jaiden Mispy](https://github.com/mispy), aided by partial sponsorship from [the Mozilla Foundation](https://www.mozilla.org/en-US/).
* The Platformer2D sample is derived from [the MonoGame Platformer2D sample](https://github.com/Mono-Game/MonoGame.Samples).
* [FNA](https://github.com/flibitijibibo/FNA) is the work of [Ethan Lee](https://github.com/flibitijibibo) and numerous other authors.
