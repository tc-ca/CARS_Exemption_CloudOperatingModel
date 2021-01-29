# escape=`

FROM mcr.microsoft.com/windows/servercore/iis

# Stage files
RUN mkdir C:\deploy-files
RUN mkdir C:\CARS

# XCopy CARS web application
WORKDIR C:\
COPY .\CARS\ .\CARS\

WORKDIR C:\deploy-files
#COPY WebDeploy_amd64_en-US.msi .
COPY .\ConfigureImage.ps1 .\ConfigureImage.ps1
COPY .\clf-module-bin\ .\clf-module-bin\
COPY .\clf-module-bin\gcwu-fegc\ .\clf-module-bin\gcwu-fegc\
COPY .\clf-module-bin\msi2xml\ .\clf-module-bin\msi2xml\
#COPY .\webdeploy\ .\WebDeploy_amd64_en-US.msi

# Install Widnows features, Web Deploy, and start services
RUN powershell .\ConfigureImage.ps1

# Install modules
WORKDIR C:\deploy-files\clf-module-bin
RUN powershell .\InstallModules.ps1

# Remove deployment files (optional)
#WORKDIR C:\
#RUN powershell -NoProfile -Command Remove-Item -Recurse C:\deploy-files\*

# Entry point
EXPOSE 80
