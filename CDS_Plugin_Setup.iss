;------------------------------------------------------------------------------
;
;       Пример установочного скрипта для Inno Setup 5.5.5
;       (c) maisvendoo, 15.04.2015
;
;------------------------------------------------------------------------------

;------------------------------------------------------------------------------
;   Определяем некоторые константы
;------------------------------------------------------------------------------

; Имя приложения
#define   Name       "CDS_Plugin"
; Версия приложения
#define   Version    "1.2.0"
; Фирма-разработчик
#define   Publisher  "by Nenashkin for CDS"
; Сафт фирмы разработчика
//#define   URL        "http://www.miramishi.com"
; Имя исполняемого модуля
#define   ExeName    "CDS Plugin_N.exe"

;------------------------------------------------------------------------------
;   Параметры установки
;------------------------------------------------------------------------------
[Setup]

; Уникальный идентификатор приложения, 
;сгенерированный через Tools -> Generate GUID
AppId={{EC18EE3B-44EF-4F64-9B02-1B329241702B}

; Прочая информация, отображаемая при установке
AppName={#Name}
AppVersion={#Version}
AppPublisher={#Publisher}
//AppPublisherURL={#URL}
//AppSupportURL={#URL}
//AppUpdatesURL={#URL}

; Путь установки по-умолчанию
DefaultDirName=C:\Program Files\Autodesk\Navisworks Manage 2022\Plugins\{#Name}
; Имя группы в меню "Пуск"
DefaultGroupName={#Name}

; Каталог, куда будет записан собранный setup и имя исполняемого файла
OutputDir=C:\Work\Navisworks\App\CDS_Plugin
OutputBaseFileName=test-setup

; Файл иконки
SetupIconFile=C:\Work\Navisworks\App\CDS_Plugin\Graphics ICO\CDS_man.ico

; Параметры сжатия
Compression=lzma
SolidCompression=yes

;------------------------------------------------------------------------------
;   Устанавливаем языки для процесса установки
;------------------------------------------------------------------------------
[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "ru"; MessagesFile: "compiler:Languages\Russian.isl"


;------------------------------------------------------------------------------
;   Файлы, которые надо включить в пакет установщика
;------------------------------------------------------------------------------
[Files]

; Исполняемый файл
//Source: "C:\Work\Navisworks\App\CDS_Plugin\CDS_Plugin"; DestDir: "{app}"; Flags: ignoreversion

; Прилагающиеся ресурсы
Source: "C:\Work\Navisworks\App\CDS_Plugin\CDS_Plugin\For setup\CDS_Plugin\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

