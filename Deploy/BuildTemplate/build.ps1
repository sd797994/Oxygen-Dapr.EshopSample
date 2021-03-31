$ctx=Read-Host "Enter BoundContext Name"
$Svc=Read-Host "Enter Domain Name"
Remove-Item -Path tmp -Recurse
New-Item -Path . -Name tmp -ItemType Directory -Force
Copy-Item %ctxplaceholder%Service tmp/ -Recurse
Copy-Item ApplicationService tmp/ -Recurse
Copy-Item Domain tmp/ -Recurse
Copy-Item Host tmp/ -Recurse
Copy-Item Infrastructure tmp/ -Recurse
Copy-Item Admin-Vue tmp/ -Recurse
$newname='%ctxplaceholder%Service'.Replace('%ctxplaceholder%',$ctx)
Rename-Item -Path tmp/%ctxplaceholder%Service -NewName $newname
Get-ChildItem -Path tmp -Recurse -Include *.cs,*.csproj,*.json,*.vue,*.js | Rename-Item -NewName { $_.Name -replace '%placeholder%',$Svc }
get-childitem -path tmp -recurse -force -Include *.cs,*.csproj,*.json,*.vue,*.js | foreach-object -process {
$content=Get-Content $_.fullname
Set-Content $_.fullname -Value $content.Replace('%ctxplaceholder%',$ctx).Replace('%placeholder%',$Svc) -Encoding "UTF8"
}
Read-Host