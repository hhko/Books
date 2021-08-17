# Getting Started With Elasticsearch for .NET Developers
https://www.pluralsight.com/courses/elasticsearch-for-dotnet-developers

The .NET Docs Show - Getting Started with Elasticsearch.NET
https://www.youtube.com/watch?v=Ll5yLL83W8M

설치
	타임존
	자동 업데이트
	기본 프로그램 설치 : vim
	WSL 2 설치
	Ubuntu 18.04 설치
	도커
	도커 설치 경로
	도커 로그 자동 삭제
	리눅스 로그?
	리눅스 로그 자동 삭제
	반복 작업 : 시간 동기화, Cache 삭제?
---------------------
JSON
NoSQL
REST API
	curl
	Postman
	브라우저
--------------------
BLUE CATS
	Beat 개발 환경
	FileBeat
	MetricBeat
	WinlogBeat
	리눅스 저널
	Heartbeat
	
	
	Logstash
	FileBeat
	MetricBeat
	
	Kafka
	Kafdrop
	Logstash
	Elasticsearch
	Kibana
	Grafana
	Blazor
	shell?
	FileBeat
	MetricBeat
	Postman
	
-----------------------------------
1장
------------------------------------
Elasticsearch
	Elasticsearch.yml
		cluster.name
		node.name
		로그 경로?
		메모리
		데이터 경로
		네트워크 ip, port
	플러그인 설치
		marvel
		
	스키마
		Index 		vs Database
		types 		vs Table
		properties	vs Columns
		field		vs Column
					vs id?
					
		
GET /					// :9200
GET /_cat/indices		// 모든 인덱스
GET /_cat/indices/????	// 특정 인덱스
	GET _cat/indices/*,-efgh
	curl -s -XGET 'hostname:post/_cat/indices/*,-efgh*'
	
	curl -XGET 'localhost:9200/_cat/health?help'
	curl -XGET 'localhost:9200/_cat/health?v
	curl -XGET 'localhost:9200/_cat/health?h=cluster,status'
	curl -XGET 'localhost:9200/_cat/indices?bytes=b'
	
	curl -XPOST "http://localhost:9200/_snapshot/es/snapshot_1/_restore" -d'
	{
		"indices": "my_index",
		"ignore_unavailable": "true",
		"rename_replacement": "your_index"
	}'
	
	월별 index에서 특정 월만 검색하기
	https://knight76.tistory.com/entry/%EB%82%A0%EC%A7%9C%EB%B3%84-index%EC%97%90%EC%84%9C-%EB%B2%94%EC%9C%84%EB%A1%9C-%EA%B2%80%EC%83%89%ED%95%98%EA%B8%B0?category=587994
	
	curl -X GET http://inhouse.google.com:9200/reqlog-*/_search/reqlog-2019-09, reqlog-2019-10/_search?ignore_unavailable=true
	{
		"query": {
			...
		}
	}
	
	curl json 데이터
	curl json 파일명
	
	alias vs. index pattern?
	
	index 
		생성 : POST 
		조회 : GET _cat/indices
		변경 : 복원(이름 변경), ?(Mappings 변경, ...?)
		삭제 : DELETE
	
	Extending NEST typesedit
	https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/extending-nest-types.html

	Field data types
	https://www.elastic.co/guide/en/elasticsearch/reference/current/mapping-types.html
	->
	Inferred .NET type mapping
	https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/auto-map.html

-------------------------
POST /my_blog
{
	"mappings": {
		"post": {
			"properties": {
				"user_id": {
					"type": "integer"
				},
				"post_text": {
					"type": "string"
				},
				"post_date": {
					"type": "date"
				}
			}
		}
	}
}
GET my_blog/_mapping
------------------------------
POST my_blog/post		// _id 랜덤 데이터 색성
{
	"post_date": "2014-08-20"
	"post_text": "This is a readl blog post!",
	"user_id": 1
}
POST my_blog/post
{
	"post_date": "2014-08-25"
	"post_text": "This is another readl blog post!",
	"user_id": 2
}		
GET my_blog/_search		// 인덱스의 모든 데이터 검색
{
	... 인덱스 요약
	"hits" {
		... 검색 요약
		"hits": [
			{
				... 검색 결과 요약
				"_source": {
					값
				}
			},
			...
		]
	}
}
GET my_blog/{값 Id}		// 딘덱스의 특정 데이터 검색
------------------------------------
POST my_blog/post/1		// _id 1 데이터 생성
{
	"post_date": "2014-08-25"
	"post_text": "This is post with id 1",
	"user_id": 2
}	
GET my_blog/post/1		// _id 1 데이터 검색

