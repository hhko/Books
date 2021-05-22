## 1. libbeat 이미지 만들기
1. Tag v7.9.1 Dockerfile 파일 다운로드 : https://github.com/elastic/beats/blob/v7.9.1/libbeat/Dockerfile
1. Dockerfile 만들기
   ```dockerfile
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
       #5 3.156 E: Release file for http://security.debian.org/debian-security/dists/buster/updates/InRelease is not valid yet 
                  (invalid for another 3d 19h 1min 50s). 
                  Updates for this repository will not be applied.
       #5 3.156 E: Release file for http://deb.debian.org/debian/dists/buster-updates/InRelease is not valid yet 
                  (invalid for another 4d 2h 3min 23s). 
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
     - 참고 사이트
       - [WSL2의 apt update에서 오류가 발생](https://odaryo.hatenablog.com/entry/2020/01/15/210432)
       - [How to Fix “Repository is not valid yet” Error in Ubuntu Linux](https://itsfoss.com/fix-repository-not-valid-yet-error-ubuntu/)
