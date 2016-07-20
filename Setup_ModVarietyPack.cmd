@ECHO OFF
SETLOCAL ENABLEEXTENSIONS
TITLE ModVarietyPack Setup

ECHO Checking ModsConfig.xml...
IF EXIST "%cd%\SaveData\Config\ModsConfig.xml" (
	ECHO ok. ModsConfig file found in the right place.
	ECHO.
	goto :configfound
)
ECHO No proper ModsConfig found!
ECHO Make sure you copied everything from the zip file correctly
PAUSE
EXIT

:configfound
ECHO Checking ModVarietyPack folder...
IF EXIST "%cd%\Mods\ModVarietyPack" (
	ECHO ok. ModVarietyPack folder found in the right place.
	ECHO.
	goto :folderandconfigfound
)
ECHO No ModVarietyPack folder found!
ECHO Make sure you copied everything from the zip file correctly
PAUSE
EXIT

:folderandconfigfound
ECHO Checking game version...
IF EXIST "%cd%\RimWorldWin.exe" (
	ECHO Steam version detected.
	ECHO.
	goto :steamversion
)

ECHO NoSteam version detected.
ECHO.
ECHO Everything seems ok :)
ECHO Creating shortcut...
ECHO -------------------
ECHO.
ECHO Set oWS = WScript.CreateObject("WScript.Shell") > CreateShortcut.vbs
ECHO sLinkFile = "%cd%\RimWorld ModVarietyPack.lnk" >> CreateShortcut.vbs
ECHO Set oLink = oWS.CreateShortcut(sLinkFile) >> CreateShortcut.vbs
ECHO oLink.TargetPath = "%cd%\RimWorld1238Win.exe" >> CreateShortcut.vbs
ECHO oLink.Arguments = " -savedatafolder=SaveData" >> CreateShortcut.vbs
ECHO oLink.Save >> CreateShortcut.vbs
cscript	CreateShortcut.vbs
del CreateShortcut.vbs
goto :end

:steamversion
ECHO Everything seems ok :)
ECHO Creating shortcut...
ECHO -------------------
ECHO.
ECHO Set oWS = WScript.CreateObject("WScript.Shell") > CreateShortcut.vbs
ECHO sLinkFile = "%cd%\RimWorld ModVarietyPack.lnk" >> CreateShortcut.vbs
ECHO Set oLink = oWS.CreateShortcut(sLinkFile) >> CreateShortcut.vbs
ECHO oLink.TargetPath = "%cd%\RimWorldWin.exe" >> CreateShortcut.vbs
ECHO oLink.Arguments = " -savedatafolder=SaveData" >> CreateShortcut.vbs
ECHO oLink.Save >> CreateShortcut.vbs
cscript	CreateShortcut.vbs
del CreateShortcut.vbs


:end
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