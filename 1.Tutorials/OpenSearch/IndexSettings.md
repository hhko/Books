```rest
# 기본 정보
GET /

# 인덱스 목록 조회
GET _cat/indices
GET _cat/indices?v

########################
# 인덱스
#  생성 : PUT [인덱스]
#  정보 : GET [인덱스]
#         GET [인덱스]/_aliases
#         GET [인덱스]/_settings
#         GET [인덱스]/_mappings
#  조회 : GET [인덱스]/_search
#  삭제 : DELETE [인덱스]
########################
# 1. 인덱스 생성
PUT my-index-1

# 2. 인덱스 정보
GET my-index-1

# 내부 필드는 "_"으로 시작(한 계층만 제공)
GET my-index-1/_aliases
GET my-index-1/_mappings
GET my-index-1/_settings

# 3. 데이터 조회
GET my-index-1/_search

# 4. 인덱스 삭제
DELETE my-index-1

########################
# 멀티 테넌시(Multi Tenancy)
#   my-index-*
########################
PUT my-index-1
PUT my-index-2

GET my-index-*
GET my-index-*/_mapping

GET my-index-*/_search

DELETE my-index-*

########################
# 인덱스 settings 생성
#   PUT my-index-1 { "settings": ... }
########################
# 1. 계층 단위 쓰기
PUT my-index-1
{
  "settings": {
    "index": {
      "number_of_shards": 3,
      "number_of_replicas": 1
    }
  }
}

GET my-index-*
DELETE my-index-*

# 2. 여러 계층(index) 함께 쓰기
PUT my-index-1
{
  "settings": {
    "index.number_of_shards": 3,
    "index.number_of_replicas": 1
  }
}

GET my-index-*
DELETE my-index-*

# 3. index 계층 생략 가능
PUT my-index-1
{
  "settings": {
    "number_of_shards": 3,
    "number_of_replicas": 1
  }
}

GET my-index-*

########################
# 인덱스 settings 변경
#   PUT my-index-1 { "settings": ... }
#     number_of_shards    변경 x  1
#     number_of_replicas  변경 o  1
#     refresh_interval    변경 o  1s
#     analysis            변경 x  (none)
########################
# shard
# replica
# refresh_interval은 Elasticsearch에서 세그먼트가 만들어지는 리프레시 타임을 설정하는 값인데 기본은 1초(1s) 입니다.

# 생성할 때
# PUT my-index-1
# {
#   "settings": {
#     "number_of_replicas": 3
#   }
# }

# 변경할 때(/_필드명)
PUT my-index-1/_settings
{
  "number_of_replicas": 2
}

GET my-index-*

PUT my-index-1/_settings
{
  "refresh_interval": "30s"
}

GET my-index-*
```
