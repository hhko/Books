# Dapr
Dapr(Distributed Application Runtime) is **a portable, event-driven, runtime** for building distributed applications across cloud and edge.

<br/>

# WSL2 Ubuntu 18.04

https://cloud-images.ubuntu.com/bionic/current/
https://cloud-images.ubuntu.com/bionic/current/bionic-server-cloudimg-amd64-wsl.rootfs.tar.gz

https://cloud-images.ubuntu.com/focal/current/
https://cloud-images.ubuntu.com/focal/current/focal-server-cloudimg-amd64-wsl.rootfs.tar.gz

wsl --import <DistributionName> <InstallLocation> <FileName>

wsl --import \
    ubuntu-18.04-dapr \
    C:\Workspace\Dev\DaprLab\ubuntu\fs \
    C:\Workspace\Dev\DaprLab\ubuntu\bionic-server-cloudimg-amd64-wsl.rootfs.tar.gz

wsl -l -v
wsl -d ubuntu-18.04-dapr

Docker for Windows > Settings > Resources > WSL INTEGRATION - ubuntu-18.04-dapr : enable

sudo apt update
sudo apt upgrade -y


https://docs.dapr.io/getting-started/install-dapr-cli/
# Install the Dapr CLI
## Step 1: Install from Terminal

wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash

Step 2: Verify the installation

dapr
dapr -v

CLI version: 1.3.0
Runtime version: n/a

- /usr/local/bin/dapr 실행 파일 다운로드

https://docs.dapr.io/getting-started/install-dapr-selfhost/
# Initialize Dapr in your local environment

## Step 1: Open an elevated terminal

## Step 2: Run the init CLI command

dapr init
⌛  Making the jump to hyperspace...
ℹ️  Installing runtime version 1.3.0
↖  Downloading binaries and setting up components...
Dapr runtime installed to /root/.dapr/bin, you may run the following to add it to your path if you want to run daprd directly:
    export PATH=$PATH:/root/.dapr/bin
✅  Downloading binaries and setting up components...
✅  Downloaded binaries and completed components set up.
ℹ️  daprd binary has been installed to /root/.dapr/bin.
ℹ️  dapr_placement container is running.
ℹ️  dapr_redis container is running.
ℹ️  dapr_zipkin container is running.
ℹ️  Use `docker ps` to check running containers.
✅  Success! Dapr is up and running. To get started, go here: https://aka.ms/dapr-getting-started


- PATH=$PATH:/root/.dapr/bin

## Step 3: Verify Dapr version
dapr -v
CLI version: 1.3.0
Runtime version: 1.3.0

## Step 4: Verify containers are running

daprio/dapr:1.3.0 : dapr_placement
openzipkin/zipkin : dapr_zipkin
redis             : dapr_redis

docker container ls
CONTAINER ID   IMAGE               COMMAND                  CREATED         STATUS                   PORTS
                                   NAMES
2a7aa9d40238   daprio/dapr:1.3.0   "./placement"            2 minutes ago   Up 2 minutes             0.0.0.0:50005->50005/tcp, :::50005->50005/tcp         dapr_placement
2cb1025ad35d   openzipkin/zipkin   "start-zipkin"           2 minutes ago   Up 2 minutes (healthy)   9410/tcp, 0.0.0.0:9411->9411/tcp, :::9411->9411/tcp   dapr_zipkin
2f2759997bdd   redis               "docker-entrypoint.s…"   3 minutes ago   Up 3 minutes             0.0.0.0:6379->6379/tcp, :::6379->6379/tcp             dapr_redis


## Step 5: Verify components directory has been initialized

$HOME/.dapr

ls $HOME/.dapr
bin  components  config.yaml


# Use the Dapr API
https://docs.dapr.io/getting-started/get-started-api/

## Step 1: Run the Dapr sidecar

dapr run --app-id myapp --dapr-http-port 3500
WARNING: no application command found.
ℹ️  Starting Dapr with id myapp. HTTP Port: 3500. gRPC Port: 35315
ℹ️  Checking if Dapr sidecar is listening on HTTP port 3500
INFO[0000] starting Dapr Runtime -- version 1.3.0 -- commit 4bab7576ed68a9ece1a4743a7925f18ef583775a  app_id=myapp instance=HHKO-LABTOP scope=dapr.runtime type=log ver=1.3.0


## Step 2: Save state

[
  {
    "key": "name",
    "value": "Bruce Wayne"
  }
]

curl -X POST -H "Content-Type: application/json" -d '[{ "key": "name", "value": "Bruce Wayne"}]' http://localhost:3500/v1.0/state/statestore


## Step 3: Get state

curl http://localhost:3500/v1.0/state/statestore/name
"Bruce Wayne"

ip addr show
http://172.27.216.76:3500/v1.0/state/statestore/name
"Bruce Wayne"

