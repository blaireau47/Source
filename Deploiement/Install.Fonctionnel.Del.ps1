$solution = "SansPapier.Variation.Portail.wsp"
$webApp = "http://qc2022:14087"

$siteColl = "/"
$featurePrefix = "SansPapier.Variation.Portail_SansPapier."
$featureListNamesDeactivate = "Columns,ContentTypes,MasterPages,PageLayouts,VisualWebParts,Lists.Definitions,Configuration"

.\DeactivateFeatures.ps1 $webApp$siteColl $featurePrefix $featureListNamesDeactivate
.\Retracter.ps1 $solution
.\Supprimer.ps1 $solution
