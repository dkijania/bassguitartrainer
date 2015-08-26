@echo on
"C:\Program Files (x86)\WiX Toolset v3.10\bin\heat.exe" dir ..\Bin\Release -sreg -scom -sfrag -srd -gg -cg BassGuitarTrainer -var var.SourceDir -t transform.xslt -out files3.wxs