-----------------------------------
2장
------------------------------------
Data Types
	string
	numbers 	: byte(1b), short(2b), integer, long, float(4b), double, 
	boolean
	date(UTC)
	binary(base 64 string, no index) : image, blob
	
POST /my_blog
{
	"settings": {
		"index": {
			"number_of_shards": 5
		}
	}
	"mappings": {
		"post": {
			"properties": {
				"user_id": {
					"type": "integer"
				},
				"post_text": {
					"type": "string"
				},
				"post_date": {
					"type": "date",
					"format": "YYYY-MM-DD"
				}
			}
		}
	}
}

DELETE my_blog
// POST 생성			POST /my_blog
// GET 인덱스 존재 확인	GET /_cat/indices
// GET 인덱스 매핑 확인	GET /my_blog/_mapping

POST my_blog/post		// 입력 실패 확인
{
	"post_date": "2014-08-20T12:01:00"
	"post_text": "This is a readl blog post!",
	"user_id": 1
}
----------------------------------------------
"enabled": false
	"store": true 		// 저장과 색인
	"store": false		// 색인
"enabled": true			// 저장과 색인

POST /my_blog
{
	"mappings": {
		"post": {
			"_source"" {
				"enabled": false
			}
			"properties": {
				"user_id": {
					"type": "integer",
					"store": true
				},
				"post_text": {
					"type": "string"
				},
				"post_date": {
					"type": "date",
					"format": "YYYY-MM-DD"
				}
			}
		}
	}
}

// 인덱스 삭제			DELETE my_blog
// 인덱스 생성			POST /my_blog
// GET 인덱스 존재 확인	GET /_cat/indices
// GET 인덱스 매핑 확인	GET /my_blog/_mapping
		
GET my_blog/post/{id}?fields=user_id,post_text		// user_id만 확인 가능

----------------------------------------------
모든 필드의 _all 검색을 지원하지 않는다. 인덱스 크기를 감소시킨다.
_all 검색?

POST /my_blog
{
	"mappings": {
		"post": {
			"_all"" {
				"enabled": false
			}
			"properties": {
				"user_id": {
					"type": "integer",
					"store": true
				},
				"post_text": {
					"type": "string"
				},
				"post_date": {
					"type": "date",
					"format": "YYYY-MM-DD"
				}
			}
		}
	}
}
---------------------------------------------
Indexes Routing
(GET) http://localhost:9200/my_blog/post/_search?q=post_text:awesome
(GET) http://localhost:9200/my_blog/post/_search?routing=2&post_text:awesome
 
POST /my_blog
{
	"mappings": {
		"post": {
			"_routing"" {
				"required": true,
				"path": "user_id"
			}
			"properties": {
				"user_id": {
					"type": "integer",
					"store": true
				},
				"post_text": {
					"type": "string"
				},
				"post_date": {
					"type": "date",
					"format": "YYYY-MM-DD"
				}
			}
		}
	}
}
----------------------------------------
Index Aliases

(POST) http://localhost:9200/_aliases
{
	"actions": {
		"add": {
			"index": "eventLog-2014-08-02",
			"alias": "eventLog"
		}
	}
}

(GET) http://localhost:9200/{aliases}/event/_search?q=event:error
http://localhost:9200/eventLog/event/_search?q=event:error

POST /eventlog-2014-08-01
{
	"mappings": {
		"event": {
			"properties": {
				"error": {
					"type": "string"
				}
			}
		}
	}
}

POST /eventlog-2014-08-02
{
	"mappings": {
		"event": {
			"properties": {
				"error": {
					"type": "string"
				}
			}
		}
	}
}

POST /eventlog-2014-08-01/event
{
	"error": "Something blew up!"
}

POST /eventlog-2014-08-02/event
{
	"error": "Another thing blew up!"
}

GET /eventLog-2014-08-02/_search