## Step 4: See how the state is stored in Redis

docker exec -it dapr_redis redis-cli

127.0.0.1:6379> keys *
1) "myapp||name"

127.0.0.1:6379> hgetall "myapp||name"
1) "data"
2) "\"Bruce Wayne\""
3) "version"
4) "1"

exit

dapr stop myapp

# Define a component
https://docs.dapr.io/getting-started/get-started-component/

## Step 1: Create a JSON secret store
mysecrets.json

{
   "my-secret" : "I'm Batman"
}

## Step 2: Create a secret store Dapr component
localSecretStore.yaml

apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: my-secret-store
  namespace: default
spec:
  type: secretstores.local.file
  version: v1
  metadata:
  - name: secretsFile
    value: <PATH TO SECRETS FILE>/mysecrets.json
  - name: nestedSeparator
    value: ":"

## Step 3: Run the Dapr sidecar
dapr run --app-id myapp --dapr-http-port 3500 --components-path ./my-components


dapr run --app-id myapp --dapr-http-port 3500 --components-path ./
WARNING: no application command found.
ℹ️  Starting Dapr with id myapp. HTTP Port: 3500. gRPC Port: 42809
ℹ️  Checking if Dapr sidecar is listening on HTTP port 3500
INFO[0000] starting Dapr Runtime -- version 1.3.0 -- commit 4bab7576ed68a9ece1a4743a7925f18ef583775a  app_id=myapp instance=HHKO-LABTOP scope=dapr.runtime type=log ver=1.3.0

curl http://localhost:3500/v1.0/secrets/my-secret-store/my-secret
{"my-secret":"I'm Batman"}


http://172.27.216.76:3500/v1.0/secrets/my-secret-store/my-secret
{"my-secret":"I'm Batman"}

dapr stop myapp


# Uninstall Dapr in a self-hosted environment
https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-uninstall/

dapr uninstall --all

- Uninstall from self-hosted mode
dapr uninstall

dapr uninstall
ℹ️  Removing Dapr from your machine...
ℹ️  Removing directory: /root/.dapr/bin
ℹ️  Removing container: dapr_placement
✅  Dapr has been removed successfully

docker container ls
CONTAINER ID   IMAGE               COMMAND                  CREATED          STATUS                    PORTS
   NAMES
2cb1025ad35d   openzipkin/zipkin   "start-zipkin"           46 minutes ago   Up 46 minutes (healthy)   9410/tcp, 0.0.0.0:9411->9411/tcp, :::9411->9411/tcp   dapr_zipkin
2f2759997bdd   redis               "docker-entrypoint.s…"   46 minutes ago   Up 46 minutes             0.0.0.0:6379->6379/tcp, :::6379->6379/tcp
   dapr_redis

ls /root/.dapr/
components  config.yaml



- Uninstall from self-hosted mode and remove .dapr directory, Redis, Placement and Zipkin containers
dapr uninstall --all

dapr uninstall --all
ℹ️  Removing Dapr from your machine...
⚠  WARNING: /root/.dapr/bin does not exist
⚠  WARNING: dapr_placement container does not exist
ℹ️  Removing container: dapr_redis
ℹ️  Removing container: dapr_zipkin
ℹ️  Removing directory: /root/.dapr
✅  Dapr has been removed successfully

docker container ls
CONTAINER ID   IMAGE     COMMAND   CREATED   STATUS    PORTS     NAMES

ls /root/


# TODO self-hosted mode on Linux
https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-overview/



## 참고 자료
- 문서
  - [ ] [MSDN | Dapr for .NET Developers e-book](https://dotnet.microsoft.com/learn/aspnet/microservices-architecture#ebook-dapr-swimlane)
  - [ ] [MSDN | Dapr](https://docs.microsoft.com/ko-kr/dotnet/architecture/dapr-for-net-developers/foreword)
  - [ ] [Dapr | Getting started with Dapr](https://docs.dapr.io/getting-started/)
  - [ ] [Dapr | Hello World](https://github.com/dapr/quickstarts/tree/v1.0.0/hello-world)
  - [ ] [Dapr | Distributed calculator](https://github.com/dapr/quickstarts/tree/v1.0.0/distributed-calculator)
  - [ ] [Blog | Learning Dapr: Simple Dotnet Core "Hello World"](https://dev.to/mkokabi/learning-dapr-simple-dotnet-core-hello-world-b0k)
  - [ ] [Blog | Dapr Run Node.js App](https://developpaper.com/microsofts-distributed-application-framework-dapr-helloworld/)

+ 동영상
  - [ ] [Getting started with Dapr](https://www.youtube.com/watch?v=oweMRGg_m8w&list=PLLajsYIn6RRTAOM2vIs2pz_p2JXnCd74Y) 
