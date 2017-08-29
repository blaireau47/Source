$solution = "SansPapier.Variation.Portail.wsp"
$webApp = "http://qc2022:14087"

$siteColl = "/"
$featurePrefix = "SansPapier.Variation.Portail_SansPapier."
$listeFeatureNames= "Columns,ContentTypes,MasterPages,PageLayouts,VisualWebParts,Lists.Definitions,Configuration"

.\Ajouter.ps1 $solution
.\Deployer.ps1 $solution $webApp
.\ActivateFeatures.ps1 $webApp$siteColl $featurePrefix $listeFeatureNames
