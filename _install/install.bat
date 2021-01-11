@ECHO OFF
SET InstallDirectory="C:\Tools\CopyFolderTool\"
SET CopyToFolder="%~dp0Executables\"

ECHO 1. Check for "C:\Tools\CopyFolderTool\" folder
IF NOT EXIST %InstallDirectory%NUL MKDIR %InstallDirectory%
ECHO.

ECHO 2. Copy files to directory
COPY %CopyToFolder% %InstallDirectory%
ECHO.

ECHO 3. Registry entry will be set (needs admin rights)
REM  set __COMPAT_LAYER=RunAsInvoker  
REGEDIT.EXE /S "%~dp0\AddToRightclickMenu.reg"
ECHO ...Done
ECHO.

PAUSE