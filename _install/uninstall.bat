@ECHO OFF
SET InstallDirectory="C:\Tools\CopyFolderTool\"

ECHO 1. Remove "C:/Tools/CopyFolderTool" folder
RMDIR /Q /S %InstallDirectory%
ECHO ...Done
ECHO.

ECHO 2. Registry entry will be removed (needs admin rights)
REM  set __COMPAT_LAYER=RunAsInvoker  
REGEDIT.EXE  /S  "%~dp0\RemoveToRightclickMenu.reg"
ECHO ...Done
ECHO.

PAUSE