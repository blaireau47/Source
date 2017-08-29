########################################################################################
## Windows PowerShell script pour attendre l'exécution d'une tâche de déploiement de 
## solution SharePoint.
##
## Arguments:
##   solution    --.Fichier WSP de la solution.
#########################################################################################

param($solution)

Set-StrictMode -version 2.0

if ($solution -eq $null) 
{ 
    Write-Host "Usage: Wait4Timer solution.wsp"
    exit
}

    Write-Host -NoNewLine "`nRecherche de la tâche du minuteur"
    
    while (($jd = Get-SPTimerJob | ?{ $_.Name -like "*solution-deployment*" + $solution + "*" }) -eq $null) 
    {
        Write-Host -NoNewLine .
        Start-Sleep -Seconds 1
    }
    $jdName = $jd.Name
    Write-Host "`nTâche : $jdName"
    Write-Host -NoNewLine Attente
    
    while ((Get-SPTimerJob $jdName) -ne $null) 
    {
       Write-Host -NoNewLine .
       Start-Sleep -Seconds 1
    }
    
    Write-Host
    $jd.HistoryEntries | %{ Write-Host Résultat de la tâche : $_.Status }
    Write-Host
