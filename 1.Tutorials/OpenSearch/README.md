## OpenSearch 설치
- [OpenSearch 컨테이너 이미지 만들기](./OpenSearch_컨테이너_이미지_만들기.md)
- [Elasitc Beat 개발 환경 만들기 by WSL2](./Elastic_Beat_개발_환경_만들기_by_WSL2.md)
- [Elasitc Beat 개발 환경 만들기 by Docker](./Elastic_Beat_개발_환경_만들기_by_Docker.md)

## Beat 설치
- 다운로드 : [링크](https://www.elastic.co/kr/downloads/past-releases#metricbeat-oss)
- 관리자 권한 실행
  ```
  powershell.exe -ExecutionPolicy UnRestricted -File .\install-service-metricbeat.ps1
  ```

## 참고자료
- [Run Metricbeat on Docker](https://www.elastic.co/guide/en/beats/metricbeat/current/running-on-docker.html)
- [Docker 컨테이너에 데이터 저장 (볼륨/바인드 마운트)](https://www.daleseo.com/docker-volumes-bind-mounts/)
- [Docker를 이용한 NodeJS 개발](https://www.daleseo.com/docker-nodejs/)
- [Elastic Search & Kafka](https://www.slipp.net/wiki/pages/viewpage.action?pageId=30771281)
- [[Monitiring Tool] Elastic Stack(Filebeat, Logstash, Elasticsearch, Kibana) 구성 실습](https://miiingo.tistory.com/216)
- [Docker로 ELK 환경 구축하기](https://velog.io/@jaryeonge/Docker%EB%A1%9C-ELK-%ED%99%98%EA%B2%BD-%EA%B5%AC%EC%B6%95%ED%95%98%EA%B8%B0)
- [docker-elk](https://github.com/deviantony/docker-elk)

## Prometheus Oracle Exporter
- [Oracle DB Exporter](https://opensourcelibs.com/lib/oracledb_exporter)
- Dashboard
  - [OracleDB Monitoring dashboard](https://grafana.com/grafana/dashboards/13555)
  - [OracleDb dashboards](https://grafana.com/grafana/dashboards?search=oracle)
- SQL
  - [How to monitor an Oracle database with Prometheus](https://sysdig.com/blog/monitor-oracle-database-prometheus/)
  - [Oralce SQL for monitoring](https://github.com/freenetdigital/prometheus_oracle_exporter/blob/master/main.go)
  - [oracle-database](https://promcat.io/apps/oracle-database)
  - [oracle-database 12.1.0.2](https://promcat.io/apps/oracle-database/12.1.0.2)
- Exporter
  - [Go언어로 나만의 Query Exporter 만들어보기!](https://gywn.net/2021/07/make-own-query-exporter-with-go/)
