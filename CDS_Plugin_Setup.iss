;------------------------------------------------------------------------------
;
;       ������ ������������� ������� ��� Inno Setup 5.5.5
;       (c) maisvendoo, 15.04.2015
;
;------------------------------------------------------------------------------

;------------------------------------------------------------------------------
;   ���������� ��������� ���������
;------------------------------------------------------------------------------

; ��� ����������
#define   Name       "CDS_Plugin"
; ������ ����������
#define   Version    "1.2.0"
; �����-�����������
#define   Publisher  "by Nenashkin for CDS"
; ���� ����� ������������
//#define   URL        "http://www.miramishi.com"
; ��� ������������ ������
#define   ExeName    "CDS Plugin_N.exe"

;------------------------------------------------------------------------------
;   ��������� ���������
;------------------------------------------------------------------------------
[Setup]

; ���������� ������������� ����������, 
;��������������� ����� Tools -> Generate GUID
AppId={{EC18EE3B-44EF-4F64-9B02-1B329241702B}

; ������ ����������, ������������ ��� ���������
AppName={#Name}
AppVersion={#Version}
AppPublisher={#Publisher}
//AppPublisherURL={#URL}
//AppSupportURL={#URL}
//AppUpdatesURL={#URL}

; ���� ��������� ��-���������
DefaultDirName=C:\Program Files\Autodesk\Navisworks Manage 2022\Plugins\{#Name}
; ��� ������ � ���� "����"
DefaultGroupName={#Name}

; �������, ���� ����� ������� ��������� setup � ��� ������������ �����
OutputDir=C:\Work\Navisworks\App\CDS_Plugin
OutputBaseFileName=test-setup

; ���� ������
SetupIconFile=C:\Work\Navisworks\App\CDS_Plugin\Graphics ICO\CDS_man.ico

; ��������� ������
Compression=lzma
SolidCompression=yes

;------------------------------------------------------------------------------
;   ������������� ����� ��� �������� ���������
;------------------------------------------------------------------------------
[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "ru"; MessagesFile: "compiler:Languages\Russian.isl"


;------------------------------------------------------------------------------
;   �����, ������� ���� �������� � ����� �����������
;------------------------------------------------------------------------------
[Files]

; ����������� ����
//Source: "C:\Work\Navisworks\App\CDS_Plugin\CDS_Plugin"; DestDir: "{app}"; Flags: ignoreversion

; ������������� �������
Source: "C:\Work\Navisworks\App\CDS_Plugin\CDS_Plugin\For setup\CDS_Plugin\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

