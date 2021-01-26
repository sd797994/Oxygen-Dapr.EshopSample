$Svc=Read-Host "Enter Service Name"
Remove-Item -Path tmp -Recurse
New-Item -Path . -Name tmp -ItemType Directory -Force
Copy-Item %placeholder%Service tmp/ -Recurse
Copy-Item ApplicationService tmp/ -Recurse
Copy-Item Domain tmp/ -Recurse
Copy-Item Host tmp/ -Recurse
Copy-Item Infrastructure tmp/ -Recurse
Copy-Item Admin-Vue tmp/ -Recurse
$newname='%placeholder%Service'.Replace('%placeholder%',$Svc)
Rename-Item -Path tmp/%placeholder%Service -NewName $newname
Get-ChildItem -Path tmp -Recurse -Include *.cs,*.csproj,*.json,*.vue,*.js | Rename-Item -NewName { $_.Name -replace '%placeholder%',$Svc }
get-childitem -path tmp -recurse -force -Include *.cs,*.csproj,*.json,*.vue,*.js | foreach-object -process {
$content=Get-Content $_.fullname
Set-Content $_.fullname -Value $content.Replace('%placeholder%',$Svc)
}
Read-Host