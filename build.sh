#!/bin/sh

dll_compilation_string="mcs /target:library /out:bin/Hexicore.dll " #haha
final_compilation_string="mcs /reference:bin/Hexicore.dll src/StartUp.cs /out:bin/Hexity.exe"
success_msg="Compilation Succeeded."

for i in `ls src/*.cs`
  do
  if [ ${i} != "src/StartUp.cs" ]
    then
    dll_compilation_string=$dll_compilation_string${i}" " 
  fi
done

echo Building Hexicore.dll...
$dll_compilation_string
echo Building Application Runtime...
$final_compilation_string