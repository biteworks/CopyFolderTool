@ECHO OFF
SET InstallDirectory="C:\Tools\CopyFolderTool\"
SET CopyToFolder="%~dp0Executables\"

ECHO 1. Check for "C:/Tools" folder
IF NOT EXIST %InstallDirectory%NUL MKDIR %InstallDirectory%
ECHO.

ECHO 2. Update files in directory
ROBOCOPY %CopyToFolder% %InstallDirectory% /XO /E
ECHO ...Done
ECHO.

PAUSE