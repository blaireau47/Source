########################################################################################
## Windows PowerShell script pour supprimer la solution
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
    Write-Host "Usage: Supprimer solution.wsp"
    exit
}


    # Remove the solution.
    
    # Set-PSDebug -Step
    
    Write-Host "Remove-SPSolution pour la solution $solution ..."     
    Remove-SPSolution -Identity $solution -Confirm:$false
    Start-Sleep -Seconds $sleepTime
    
    # Restart services.
    Write-Host "Redémarrage des services ..."

    Restart-Service -DisplayName "SharePoint 2010 Timer"
    Start-Sleep -Seconds $sleepTime
    
    Write-Host "Redémarrage IIS ..."
    IISReset
    

Write-Host "Suppression complétée pour la solution $solution"