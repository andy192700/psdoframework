using module ".\lib\VersionCalculator.psm1";

[string] $version = [VersionCalculator]::GetLatest("PSGallery");

Write-Host "Creating release $version";

git config --global user.name "github-actions";
git config --global user.email "github-actions@github.com";
git tag -a $version -m "Release $version";
git push origin --tags;