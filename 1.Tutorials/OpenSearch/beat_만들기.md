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

## 2.
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
1. TODO? dep
   ```
   go get github.com/golang/dep/cmd/dep
   go install github.com/golang/dep/cmd/dep
   빌드 확인
   ```
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
   ```
   - alien : `rpm` 패키지 설치 및 `deb` 파일 변환 패키지
   - 참고 사이트
     - [Oracle Instant Client Downloads for Linux x86-64 (64-bit)](https://www.oracle.com/kr/database/technologies/instant-client/linux-x86-64-downloads.html) 
1. TODO? GitHub 기본 설정
   ```
   git config --global user.name "xxx"
   git config --global user.email xxx@xxx.com
   로컬 설정은?
   ```
1. VS Code 설치
   - 확장 도구
     - Remote Development Extension Pack 설치
     - Docker ?
     - Go?
   - Go 개발 환경(Ctrl+Shift+P)
     - `Go: Install/Update Tool` 
