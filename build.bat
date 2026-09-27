@echo off
setlocal
cd /d "%~dp0"
echo =======================================================
echo   Compiling Goi Con Tim Lam Qua (C# Edition)...
echo =======================================================
set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
set "WPF_LIB=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\WPF"

"%CSC%" /nologo /target:winexe /out:GoiConTimLamQua.exe /lib:"%WPF_LIB%" /r:System.dll /r:System.Drawing.dll /r:System.Windows.Forms.dll /r:PresentationCore.dll /r:WindowsBase.dll PixelArt.cs CodeHighlighter.cs Timeline.cs MainForm.cs Program.cs

if %ERRORLEVEL% EQU 0 (
    echo [SUCCESS] GoiConTimLamQua.exe has been compiled successfully!
) else (
    echo [ERROR] Compilation failed.
)
if "%1" neq "/nopause" pause
