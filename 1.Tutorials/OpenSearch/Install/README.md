## 준비
- WSL2 설치
- Docker for Windows 설치
- Linux Kernel 설정(max_map_count) : [링크](https://www.elastic.co/guide/en/elasticsearch/reference/current/docker.html#_set_vm_max_map_count_to_at_least_262144)
  ```
  # docker-desktop shell 접속하기
  wsl -d docker-desktop     

  # max_map_count 값 변경하기
  cat /proc/sys/vm/max_map_count
  65530
  echo 262144 >> /proc/sys/vm/max_map_count
  cat /proc/sys/vm/max_map_count
  262144
  exit
  ```
  - max_map_count가 262144 보다 적을 때 에러 메시지
    ```
    opensearch-node1       | ERROR: [1] bootstrap checks failed
    opensearch-node1       | [1]: max virtual memory areas vm.max_map_count [65530] is too low, increase to at least [262144]
    ```

## OpenSearch docker-compose
```yaml

```
docker container exec -it opensearch-node1 /bin/bash

/usr/share/opensearch/plugins/opensearch-performance-analyzer
/usr/share/opensearch/plugins/opensearch-performance-analyzer/pa_config

docker-compose up -d
docker-compose down -v
docker-compose logs -f
docker-compose logs -f opensearch-node1 
docker-compose logs -f opensearch-dashboards

https://localhost:9200
{
  "name" : "opensearch-node1",
  "cluster_name" : "opensearch-cluster",
  "cluster_uuid" : "lvLttJ38TEaypaQsurV1GQ",
  "version" : {
    "distribution" : "opensearch",
    "number" : "1.0.0",
    "build_type" : "tar",
    "build_hash" : "34550c5b17124ddc59458ef774f6b43a086522e3",
    "build_date" : "2021-07-02T23:22:21.383695Z",
    "build_snapshot" : false,
    "lucene_version" : "8.8.2",
    "minimum_wire_compatibility_version" : "6.8.0",
    "minimum_index_compatibility_version" : "6.0.0-beta1"
  },
  "tagline" : "The OpenSearch Project: https://opensearch.org/"
}


WSL2
https://docs.microsoft.com/en-us/windows/wsl/install-win10

http://localhost:5601
admin/admin

curl -XGET https://localhost:9200 -u 'admin:admin' --insecure
Invoke-WebRequest -Uri "$Uri" -Method Get 

curl http://localhost:8545 -H "Content-Type:application/json" -X POST --data '{"jsonrpc":"2.0","method":"web3_clientVersion","params":[],"id":1}
curl http://localhost:8545 -contenttype "application/json" -method post -body '{"jsonrpc":"2.0","method":"web3_clientVersion","params":[],"id":1}'


curl -XPOST localhost:9200/_plugins/_performanceanalyzer/cluster/config -H 'Content-Type: application/json' -d '{"enabled": true}'
curl https://localhost:9200/_plugins/_performanceanalyzer/cluster/config -contenttype "application/json" -method post -body '{"enabled": true}'

Invoke-WebRequest http://192.168.0.35:8545 -SessionVariable fb -method post -body $body -contenttype "application/json"

$user = "admin"
$pass= "admin"
$secpasswd = ConvertTo-SecureString $pass -AsPlainText -Force
$credential = New-Object System.Management.Automation.PSCredential($user, $secpasswd)
curl https://localhost:9200/_plugins/_performanceanalyzer/cluster/config -contenttype "application/json" -method post -body '{"enabled": true}' -Credential $credential


$user = "admin"
$pass= "admin"
$pair = "$($user):$($pass)"
$encodedCredentials = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($Pair))
$headers = @{ Authorization = "Basic $encodedCredentials" }
curl https://localhost:9200/_plugins/_performanceanalyzer/cluster/config -contenttype "application/json" -method post -body '{"enabled": true}' -Headers $headers

$user = 'admin'
$pass = 'admin'

$pair = "$($user):$($pass)"

$encodedCreds = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($pair))

$basicAuthValue = "Basic $encodedCreds"

$Headers = @{
    Authorization = $basicAuthValue
}

Invoke-WebRequest -Uri 'https://whatever' -Headers $Headers

curl https://localhost:9200/_plugins/_performanceanalyzer/cluster/config -contenttype "application/json" -method post -body '{"enabled": true}' -headers $Headers


curl -XPOST localhost:9200/_plugins/_performanceanalyzer/rca/cluster/config -H 'Content-Type: application/json' -d '{"enabled": true}'
curl localhosts:9200/_plugins/_performanceanalyzer/rca/cluster/config -contenttype "application/json" -method post -body '{"enabled": true}'


https://opensearch.org/docs/opensearch/install/docker/#start-a-cluster
https://opensearch.org/docs/opensearch/install/docker/#optional-set-up-performance-analyzer
https://opensearch.org/docs/opensearch/install/docker-security/