# Elastic Beat 개발 환경 만들기 by Docker

## Dockerfile : [파일 다운로드](./Dockerfile)
```dockerfile
#
# 단계 1. libbeat 이미지 만들기
#  - 사이트 : https://github.com/elastic/beats/blob/7.10/libbeat/Dockerfile
#
FROM golang:1.14.12

RUN \
    apt-get update \
      && apt-get install -y --no-install-recommends \
         netcat \
         libpcap-dev \
         python3 \
         python3-pip \
         python3-venv \
      && rm -rf /var/lib/apt/lists/*

RUN pip3 install --upgrade pip==20.1.1
RUN pip3 install --upgrade setuptools==47.3.2
RUN pip3 install --upgrade docker-compose==1.23.2

# Libbeat specific
RUN mkdir -p /etc/pki/tls/certs


#
# 단계 2. 필수 패키지 설치
#  - alien : convert and install rpm and other packages
#  - libaio1 : Linux kernel AIO access library - shared library
#  - libaio-dev : Linux kernel AIO access library - development files
#
RUN \
	apt-get -y update \
	&& apt-get install -y --no-install-recommends \
		alien \
		libaio1 \
		libaio-dev \
	&& rm -rf /var/lib/apt/lists/*

	
#
# 단계 3. Go 빌드 패키지 mage 설치
#	
RUN \
	go get -u -d github.com/magefile/mage \
	&& cd $GOPATH/src/github.com/magefile/mage \
	&& go run bootstrap.go 

	
#
# 단계 4. oracle driver 설치
#  - 버전 : 21.1.0.0.0(2021-05-29 기준)
#  - 사이트 : https://www.oracle.com/kr/database/technologies/instant-client/linux-x86-64-downloads.html
#  - oracle-instantclient-basic-21.1.0.0.0-1.x86_64
#  - oracle-instantclient-sqlplus-21.1.0.0.0-1.x86_64
#  - oracle-instantclient-devel-21.1.0.0.0-1.x86_64
#
RUN \
	cd /tmp \
	&& wget https://download.oracle.com/otn_software/linux/instantclient/211000/oracle-instantclient-basic-21.1.0.0.0-1.x86_64.rpm \
	&& wget https://download.oracle.com/otn_software/linux/instantclient/211000/oracle-instantclient-sqlplus-21.1.0.0.0-1.x86_64.rpm \
	&& wget https://download.oracle.com/otn_software/linux/instantclient/211000/oracle-instantclient-devel-21.1.0.0.0-1.x86_64.rpm \
	&& alien -i oracle-instantclient-basic-21.1.0.0.0-1.x86_64.rpm \
	&& alien -i oracle-instantclient-sqlplus-21.1.0.0.0-1.x86_64.rpm \
	&& alien -i oracle-instantclient-devel-21.1.0.0.0-1.x86_64.rpm \
	&& rm -rf /tmp/* 

	
#
# 단계 5. godror 설치 : Oracle Go 패키지
#
RUN go get github.com/godror/godror  


#
# 단계 6. beats 7.10 브랜치 소스 받기
#
RUN \
	git clone https://github.com/elastic/beats.git $GOPATH/src/github.com/elastic/beats --branch 7.10 \
	&& git config --global user.name "mirero" \
	&& git config --global user.email "support@mirero.co.kr" 

	
#
# 단계 7. 사용자 정의 비트 만들기 전에 .cache 삭제
#
RUN rm -rf ~/.cache/pip
	&& rm -rf /root/.cache/*
	&& rm -rf $GOPATH/src/golang.org/*
	&& rm -rf $GOPATH/src/github.com/go-logfmt
	&& rm -rf $GOPATH/src/github.com/godror
	&& rm -rf $GOPATH/src/github.com/magefile
```

## 컨테이너 사용법 

- 단계 1. 컨테이너 이미지 만들기
  - `docker image build -t beats/dev:7.10 .`

+ 단계 2. 컨테이너 이미지 내보내기(image -> .tar 파일)
  - `docker image save -o beats_dev_7.10.tar beats/dev:7.10`

