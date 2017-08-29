########################################################################################
## Windows PowerShell script pour rétracter la solution
##
## Arguments:
##   solution    --.Fichier WSP de la solution.
#########################################################################################

param($solution)

Set-StrictMode -version 2.0
# Variables de déploiement
[int]$sleepTime = 5     # Sleep for five seconds.

if ($solution -eq $null) 
{ 
    Write-Host "Usage: Retracter solution.wsp"
    exit
}


    # Remove the solution.
    
    # Set-PSDebug -Step
    
    Write-Host "Uninstall-SPSolution pour la solution $solution ..."
    Uninstall-SPSolution -Identity:$solution -AllWebApplication -Confirm:$false
    
    .\Wait4Timer.ps1 $solution
    

Write-Host "Désinstallation complétée pour la solution $solution"