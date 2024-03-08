# DiscountApp



## Requirements:

- dotnet sdk net8.0
- dotnet tool: dotnet-ef

## Start the server

Before starting the server, we need to setup the sqllite database, to do so, run the following commands on the directory `DiscountApp.Server`:

```cmd
 dotnet tool install --global dotnet-ef
 dotnet ef database update
```

To start the server, you can navigate to the directory `DiscountApp.Server`, and then run the following command:

```cmd
dotnet run
```

## Use the Console client

The Console client is a command line, it has 3 commands, to use it you can navigate to the directory `DiscountApp.ConsoleClient`, and then run the following command:

```cmd
dotnet run -- generate -c 2000 -l 8 // Generates 2000 discount code with length equal to 8
dotnet run -- useCode -c "code" // use given code
dotnet run -- getUnusedCodes -c 1 // Get 1 unused code
```


