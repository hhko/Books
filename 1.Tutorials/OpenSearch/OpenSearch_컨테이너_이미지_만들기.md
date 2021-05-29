## opensearch_security 없는 OpenSearch 배포하기
### OpenSearch 이미지 만들기
- Dockerfile 파일
  ```Dockerfile
  FROM opensearchproject/opensearch:1.0.0-beta1
  RUN /usr/share/opensearch/bin/opensearch-plugin remove opensearch_security
  COPY --chown=opensearch:opensearch opensearch.yml /usr/share/opensearch/config/
  ```
- opensearch.yml 파일
  ```yml
  network.host: 0.0.0.0
  ```
- Image 만들기 : 버전은 1.0.1로 정의한다.
  ```
  docker build -t opensearch-no-security:1.0.0-beta1 .
  ```
  
 <br/>

### OpenSearch-dashboards 이미지 만들기
- Dockerfile 파일
  ```Dockerfile
  FROM opensearchproject/opensearch-dashboards:1.0.0-beta1
  RUN /usr/share/opensearch-dashboards/bin/opensearch-dashboards-plugin remove opensearchSecurityOpenSearch Dashboards
  COPY --chown=opensearch-dashboards:opensearch-dashboards opensearch_dashboards.yml /usr/share/opensearch-dashboards/config/
  ```
- opensearch_dashboards.yml 파일
  ```yml
  server.name: opensearch-dashboards
  server.host: "0"
  ```
- Image 만들기 : 버전은 1.0.1로 정의한다.
  ```
  docker build -t opensearch-dashboards-no-security:1.0.0-beta1 .
  ```
     
 <br/>    
     
### OpenSearch 실행하기
```yml
version: '3'
services:
  opensearch-node1:
    # image: opensearchproject/opensearch:1.0.0-beta1
    image: opensearch-no-security:1.0.0-beta1
    container_name: opensearch-node1
    environment:
      - discovery.type=single-node
      - cluster.name=opensearch-cluster
      - node.name=opensearch-node1
      # - discovery.seed_hosts=opensearch-node1,opensearch-node2
      # - cluster.initial_master_nodes=opensearch-node1,opensearch-node2
      - bootstrap.memory_lock=true # along with the memlock settings below, disables swapping
      - "OPENSEARCH_JAVA_OPTS=-Xms512m -Xmx512m" # minimum and maximum Java heap size, recommend setting both to 50% of system RAM
    ulimits:
      memlock:
        soft: -1
        hard: -1
      nofile:
        soft: 65536 # maximum number of open files for the OpenSearch user, set to at least 65536 on modern systems
        hard: 65536
    volumes:
      - opensearch-data1:/usr/share/opensearch/data
    ports:
      - 9200:9200
      - 9600:9600 # required for Performance Analyzer
    networks:
      - opensearch-net
  opensearch-dashboards:
    # image: opensearchproject/opensearch-dashboards:1.0.0-beta1
    image: opensearch-dashboards-no-security:1.0.0-beta1
    container_name: opensearch-dashboards
    ports:
      - 5601:5601
    expose:
      - "5601"
    environment:
      # OPENSEARCH_URL: http://opensearch-node1:9200
      OPENSEARCH_HOSTS: http://opensearch-node1:9200
    networks:
      - opensearch-net

volumes:
  opensearch-data1:

networks:
  opensearch-net:
```

<br/>

### 참고 사이트
- [opensearch_security 없는 OpenSearch](https://docs-beta.opensearch.org/docs/opensearch/install/docker/#customize-the-docker-image)
- [opensearch_security 없는 OpenSearch-dashboards](https://docs-beta.opensearch.org/docs/security/configuration/disable/#remove-opensearch-dashboards-plugin)
- https://blog.aaronroh.org/118
- https://jonnung.dev/docker/2020/02/16/docker_network/
- https://bluese05.tistory.com/15
