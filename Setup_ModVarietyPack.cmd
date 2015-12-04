@ECHO OFF
TITLE ModVarietyPack Setup

IF EXIST %cd%\SaveData\Config\ModsConfig.xml (
	ECHO ModsConfig found in the right place.
) ELSE (
	ECHO No proper ModsConfig found!
	ECHO Make sure you copied everything from the zip file correctly
	PAUSE
	EXIT
)
IF EXIST %cd%\Mods\ModVarietyPack (
	ECHO ModVarietyPack folder found in the right place.
) ELSE (
	ECHO No ModVarietyPack folder found!
	ECHO Make sure you copied everything from the zip file correctly
	PAUSE
	EXIT
)
ECHO.
ECHO Everything seems ok :)
ECHO Creating shortcut...
ECHO -------------------
ECHO.

ECHO Set oWS = WScript.CreateObject("WScript.Shell") > CreateShortcut.vbs
ECHO sLinkFile = "%cd%\RimWorld ModVarietyPack.lnk" >> CreateShortcut.vbs
ECHO Set oLink = oWS.CreateShortcut(sLinkFile) >> CreateShortcut.vbs
ECHO oLink.TargetPath = "%cd%\RimWorld914Win.exe" >> CreateShortcut.vbs
ECHO oLink.Arguments = " -savedatafolder SaveData" >> CreateShortcut.vbs
ECHO oLink.Save >> CreateShortcut.vbs
cscript	CreateShortcut.vbs
del CreateShortcut.vbs

ECHO -------------------
ECHO.
ECHO Setup for ModVarietyPack completed.
ECHO Start the game with the new shortcut "RimWorld ModVarietyPack"
ECHO.
ECHO Have Fun :)
ECHO.
ECHO.
PAUSE
del Setup_ModVarietyPack.cmd