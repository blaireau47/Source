########################################################################################
## Windows PowerShell script pour l'ajout et le déploiement de la solution
##
## Arguments:
##   solution    --.Fichier WSP de la solution.
##   webApp	 -- URL de l'application Web (sans / à la fin).
## 
## Exemple: 
##   Deployer MonPackage.wsp https://monsite
#########################################################################################

param($solution, $webApp)

Set-StrictMode -version 2.0
# Variables de déploiement
[bool]$argErr = $false
[int]$sleepTime = 5     # Sleep for five seconds.

if ($solution -eq $null) 
{ 
    $argErr = $true 
}
else
{
    $packagePath = (Convert-Path (Get-Location -PSProvider FileSystem))+"\"
    $inFile = New-Object System.IO.FileInfo $packagePath$solution
    
    if (-not $infile.Exists)
    {
        Write-Error -message "Le fichier WSP n'existe pas : $solution" -category InvalidArgument
        $argErr = $true 
    }
}

if ($webApp -eq $null) 
{ 
    $argErr = $true 
}


if ($argErr)
{ 
    Write-Host "Usage: Deployer solution webApp"
    Write-Host "Exemple: `n`t Deployer MonPackage.wsp https://sousdomaine.domaine.com"
    exit
}


$fileName = $inFile.Name

    # Install the solution.    
    # Set-PSDebug -Step

    Write-Host "Install-SPSolution pour la solution $solution ..."
    Install-SPSolution -Identity:$solution -WebApplication $webApp -GACDeployment -force

    .\Wait4Timer.ps1 $solution


    # Set-PSDebug -Off

Write-Host "Installation complétée pour la solution $solution"