########################################################################################
## Windows PowerShell script pour la désactivation des fonctionnalités
##
## Arguments:
##   siteCollUrl 	-- url de la collection de site (mettre / à la fin si racine).
##   featurePrefix	-- préfixe pour le nom des features
##   featureList	-- liste des features à désactiver délimités par une virgule
#########################################################################################

param($siteCollUrl, $featurePrefix, $featureList)

Set-StrictMode -version 2.0
# Variables de déploiement
[int]$sleepTime = 5     # Sleep for five seconds.


if ($siteCollUrl -eq $null) 
{ 
    Write-Host "`nUsage: DeactivateFeatures siteCollUrl featurePrefix featureList`n"
    exit
}


    # Retract the solution.
    
    # Set-PSDebug -Step
    
	# Liste des features à désactiver dans l'ordre de désactivation
	$listeFeatureNamesArray = ($featureList).split(",")

	foreach ($featureName in $listeFeatureNamesArray)
	{
		Write-Host 'Désactivation de la fonctionnalité :' $featureName 
		Disable-SPFeature $featurePrefix$featureName -Confirm:$false -Url:$siteCollUrl -Force
	}
    Start-Sleep -Seconds $sleepTime

# Set-PSDebug -Off
