########################################################################################
## Windows PowerShell script pour la d�sactivation des fonctionnalit�s
##
## Arguments:
##   siteCollUrl 	-- url de la collection de site (mettre / � la fin si racine).
##   featurePrefix	-- pr�fixe pour le nom des features
##   featureList	-- liste des features � d�sactiver d�limit�s par une virgule
#########################################################################################

param($siteCollUrl, $featurePrefix, $featureList)

Set-StrictMode -version 2.0
# Variables de d�ploiement
[int]$sleepTime = 5     # Sleep for five seconds.


if ($siteCollUrl -eq $null) 
{ 
    Write-Host "`nUsage: DeactivateFeatures siteCollUrl featurePrefix featureList`n"
    exit
}


    # Retract the solution.
    
    # Set-PSDebug -Step
    
	# Liste des features � d�sactiver dans l'ordre de d�sactivation
	$listeFeatureNamesArray = ($featureList).split(",")

	foreach ($featureName in $listeFeatureNamesArray)
	{
		Write-Host 'D�sactivation de la fonctionnalit� :' $featureName 
		Disable-SPFeature $featurePrefix$featureName -Confirm:$false -Url:$siteCollUrl -Force
	}
    Start-Sleep -Seconds $sleepTime

# Set-PSDebug -Off
