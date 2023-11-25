@echo off

REM 获取脚本所在路径
set "ScriptPath=%~dp0"

REM 切换到要操作的目录
cd /d "%ScriptPath%"

REM 删除 "bin" 和 "obj" 文件夹
for /r %%x in (.) do (
    if exist "%%x\bin" rd /s /q "%%x\bin"
    if exist "%%x\obj" rd /s /q "%%x\obj"
)

echo 删除完成。