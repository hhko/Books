## 1. libbeat 이미지 만들기
1. Tag v7.9.1 Dockerfile 파일 다운로드
   - [Dockerfile](https://github.com/elastic/beats/blob/v7.9.1/libbeat/Dockerfile)
3. Dockerfile 만들기
   ```dockerfile
   FROM golang:1.14.7
   
   RUN \
       apt-get update \
         && apt-get install -y --no-install-recommends \
            netcat \
            libpcap-dev \
            python3 \
            python3-pip \
            python3-venv \
         && rm -rf /var/lib/apt/lists/*
   
   ENV PYTHON_ENV=/tmp/python-env
   
   RUN pip3 install --upgrade pip==20.1.1
   RUN pip3 install --upgrade setuptools==47.3.2
   RUN pip3 install --upgrade docker-compose==1.23.2

   # Libbeat specific
   RUN mkdir -p /etc/pki/tls/certs
   ```
1. libbeat 이미지 만들기
   ```shell
   docker image build -t libbeat:7.9.1 .
   docker image ls
   REPOSITORY                                   TAG       IMAGE ID       CREATED         SIZE
   libbeat                                      7.9.1     e9e7183b2bc1   8 seconds ago   861MB
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
   docker container run -itd --name libbeat libbeat:7.9.1
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
1. TODO? dep
   ```
   go get github.com/golang/dep/cmd/dep
   go install github.com/golang/dep/cmd/dep
   빌드 확인
   ```
   - dep : 
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
   - 참고 사이트
     - [Git을 사용하기 위해 해야하는 최초 설정](https://coding-groot.tistory.com/97)  
1. Beats 소스 받기
   ```
   root@60737c2bf952:/go# git clone https://github.com/elastic/beats.git $GOPATH/src/github.com/elastic/beats --branch 7.9
   
   root@60737c2bf952:/go# cd $GOPATH/src/github.com/elastic/beats
   root@60737c2bf952:/go/src/github.com/elastic/beats# git branch
   * 7.9

   # TODO? 태그 이동
   git checkout tags/v7.9.1
   root@60737c2bf952:/go/src/github.com/elastic/beats# git branch
   * (HEAD detached at v7.9.1)
     7.9

   git checkout 7.9
   root@60737c2bf952:/go/src/github.com/elastic/beats# git branch
   * 7.9
   ```
1. Beat 소스 템플릿 만들기
   ```
   # pip 캐시 삭제
   rm -rf ~/.cache/pip
   
   # 템플릿 생성   
   root@60737c2bf952:/go/src/github.com/elastic/beats# mage GenerateCustomBeat
   Enter the beat name [examplebeat]: lsbeat
   Enter your github name [your-github-name]: mirero
   Enter the beat path [github.com/mirero/lsbeat]:
   Enter your full name [Firstname Lastname]:
   Enter the beat type [beat]:
   Enter the github.com/elastic/beats revision [master]: 7.9
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
   
   ./lsbeat -e -d "*"
     -c lsbeat.yml : yml 설정 파일 지정
     -e : 로그 출력을 stdout으로 한다(파일로 생성하지 않는다).
     -d : 로그 수준은 Debug이다.
   
   # 윈도우 빌드
   ```
   
1. VS Code 설치
   - 확장 도구
     - Remote Development Extension Pack 설치
     - Docker ?
     - Go?
   - Go 개발 환경(Ctrl+Shift+P)
     - `Go: Install/Update Tool` 

## 참고 사이트
- [Beats Developer Guide](https://www.elastic.co/guide/en/beats/devguide/current/index.html)
- https://coding-groot.tistory.com/category/Git
- https://seizze.github.io/2019/12/24/Git-Tag-%EA%B4%80%EB%A0%A8-%EB%AA%85%EB%A0%B9%EC%96%B4-%EC%A0%95%EB%A6%AC.html

## 로그 출력
```
2021-05-22T08:15:23.694Z        DEBUG   [processors]    processing/processors.go:187    Publish event: {
  "@timestamp": "2021-05-22T08:15:23.693Z",
  "@metadata": {
    "beat": "lsbeat",
    "type": "_doc",
    "version": "7.9.4"
  },
  "host": {
    "id": "a8eb6cac33e701ae867269db5ce80e7f",
    "containerized": true,
    "name": "60737c2bf952",
    "ip": [
      "172.17.0.2"
    ],
    "mac": [
      "02:42:ac:11:00:02"
    ],
    "hostname": "60737c2bf952",
    "architecture": "x86_64",
    "os": {
      "version": "10 (buster)",
      "family": "debian",
      "name": "Debian GNU/Linux",
      "kernel": "5.4.72-microsoft-standard-WSL2",
      "codename": "buster",
      "platform": "debian"
    }
  },
  "counter": 95,
  "type": "60737c2bf952",
  "agent": {
    "hostname": "60737c2bf952",
    "ephemeral_id": "ac4aa143-f740-4e07-b453-63b3bf2e17bf",
    "id": "2f8b6823-fb72-41c5-9945-5861dc111bae",
    "name": "60737c2bf952",
    "type": "lsbeat",
    "version": "7.9.4"
  },
  "ecs": {
    "version": "1.8.0"
  }
}
```

## TODO
1. 7.9.1 배포 만들기
   - 저장소? 
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
