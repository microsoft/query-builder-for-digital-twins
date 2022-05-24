param(
    [Parameter(Mandatory=$true)] [string]$clientId,
    [Parameter(Mandatory=$true)] [string]$workspace,
    [Parameter(Mandatory=$true)] [string]$subscriptionId,
    [Parameter(Mandatory=$true)] [string]$storage_name,
    [Parameter(Mandatory=$true)] [string]$container_name,
    [Parameter(Mandatory=$true)] [string]$vault_name,
    [Parameter(Mandatory=$true)] [string]$aad_cert,
    [Parameter(Mandatory=$true)] [string]$sign_cert,
    [Parameter(Mandatory=$true)] [string]$signing_cert_fingerprint,
    [Parameter(Mandatory=$false)] [switch]$user_login
)

$fileName = (Get-ChildItem -Recurse -Filter *.nupkg | Select-Object -Property Name -First 1).Name
if ($fileName -match ' ') {
    # This accounts for cases where it finds the same package and we end up with $fileName = "proj.1.1.0.nupkg proj.1.1.0.nupkg"
    $fileName = $fileName.split()[0]
}

if ([string]::IsNullOrWhiteSpace($fileName)) {
    throw "Unable to find nupkg for signing. Ensure the 'dotnet pack' command has been run and that it's output to a directory called 'unsigned'."
}

Write-Host "Found nupkg: $fileName"
if ($user_login) {
    Write-Host 'Logging into Azure.'
    az login
    az account set --subscription $subscriptionId
}

if (Test-Path 'signed') {
    Write-Host "'signed' directory already exists."
} else {
    mkdir signed
    Write-Host "'signed' directory created successfully."
}

Write-Host "Generating 'auth.json' and 'input.json' files for ESRP Client."
$authJson = @"
{
    "Version": "1.0.0",
    "AuthenticationType": "AAD_CERT",
    "TenantId": "72f988bf-86f1-41af-91ab-2d7cd011db47",
    "ClientId": "$clientId",
    "AuthCert": {
        "SubjectName": "CN=$clientId.microsoft.com",
        "StoreLocation": "LocalMachine",
        "StoreName": "My",
        "SendX5c": "true"
    },
    "RequestSigningCert": {
        "SubjectName": "CN=$clientId",
        "StoreLocation": "LocalMachine",
        "StoreName": "My"
    }
}
"@
$inputJson = @"
{
    "Version": "1.0.0",
    "SignBatches": [
        {
            "SourceLocationType": "UNC",
            "SourceRootDirectory": "$workspace\\unsigned",
            "DestinationLocationType": "UNC",
            "DestinationRootDirectory": "$workspace\\signed",
            "SignRequestFiles": [
                {
                    "SourceLocation": "$fileName",
                    "DestinationLocation": "$fileName"
                }
            ],
            "SigningInfo": {
                "Operations": [
                    {
                        "KeyCode": "CP-401405",
                        "OperationCode": "NuGetSign",
                        "ToolName": "sign",
                        "ToolVersion": "1.0"
                    },
                    {
                        "KeyCode": "CP-401405",
                        "OperationCode": "NuGetVerify",
                        "ToolName": "sign",
                        "ToolVersion": "1.0"
                    }
                ]
            }
        }
    ]
}
"@
Out-File -FilePath .\auth.json -InputObject $authJson
Out-File -FilePath .\input.json -InputObject $inputJson
Write-Host 'Done.'
try {
    Write-Host 'Downloading ESRP Client.'
    az storage blob download --auth-mode login --subscription  $subscriptionId --account-name $storage_name -c $container_name -n microsoft.esrpclient.1.2.76.zip -f esrp.zip
    if (Test-Path 'esrp.zip') {
        Write-Host 'Done.'
    } else {
        throw 'Download did not complete successfully. This is likely due to an access issue.'
    }
    
    Write-Host 'Unzipping ESRP Client.'
    Expand-Archive -Path 'esrp.zip' -DestinationPath './esrp' -Force
    Write-Host 'Done.'
    Write-Host 'Downloading & Installing Certifictes.'
    Remove-Item cert.pfx -ErrorAction SilentlyContinue
    az keyvault secret download --subscription $subscriptionId --vault-name $vault_name --name $aad_cert -f cert.pfx
    certutil -silent -f -importpfx cert.pfx
    Remove-Item cert.pfx
    az keyvault secret download --subscription $subscriptionId --vault-name $vault_name --name $sign_cert -f cert.pfx
    certutil -silent -f -importpfx cert.pfx
    Remove-Item cert.pfx
    Write-Host 'Done.'
    Write-Host 'Executing ESRP Client.'
    ./esrp/tools/EsrpClient.exe sign -a ./auth.json -p ./esrp/tools/Policy.json -c ./esrp/tools/Config.json -i ./input.json -o ./Output.json -l Verbose -f STDOUT
    Write-Host 'Done. Signing Complete.'
    Write-Host 'Verifying signatures with NuGet.'
    $result = nuget verify -Signatures signed/$fileName -CertificateFingerprint $signing_cert_fingerprint
    Write-Host $result
    $anyMatches = $result | Where-Object { $_ -match 'Package signature validation failed.'}
    if ([string]::IsNullOrWhiteSpace($anyMatches)) {
        Write-Host 'Done. Signatures verified.'
        Write-Host 'Package ready for upload.'
    } else {
        throw 'Package signature validation failed.'
    }
} catch {
    throw
} finally {
    if ($user_login) {
        az logout
    }
}