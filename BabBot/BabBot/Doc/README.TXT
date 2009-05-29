BabBot - WoW Automa
===================

Welcome to BabBot. This is a World of Warcraft bot. 

** At the current stage of development this should be considered an early ALPHA release.**


Configuration
=============
Some parameters are still hardcoded and will be editable during a later release. Right now you're 
required to run on a 32bit OS (XP or Vista). Development releases for x64 processors are
under testing. You can still compile it in 64bit mode from the sources though.

In order to run, BabBot requires to be run from a user with administrator privileges
so that it can run the WoW client as a guest user (currently the username is "guest" and
it's got to be enabled in your OS).


Basic usage
===========
Run BabBot and either launch WoW through the "Run WoW" button or attach to an 
already running client instance (but beware that attaching to a running 
instance might make you vulnerable to Warden and get you banned as your
client might be running as a privileged user and you don't want this to 
happen!)

Enter your username and password in WoW, log into the game with your favorite
toon and let's start rocking.

If it's the first time you're running BabBot, you will want to make a new
profile to work from. Enter a name and a description in the fields you
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
When you run the bot again you will be able to load your profilo with the
"Load" button.
