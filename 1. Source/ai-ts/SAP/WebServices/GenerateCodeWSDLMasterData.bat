cd C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin
Set My_Path=%~dp0
wsdl /l:CS /o:"%My_Path%\MasterData.cs" http://localhost:33885/MasterData.asmx?wsdl