POST _aliases
{
	"actions": {
		"add": {
			"index": "eventLog-2014-08-01",
			"alias": "eventLog"
		}
	}
}

POST _aliases
{
	"actions": {
		"add": {
			"index": "eventLog-2014-08-02",
			"alias": "eventLog"
		}
	}
}

GET /eventLog/_search

----------------------------------------
2장 요약

POST /my_blog
{
	"settings": {
		"index": {
			"number_of_shards": 5
		}
	}
	"mappings": {
		"post": {
			"_source"" {
				"enabled": false
			},
			"_all"" {
				"enabled": false
			},
			"_routing"" {
				"required": true,
				"path": "user_id"
			}
			"properties": {
				"user_id": {
					"type": "integer",
					"store": true
				},
				"post_text": {
					"type": "string"
				},
				"post_date": {
					"type": "date",
					"format": "YYYY-MM-DD"
				}
			}
		}
	}
}

POST _aliases
{
	"actions": {
		"add": {
			"index": "eventLog-2014-08-01",
			"alias": "eventLog"
		}
	}
}

-----------------------------------
3장
------------------------------------

(GET) http://localhost:9200/my_blog/post/_search?q=post_text:awesome
(GET) http://localhost:9200/my_blog/_search?q=post_text:awesome

POST /my_blog
{
	"mappings": {
		"post": {
			"properties": {
				"user_id": {
					"type": "integer"
				},
				"post_text": {
					"type": "string"
				},
				"post_date": {
					"type": "date"
				},
				"post_word_count": {
					"type": "integer"
				}
			}
		}
	}
}

POST my_blog/post		
{
	"post_text": "yet another blog post!",
	"user_id": 1,
	"post_date": "2014-08-22",
	"post_word_count": 4
}

POST my_blog/post		
{
	"post_text": "this is a wonderful blog post",
	"user_id": 1,
	"post_date": "2014-08-18",
	"post_word_count": 6
}

POST my_blog/post		
{
	"post_text": "i really enjoy writing blog posts",
	"user_id": 2,
	"post_date": "2014-08-15",
	"post_word_count": 6
}

GET my_blog/post/_search?1=post_text:blog
GET my_blog/post/_search?1=post_text:wonderful

Query DSL(Domain Specific Language)

GET my_blog/post/_search
{
	"query": {
		"match": {
			"post_text": "wonderful"
		}
	}
}

// wonderful, blog, wonderful blog 모두가 있는 것
// _score
GET my_blog/post/_search
{
	"query": {
		"match": {
			"post_text": "wonderful blog"
		}
	}
}

// "wonderful blog" 정확히 일치한 검색
GET my_blog/post/_search
{
	"query": {
		"match_phrase": {
			"post_text": "wonderful blog"
		}
	}
}

GET my_blog/post/_search
{
	"query": {
		"filtered": {				<---?
			"filter": {
				"range": {
					"post_date": {
						"gt": "2014-10-18"
					}
				}
			},
			"query": {
				"match": {
					"post_text": "wonderful blog"
				}
			}
		}
	}
}

GET my_blog/post/_search
{
	"query": {
		"filtered": {				<---?
			"query": {
				"match": { 
					"post_text": "blog"
				}
			},
			"filter": {
				"term": {
					"user_id": "2"
				}
			}
		}
	}
}

GET my_blog/post/_search
{
	"query": {
		"filtered": {				<---?
			"query": {
				"match": { 
					"post_text": "blog"
				}
			},
			"filter": {
				"range": {
					"post_date": {
						"gt": "2014-09-16"
					}
				}
			}
		}
	}
}

-------------------------
GET my_blog/post/_search
{
	"query": {				-> _source
		"match": { 
			"post_text": "awesome"
		}
	},
	"highlight": {			-> highlight
		"fields": {
			"post_text": {}
		}
	}
}

-------------------------
GET my_blog/post/_search
{
	"query": {				-> _source
		"match": { 
			"post_text": "blog"
		}
	},
	"aggs": {				-> aggregations
		"all_words": {		-> 이름
			"terms": {		-> 연산: 단어 빈도 수
				"field": "post_text"
			}
		}
	}
}

