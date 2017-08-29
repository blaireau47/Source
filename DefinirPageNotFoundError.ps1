$spsite = Get-SPSite {urlSite}
$spsite.FileNotFoundUrl = "/Pages/pagenotfounderror.aspx"