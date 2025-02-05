using namespace System.Text;
using module ".\lib\VersionCalculator.psm1";

[string] $version = [VersionCalculator]::GetLatest("PSGallery");

Write-Host "Creating release $version";

git config --global user.name "github-actions";
git config --global user.email "github-actions@github.com";
git tag -a $version -m "Release $version";
git push origin --tags;

[StringBuilder] $sb = [StringBuilder]::new();

$sb.Append("Check out the [Release Notes](https://github.com/andy192700/psdoframework/tree/main/Documentation/ReleaseNotes/v$($version).md) ");
$sb.Append("and view the published Module on the [PowerShell Gallery](https://www.powershellgallery.com/packages/PSDoFramework/$version).");

gh release create $version --title "Release $version" --notes $sb.ToString();
    