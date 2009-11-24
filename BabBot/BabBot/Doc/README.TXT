BabBot - WoW Automa
===================

Welcome to BabBot. This is a World of Warcraft bot. 

** At the current stage of development this should be considered an early ALPHA release.**


Documentation
=============
The latest up to date documentation can always be found at the project's wiki
here:  http://sourceforge.net/apps/trac/babbot/#Documentation


System Requirements 
===================
OS

BabBot has been tested on the following platforms:

    * Windows XP 32-bit
    * Windows Vista 32-bit
    * Windows XP 64-bit
    * Windows Vista 64-bit 

The current development version is under testing for:

    * Windows 7 32-bit
    * Windows 7 64-bit 

Privileges

In order to run BabBot it's necessary to have administrator privileges for the 
account running BabBot itself. This is because the bot will instantiate 
WoW's client as the guest user.

Guest user has to be enabled in your OS.


Configuration
=============
In order to run, BabBot requires to be run from a user with administrator privileges
so that it can run the WoW client as a guest user (default username is "guest" and
can be changed from the Options menu) which has to be enabled in your OS.


Basic usage
===========
Run BabBot and launch WoW through the "Run WoW" button. If your copy of WoW has 
been installed properly, BabBot will detect its path and use it. Otherwise 
you'll have to set it in the Options menu.

If you've enabled autologin in the Options, BabBot will use your credentials 
to log into the game directly, otherwise you will have to log in by hand.

''Make sure that you have enabled "Click to move" and "Auto-loot" in WoW or 
some features might not work as expected.''

If it's the first time you're running BabBot, you will want to make a new 
profile to start with. Enter a name and a description in the fields you
see on the Main tab and then select the WayPoints tab.
Make sure that "Normal" is selected in the dropdown list and check
"Auto add waypoints". Now start moving to record the path your bot will walk
through during its farming cycle. When you are done, uncheck "Auto add 
waypoints".

Now it's time to choose which mobs we will attack. So select the "Enemies"
tab. Run around the path you made before and target the mobs you want to kill
by selecting them and clicking on the "Add to list" button. 
You don't need to add each mob as BabBot will attack any mob that has the
same name as those you have put into the list. 

Once you are done, it's time to save your work so that you don't have to
do it again when you shutdown the bot. Just click on the "Main" tab and
click "Save" and choose a filename for your profile. 
When you run the bot again you will be able to load your profile with the
"Load" button.

The next step is to load a script. A script is a collection of commands that 
your bot will execute in order to perform things like farming, grinding, 
walking around, sell stuff, repairs, etc..

BabBot comes with a sample script that has been written based on the Paladin 
class. Anyone can take that script and modify it and customize it for any 
other class.

So, for the time being, just load the example script by clicking on "Load" on 
the Main tab under the "Script" group, browse to the scripts folder, enter the 
Paladin folder and select Core.cs

BabBot will load the script and be ready to start. It's time to click on the 
"Start" button and enjoy watching your bot doing its duties.