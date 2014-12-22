@echo on
"C:\Program Files (x86)\WiX Toolset v3.9\bin\heat.exe" dir ..\bin\Release -sreg -scom -sfrag -srd -gg -cg BassGuitarTrainer -var var.SourceDir -t transform.xslt -out files2.wxs