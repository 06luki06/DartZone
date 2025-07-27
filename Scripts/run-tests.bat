@echo off
setlocal enabledelayedexpansion

echo Delete old TestResults directory...
if exist "Tests\TestResults" (
    rmdir /s /q "Tests\TestResults"
)

echo Running dotnet test...
dotnet test --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=cobertura,opencover
if errorlevel 1 (
    echo Tests failed.
    exit /b %errorlevel%
)

echo Searching for coverage file...
set "coverageFile="
for /d %%D in (Tests\TestResults\*) do (
    if exist "%%D\coverage.cobertura.xml" (
        set "coverageFile=%%D\coverage.cobertura.xml"
        goto found
    )
)

:found
if not defined coverageFile (
    echo coverage.cobertura.xml not found!
    exit /b 1
)

echo Coverage file found: %coverageFile%
echo Generating HTML report...

reportgenerator -reports:"%coverageFile%" -targetdir:"coveragereport" -reporttypes:Html

echo Done. Report is located at coveragereport\index.html
