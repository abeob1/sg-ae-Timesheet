cd C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin
Set My_Path=%~dp0	
wsdl /l:CS /o:"C:\Transaction.cs" http://localhost:33885/Transaction.asmx?wsdl