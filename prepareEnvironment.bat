@echo off

for /f "tokens=* delims=" %%a in ('node -v') do (
    set "res=%%a"
)
SET /A ver = "%res:~1,2%"
if /i %ver% gtr 16 (
    set NODE_OPTIONS=--openssl-legacy-provider
)

ping -n 1 mon.testkontur.ru | find "TTL=" > nul
if errorlevel 1 (
    cd "Source\Kontur.BigLibrary.Service\ClientApp"
    del /f .npmrc
    echo .npmrc was deleted
    cd ../../..
)