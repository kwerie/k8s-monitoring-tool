# README

This project houses a small monitoring tool for k8s written using the C# k8s client.

## Prerequisites 
Make sure you have the .NET Code CLI tools for EntityFramework installed. \
If not, you can install them with `dotnet tool install --global dotnet-ef`.

## Setting up the project
1. Create a file in `src/K8sMonitoringTool/.kube` called `config` (`touch ./src/K8sMonitoringTool/.kube/config`) and place your desired kubeconfig in there.
2. Run `docker compose up (-d | for detached) to start (and build) the database container
3. When the database container is running, run migrations: `dotnet ef database update`

## Migrations
To create a new migration, run the following command:

```bash
dotnet ef migrations add <Name>
```

To undo creation of the last created migration, run:
```bash
dotnet ef migrations remove
```

To run migrations, run:
```bash
dotnet ef database update
```

To undo already executed migrations, you can run the following command:\
***Please note: this command can result in data loss!***
```bash
dotnet ef migrations remove -f
```
