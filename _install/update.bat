@ECHO OFF
SET InstallDirectory="C:\Tools\CopyFolderTool\"
SET CopyToFolder="%~dp0Executables\"

ECHO 1. Check for "C:\Tools\CopyFolderTool\" folder
IF NOT EXIST %InstallDirectory%NUL MKDIR %InstallDirectory%
ECHO.

ECHO 2. Update files in directory
COPY %CopyToFolder% %InstallDirectory%
ECHO ...Done
ECHO.

PAUSE