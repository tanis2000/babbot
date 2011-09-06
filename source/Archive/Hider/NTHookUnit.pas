(*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*)

unit NTHookUnit;

interface
uses
  Classes, Windows, SysUtils, Messages;

type
  TImportCode = packed record
     JumpInstruction: Word;
     AddressOfPointerToFunction: PPointer;
  end;

  PImage_Import_Entry = ^Image_Import_Entry;
  Image_Import_Entry = record
    Characteristics: DWORD;
    TimeDateStamp: DWORD;
    MajorVersion: Word;
    MinorVersion: Word;
    Name: DWORD;
    LookupTable: DWORD;
  end;

  PImportCode = ^TImportCode;
  TLongJmp = packed record
     JmpCode: ShortInt;
     FuncAddr: DWORD;
  end;

  THookClass = class
  private
     Trap:boolean;
     hProcess: Cardinal;
     AlreadyHook:boolean;
     AllowChange:boolean;
     Oldcode: array[0..4]of byte;
     Newcode: TLongJmp;
  private
  public
     OldFunction,NewFunction:Pointer;
     constructor Create(IsTrap:boolean;OldFun,NewFun:pointer);
     destructor Destroy;
     procedure Restore;
     procedure Change;
  published
  end;

implementation

(*----------------------------------------------------------------------------*)
function FinalFunctionAddress(Code: Pointer): Pointer;
var
  func: PImportCode;
begin
  Result:=Code;
  if Code=nil then exit;
  try
    func:=code;
    if (func.JumpInstruction = $25FF) then
      Func:=func.AddressOfPointerToFunction^;
    result:=Func;
  except
    Result:=nil;
  end;
end;

(*----------------------------------------------------------------------------*)
function PatchAddressInModule(BeenDone:Tlist;hModule: THandle; OldFunc,NewFunc: Pointer):integer;
const
   SIZE=4;
Var
   Dos: PImageDosHeader;
   NT: PImageNTHeaders;
   ImportDesc: PImage_Import_Entry;
   rva: DWORD;
   Func: PPointer;
   DLL: String;
   f: Pointer;
   written: DWORD;
   mbi_thunk:TMemoryBasicInformation;
   dwOldProtect:DWORD;
begin
  Result:=0;
  if hModule=0 then exit;
  Dos:=Pointer(hModule);
  if BeenDone.IndexOf(Dos)>=0 then exit;
  BeenDone.Add(Dos);
  OldFunc:=FinalFunctionAddress(OldFunc);
  if IsBadReadPtr(Dos,SizeOf(TImageDosHeader)) then exit;
  if Dos.e_magic <> IMAGE_DOS_SIGNATURE then exit;
  NT := Pointer(Integer(Dos) + dos._lfanew);
  RVA:=NT^.OptionalHeader.
     DataDirectory[IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress;
  if RVA=0 then exit;
  ImportDesc := pointer(DWORD(Dos)+RVA);

  while (ImportDesc^.Name <> 0) do
  begin
    DLL:= PChar(DWORD(Dos)+ImportDesc^.Name);
    PatchAddressInModule(BeenDone,GetModuleHandle(PChar(DLL)),OldFunc,NewFunc);
    Func:= Pointer(DWORD(DOS)+ImportDesc.LookupTable);
    while (Func^ <> nil) do
    begin
      f:=FinalFunctionAddress(Func^);
      if f=OldFunc then
      begin
         VirtualQuery(Func,mbi_thunk, sizeof(TMemoryBasicInformation));
         VirtualProtect(Func,SIZE,PAGE_EXECUTE_WRITECOPY,mbi_thunk.Protect);
         WriteProcessMemory(GetCurrentProcess,Func,@NewFunc,SIZE,written);
         VirtualProtect(Func, SIZE, mbi_thunk.Protect,dwOldProtect);
      end;
      if Written=4 then Inc(Result);
      Inc(Func);
    end;
    Inc(ImportDesc);
  end;
end;

(*----------------------------------------------------------------------------*)
constructor THookClass.Create(IsTrap : boolean; OldFun,NewFun : pointer);
begin
   OldFunction:=FinalFunctionAddress(OldFun);
   NewFunction:=FinalFunctionAddress(NewFun);
   Trap:=IsTrap;
   if Trap then
   begin
      hProcess := OpenProcess(PROCESS_ALL_ACCESS,FALSE, GetCurrentProcessID());
      Newcode.JmpCode := ShortInt($E9);
      NewCode.FuncAddr := DWORD(NewFunction) - DWORD(OldFunction) - 5;
      Move(OldFunction^,OldCode,5);
      AlreadyHook := False;
   end;
   if not Trap then AllowChange:=true;
   Change;
   if not Trap then AllowChange:=false;
end;

(*----------------------------------------------------------------------------*)
destructor THookClass.Destroy;
begin
  if not Trap then AllowChange:=true;
  Restore;
  if Trap then
    CloseHandle(hProcess);
end;

(*----------------------------------------------------------------------------*)
procedure THookClass.Change;
var
   nCount: DWORD;
   BeenDone: TList;
begin
  if Trap then
    begin
      if (AlreadyHook)or (hProcess = 0) or (OldFunction = nil) or (NewFunction = nil) then
          exit;
      AlreadyHook:=true;
      WriteProcessMemory(hProcess, OldFunction, @(Newcode), 5, nCount);
    end
  else begin
    if (not AllowChange)or(OldFunction=nil)or(NewFunction=nil)then exit;
    BeenDone:=TList.Create;
    try
     PatchAddressInModule(BeenDone,GetModuleHandle(nil),OldFunction,NewFunction);
    finally
     BeenDone.Free;
    end;
  end;
end;

(*----------------------------------------------------------------------------*)
procedure THookClass.Restore;
var
   nCount: DWORD;
   BeenDone: TList;
begin
  if Trap then
    begin
      if (not AlreadyHook) or (hProcess = 0) or (OldFunction = nil) or (NewFunction = nil) then
          exit;
      WriteProcessMemory(hProcess, OldFunction, @(Oldcode), 5, nCount);
      AlreadyHook := False;
    end
  else begin
    if (not AllowChange)or(OldFunction=nil)or(NewFunction=nil)then exit;
    BeenDone := TList.Create;
    try
      PatchAddressInModule(BeenDone,GetModuleHandle(nil),NewFunction,OldFunction);
    finally
      BeenDone.Free;
    end;
  end;
end;

end.

