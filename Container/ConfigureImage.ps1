# Start recording transcript
$startTranscript = Start-Transcript
Write-Output "Starting script [$($MyInvocation.MyCommand.Source)]"

# Install Windows Features
Write-Output "------------------------------------------------------------"
Write-Output "Installing Windows features"
Write-Output ""
Install-WindowsFeature Web-Mgmt-Service
Install-WindowsFeature Web-Windows-Auth
Install-WindowsFeature NET-Framework-45-ASPNET
Install-WindowsFeature Web-Asp-Net45
Install-WindowsFeature NET-WCF-HTTP-Activation45
Install-WindowsFeature Web-Scripting-Tools
Install-WindowsFeature Web-Http-Tracing

# Maximum wait time (in seconds) for installer completion
$timeout = 180

# Download WebDeploy
Write-Output "------------------------------------------------------------"
Write-Output "Downloading WebDeploy installer from https://www.iis.net/downloads/microsoft/web-deploy"
Write-Output ""
$url = "https://download.microsoft.com/download/0/1/D/01DC28EA-638C-4A22-A57B-4CEF97755C6C"
$file = "WebDeploy_amd64_en-US.msi"
Start-Process -Wait -FilePath "cmd.exe" -ArgumentList "/c","curl.exe","-L","-O","$url/$file"

# Install WebDeploy
Write-Output "------------------------------------------------------------"
Write-Output "Installing WebDeploy"
Write-Output ""
$process = Start-Process -FilePath "msiexec.exe" -ArgumentList "/log","WebDeploy_amd64_en-US.log","/i","WebDeploy_amd64_en-US.msi","AGREETOLICENSE=yes","ADDLOCAL=ALL","/qn" -PassThru
Wait-Process -InputObject $process -Timeout $timeout
Stop-Process $process

# Start WebDeploy
Write-Output "------------------------------------------------------------"
Write-Output "Starting WebDeploy"
Write-Output ""
Start-Service MsDepSvc

# Configure web management services
Write-Output "------------------------------------------------------------"
Write-Output "Configuring web management service as autorun, starting it, and enabling remote management"
Write-Output ""
Set-Service -Name "WMSvc" -StartupType automatic
Start-Service -Name "WMSvc"
Set-ItemProperty -Path HKLM:\SOFTWARE\Microsoft\WebManagement\Server -Name EnableRemoteManagement -Value 1

#Add user for Remote IIS Manager Login
Write-Output "------------------------------------------------------------"
Write-Output "Add user for Remote IIS Manager Login"
Write-Output ""
Start-Process -Wait -FilePath "net" -ArgumentList "user","iisadmin","Password~1234","/add"
Start-Process -Wait -FilePath "net" -ArgumentList "localgroup","administrators","iisadmin","/add"

#Create CARS Site
Write-Output "------------------------------------------------------------"
Write-Output "Create the CARS website"
Write-Output ""
Remove-Website -Name 'Default Web Site'; 
New-IISSite -Name "CARS"  -PhysicalPath 'c:\CARS' -BindingInformation "*:80:";

#Set Folder permission
Write-Output "--------------------------
Write-Output "Set the CARS folder permissons for everyone to access (may not need)"
Write-Output ""
$sharepath = "C:\CARS"
$Acl = Get-ACL $SharePath
$AccessRule= New-Object System.Security.AccessControl.FileSystemAccessRule("everyone","FullControl","ContainerInherit,Objectinherit","none","Allow")
$Acl.AddAccessRule($AccessRule)
Set-Acl $SharePath $Acl

# Stop recording transcript
$stopTranscript = Stop-Transcript
Write-Output "`ttranscript output is located in: $($startTranscript.Path)`n"
