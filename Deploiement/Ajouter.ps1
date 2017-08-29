########################################################################################
## Windows PowerShell script pour l'ajout de la solution
##
## Arguments:
##   solution    --.Fichier WSP de la solution.
#########################################################################################

param($solution)

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

if ($argErr)
{ 
    Write-Host "Usage: Ajouter solution.wsp"
    exit
}


$fileName = $inFile.Name

    # Install the solution.    
    # Set-PSDebug -Step

    Write-Host "Add-SPSolution pour la solution $fileName ..."
    Add-SPSolution -LiteralPath $inFile.FullName -Confirm:$false
    Start-Sleep -s $sleepTime
    

    # Set-PSDebug -Off

Write-Host "Ajout complété pour la solution $solution"