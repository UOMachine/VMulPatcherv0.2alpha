VMulPatcher v. 0.2 alpha
Copyright ©2019  Mutagen alias Venushja


VMulPatcher is designed for Ultima Online developers (GMs) and for gamers. It can insert images into Art, ArtLands, Gump and Textures. It can work with .mul and .uop file formats. This program uses the original UO library, which means it cannot patch files from clients version 7.0.60 and higher. But up to this version works reliably.

There are several ways to work with this application.
For developers:
 
There are 2 patching options, via configuration files or just by inserting .bmp files into a folder.

The option via the folder is the simplest, just insert the .bmp image with the file name as the graphics ID.
For example, 0x000F.bmp this image will be uploaded to position 0x000F in the UO file.
So for example I want to overwrite position 0x000F in arty so I put this file "0x000F.bmp" into the Arts folder (if it's ArtLand then put it in the Arts / Lands folder) and run the program and select 1 (mul files) or 11 for uop files. It is not possible to combine MUL and UOP so if you need to create UOP files directly, there must be .uop file in MulFiles folder)

The option via the configuration file is such that it supports named images eg pentagram.bmp (which evaluates the first option as a bad file format)

Example in .ini configuration file:

pentagram = 0x0000
pentagram = {0x0001, 0x0002, 0x0003, 0xC000, 0xC001, 0xFDBC}
pentagram = [0x000A - 0x000F, 0x00A0 - 0x00F0]

I'll explain line by line:
replaces one position
replaces all listed positions
replaces all positions from 0x000A up to 0x000F
Commenting rows does not go side by side, but below each other, as follows:

# replaces one position
pentagram = 0x0000
# replaces all listed positions
pentagram = {0x0001, 0x0002, 0x0003, 0xC000, 0xC001, 0xFDBC}
# replaces all positions from 0x000A up to 0x000F (it is possible to write further intervals after the comma)
pentagram = [0x000A - 0x000F, 0x00A0 - 0x00F0]

These 2 options can be combined with the so-called configured config with pentragram.bmp and also have files in the folder named positions.

You need to put the files you want to update in the MulFiles folder. It depends on what format you are doing.
You will then find the updated files in the Patched folder.


For players:

I added support for Drag & Drop. This is a very easy, fast and reliable way to apply a patch without having to do anything (except to choose whether to patch to MUL or UOP).
Then I created VMulPatcherCreator.exe, which you can easily create your own patch, which you can then share with others. The program is very simple and dynamically combinable, that is to say that it is possible to insert both Arty, LandArty, Gumpy, Texture into one patch at the same time and only one patch is created (currently created under the name "Patch.pvmul").
However, to apply this combined patch it is necessary to have all MUL / UOP files in the "MulFiles" folder, otherwise there is a probable chance that the program will crash or throw an error message.
For testing I added one patch (to the stumps) to the folder "_Examples" and you can try it yourself (it is really only Drag & Drop usage).
Working with VMulPatcherCreator.exe is intuitive and easy. If for some reason you do not want to apply a patch using Drag & Drop, you can write a .bat file.

For example, the start.bat file in which the command will be:

VMulPatcher.exe Patch.pvmul -f -g

You can change and overwrite the name of the Patch.pvmul file. For example, Parezy.pvmul exists in the _Examples folder. The command for the .bat file will be as follows:

VMulPatcher.exe Parezy.pvmul -f -g

And that's about all.




