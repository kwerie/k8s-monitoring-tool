# README

This project houses a small monitoring tool for k8s written using the C# k8s client.

## Setting up the project
1. Create a file in `.kube` called `config` (`touch ./.kube/config`) and place your desired kubeconfig in there.
2. Run `docker compose up (-d | for detached) to start the database container
