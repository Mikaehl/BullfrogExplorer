# BullfrogExplorer
Bullfrog Production games asset explorer. Currently only support Theme Park (Well and will probably ever only support Theme Park).

# Introduction
This program currently load almost all the the various data of the game excepted the music and video files.

# Getting Started
You need the original Theme Park datafiles to be able to load them. Change the pathname of your files in the config file named CONFIG.

# Build and Test
You need monogame to be able to build this project.
This program currently use .NET Framework 4.5. The project is a Visual Studio 2019 one but should work with Visual Studio 2017 either.

# Contribute
Feel free to contribute either by adding other games or resolving some issues. The code is not really perfect but this could easily been ehanced.

# Futur plans
I may plan to add the music files and maybe the video as well. Later on, to include the support of other games, I will create a separate library called maybe Bullfrog Loader.
Don't expect too much from myself as I'm not a professionnal developper, have a lot of various ideas for a lot of things and a lot of other interests.

The music use the HMP format, I need a .NET midi library able to open and play them. I'm not to familiar with midi libraries, if you find and have some code able to load a HMP file, feel free to contribute (the data loader is ready, I've tested the data and there are correctly loaded).

The movies are something like flic if I remember correctly some viewer exists, maybe I'll use a library if I find one.

There are still some data I've no idea what they are (ridexxx.ani). Feel free to explain them to me !

# Datafiles
It's mostly the same specs as for Syndicate or Theme Hospital, largely documented on the Internet but with less functionnalities and some mysteries there and there.
