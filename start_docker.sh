docker build -t lanfeust-bridge .

if [ -z "$ASPNETCORE_Authentication__Google__ClientId" ]
then
    while read line
    do
        k=$(echo $line | perl -ne '$k = $1 if /^(\S*)\s*=/; $k =~ s/:/__/g; print "ASPNETCORE_$k"')
        v=$(echo $line | perl -ne 'print $1 if /^.*=\s*(.*)/')
        export $k=$v
    done < <(dotnet user-secrets list)
fi
docker run -d -e ASPNETCORE_Authentication__Google__ClientId -e ASPNETCORE_Authentication__Google__ClientSecret -e ASPNETCORE_SendGrid__ApiKey -p 5000:5000 -p 5001:5001 lanfeust-bridge
