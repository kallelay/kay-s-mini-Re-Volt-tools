[Files]
Source: SevenZipSharp.dll; DestDir: {app}
Source: 7z.dll; DestDir: {app}
Source: track_installer.exe; DestDir: {app}; Flags: comparetimestamp ignoreversion
Source: Config.exe; DestDir: {app}\Config\; DestName: Configuration
Source: ..\..\..\..\..\..\track.ico; DestDir: {app}; Flags: comparetimestamp
Source: C:\Windows\Fonts\calibri.ttf; DestDir: {fonts}; Flags: onlyifdoesntexist uninsneveruninstall; FontInstall: Calibri
[Icons]
Name: {group}\Re-Volt Live installers\; Filename: {app}\Config\Configuration; WorkingDir: {app}; IconFilename: {app}\track_installer.exe; Comment: Configuration file; IconIndex: 0
[Registry]
Root: HKCR; SubKey: .rvt; ValueType: string; ValueData: Re-Volt track; Flags: uninsdeletekey
Root: HKCR; SubKey: Re-Volt track; ValueType: string; ValueData: Re-Volt track Installer file; Flags: uninsdeletekey
Root: HKCR; SubKey: Re-Volt track\Shell\Open\Command; ValueType: string; ValueData: """{app}\track_installer.exe"" ""%1"""; Flags: uninsdeletevalue
Root: HKCR; Subkey: Re-Volt track\DefaultIcon; ValueType: string; ValueData: {app}\track.ico,0; Flags: uninsdeletevalue
[Setup]
InternalCompressLevel=max
VersionInfoVersion=1.2
VersionInfoCompany=Kallel ©
VersionInfoDescription=Re-Volt track Installer Installer
VersionInfoCopyright=Kallel ©
Compression=lzma/ultra
VersionInfoProductName=RVLLAB Track installer
VersionInfoProductVersion=1.2
AppCopyright=Kallel ©
AppName=Re-Volt track installer
AppVerName=1.2
ChangesAssociations=true
RestartIfNeededByRun=false
ShowLanguageDialog=auto
WindowVisible=false
OutputDir={pf}\ReVolt Live Lab\Track installer\
DefaultDirName={pf}\ReVolt Live Lab\Track installer\
AppPublisher=Kallel
AppPublisherURL=http://kallel.co.cc/
AppSupportURL=http://z3.invisionfree.com/Revolt_Live/
AppVersion=Re-Volt Live Track Installer: Uninstaller
AppID={{595f0531-e817-4f4b-89e1-7ffd72031280}
UninstallDisplayIcon={app}\track.ico
UninstallDisplayName=Re-Volt Live Track Installer: Uninstaller
WizardImageBackColor=$dc8f10
SetupIconFile=C:\Users\kallel\Documents\track.ico
SetupLogging=true
WizardImageFile=C:\Users\kallel\Pictures\bmp.bmp
FlatComponentsList=true
AllowNoIcons=true
DefaultGroupName=ReVolt Live Lab\Track installer\
OutputBaseFilename=Track installer
DirExistsWarning=no
UninstallLogMode=overwrite
WizardSmallImageFile=C:\Users\kallel\Pictures\bmp2.bmp
WizardImageStretch=false
[Run]
Filename: {app}\Config\Configuration; WorkingDir: {app}; StatusMsg: Launching Configuration Application
