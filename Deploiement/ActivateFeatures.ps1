########################################################################################
## Windows PowerShell script pour l'activation des fonctionnalit�s
##
## Arguments:
##   siteCollUrl 	-- url de la collection de site (mettre / � la fin si racine).
##   featurePrefix	-- pr�fixe pour le nom des features
##   featureList	-- liste des features � activer d�limit�s par une virgule
#########################################################################################

param($siteCollUrl, $featurePrefix, $listeFeatureNames)

Set-StrictMode -version 2.0
# Variables de d�ploiement
[int]$sleepTime = 5     # Sleep for five seconds.


if ($siteCollUrl -eq $null) 
{ 
    Write-Host "`nUsage: ActivateFeatures siteCollUrl featurePrefix listFeatureNames`n"
    exit
}


# Set-PSDebug -Step

# Liste des features � activer dans l'ordre d'activation
$listeFeatureNamesArray = ($listeFeatureNames).split(",")
   
foreach ($featureName in $listeFeatureNamesArray)
{
	Write-Host 'Activation de la fonctionnalit� :' $featureName 
	Enable-SPFeature $featurePrefix$featureName -Confirm:$false -url:$siteCollUrl �force
}

Start-Sleep -s $sleepTime

# Set-PSDebug -Off
