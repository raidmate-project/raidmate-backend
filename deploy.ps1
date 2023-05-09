<#
.SYNOPSIS
  Deploys the stack.
.PARAMETER profile
  Mandatory. Specifies the AWS profile to use.
#>

[CmdletBinding()]
param (
	[Parameter(Mandatory = $true)]
	[string]$clientId,
	[Parameter(Mandatory = $true)]
	[string]$clientSecret,
	[Parameter(Mandatory = $true)]
	[string]$profile
)

$exitCodes = @();

dotnet tool restore

dotnet lambda package -pl src\RaidmateBackend\ -o output\RaidmateBackend.zip

$exitCodes += $LASTEXITCODE

foreach ($exitCode in $exitCodes) {
	if ($exitCode -ne 0) {
		exit 1;
	}
}

cdk deploy --context clientId=$clientId --context clientSecret=$clientSecret --profile $profile

