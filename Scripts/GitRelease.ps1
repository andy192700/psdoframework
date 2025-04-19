using namespace System.Text;
using module ".\lib\VersionCalculator.psm1";

[System.Version] $readmeVersion = [VersionCalculator]::GetLatestReadMe();
[System.Version] $latestVersion = [System.Version]::Parse([VersionCalculator]::GetLatest("PSGallery"));

[string] $version = $latestVersion.ToString();

Write-Host "Creating release $version";

git config --global user.name "github-actions";
git config --global user.email "github-actions@github.com";
git tag -a $version -m "Release $version";
git push origin --tags;

[StringBuilder] $sb = [StringBuilder]::new();

if ($latestVersion -gt $readmeVersion) {
    $sb.Append("Patch release $latestVersion for official version $readmeVersion.")
    $sb.AppendLine();
}

$sb.Append("Check out the [Release Notes](https://github.com/andy192700/psdoframework/tree/main/Documentation/ReleaseNotes/v$($readmeVersion).md) ");
$sb.Append("and view the published Module on the [PowerShell Gallery](https://www.powershellgallery.com/packages/PSDoFramework/$version).");

gh release create $version --title "Release $version" --notes $sb.ToString();
    