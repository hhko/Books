- 상태 확인
  - HTTP : `http://localhost:9200/`
  - SHELL : `curl -XGET localhost:9200`
  - DEV : `GET /`
- curl 
  - json 파일명 : 
    ```curl
    curl -H 'Content-Type: application/json' -XPOST localhost:9200/classes/class/2/ -d @goddamn.json
    ```
  - josn 데이터 : 
    ```curl
    curl -XPOST "http://localhost:9200/_snapshot/es/snapshot_1/_restore" -d'
	{
		"indices": "my_index",
		"ignore_unavailable": "true",
		"rename_replacement": "your_index"
	}'
    ```

## 용어
- Index     <>  Database
- Type     <>  Table
- Document<>  Row
- Property(Field)     <>  Column
- Mapping  <>  Schema

## HTTP API
- GET      <>  Select
- PUT      <>  Update 
- POST    <>  Insert
- DELETE  <>  Delete

## Index
- 생성
- 조회 
- 변경 : 이름 변경(복원), 데이터 변경, 속성(Mappings, Settings, ...?) 변경
- 삭제