GET my_blog/post/_search
{
	"query": {				-> _source
		"match": { 
			"post_text": "blog"
		}
	},
	"aggs": {				-> aggregations
		"avg_word_count": {	-> 이름
			"avg": {		-> 연산
				"field": "post_word_count"
			}
		}
	}
}
-------------------------
standard	: 소문자
	title-case 		: title, case
	ToLower(string)	: tolower, string
whitespace	: 대/소문자 구분
	title-case 		: title-case
	ToLower(string)	: ToLower(string)
simple		: 소문자
not_analyzed						<-- 언제 사용하나?
	"user_id": {
		"type": int,				<-- "integer"?
		"index": "not_analyzed"
	}


(GET) http://localhost:9200/_analyze?analyzer=standard
Convert the title-case text using the ToLower(string) command.

POST /my_blog
{
	"mappings": {
		"post": {
			"properties": {
				"user_id": {
					"type": "integer"
				},
				"post_text": {
					"type": "string",
					"analyzer": "standard"	<-- 기본 값
				},
				"post_date": {
					"type": "date"
				},
				"post_word_count": {
					"type": "integer"
				}
			}
		}
	}
}

POST my_blog/post
{
	"user_id": 1,
	"post_date": "2014-10-20",
	"post_text": "Convert the title-case text using the ToLower(string) command."
	"post_word_count": 8
}

GET my_blog/post/_search
{
	"query": {
		"term": {
			"post_text": "tolower"
		}
	}
}

POST /my_blog
{
	"mappings": {
		"post": {
			"properties": {
				"user_id": {
					"type": "integer"
				},
				"post_text": {
					"type": "string",
					"analyzer": "whitespace"
				},
				"post_date": {
					"type": "date"
				},
				"post_word_count": {
					"type": "integer"
				}
			}
		}
	}
}

POST my_blog/post
{
	"user_id": 1,
	"post_date": "2014-10-20",
	"post_text": "Convert the title-case text using the ToLower(string) command."
	"post_word_count": 8
}

GET my_blog/post/_search
{
	"query": {
		"term": {
			"post_text": "ToLower(string)"
		}
	}
}

-------------------------
3장 요약

query
	match
	match_phrase
	filter
aggs
analyzer
	standard
	whitespace
	simple
	not_analyzed
	
-----------------------------------
4장
------------------------------------

elasticsearch-head

Nest

Uri node = new Uri("...:9200");
ConnectionSettings settings = new ConnectionSettings(node, 
	settings: "my_blog");		// 인덱스 이름
ElasticClient client = new ElasticClient(settings);

IndexSettings indexSettings = new IndexSettings();
indexSettings.NumberOfReplicas = 1;
indexSettings.NumberOfShards = 1;

client.CreateIndex(c => c
	.Index("my_blog")
	.InitializeUsing(indexSettings)
	.AddMapping<Post>(m => m.MapFromAttributes()));	<-- 소문자로 시작한다.
	
var newBlogPost = new Post
{
	...
}

client.Index(newBlogPost);		// 데이터 추가


client.Search<Post>(s => s
	.Query(p => p.Term(q => q.PostText, "blog")));
	
client.Search<Post>(s => s
	.Query(q => q.MatchPhrase(m => m.OnField("postText").Query("..."))));
	