- 단계 3. 컨테이너 이미지 가져요기(image <- .tar 파일)
  - `docker iamge load -i beats_dev_7.10.tar 

+ 단계 4. 컨테이너 실행 및 접속하기
  - `docker container run -itd --name beats beats/dev:7.10`
    - `-itd` : 백그라운드로 실행하면서 대화형 쉘을 제공한다(실행 후에 shell 접속을 허용하기 위해서는 it 옵션이 필요한다).
      - `-i` : --interactive, Keep STDIN open even if not attached
      - `-t` : --tty, Allocate a pseudo-TTY
      - `-d` : --detach, Run container in background and print container ID
    - `--security-opt="apparmor=unconfined" --cap-add-SYS_PTRACT` : VS Code에서 보안 문제로 접속안될 때 추가 옵션
      ```
	  could not launch process: ... operation not permitted
	  ```
  - `docker container exec -it [컨테이너 ID] bash`


- 단계 5. 사용자 정의 Beat 템플릿 코드 만들기
  - Creating a New Beat : https://www.elastic.co/guide/en/beats/devguide/current/new-beat.html
  ```
  rm -rf ~/.cache/pip
  cd $GOPATH/src/github.com/elastic/beats

  /go/src/github.com/elastic/beats# mage GenerateCustomBeat
     Enter the beat name [examplebeat]: lsbeat             		# beat 이름
     Enter your github name [your-github-name]: mirero     		# 폴더
     Enter the beat path [github.com/mirero/lsbeat]:       		# 생성될 $GOPATH/src 하위 폴더 경로
     Enter your full name [Firstname Lastname]:
     Enter the beat type [beat]:
     Enter the github.com/elastic/beats revision [master]: v7.9.1	# Beat 버전 : GitHub Tag 이름
  ```


- 단계 6. 빌드 및 실행하기
  - 빌드
    ```
    cd $GOPATH/src/github.com/mirero/lsbeat
    make update		# 설정 파일 생성하기 : fields.yml, lsbeat.yml, lsbeat.reference.yml, lsbeat.docker.yml
    mage build		# 빌드
    ```
    - go build -o lsbeat
    - GOOS=linux GOARCH=amd64 CGO_ENABLED=0 go build -o lsbeat -ldflags "-X main.qualifier=mirero"
    - GOOS=windows GOARCH=386 go build -o lsbeat_x86.exe -ldflags "-X main.qualifier=mirero"
    - TODO? x86, x64 빌드 방법
- 실행 옵션 
  ```
  ./lsbeat -h
  Usage:
    lsbeat [flags]
    lsbeat [command]
  
  Available Commands:
    export      Export current config or index template
    help        Help about any command
    keystore    Manage secrets keystore
    run         Run lsbeat
    setup       Setup index template, dashboards and ML jobs
    test        Test config
    version     Show current version info
  
  Flags:
    -E, --E setting=value              Configuration overwrite
    -N, --N                            Disable actual publishing for testing
    -c, --c string                     Configuration file, relative to path.config (default "lsbeat.yml")
        --cpuprofile string            Write cpu profile to file
    -d, --d string                     Enable certain debug selectors
    -e, --e                            Log to stderr and disable syslog/file output
        --environment environmentVar   set environment being ran in (default default)
    -h, --help                         help for lsbeat
        --httpprof string              Start pprof http server
        --memprofile string            Write memory profile to this file
        --path.config string           Configuration path
        --path.data string             Data path
        --path.home string             Home path
        --path.logs string             Logs path
        --plugin pluginList            Load additional plugins
        --strict.perms                 Strict permission checking on config files (default true)
    -v, --v                            Log at INFO level
  ```
- 실행 : Beat 버전 확인
  ```
  ./lsbeat -e -d "*"				# 디거그 모드 콘솔 출력
 
  2021-05-29T12:31:40.889Z        DEBUG   [processors]    processing/processors.go:187    Publish event: {
    "@timestamp": "2021-05-29T12:31:40.888Z",
    "@metadata": {
      "beat": "lsbeat",
      "type": "_doc",
      "version": "7.9.1"				# Beat 버전 확인
    },
  ```

## 개발 환경

- 단계 1. VS Code 설치
  - 사이트 : https://code.visualstudio.com/

+ 단계 2. VS Code Host 확장 도구 설치
  - Remote Containers
  - Docker

- 단계 3. VS Code에서 Container 접속
  

+ 단계 4. VS Code Container 확장 도구 설치
  - Go (Go Team at Google)
 
- 단계 5. Go 도구 설치
  - Ctrl+Shift+P > Go: Install/Update Tools > 전부 체크
    ```
    Tools environment: GOPATH=/go
    Installing 10 tools at /go/bin in module mode.
      gopkgs
      go-outline
      gotests
      gomodifytags
      impl
      goplay
      dlv
      dlv-dap
      staticcheck
      gopls
    ```

+ 단계 6. VS Code에서 Beat 폴더 열기

- 단계 7. VS Code 설정
  - launch.json : 메뉴 > Run > Add Configuration... > Go: Launch Package
    ```json
    {
        "version": "0.2.0",
        "configurations": [
            {
                "name": "Launch Package",
                "type": "go",
                "request": "launch",
                "mode": "debug",
                "program": "${workspaceFolder}",
                "args": [
                    "-c", "lsbeat.yml",
                    "-e", 
                    "-d", "\"*\"" 
                ]
            }
        ]
    }
    ```
  - settings.json : Ctrl+Shift+P > Perferences: Open Workspace Settings (JSON)
    ```json
    {
        "go.toolsEnvVars": {
            "GOOS":"linux",
            "GOARCH":"amd64",
            "CGO_ENABLED":"1"
        } , 
        "go.buildFlags": [
            "-o lsbeat "
        ]   
    }
    ```
  - go.toolsEnvVars : Go 환경 변수
    - GOOS : OS
    - GOARCH : 아키텍처
    - CGO_ENABLED : CGO 활성화(1 = enable, 0 = disable)
    - CGO : Go에서 C코드 호출할 수 있도록 하는 기능(크로스 컴파일링 시 비활성화된다.)
  - go.buildFlags : Go 빌드 플래그
    - go build -o lsbeat
  - F5 디버깅
