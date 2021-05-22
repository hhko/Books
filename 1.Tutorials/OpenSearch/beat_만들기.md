## 1. libbeat 이미지 만들기
1. 7.10 브랜치 Dockerfile 다운로드
   - https://github.com/elastic/beats/blob/7.10/libbeat/Dockerfile
1. Dockerfile 만들기
   ```dockerfile
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
   ```
1. 이미지 만들기
   ```
   docker image build -t libbeat:7.10 .
   
   # 이미지 확인
   docker image ls
   REPOSITORY                                   TAG       IMAGE ID       CREATED              SIZE
   libbeat                                      7.10      7950121cdbf0   About a minute ago   862MB
   ```
1. 이미지 빌드 과정 에러
   - apt-get update 에러
     - 현상
       ```
       apt-get update
       #5 3.156 E: Release file for http://security.debian.org/debian-security/dists/buster/updates/InRelease 
            is not valid yet (invalid for another 3d 19h 1min 50s). 
            Updates for this repository will not be applied.
       #5 3.156 E: Release file for http://deb.debian.org/debian/dists/buster-updates/InRelease 
            is not valid yet (invalid for another 4d 2h 3min 23s). 
            Updates for this repository will not be applied.
       ```
     - 원인
       - 시간이 동기화되지 않기 때문에 발생한다.
     - 대응
       - Case 1. LxssManager 윈도우 서비스 재시작 : 관리자 권한 Powershell
         ```
         net stop LxssManager
         net start LxssManager
         ```
       - Case 2. Docker for Windows 재시작
         - wsl --shutdown
         - Docker for Windows > 메뉴 > "Restart Docker..."
     - 참고 사이트
       - [WSL2의 apt update에서 오류가 발생](https://odaryo.hatenablog.com/entry/2020/01/15/210432)
       - [How to Fix “Repository is not valid yet” Error in Ubuntu Linux](https://itsfoss.com/fix-repository-not-valid-yet-error-ubuntu/)
       - [WSL2 ssh 설정](https://oboki.net/workspace/system/windows/windows-%EB%A1%9C%EC%BB%AC%EC%97%90%EC%84%9C-linux-%EA%B0%9C%EB%B0%9C-%ED%99%98%EA%B2%BD-%EB%A7%8C%EB%93%A4%EA%B8%B0/)

## 2. libbeat 컨테이너 개발환경 
1. 컨테이너 실행
   ```
   docker container run -itd --name libbeat libbeat:7.10
   ```
   - `-itd` : 백그라운드로 실행하면서 대화형 쉘을 제공한다(실행 후에 shell 접속을 허용하기 위해서는 it 옵션이 필요한다). 
     - `i` : --interactive, Keep STDIN open even if not attached
     - `t` : --tty, Allocate a pseudo-TTY
     - `d` : --detach, Run container in background and print container ID
   - `--name` : 컨테이너 이름
   - TODO? --security-opt="apparmor=unconfined" --cap-add-SYS_PTRACT
     ```
     could not launch process: ... operation not permitted
     ```
1. 컨테이너 접속하기(안으로 들어가기)
   ```
   docker container exec -it libbeat bash
   ```
1. 패키지 최신화
   ```
   root@60737c2bf952:/go# apt-get -y update
   root@60737c2bf952:/go# apt-get -y upgrade
   ```
1. mage 설치
   - GOPATH 기반 설치
     ```
     root@60737c2bf952:/go# go get -u -d github.com/magefile/mage       # 소스 다운로드
     root@60737c2bf952:/go# cd $GOPATH/src/github.com/magefile/mage
     root@60737c2bf952:/go# go run bootstrap.go                         # 소스 빌드
     root@60737c2bf952:/go# ls $GOPATH/bin -al                          # 소스 빌드 확인
     total 4756
     drwxrwxrwx 1 root root    4096 May 22 12:25 .
     drwxrwxrwx 1 root root    4096 Nov 19  2020 ..
     -rwxr-xr-x 1 root root 4856298 May 22 12:25 mage
     ```
   - TODO : Go Modules 기반 설치(경로 등 확인 필요) 
     ```
     root@60737c2bf952:/go# git clone https://github.com/magefile/mage
     root@60737c2bf952:/go# cd mage
     root@60737c2bf952:/go# go run bootstrap.go
     빌드 확인
     ```
   - 참고 사이트
     - [Mage GitHub](https://github.com/magefile/mage)
     - Mage : 
1. Oracle Driver 설치
   ```
   # alien 패키지 설치
   root@60737c2bf952:/go# apt-get install -y alien
   
   # alien 패키지 설치 확인(apt-cache show alien)
   root@60737c2bf952:/go# dpkg -l | alien
   
   # oracle driver 다운로드 및 설치
   root@60737c2bf952:/go# cd /tmp
   root@60737c2bf952:/tmp# wget https://download.oracle.com/otn_software/linux/instantclient/211000/oracle-instantclient-basic-21.1.0.0.0-1.x86_64.rpm
   root@60737c2bf952:/tmp# wget https://download.oracle.com/otn_software/linux/instantclient/211000/oracle-instantclient-sqlplus-21.1.0.0.0-1.x86_64.rpm
   root@60737c2bf952:/tmp# wget https://download.oracle.com/otn_software/linux/instantclient/211000/oracle-instantclient-devel-21.1.0.0.0-1.x86_64.rpm
   root@60737c2bf952:/tmp# alien -i oracle-instantclient-basic-21.1.0.0.0-1.x86_64.rpm
   root@60737c2bf952:/tmp# alien -i oracle-instantclient-sqlplus-21.1.0.0.0-1.x86_64.rpm
   root@60737c2bf952:/tmp# alien -i oracle-instantclient-devel-21.1.0.0.0-1.x86_64.rpm
   
   # oracle driver 설치 확인
   ?
   ```
   - alien : `rpm` 패키지 설치 및 `deb` 파일 변환 패키지
   - 참고 사이트
     - [Oracle Instant Client Downloads for Linux x86-64 (64-bit)](https://www.oracle.com/kr/database/technologies/instant-client/linux-x86-64-downloads.html) 
1. SQL 
   ```
   # 패키지 설치
   root@60737c2bf952:/go# apt-get install -y libaio1 libaio-dev
   
   # 패키지 설치 확인
   root@60737c2bf952:/go# dpkg -l | grep libaio1
   root@60737c2bf952:/go# dpkg -l | grep libaio-dev
   ```
   - libaio1 :  Linux kernel AIO access library - shared library
   - libaio-dev : Linux kernel AIO access library - development files
1. GoDror 패키지 설치
   ```
   root@60737c2bf952:/go# go get github.com/godror/godror                     # 소스 다운로드 및 빌드
   root@60737c2bf952:/go# ls $GOPATH/pkg/linux_amd64/github.com/godror -al    # 소스 빌드 확인
   ```
1. Beats 소스 받기
   ```
   root@60737c2bf952:/go# git clone https://github.com/elastic/beats.git $GOPATH/src/github.com/elastic/beats --branch 7.10
   
   root@60737c2bf952:/go# cd $GOPATH/src/github.com/elastic/beats
   root@60737c2bf952:/go/src/github.com/elastic/beats# git branch
   * 7.10
   ```
1. GitHub 기본 설정(생략?)
   ```
   root@60737c2bf952:/go/src/github.com/elastic/beats# git config --global user.name "mirero"
   root@60737c2bf952:/go/src/github.com/elastic/beats# git config --global user.email "support@mirero.co.kr"
   root@60737c2bf952:/go/src/github.com/elastic/beats# git config --list
   user.name=mirero
   user.email=support@mirero.co.kr
   
   로컬 설정은?
   git config --local 
   ```
   - 사용자 정의 Beat 코드 템플릿 에러 : git config 설정이 없을 때 발생한다.
     ```
     *** Please tell me who you are.
     
     Run
     
       git config --global user.email "you@example.com"
       git config --global user.name "Your Name"
     
     to set your account's default identity.
     Omit --global to set the identity only in this repository.
     
     fatal: unable to auto-detect email address (got 'root@4463aa931db9.(none)')
     Error: running "git commit -q -m Initial commit, Add generated files" failed with exit code 128
     ```
   - 참고 사이트
     - [Git을 사용하기 위해 해야하는 최초 설정](https://coding-groot.tistory.com/97)  
1. Beat 소스 템플릿 만들기
   ```
   # pip 캐시 삭제
   # .cache/pip 폴더를 사전에 삭제하지 않으면 에러가 발생한다.
   #   Cache entry deserialization failed, entry ignored
   rm -rf ~/.cache/pip
   
   # 템플릿 생성
   root@60737c2bf952:/go/src/github.com/elastic/beats# mage GenerateCustomBeat
   Enter the beat name [examplebeat]: lsbeat
   Enter your github name [your-github-name]: mirero
   Enter the beat path [github.com/mirero/lsbeat]:
   Enter your full name [Firstname Lastname]:
   Enter the beat type [beat]:
   Enter the github.com/elastic/beats revision [master]: v7.9.1   # tag 이름 : GitHub Tag 이름과 일치해야 한다.
   go: creating new go.mod: module github.com/mirero/lsbeat
   ...
   go: downloading github.com/elastic/beats/v7 v7.9.1
   ...
   Generated fields.yml for lsbeat to /go/src/github.com/mirero/lsbeat/fields.yml
   =======================
   Your custom beat is now available as /go/src/github.com/mirero/lsbeat
   =======================
   
   cd $GOPATH/src/github.com/mirero/lsbeat
   root@60737c2bf952:/go/src/github.com/mirero/lsbeat# make update
   Generated fields.yml for lsbeat to /go/src/github.com/mirero/lsbeat/fields.yml
   >> Building lsbeat.yml for linux/amd64
   >> Building lsbeat.reference.yml for linux/amd64
   >> Building lsbeat.docker.yml for linux/amd64
   Updating generated files for lsbeat
   ...
   root@60737c2bf952:/go/src/github.com/mirero/lsbeat# mage build  # 빌드
   
   ls -al
   -rwxr-xr-x  1 root root 58116840 May 22 07:52 lsbeat
   
   ./lsbeat -c lsbeat.yml -e -d "*"
     -c lsbeat.yml : yml 설정 파일 지정
       -c, --c string : Configuration file, relative to path.config (default "lsbeat.yml")
     -e : 로그 출력을 stdout으로 한다(파일로 생성하지 않는다).
       -e, --e : Log to stderr and disable syslog/file output
     -d : 로그 수준은 Debug이다.
       -d, --d string : Enable certain debug selectors
   
   # 윈도우 빌드
   ```
1. TODO? dep
   ```
   go get github.com/golang/dep/cmd/dep
   go install github.com/golang/dep/cmd/dep
   빌드 확인
   ```
   - dep : 

## 개발 환경
1. VS Code 설치
   - 확장 도구
     - Remote Development Extension Pack 설치
     - Docker ?
     - Go?
   - Go 개발 환경(Ctrl+Shift+P)
     - `Go: Install/Update Tool` 

## Beat 프로세스 명령어 : `-c lsbeat.yml -e -d "*"`
```
root@60737c2bf952:/go/src/github.com/mirero/lsbeat# ./lsbeat --help
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

## 로그 출력 : `agent.version 7.9.1`
```
2021-05-22T22:05:12.430+0900    DEBUG   [processors]    processing/processors.go:187    Publish event: {
  "@timestamp": "2021-05-22T13:05:12.430Z",
  "@metadata": {
    "beat": "lsbeat",
    "type": "_doc",
    "version": "7.9.1"
  },
  "type": "HHKO-LABTOP",
  "counter": 54,
  "host": {
    "ip": [
      "172.28.117.191",
      "fe80::215:5dff:fe5a:98d1"
    ],
    "mac": [
      "ee:7b:77:85:09:64",
      "0e:bc:7a:0e:fb:8f",
      "00:15:5d:5a:98:d1"
    ],
    "hostname": "HHKO-LABTOP",
    "architecture": "x86_64",
    "os": {
      "kernel": "5.4.72-microsoft-standard-WSL2",
      "codename": "bionic",
      "platform": "ubuntu",
      "version": "18.04.5 LTS (Bionic Beaver)",
      "family": "debian",
      "name": "Ubuntu"
    },
    "name": "HHKO-LABTOP",
    "containerized": false
  },
  "agent": {
    "hostname": "HHKO-LABTOP",
    "ephemeral_id": "a5d36715-c74d-4b23-a82b-3ee89d7050ec",
    "id": "fc354ea4-c20f-488c-a61f-306a483c2fdf",
    "name": "HHKO-LABTOP",
    "type": "lsbeat",
    "version": "7.9.1"
  },
  "ecs": {
    "version": "1.8.0"
  }
}
```

## 참고 사이트
- [Beats Developer Guide](https://www.elastic.co/guide/en/beats/devguide/current/index.html)
- https://coding-groot.tistory.com/category/Git
- https://seizze.github.io/2019/12/24/Git-Tag-%EA%B4%80%EB%A0%A8-%EB%AA%85%EB%A0%B9%EC%96%B4-%EC%A0%95%EB%A6%AC.html
- [Ubuntu 18.04 Golang 설치](https://antilibrary.org/2594)  
  `sudo echo 'export PATH=$PATH:/usr/local/go/bin' >> ~/.zshrc`
- https://www.vultr.com/docs/install-the-latest-version-of-golang-on-ubuntu


## TODO
1. LsBeat 컴파일
1. LsBeat 자료구조 정리
1. Golang Windows 빌드
---
1. OracleBeat 컴파일
1. OracleBeat 자료구조 정리
1. OracleBeat 배포 방법?
---
1. Docker Host 네트워크
1. 원격에서 WSL2 접근
---
1. 모든 개발 환겨이 구성된 도커 이미지

```
Error response from daemon: i/o timeout

1. Open "Window Security"
2. Open "App & Browser control"
3. Click "Exploit protection settings" at the bottom
4. Switch to "Program settings" tab
5. Locate "C:\WINDOWS\System32\vmcompute.exe" in the list and expand it
6. Click "Edit"
7. Scroll down to "Code flow guard (CFG)" and uncheck "Override system settings"
8. Delete all files from C:\Users\<name>\AppData\Roaming\Docker
9. Start vmcompute from powershell "net start vmcompute"

1. Open "Window Security"
2. Open "App & Browser control"
3. Click "Exploit protection settings" at the bottom
4. Switch to "Program settings" tab
5. Locate "C:\WINDOWS\System32\vmcompute.exe" in the list and expand it
6. Click "Edit"
7. Scroll down to "Code flow guard (CFG)" and uncheck "Override system settings"
8. Delete all files from C:\Users\<name>\AppData\Roaming\Docker
9. Start vmcompute from powershell "net start vmcompute"
```

## Go 설치 Shell Script : `chmod +x setup_beat_env.sh`
```bash
#!/bin/bash

# https://golang.org/dl/go1.16.4.linux-amd64.tar.gz

#
# 1. 패키지 설치
#    - alien : convert and install rpm and other packages
#    - libaio1 : Linux kernel AIO access library - shared library
#    - libaio-dev : Linux kernel AIO access library - development files
#
apt-get -y update
apt-get -y upgrade
apt-get install -y alien
apt-get install -y libaio1 libaio-dev

#
# 2. mage 설치 : 빌드 Go 패키지
#
go get -u -d github.com/magefile/mage 
cd $GOPATH/src/github.com/magefile/mage
go run bootstrap.go 

#
# 3. oracle driver 설치
#    - oracle-instantclient-basic-21.1.0.0.0-1.x86_64
#    - oracle-instantclient-sqlplus-21.1.0.0.0-1.x86_64
#    - oracle-instantclient-devel-21.1.0.0.0-1.x86_64
#
cd /tmp
wget https://download.oracle.com/otn_software/linux/instantclient/211000/oracle-instantclient-basic-21.1.0.0.0-1.x86_64.rpm
wget https://download.oracle.com/otn_software/linux/instantclient/211000/oracle-instantclient-sqlplus-21.1.0.0.0-1.x86_64.rpm
wget https://download.oracle.com/otn_software/linux/instantclient/211000/oracle-instantclient-devel-21.1.0.0.0-1.x86_64.rpm
alien -i oracle-instantclient-basic-21.1.0.0.0-1.x86_64.rpm
alien -i oracle-instantclient-sqlplus-21.1.0.0.0-1.x86_64.rpm
alien -i oracle-instantclient-devel-21.1.0.0.0-1.x86_64.rpm

#
# 4. godror 설치 : Oracle Go 패키지
#
go get github.com/godror/godror  

#
# 5. beats 7.10 브랜치 소스 받기
#
git clone https://github.com/elastic/beats.git $GOPATH/src/github.com/elastic/beats --branch 7.10

#
# 6. Git 기본 설정
#
git config --global user.name "mirero"
git config --global user.email "support@mirero.co.kr"

#
# 7. 사용자 정의 비트 만들기 전에 .cache 삭제
#
rm -rf ~/.cache/pip
cd $GOPATH/src/github.com/elastic/beats

##
## 8. 사용자 정의 beat 템플릿 코드 만들기
##
## /go/src/github.com/elastic/beats# mage GenerateCustomBeat
##   Enter the beat name [examplebeat]: lsbeat             # beat 이름
##   Enter your github name [your-github-name]: mirero     # 폴더
##   Enter the beat path [github.com/mirero/lsbeat]:
##   Enter your full name [Firstname Lastname]:
##   Enter the beat type [beat]:
##   Enter the github.com/elastic/beats revision [master]: v7.9.1   
##                                                         # tag 이름 : GitHub Tag 이름과 일치해야 한다.
## 
## cd $GOPATH/src/github.com/mirero/lsbeat
## make update
## mage build
## ./lsbeat -c lsbeat.yml -e -d "*"

##
## chmod +x setup_beat_env.sh
##

exit 0
```
