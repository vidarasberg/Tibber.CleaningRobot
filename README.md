# Tibber Technical Case

## Running the application

### Using docker

Ensure docker is running then execute in the root of the repository.
This starts the app and a postgreSQL in docker containers.


```console
docker compose up
```

### Alternatively using dotnet
The project targets dotnet 9 so install the SDK or runtime.
Configure the value of PostgreDB in appsettings.Development to use your db.

NOTE: migrations are automatically applied for Development

```console
dotnet run --project Tibber.CleaningBotWebAPI
```

## Test
To run the tests execute the following in the root of the directory
```console
dotnet test
```

## Development vs Production


Database migrations are automatically run when ASPNETCORE_ENVIRONMENT=Development.
For production use [dotnet ef tool](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli)
to generate sql script for the migrations.

## Calling the api
In the Tibber.CleaningBotWebAPI project there is a .http file which can be used to send requests
to the endpoint when the app is running. Curl or Postman are alternatives.

## Notes to examiners
- I decided to save the timestamp with timezone and in UTC. Not doing that tends to end in regret further down the line
- Is the precision of 6 digits in duration a requirement, as of now all significant numbers are saved.
- There is a typo in the request example "commmands", I decided it was an error and the api expects "commands"