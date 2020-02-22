docker build -t lanfeust-bridge .

# variables from user-secrets
if ( !$Env:ASPNETCORE_Authentication__Google__ClientId)
{
    foreach ($line in (dotnet user-secrets list))
    {
        $a = $line.Split(' = ')
        $k = 'ASPNETCORE_' + ($a[0] -replace ':', '__')
        Set-Item Env:/$k $a[1]
    }
}

# variables for https certificate
$cert = Get-ChildItem Cert:/CurrentUser/My |
    Where-Object { $_.FriendlyName -eq 'ASP.NET Core HTTPS development certificate'} |
    Sort-Object -Property NotAfter | Select-Object -Last 1
$password = New-Guid
$bytes = $cert.Export('Pfx', $password)
if (!(Test-Path cert)) { New-Item -Type Directory cert }
$certDir = Join-Path (Get-Location) cert
[IO.File]::WriteAllBytes((Join-Path $certDir dev-cert.pfx), $bytes)
$Env:ASPNETCORE_Kestrel__Certificates__Default__Password = $password

docker run -d -e ASPNETCORE_Authentication__Google__ClientId `
    -e ASPNETCORE_Authentication__Google__ClientSecret `
    -e ASPNETCORE_SendGrid__ApiKey `
    -v ${certDir}:/app/cert `
    -e ASPNETCORE_Kestrel__Certificates__Default__Path=cert/dev-cert.pfx `
    -e ASPNETCORE_Kestrel__Certificates__Default__Password `
    -p 5000:5000 -p 5001:5001 lanfeust-bridge
