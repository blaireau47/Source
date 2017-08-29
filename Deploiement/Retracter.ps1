########################################################################################
## Windows PowerShell script pour r�tracter la solution
##
## Arguments:
##   solution    --.Fichier WSP de la solution.
#########################################################################################

param($solution)

Set-StrictMode -version 2.0
# Variables de d�ploiement
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
    

Write-Host "D�sinstallation compl�t�e pour la solution $solution"