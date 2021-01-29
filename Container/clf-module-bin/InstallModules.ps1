# Start recording transcript
$startTranscript = Start-Transcript
Write-Output "Starting script [$($MyInvocation.MyCommand.Source)]"

# Patch the streamtagfilter module
Write-Output "------------------------------------------------------------"
Write-Output "Patching streamtagfiltersetup.msi with updated Setup.dll file"
Write-Output ""
Start-Process -Wait -FilePath ".\msi2xml\msi2xml.exe" -ArgumentList "-c","InstallerFiles","streamtagfiltersetup.msi"
Copy-Item -Path "Setup-no-user-interface.dll" -Destination ".\InstallerFiles\_ED1B7410A8B2866D347A3C74B0665222"
Start-Process -Wait -FilePath ".\msi2xml\xml2msi.exe" -ArgumentList "-m","streamtagfiltersetup.xml"

<#
    The xml2msi utility has the odd behavior when recreating the installer
    of uppercasing the msi extension generated for the finished file name.
    E.g. "streamtagfiltersetup.MSI" instead of "streamtagFiltersetup.msi".
    This, in turn, causes an issue when this PowerShell script runs
    Start-Process "msiexec.exe" resulting in installer failing with no
    useful information in the .log file. As a result, we have to perform
    the step of renaming the .MSI to .msi in the next command so that the
    msiexec.exe command to install is successful. Since this
    is a Windows-based container, case differences in file names should not
    make any difference, but apparently in this case they do.
#>
Rename-Item -Path .\streamtagfiltersetup.MSI -NewName .\streamtagfiltersetup.msi

# Maximum wait time (in seconds) for installer completion
$timeout = 180

# Install streamtagfilter module
Write-Output "------------------------------------------------------------"
Write-Output "Installing streamtagfiltersetup.msi"
Write-Output ""
Start-Process -Wait -FilePath "msiexec.exe" -ArgumentList "/package","streamtagfiltersetup.msi","/quiet","/log","streamtagfiltersetup.log"
<#
$process = Start-Process -FilePath "msiexec.exe" -ArgumentList "/package","streamtagfiltersetup.msi","/quiet","/log","streamtagfiltersetup.log" -PassThru
Wait-Process -InputObject $process -Timeout $timeout
Stop-Process $process
#>

# Install CLFPagePlugin module
Write-Output "------------------------------------------------------------"
Write-Output "Installing CLFPagePluginSetup.msi"
Write-Output ""
Start-Process -Wait -FilePath "msiexec.exe" -ArgumentList "/package","CLFPagePluginSetup.msi","/quiet","/log","CLFPagePluginSetup.log"
<#
$process = Start-Process -FilePath "msiexec.exe" -ArgumentList "/package","CLFPagePluginSetup.msi","/quiet","/log","CLFPagePluginSetup.log" -PassThru
Wait-Process -InputObject $process -Timeout $timeout
Stop-Process $process
#>

# Install Access Database engine 
Write-Output "------------------------------------------------------------"
Write-Output "Installing accessdatabaseengine_X64.exe"
Write-Output ""
Start-Process -Wait -FilePath "accessdatabaseengine_X64.exe" -ArgumentList "/quiet"


# Stop recording transcript
$stopTranscript = Stop-Transcript
Write-Output "`ttranscript output is located in: $($startTranscript.Path)`n"
