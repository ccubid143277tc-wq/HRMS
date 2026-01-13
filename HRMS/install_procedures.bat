@echo off
echo Installing Room and Guest stored procedures...
echo.

REM Find MySQL client path (common locations)
set MYSQL_PATH=
if exist "C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" (
    set MYSQL_PATH="C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe"
) else if exist "C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe" (
    set MYSQL_PATH="C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe"
) else if exist "C:\xampp\mysql\bin\mysql.exe" (
    set MYSQL_PATH="C:\xampp\mysql\bin\mysql.exe"
) else (
    echo ERROR: MySQL client not found in common locations.
    echo Please install MySQL or add mysql.exe to your PATH.
    pause
    exit /b 1
)

echo Found MySQL at: %MYSQL_PATH%
echo.

echo Installing Room procedures...
%MYSQL_PATH% -u root -p < "DbContext\StoredProcedures\Room_Procedures.sql"
if %errorlevel% neq 0 (
    echo ERROR: Failed to install Room procedures.
    pause
    exit /b 1
)

echo.
echo Installing Guest procedures...
%MYSQL_PATH% -u root -p < "DbContext\StoredProcedures\Guest_Procedures.sql"
if %errorlevel% neq 0 (
    echo ERROR: Failed to install Guest procedures.
    pause
    exit /b 1
)

echo.
echo Installing Reservation procedures...
%MYSQL_PATH% -u root -p < "DbContext\StoredProcedures\Reservation_Procedures.sql"
if %errorlevel% neq 0 (
    echo ERROR: Failed to install Reservation procedures.
    pause
    exit /b 1
)

echo.
echo Installing Users procedures...
%MYSQL_PATH% -u root -p < "DbContext\StoredProcedures\Users_Procedures.sql"
if %errorlevel% neq 0 (
    echo ERROR: Failed to install Users procedures.
    pause
    exit /b 1
)

echo.
echo Installing Receipt procedures...
%MYSQL_PATH% -u root -p < "DbContext\StoredProcedures\Receipt_Procedures.sql"
if %errorlevel% neq 0 (
    echo ERROR: Failed to install Receipt procedures.
    pause
    exit /b 1
)

echo.
echo All stored procedures installed successfully!
pause
