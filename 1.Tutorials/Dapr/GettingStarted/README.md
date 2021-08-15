# Install Dapr

## 목차
- 설치
  - Ubuntu 18.04 on WSL2 설치
  - Dapr CLI 설치
  - Dapr Runtime 초기화
  - Dapr 제거
- Helloword 예제
  - Dapr API Helloworld 1. 예제
  - Dapr API Helloworld 2. 예제
  - Dapr Component Hellowrod 예제

<br/>

## Ubuntu 18.04 on WSL2 설치
- 이미지 파일
  - [Ubuntu 18.04](https://cloud-images.ubuntu.com/bionic/current/) : [bionic-server-cloudimg-amd64-wsl.rootfs.tar.gz](https://cloud-images.ubuntu.com/bionic/current/bionic-server-cloudimg-amd64-wsl.rootfs.tar.gz)
  - [Ubuntu 20.04](https://cloud-images.ubuntu.com/focal/current/) : [focal-server-cloudimg-amd64-wsl.rootfs.tar.gz](https://cloud-images.ubuntu.com/focal/current/focal-server-cloudimg-amd64-wsl.rootfs.tar.gz)
- 이미지 가져오기(Import)
  - 형식 : `wsl --import <DistributionName> <InstallLocation> <FileName>`
  - 명령
    ```
    wsl --import \
        ubuntu-18.04-dapr \
        C:\Workspace\Dev\DaprLab\ubuntu\fs \
        C:\Workspace\Dev\DaprLab\ubuntu\bionic-server-cloudimg-amd64-wsl.rootfs.tar.gz

    wsl -l -v
    wsl -d ubuntu-18.04-dapr
    ```
- Docker 활성화
  - `Docker for Windows > Settings > Resources > WSL INTEGRATION - ubuntu-18.04-dapr : enable`
- Ubuntu 실행 및 패키지 업그레이드  
    ```
    wsl -d ubuntu-18.04-dapr

    sudo apt update
    sudo apt upgrade -y
    ```

<br/>

## [Dapr CLI 설치](https://docs.dapr.io/getting-started/install-dapr-cli/)
- Step 1: Install from Terminal
  ```
  wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash
  ```
- Step 2: Verify the installation
  ```
  dapr
  dapr -v

  CLI version: 1.3.0
  Runtime version: n/a
  ```
  - 설치 결과물 : `/usr/local/bin/dapr` 실행 파일

<br/>

## [Dapr Runtime 초기화](https://docs.dapr.io/getting-started/install-dapr-selfhost/)
- Step 1: Run the init CLI command
  ```
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
  ```
  - PATH=$PATH:/root/.dapr/bin
- Step 2: Verify Dapr version
  ```
  dapr -v
  CLI version: 1.3.0
  Runtime version: 1.3.0
  ```
- Step 3: Verify containers are running
  ```
  docker container ls
  CONTAINER ID   IMAGE               COMMAND                  CREATED         STATUS                   PORTS
                                     NAMES
  2a7aa9d40238   daprio/dapr:1.3.0   "./placement"            2 minutes ago   Up 2 minutes             0.0.0.0:50005->50005/tcp,   :::50005->50005/tcp         dapr_placement
  2cb1025ad35d   openzipkin/zipkin   "start-zipkin"           2 minutes ago   Up 2 minutes (healthy)   9410/tcp, 0.0.0.0:9411->9411/  tcp, :::9411->9411/tcp   dapr_zipkin
  2f2759997bdd   redis               "docker-entrypoint.s…"   3 minutes ago   Up 3 minutes             0.0.0.0:6379->6379/tcp,   :::6379->6379/tcp             dapr_redis
  ```
  - daprio/dapr:1.3.0 : dapr_placement
  - openzipkin/zipkin : dapr_zipkin
  - redis : dapr_redis

- Step 4: Verify components directory has been initialized
  ```
  $HOME/.dapr

  ls $HOME/.dapr
  bin  components  config.yaml
  ```

<br/>

## [Dapr 제거](https://docs.dapr.io/operations/hosting/self-hosted/self-hosted-uninstall/)
- 명령어 비교
  - `dapr uninstall`
  - `dapr uninstall --all` : Uninstall from self-hosted mode and remove .dapr directory, Redis, Placement and Zipkin containers
- `dapr uninstall`
  ```
  dapr uninstall
  ℹ️  Removing Dapr from your machine...
  ℹ️  Removing directory: /root/.dapr/bin
  ℹ️  Removing container: dapr_placement
  ✅  Dapr has been removed successfully

  docker container ls
  CONTAINER ID   IMAGE               COMMAND                  CREATED          STATUS                    PORTS
     NAMES
  2cb1025ad35d   openzipkin/zipkin   "start-zipkin"           46 minutes ago   Up 46 minutes (healthy)   9410/tcp, 0.0.0.0:9411->9411/  tcp, :::9411->9411/tcp   dapr_zipkin
  2f2759997bdd   redis               "docker-entrypoint.s…"   46 minutes ago   Up 46 minutes             0.0.0.0:6379->6379/tcp,   :::6379->6379/tcp
     dapr_redis
  
  ls /root/.dapr/
  components  config.yaml
  ```
- `dapr uninstall --all`
  ```
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
  ```

<br/>

## [Dapr API Helloworld 1. 예제](https://www.youtube.com/watch?v=oweMRGg_m8w&t)
- 개요
- 명령
  ```
  dapr run
  포트 확인

  POST http://localhost:포트/v1.0/state/statestore/
  {
    {
      "key": "KeyValue",
      "value": "Value"
    }
  }

  GET http://localhost:포트/v1.0/state/statestore/키값
  iwr http://localhost:포트/v1.0/state/statestore/키값
  iwr http://localhost:포트/v1.0/state/statestore/키값 -Method Delete

  Redis 컨테이너 값 확인 
  ```

## [Dapr API Helloworld 2. 예제](https://docs.dapr.io/getting-started/get-started-api/)
- 개요
  - Redis에 Key & Value 데이터를 추가하고 조회한다.
    - 추가 : `http://localhost:3500/v1.0/state/statestore`, POST, JSON
    - 조회 : `http://localhost:3500/v1.0/state/statestore/name` 또는 Redis 컨테이너 안 `keys *`, `hgetall "myapp||name"`
- Step 1: Run the Dapr sidecar
  ```
  dapr run --app-id myapp --dapr-http-port 3500
  WARNING: no application command found.
  ℹ️  Starting Dapr with id myapp. HTTP Port: 3500. gRPC Port: 35315
  ℹ️  Checking if Dapr sidecar is listening on HTTP port 3500
  ```
  - id : `--app-id`
  - REST API : `--dapr-http-port`
- Step 2: Save state
  ```json
  [
    {
      "key": "name",
      "value": "Bruce Wayne"
    }
  ]
  ```
  ```
  curl -X POST -H "Content-Type: application/json" -d '[{ "key": "name", "value": "Bruce Wayne"}]' http://localhost:3500/v1.0/state/statestore
  ```
- Step 3: Get state
  ```
  curl http://localhost:3500/v1.0/state/statestore/name
  "Bruce Wayne"
  
  ip addr show
  http://172.27.216.76:3500/v1.0/state/statestore/name
  "Bruce Wayne"
  ```
- Step 4: See how the state is stored in Redis
  ```
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
  ```

<br/>

## [Dapr Component Hellowrod 예제](https://docs.dapr.io/getting-started/get-started-component/)
- 개요
  - yaml 파일에 secretstores.local.file 타입의 component(-> json)을 생성하고 조회한다.
    - 생성 : localSecretStore.yaml, mysecrets.json
    - 조회 : http://localhost:3500/v1.0/secrets/my-secret-store/my-secret
      - my-secret-store : localSecretStore.yaml(metadata > name)
      - my-secret : mysecrets.json(key)
- Step 1: Create a JSON secret store
  ```json
  {
     "my-secret" : "I'm Batman"
  }
  ```
  - mysecrets.json
- Step 2: Create a secret store Dapr component
  ```yaml
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
      value: ./mysecrets.json
    - name: nestedSeparator
      value: ":"
  ```
  - localSecretStore.yaml
- Step 3: Run the Dapr sidecar
  ```
  dapr run --app-id myapp --dapr-http-port 3500 --components-path ./
  WARNING: no application command found.
  ℹ️  Starting Dapr with id myapp. HTTP Port: 3500. gRPC Port: 42809
  ℹ️  Checking if Dapr sidecar is listening on HTTP port 3500

  curl http://localhost:3500/v1.0/secrets/my-secret-store/my-secret
  {"my-secret":"I'm Batman"}

  ip addr show
  http://172.27.216.76:3500/v1.0/secrets/my-secret-store/my-secret
  {"my-secret":"I'm Batman"}

  dapr stop myapp
  ```
  - component : `--components-path`

<br/>