client.Search<Post>(s => s
	.Query(p => p.Term(q => q.PostText, "blog"))
	.Filter(f => f.Range(r => r.OnField("postDate").Greater("2014-10-29))));
	
------------------------------
용어
------------------------------
도큐먼트(Document) 		: 단일 데이터 단위 
색인(Indexing)
인덱스(Index, Indices) 	: 도큐먼트를 모아놓은 집합
검색(Search)
질의(Query)
샤드(shard)				: 인덱스는 

------------------------------
설치
------------------------------

------------------------------
실행 및 종료
------------------------------
-d
-p 파일명
ps -ef | grep elasticsearch
kill {pid}

echo 'bin/elasticsearch -d -p es.pid' > start.sh
echo 'kill `cat es.pid`' > stop.sh
chmod 755 start.sh stop.sh

------------------------------
옵션
------------------------------
jvm.options
	-Xms1g
	-Xmx1g
	
elasticsearch.yml
	cluster.name: "bluecats"
	node.name: "node01"
	node.attr.<key>: "value"				// 네임스페이스
	path.data: [ "경로" ]						// 데이터 경로, ./data/
	path.logs: "경로"							// 로그 경로, ./logs/{클러스터명}.log
	bootstrap.memory_lock: true				// 힙메모리 영역 점유
	network.host: "ip주소"					// 내/외부망
		network.bind_host					// 내부망
		network.public_host					// 외부망
		
		_local_								// 127.0.0.1
		_site_								// 로컬 네트워크 주소
		_global_							// 외부에서 바라보는 주소?
		
		ex. network.host: _site_			// IP 변화 대응 가능
	http.port: 포트번호						// 9200 기본 포트(9200 ~ 9299)
	transport.port 포트번호					// 9300 기본 TCP 포트(9300 ~ 9399), 노드들 끼리 통신
	
	discovery.seed_hosts: [ "호스명" ]			// 클러스터를 구성을 위해 바인딩할 노드, 9300 ~ 9305 기본 TCP 포트, 그 외 일경우?
	cluster.inital_master_nodes: [ "노트" ]	// 마스터 노드 선출 대상
	
	${환경변수명}
	
	node.master: true						// 마스터 후보(eligible) 노드 여부
	node.data: true							// 데이터 저장 여부
	node.ingest: true						// 데이터 색인시 전처리 작업 여부
	node.ml: true							// 머신러닝 수행 여부
	
	bin/elasticsearch -E cluster.name=bluecats -E node.name="node-1"
	
log4j2.properties

마스터 시작 로그
[노드명] node name [...], node ID [...] cluster name [...]
[...TransportService           ][노드명] publish_address (...:9300), ...
[...AbstractHttpServerTransport][노드명] publish_address (...:9200), ...
[...MasterService              ][노드명] elected-as-master ...

새 노드 추가시 마스터 로그
[...MasterService              ][노드명] node-join[{노드명} ...
[...ClusterApplierService      ][노드명] added {{노드명} ...

curl -XPUT "http://localhost:9200/books" -H 'Content-Type: application/json' -d'
{
	"settings": {
		"number_of_shards": 1,
		"number_of_replicas": 1
	}
}'

curl -XPUT "http://localhost:9200/books/_settings" -H 'Content-Type: application/json' -d'
{
	"number_of_replicas": 0
}'



1단계 : Linux용 Windows 하위 시스템 사용
"Linux용 Windows 하위 시스템"을 활성화 시킨다.

PowerShell 관리자 권한
dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart

2단계 : 2단계 - WSL 2 실행을 위한 요구 사항 확인
x64 시스템의 경우: 버전 1903 이상, 빌드 18362 이상

msinfo32

sudo ip addr
172.18.22.172

WSL2 Ubuntu GUI
https://www.youtube.com/watch?v=IL7Jd9rjgrM

id
lsb_release -a

-------------------------------------------------------

-------------------------------------------------------
sudo apt update && sudo apt -y upgrade
sudo apt install -y xrdp
sudo apt install -y xfce4
	gdm3
sudo apt install -y xfce4-goodies
sudo cp /etc/xrdp/xrdp.ini /etc/xrdp/xrdp.ini.bak
sudo sed -i 's/3389/3390/g' /etc/xrdp/xrdp.ini
sudo sed -i 's/max_bpp=32/#max_bpp=32\nmax_bpp=128/g' /etc/xrdp/xrdp.ini
sudo sed -i 's/xserverbpp=24/#xserverbpp=24\nxserverbpp=128/g' /etc/xrdp/xrdp.ini
echo xfce4-session > ~/.xsession
sudo nano /etc/xrdp/startwm.sh
# test -x ...
# exec /bin/sh ...

# xfce
startxfce4
Ctrl+X

sudo /etc/init.d/xrdp start

localhost:3390

ip addr | grep eth0
	172.23.239.4/20
	
ipconfig
	172.23.224.1		// wsl
	192.168.1.132		// host
	
ping 172.23.239.4

netsh interface portproxy add v4tov4 listenpport=3390 listenaddress=0.0.0.0 connectport=3390 connectaddress=172.23.239.4

github.com/microsoft/wsl/issues/4150

WSL 2 Networking
https://www.youtube.com/watch?v=yCK3easuYm4

https://www.youtube.com/watch?v=_fntjriRe48&list=PLhfrWIlLOoKNMHhB39bh3XBpoLxV3f0V9