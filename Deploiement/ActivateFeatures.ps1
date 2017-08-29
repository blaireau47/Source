########################################################################################
## Windows PowerShell script pour l'activation des fonctionnalités
##
## Arguments:
##   siteCollUrl 	-- url de la collection de site (mettre / à la fin si racine).
##   featurePrefix	-- préfixe pour le nom des features
##   featureList	-- liste des features à activer délimités par une virgule
#########################################################################################

param($siteCollUrl, $featurePrefix, $listeFeatureNames)

Set-StrictMode -version 2.0
# Variables de déploiement
[int]$sleepTime = 5     # Sleep for five seconds.


if ($siteCollUrl -eq $null) 
{ 
    Write-Host "`nUsage: ActivateFeatures siteCollUrl featurePrefix listFeatureNames`n"
    exit
}


# Set-PSDebug -Step

# Liste des features à activer dans l'ordre d'activation
$listeFeatureNamesArray = ($listeFeatureNames).split(",")
   
foreach ($featureName in $listeFeatureNamesArray)
{
	Write-Host 'Activation de la fonctionnalité :' $featureName 
	Enable-SPFeature $featurePrefix$featureName -Confirm:$false -url:$siteCollUrl –force
}

Start-Sleep -s $sleepTime

# Set-PSDebug -Off
